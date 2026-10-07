using System;
using System.Net;
using System.Web.Http;
using DataHelper.Framework;
using Entity.Components;
using Service.Components;
using PhyeGanCore;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SalesInvoiceApi
{
    // Yo file le POST user garxa to insert in 3 tables: customer if needed, sales_invoice_master, sales_invoice_detail
    //
    // Fully self-contained and portable: does NOT rely on OWIN, OAuth, or any
    // header-based auth attribute. Auth is a single check against
    // Credentials.USERNAME / Credentials.PASSWORD in the JSON body, hashed the
    // same way the rest of the app hashes login credentials (ED.md5), then
    // checked against LoginService. To drop this module into a new project:
    // copy this folder in, make sure the host project references the same
    // DataHelper.Framework / Entity.Components / Service.Components /
    // PhyeGanCore assemblies, and register WebApiConfig.Register from
    // Global.asax. No OWIN Startup, no token endpoint, nothing else required.
    // See README.md.
    //
    // All responses use the standardized envelope:
    //   { "CODE": "...", "MESSAGE": "...", "VALUE": "..." }
    //
    //   101  -> credentials problem (missing / incorrect / unauthorized group / unresolved employee)
    //   200  -> success, VALUE = "[PK_ID],[INVOICE_NUMBER]"
    //   400  -> validation failure, VALUE = the specific validation message
    //   408  -> unknown/unexpected error
    //   409  -> duplicate submission, VALUE = "Invoice already exist."
    [RoutePrefix("api/sales")]
    public class SalesInvoiceCreateApiController : ApiController
    {
        private readonly SALES_INVOICE_MASTERService SIMSer = new SALES_INVOICE_MASTERService();
        private readonly SALES_INVOICE_DETAILService SDSer = new SALES_INVOICE_DETAILService();
        private readonly CUSTOMERService CSer = new CUSTOMERService();
        private readonly PRODUCTService PSer = new PRODUCTService();
        private readonly EMPLOYEESService ESer = new EMPLOYEESService();
        private readonly PhyeGanDate PGD = new PhyeGanDate();
        private readonly PhyeGan PG = new PhyeGan();
        private readonly HelperFunction hf = new HelperFunction();

        // ============================================================
        // Response envelope helper
        // ============================================================
        private IHttpActionResult ApiResult(int httpStatusCode, string code, string message, string value)
        {
            return Content((HttpStatusCode)httpStatusCode, new
            {
                CODE = code,
                MESSAGE = message,
                VALUE = value
            });
        }

        private IHttpActionResult Fail101(string value)
        {
            return ApiResult(401, "101", "API credentials do not match ", value);
        }

        private IHttpActionResult Fail400(string value)
        {
            return ApiResult(400, "400", "Fail", value);
        }

        private IHttpActionResult Fail408(string value)
        {
            return ApiResult(408, "408", "Fail", value ?? "Unknown error.");
        }

        private IHttpActionResult Fail409(string value)
        {
            return ApiResult(409, "409", "Fail", value);
        }

        private IHttpActionResult Success200(string pkId, string invoiceNumber)
        {
            string value = string.Format("[{0}],[{1}]", pkId, invoiceNumber);
            return ApiResult(200, "200", "Inserted", value);
        }

        [HttpPost, Route("")]
        public IHttpActionResult Create([FromBody] SalesInvoiceCreateRequest request)
        {
            if (request == null || request.Invoice == null)
                return Fail400("Request body must include at least an 'Invoice' object.");

            if (request.Details == null || request.Details.Count == 0)
                return Fail400("Request body must include at least one entry in 'Details'.");

            if (request.Credentials == null ||
                string.IsNullOrEmpty(request.Credentials.USERNAME) ||
                string.IsNullOrEmpty(request.Credentials.PASSWORD))
                return Fail101("Credentials.USERNAME and Credentials.PASSWORD are required.");

            // ============================================================
            // Auth: single path, body-supplied credentials, hashed the same
            // way the rest of the app stores/hashes them (ED.md5). No
            // header-based auth, no bearer token, no OWIN.
            // ============================================================

            var LSer = new LoginService();

            var loginCheck = (Entity.Components.Login)LSer.GetSingle(new Entity.Components.Login
            {
                LOGINID = request.Credentials.USERNAME,
                PASSWORD = request.Credentials.PASSWORD
            });

            if (loginCheck == null)
                return Fail101("Incorrect username or password.");

            if (loginCheck.GROUPID != "6") // api user group 5
                return Fail101("This account is not authorized to use this API.");

            string authenticatedUsername = loginCheck.FULLDETAILS;
            string employeeId = loginCheck.EMPLOYEEID;

            if (string.IsNullOrEmpty(employeeId))
                return Fail101("Could not resolve authenticated employee. Please log in again.");

            string nextInvoiceId = authenticatedUsername + hf.getmaxinvid();

            // IsWalkIn is the explicit source of truth (set from the UI checkbox,
            // sent as 1/0)   not inferred from an empty CUSTOMER_CODE.
            if (request.IsWalkIn != 0 && request.IsWalkIn != 1)
                return Fail400("IsWalkIn must be 0 or 1.");

            bool isWalkIn = request.IsWalkIn == 1;

            if (!isWalkIn && (request.Customer == null || string.IsNullOrEmpty(request.Customer.CUSTOMER_CODE)))
                return Fail400("Customer code is required unless this is a walk-in sale.");

            if (string.IsNullOrEmpty(request.Invoice.TRANSACTION_DATE))
                return Fail400("TRANSACTION_DATE is required in dd/mm/yyyy format.");

            DateTime transactionDate;
            if (!DateTime.TryParseExact(
                    request.Invoice.TRANSACTION_DATE,
                    "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out transactionDate))
            {
                return Fail400(string.Format(
                    "Invalid TRANSACTION_DATE '{0}'. Expected format is dd/mm/yyyy.",
                    request.Invoice.TRANSACTION_DATE));
            }

            // ============================================================
            // TRANSACTION_DAY / TRANSACTION_MONTH / TRANSACTION_YEAR are optional. If the client sends them, we validate they're
            // numeric and agree with TRANSACTION_DATE, then use exactly what was sent (no reformatting). If the client omits them,
            // we derive all three from TRANSACTION_DATE ourselves, same as before.
            // ============================================================
            bool anyPartSent = !string.IsNullOrEmpty(request.Invoice.TRANSACTION_DAY) ||
                                !string.IsNullOrEmpty(request.Invoice.TRANSACTION_MONTH) ||
                                !string.IsNullOrEmpty(request.Invoice.TRANSACTION_YEAR);

            string resolvedTransactionDay, resolvedTransactionMonth, resolvedTransactionYear;

            if (!anyPartSent)
            {
                // Nothing sent   derive from TRANSACTION_DATE, same as before.
                resolvedTransactionDay = transactionDate.Day.ToString("00");
                resolvedTransactionMonth = transactionDate.Month.ToString("00");
                resolvedTransactionYear = transactionDate.Year.ToString();
            }
            else
            {
                // At least one was sent   at that point we need all three,
                // so we can validate them as a set against TRANSACTION_DATE.
                if (string.IsNullOrEmpty(request.Invoice.TRANSACTION_DAY) ||
                    string.IsNullOrEmpty(request.Invoice.TRANSACTION_MONTH) ||
                    string.IsNullOrEmpty(request.Invoice.TRANSACTION_YEAR))
                    return Fail400("If any of TRANSACTION_DAY, TRANSACTION_MONTH, or TRANSACTION_YEAR is not sent, all three must be sent   or donot send TRANSACTION_DAY, TRANSACTION_MONTH, and TRANSACTION_YEAR .");

                int txDay, txMonth, txYear;

                if (!int.TryParse(request.Invoice.TRANSACTION_DAY, out txDay))
                    return Fail400(string.Format(
                        "Invalid TRANSACTION_DAY '{0}'. It must be a number.", request.Invoice.TRANSACTION_DAY));

                if (!int.TryParse(request.Invoice.TRANSACTION_MONTH, out txMonth))
                    return Fail400(string.Format(
                        "Invalid TRANSACTION_MONTH '{0}'. It must be a number.", request.Invoice.TRANSACTION_MONTH));

                if (!int.TryParse(request.Invoice.TRANSACTION_YEAR, out txYear))
                    return Fail400(string.Format(
                        "Invalid TRANSACTION_YEAR '{0}'. It must be a number.", request.Invoice.TRANSACTION_YEAR));

                if (txDay != transactionDate.Day || txMonth != transactionDate.Month || txYear != transactionDate.Year)
                    return Fail400(string.Format(
                        "TRANSACTION_DAY/TRANSACTION_MONTH/TRANSACTION_YEAR ({0}/{1}/{2}) do not match TRANSACTION_DATE '{3}'.",
                        request.Invoice.TRANSACTION_DAY, request.Invoice.TRANSACTION_MONTH, request.Invoice.TRANSACTION_YEAR,
                        request.Invoice.TRANSACTION_DATE));

                // Client sent them   keep exactly what they sent, unformatted.
                resolvedTransactionDay = request.Invoice.TRANSACTION_DAY;
                resolvedTransactionMonth = request.Invoice.TRANSACTION_MONTH;
                resolvedTransactionYear = request.Invoice.TRANSACTION_YEAR;
            }

            string customerId = null;
            string invoicePkId = null;
            string invoiceNumberForResponse = null;

            using (var DT = new DistributedTransaction())
            {
                try
                {
                    // fetch offcie code from employee
                    var employee = (EMPLOYEES)ESer.GetSingle(new EMPLOYEES { EMPLOYEEID = employeeId }, DT);

                    if (employee == null || string.IsNullOrEmpty(employee.OFFICE_CODE))
                        throw new ApplicationException(
                            string.Format("No office code found for EMPLOYEEID '{0}'.", employeeId));

                    string resolvedOfficeCode = employee.OFFICE_CODE;

                    // ============================================================
                    // STEP 0.6: Reject if this exact submission already went through
                    // ============================================================
                    var duplicateCheck = (SALES_INVOICE_MASTER)SIMSer.GetSingle(new SALES_INVOICE_MASTER { NEXT_INVOIICE_ID = nextInvoiceId }, DT);

                    if (duplicateCheck != null)
                    {
                        return Fail409("Invoice already exist.");
                    }

                    // ============================================================
                    // STEP 1: Handle the customer
                    //
                    // Walk-in = 0 (registered customer):
                    //   CUSTOMER_CODE is mandatory (already enforced above) and MUST  match an existing row in CUSTOMER. 
                    //   If it doesn't match, fail with 400 "Customer ID not found" .                   

                    // Walk-in = 1:
                    //   No CUSTOMER_CODE check at all. Customer details are taken straight from request.Invoice (name defaults to "Cash"),
                    //   and CUSTOMER_ID on the invoice is left null.
                    // ============================================================
                    CUSTOMER CUSEnt = null;

                    if (!isWalkIn)
                    {
                        CUSEnt = (CUSTOMER)CSer.GetSingle(new CUSTOMER { CUSTOMER_CODE = request.Customer.CUSTOMER_CODE }, DT);

                        if (CUSEnt == null)
                        {
                            return Fail400(string.Format("Customer ID not found: '{0}'.", request.Customer.CUSTOMER_CODE));
                        }

                        customerId = CUSEnt.PK_ID;
                    }
                    else
                    {
                        // if customer name is not send, by default make it cash
                        if (string.IsNullOrEmpty(request.Invoice.CUSTOMER_NAME))
                            request.Invoice.CUSTOMER_NAME = "Cash";
                        customerId = null;
                    }

                    // ============================================================
                    // STEP 1.6: Validate every detail line's product exists, capture PK_ID per line, and do the same qty * rate - scheme_discount

                    // NOTE: No VAT and no invoice-level DISCOUNT_PERCENT for
                    // API sales (confirmed) 
                    //     TOTAL               = QUANTITY * RATE
                    //     AFTER_SCHEME_DISC   = TOTAL - SCHEME_DISCOUNT

                    //     and invoice-level:
                    //     SUB_TOTAL           = sum(TOTAL)
                    //     TOTAL_SCHEME_DISC   = sum(SCHEME_DISCOUNT)
                    //     GRAND_TOTAL         = SUB_TOTAL - TOTAL_SCHEME_DISC
                    // ============================================================
                    var productIds = new List<string>();            // parallel to request.Details
                    var lineTotals = new List<decimal>();            // QTY * RATE, parallel to request.Details
                    var lineAfterSchemeDisc = new List<decimal>();   // TOTAL - SCHEME_DISCOUNT, parallel to request.Details

                    decimal subTotal = 0m;
                    decimal totalSchemeDiscount = 0m;

                    foreach (var detailDto in request.Details)
                    {
                        if (detailDto == null)
                            return Fail400("Each entry in 'Details' cannot be null.");

                        if (string.IsNullOrEmpty(detailDto.PRODUCT_ID))
                            return Fail400("Each detail line must include a PRODUCT_ID.");

                        if (string.IsNullOrEmpty(detailDto.QUANTITY))
                            return Fail400(string.Format(
                                "QUANTITY is required for product '{0}'.", detailDto.PRODUCT_ID));

                        if (string.IsNullOrEmpty(detailDto.RATE))
                            return Fail400(string.Format(
                                "RATE is required for product '{0}'.", detailDto.PRODUCT_ID));

                        var product = (PRODUCT)PSer.GetSingle(new PRODUCT { PRODUCT_CODE = detailDto.PRODUCT_ID }, DT);

                        if (product == null)
                        {
                            return Fail400(string.Format("Product not found: '{0}'.", detailDto.PRODUCT_ID));
                        }

                        productIds.Add(product.PK_ID);

                        decimal qty, rate, schemeDisc;

                        if (!decimal.TryParse(detailDto.QUANTITY, out qty) || qty <= 0)
                            return Fail400(string.Format("Invalid QUANTITY for product '{0}'. It must be a number greater than 0.", detailDto.PRODUCT_ID));

                        if (!decimal.TryParse(detailDto.RATE, out rate) || rate < 0)
                            return Fail400(string.Format("Invalid RATE for product '{0}'. It must be a non-negative number.", detailDto.PRODUCT_ID));

                        if (string.IsNullOrEmpty(detailDto.SCHEME_DISCOUNT))
                        {
                            schemeDisc = 0m; // optional field, defaults to 0 when not provided
                        }
                        else if (!decimal.TryParse(detailDto.SCHEME_DISCOUNT, out schemeDisc) || schemeDisc < 0)
                        {
                            return Fail400(string.Format("Invalid SCHEME_DISCOUNT for product '{0}'. It must be a non-negative number.", detailDto.PRODUCT_ID));
                        }

                        decimal lineTotal = qty * rate;
                        decimal afterSchemeDisc = lineTotal - schemeDisc;

                        if (afterSchemeDisc < 0)
                            return Fail400(string.Format("SCHEME_DISCOUNT for product '{0}' cannot be greater than the line total ({1}).", detailDto.PRODUCT_ID, lineTotal));

                        lineTotals.Add(lineTotal);
                        lineAfterSchemeDisc.Add(afterSchemeDisc);

                        subTotal += lineTotal;
                        totalSchemeDiscount += schemeDisc;
                    }

                    // No VAT, no overall invoice discount for API sales, so the taxable/grand total collapse to the same figure.
                    decimal discountPercent = 0m;
                    if (!string.IsNullOrEmpty(request.Invoice.DISCOUNT_PERCENT))
                    {
                        if (!decimal.TryParse(request.Invoice.DISCOUNT_PERCENT, out discountPercent) || discountPercent < 0 || discountPercent > 100)
                            return Fail400("DISCOUNT_PERCENT must be a number between 0 and 100.");
                    }

                    decimal amountAfterSchemeDiscount = subTotal - totalSchemeDiscount;
                    decimal discountAmount = amountAfterSchemeDiscount * discountPercent / 100m;

                    if (discountAmount > amountAfterSchemeDiscount)
                        return Fail400("DISCOUNT_PERCENT results in a discount amount greater than the total after scheme discounts.");

                    decimal grandTotal = amountAfterSchemeDiscount - discountAmount;

                    // ============================================================
                    // STEP 1.5 / 2: Build the real invoice entity, then save it
                    //
                    // Customer fields on the invoice header:
                    //   - Not walk-in: NAME / ADDRESS / PAN_VAT / CONTACT_NUMBER are  sourced from the matched CUSTOMER row (resolvedCustomer),
                    //     never from client input, so the invoice reflects what's     actually on file. CONTACT_NUMBER comes from CUSTOMER.PHONE.
                    //   - Walk-in: sourced from request.Invoice as sent by the    client (name defaults to "Cash" if blank); no contact  number is set.
                    // ============================================================

                    #region to insert in Invoice Master

                    var invoice = new SALES_INVOICE_MASTER
                    {
                        TRANSACTION_DAY = resolvedTransactionDay,
                        TRANSACTION_MONTH = resolvedTransactionMonth,
                        TRANSACTION_YEAR = resolvedTransactionYear,
                        TRANSACTION_DATE = request.Invoice.TRANSACTION_DATE,

                        CUSTOMER_NAME = isWalkIn
                            ? (string.IsNullOrEmpty(request.Invoice.CUSTOMER_NAME) ? "Cash" : request.Invoice.CUSTOMER_NAME)
                            : CUSEnt.CUSTOMER_NAME,
                        CUSTOMER_ADDRESS = isWalkIn
                            ? request.Invoice.CUSTOMER_ADDRESS
                            : CUSEnt.ADDRESS,
                        COSTOMER_PAN_VAT = isWalkIn
                            ? request.Invoice.COSTOMER_PAN_VAT
                            : CUSEnt.PAN_VAT,
                        CUSTOMER_CONTACT_NUMBER = isWalkIn
                            ? null
                            : CUSEnt.PHONE,
                        //AGE = request.Invoice.AGE,
                        //GENDER = request.Invoice.GENDER,
                        SALES_TYPE_ID = request.Invoice.MODE_OF_PAYMENT,
                        DISCOUNT_PERCENT = discountPercent.ToString("0.00"),
                        TAXABLE_DISC_AMOUNT = discountAmount.ToString("0.00"),
                        EXEMPTED = "0",
                        TAX_VAT_AMOUNT = "0.00",
                        TAXABLE_SUB_TOTAL = amountAfterSchemeDiscount.ToString("0.00"),
                        TAXABLE_TOTAL = grandTotal.ToString("0.00"),
                        NON_TAXABLE_SUB_TOTAL = "0.00",
                        NON_TAXABLE_TOTAL = "0.00",
                        GRAND_TOTAL = grandTotal.ToString("0.00"),
                        ROUND_OFF = "0.00",
                        INVOICE_AMOUNT = grandTotal.ToString("0.00"),
                        USER_ID = employeeId,
                        OFFICE_CODE = resolvedOfficeCode,
                        CUSTOMER_ID = customerId,
                        INVOICE_DATE = PGD.GetTodayDate("dd/mm/yyyy"),
                        NEXT_INVOIICE_ID = nextInvoiceId,
                        IS_PRINTED = "0",
                        CANCEL_STATUS = "0",
                        COPY_PRINTNO = "0",
                        PRINT_BY = employeeId
                    };

                    string nepaliToday = PGD.GetTodayNepaliDate();
                    string[] nepaliParts = nepaliToday.Split('/');

                    invoice.INVOICE_DAY = nepaliParts[0];
                    invoice.INVOICE_MONTH = nepaliParts[1];
                    invoice.INVOICE_YEAR = nepaliParts[2];
                    invoice.INVOICE_FY = PGD.checkFiscalYear(nepaliParts[1], nepaliParts[2]);

                    invoicePkId = SIMSer.Insert(invoice, DT).ToString();

                    #endregion
                    if (!DT.HAPPY)
                        throw new ApplicationException(
                            "Insert failed for the invoice header (SALES_INVOICE_MASTER)   the database rejected the row (check required/NOT NULL fields).");


                    #region to insert in Invoice Detail
                    int sno = 1;
                    for (int i = 0; i < request.Details.Count; i++)
                    {
                        var detailDto = request.Details[i];

                        // Computed in STEP 1.6 above:
                        //   TOTAL             = QUANTITY * RATE
                        //   AFTER_SCHEME_DISC = TOTAL - SCHEME_DISCOUNT
                        // No VAT for API sales (confirmed), so the line's
                        // taxable/tax fields are zeroed out and the full
                        // after-scheme-discount amount goes to NON_TAXABLE_TOTAL.
                        decimal lineTotal = lineTotals[i];
                        decimal afterSchemeDisc = lineAfterSchemeDisc[i];

                        var detail = new SALES_INVOICE_DETAIL
                        {
                            PRODUCT_ID = productIds[i],
                            QUANTITY = detailDto.QUANTITY,
                            RATE = detailDto.RATE,
                            SCHEME_DISCOUNT = string.IsNullOrEmpty(detailDto.SCHEME_DISCOUNT) ? "0" : detailDto.SCHEME_DISCOUNT,
                            TOTAL = lineTotal.ToString("0.00"),
                            TAXABLE_TOTAL = afterSchemeDisc.ToString("0.00"),
                            NON_TAXABLE_TOTAL = "0.00",
                            TAX_AMOUNT = "0.00",
                            AMOUNT_AFTER_TAX = afterSchemeDisc.ToString("0.00"),
                            OFFICE_CODE = resolvedOfficeCode,
                            SNO = sno.ToString(),
                            SALES_INVOICE_ID = invoicePkId
                        };

                        SDSer.Insert(detail, DT);

                        if (!DT.HAPPY)
                            throw new ApplicationException(string.Format("Insert failed for detail line SNO '{0}'   the database rejected the row (check required/NOT NULL fields).", detail.SNO));

                        sno++;
                    }
                    #endregion

                    if (DT.HAPPY == true)
                    {
                        DT.Commit();
                        var savedInvoice = (SALES_INVOICE_MASTER)SIMSer.GetSingle(new SALES_INVOICE_MASTER { PK_ID = invoicePkId });
                        invoiceNumberForResponse = savedInvoice != null ? savedInvoice.INVOICE_NUMBER : null;

                        #region fo IRD API

                        if (PG.CBMSPush() == "ON")
                        {
                            double taxable_sub_total = 0;
                            double nontaxable_sub_total = 0;
                            if (savedInvoice.EXEMPTED != "1")
                            {
                                taxable_sub_total = Convert.ToDouble(savedInvoice.TAXABLE_SUB_TOTAL);
                            }
                            else
                            {
                                nontaxable_sub_total = Convert.ToDouble(savedInvoice.TAXABLE_SUB_TOTAL);
                            }
                            using (var client = new HttpClient())
                            {
                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                BillViewModel p = new BillViewModel
                                {
                                    username = PG.CBMSUsername(),
                                    password = PG.CBMSPassword(),
                                    seller_pan = PG.CompanyVATPan(),
                                    buyer_pan = savedInvoice.COSTOMER_PAN_VAT,
                                    buyer_name = savedInvoice.CUSTOMER_NAME,
                                    fiscal_year = PGD.CBMSFY(savedInvoice.INVOICE_FY), // "2076.077",                                   
                                    invoice_number = savedInvoice.INVOICE_NUMBER,
                                    invoice_date = savedInvoice.INVOICE_YEAR + "." + savedInvoice.INVOICE_MONTH + "." + savedInvoice.INVOICE_DAY, // "2077.07.06",
                                    total_sales = Convert.ToDouble(savedInvoice.TAXABLE_SUB_TOTAL),
                                    taxable_sales_vat = 0,
                                    vat = 0,
                                    excisable_amount = 0,
                                    excise = 0,
                                    taxable_sales_hst = taxable_sub_total,
                                    hst = Convert.ToDouble(savedInvoice.TAX_VAT_AMOUNT),
                                    amount_for_esf = 0,
                                    esf = 0,
                                    export_sales = 0,
                                    tax_exempted_sales = nontaxable_sub_total,
                                    isrealtime = true,
                                    datetimeClient = DateTime.Now
                                };

                                client.BaseAddress = new Uri(PG.CBMSURL());
                                try
                                {
                                    // yo line le APL ko SSL certificate expire bhaye pani kam garni huncha 
                                    // yo first line 1 ta bhaye na bhane SSL certificate expire bhayo bhana API le kam gardaina
                                    //  ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

                                    var response = client.PostAsJsonAsync("api/bill", p).Result;

                                    if (response.IsSuccessStatusCode)
                                    {
                                        var result = response.Content.ReadAsStringAsync();
                                        Console.Write(result.Result);
                                        Console.ReadLine();
                                        savedInvoice.CBMS_PUSH = result.Result;
                                        if (result.Result == "200")
                                            savedInvoice.CBMS_PUSH_RT = "YES";
                                        else
                                            savedInvoice.CBMS_PUSH_RT = "NO";

                                        //104: model invalid
                                        //200: success
                                        //102: exception while saving credit note details
                                        //101: bill does not exists
                                        //100: API credentials do not match
                                        //103: Unknown exceptions
                                        //105: Bill does not exists (for Sales Return)
                                    }
                                    else
                                    {
                                        savedInvoice.CBMS_PUSH_RT = "NO";
                                    }
                                }
                                catch (Exception ee)
                                {
                                    savedInvoice.CBMS_PUSH_RT = "NO";
                                }
                            }
                            SIMSer.Update(savedInvoice);
                        }
                        else
                        {
                            savedInvoice.CBMS_PUSH = "CBMS OFF";
                            savedInvoice.CBMS_PUSH_RT = "";
                            SIMSer.Update(savedInvoice);
                        }
                        #endregion
                    }
                    else
                    {
                        DT.Abort();
                        invoiceNumberForResponse = null;
                    }
                }
                catch (Exception ex)
                {
                    if (!DT.Done)
                        DT.Abort();

                    return Fail408(ex.Message);
                }
            }

            return Success200(invoicePkId, invoiceNumberForResponse);
        }
    }

    public class BillViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string seller_pan { get; set; }
        public string buyer_pan { get; set; }
        public string fiscal_year { get; set; }
        public string buyer_name { get; set; }
        public string invoice_number { get; set; }
        public string invoice_date { get; set; }
        public double total_sales { get; set; }
        public Nullable<double> taxable_sales_vat { get; set; }
        public Nullable<double> vat { get; set; }
        public Nullable<double> excisable_amount { get; set; }
        public Nullable<double> excise { get; set; }
        public Nullable<double> taxable_sales_hst { get; set; }
        public Nullable<double> hst { get; set; }
        public Nullable<double> amount_for_esf { get; set; }
        public Nullable<double> esf { get; set; }
        public Nullable<double> export_sales { get; set; }
        public Nullable<double> tax_exempted_sales { get; set; }
        public bool isrealtime { get; set; }
        public DateTime datetimeClient { get; set; }
    }

}
