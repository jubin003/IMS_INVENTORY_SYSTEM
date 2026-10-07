using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace SalesInvoiceApi
{
    public class InvoiceCreateDto
    {
        public string TRANSACTION_DAY { get; set; }
        public string TRANSACTION_MONTH { get; set; }
        public string TRANSACTION_YEAR { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_ADDRESS { get; set; }
        public string COSTOMER_PAN_VAT { get; set; }
        public string DISCOUNT_PERCENT { get; set; }
        //public string AGE { get; set; }
        //public string GENDER { get; set; }
        public string MODE_OF_PAYMENT { get; set; }
    }
}