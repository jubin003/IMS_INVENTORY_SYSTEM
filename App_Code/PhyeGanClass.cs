using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Entity.Components;
using Service.Components;
using System.Net;
using System.Data;
using Entity.Framework;

namespace PhyeGanCore
{
    public class ED
    {
        #region Encrypt and Decrypt

        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText)
        {
            string passPhrase = "ANIL BIR SINGH TULADHAR";
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            string passPhrase = "ANIL BIR SINGH TULADHAR";
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
        public string md5(string sPassword)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(sPassword);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }



        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        public string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        #endregion
    }
    public class PhyeGan
    {
        NAME_COMPANY NCEnt = new NAME_COMPANY();
        NAME_COMPANYService NCSer = new NAME_COMPANYService();

        OFFICE OEnt = new OFFICE();
        OFFICEService OSer = new OFFICEService();

        HelperFunction hf = new HelperFunction();
        public string CompanyName()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = ED.Decrypt(NCEnt.ORG_NAME);
            }
            return ret;
        }
        public bool ProductManufacturerStatus()
        {
            bool ret = false; // default to false

            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = NCEnt.IS_MANUFACTURER == "1";

            }

            return ret;
        }

        public string CompanyAddress()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = ED.Decrypt(NCEnt.ORG_ADDRESS);
            }
            return ret;
        }
        public string CompanyRegistration()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = ED.Decrypt(NCEnt.REG_NO);
            }
            return ret;
        }
        public string CompanyVATPan()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = ED.Decrypt(NCEnt.PAN_NO);
            }
            return ret;
        }
        public string CompanyTAXType()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = NCEnt.TAX_TYPE;
            }
            return ret;
        }
        public string CompanyTAXPercent()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = NCEnt.TAX_PERCENT;
            }
            return ret;
        }
        public string CompanyEmail()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = NCEnt.EMAIL_ID;
            }
            return ret;
        }
        public string CompanyWebsite()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = NCEnt.WEBSITE;
            }
            return ret;
        }
        public string CompanyContact()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = NCEnt.CONTACT_DETAIL;
            }
            return ret;
        }
        public string AccountEnabled()
        {
            string return_value = "ENABLE";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                // return_value = ED.Decrypt(NCEnt.ACC_E);
            }
            return return_value;
        }
        public string SoftwareValidDate()
        {
            string date = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                date = ED.Decrypt(NCEnt.VE_DATE);
            }
            return date;
        }
        public string SoftwareExpiryDate()
        {
            string date = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                date = ED.Decrypt(NCEnt.E_DATE);
            }
            return date;
        }
        public string RenewedExpiryDate(string licence_key)
        {
            string expdate = "";
            WebClient wbClient = new WebClient();
            expdate = wbClient.DownloadString("http://phyegan.com/licensekey.php?pannumber=" + ED.Decrypt(NCEnt.PAN_NO) + "&productcode=IMS&license_key=" + licence_key);

            return expdate;
        }
        public void UpdateExpireDate(string licence_key)
        {
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            NCEnt.E_DATE = ED.Encrypt(RenewedExpiryDate(licence_key));
            NCSer.Update(NCEnt);
        }
        public string Org_Base_URL()
        {
            string Org_Base_URL = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                Org_Base_URL = NCEnt.BASE_URL.ToLower();
            }
            return Org_Base_URL;
        }
        public string InvoiceHeading()
        {
            string return_value = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                return_value = ED.Decrypt(NCEnt.INVOICE);
            }
            return return_value;
        }
        public string CBMSPush()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = NCEnt.CBMS_PUSH;
            }
            return ret;
        }
        public string CBMSUsername()
        {
            string return_value = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                return_value = NCEnt.CBMS_USERNAME;
            }
            return return_value;
        }
        public string CBMSURL()
        {
            string return_value = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                return_value = NCEnt.CBMS_URL;
            }
            return return_value;
        }
        public string CBMSPassword()
        {
            string return_value = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                return_value = NCEnt.CBMS_PASSWORD;
            }
            return return_value;
        }
        public string CBMS_Approved_Date()
        {
            string return_value = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                try
                {
                    return_value = ED.Decrypt(NCEnt.CBMS_A_DATE);
                }
                catch { }
            }
            return return_value;
        }
        public bool IS_CBMS_Approved()
        {
            bool return_value = true;
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                if (NCEnt.CBMS_PUSH == "OFF")
                {
                    return_value = false;
                }
            }
            return return_value;
        }
        public bool CompanyBranch_Status()
        {
            bool return_value = true;
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                if (NCEnt.BRANCH_STATUS == "0")
                {
                    return_value = false;
                }
            }
            return return_value;
        }
        public string EXIMCODE()
        {
            string ret = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                ret = ED.Decrypt(NCEnt.EXIM_CODE);
            }
            return ret;
        }


        #region BRANCH
        public string BranchContact(string office_code)
        {
            string ret = "";
            OEnt = new OFFICE();
            OEnt.PK_ID = office_code;
            OEnt = (OFFICE)OSer.GetSingle(OEnt);
            if (OEnt != null)
            {
                ret = OEnt.PHONE_NO;
            }
            return ret;
        }
        public string BranchEmail(string office_code)
        {
            string ret = "";
            OEnt = new OFFICE();
            OEnt.PK_ID = office_code;
            OEnt = (OFFICE)OSer.GetSingle(OEnt);
            if (OEnt != null)
            {
                ret = OEnt.EMAIL;
            }
            return ret;
        }
        public string BranchAddress(string office_code)
        {
            string ret = "";
            OEnt = new OFFICE();
            OEnt.PK_ID = office_code;
            OEnt = (OFFICE)OSer.GetSingle(OEnt);
            if (OEnt != null)
            {
                ret = OEnt.STREET;
            }
            return ret;
        }

        #endregion


        public Boolean checkPageAccess(string pageurl, string groupid)
        {
            string pageid = "";
            PAGES PGEnt = new PAGES();
            PAGESService PGSer = new PAGESService();
            PGEnt.PAGENAME = pageurl;
            PGEnt = (PAGES)PGSer.GetSingle(PGEnt);
            if (PGEnt != null)
            {
                pageid = PGEnt.PK_ID;
                try
                {
                    string permission = hf.getPagePermission(pageid, groupid);
                    if (permission != "00000000")
                        return true;
                    else
                        return false;
                }
                catch
                { return false; }
            }
            else
                return false;
        }

        public Boolean checkBranchAccess(string pageurl, string groupid)
        {

            string pageid = "";
            PAGES PGEnt = new PAGES();
            PAGESService PGSer = new PAGESService();
            PGEnt.PAGENAME = pageurl.ToLower();
            PGEnt = (PAGES)PGSer.GetSingle(PGEnt);
            if (PGEnt != null)
            {
                pageid = PGEnt.PK_ID;
                string permission = hf.getPagePermission(pageid, groupid);
                if (permission.Substring(7, 1) == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public Boolean VoucherCreateAccess(string pageurl, string groupid)
        {
            string pageid = "";
            PAGES PGEnt = new PAGES();
            PAGESService PGSer = new PAGESService();
            PGEnt.PAGENAME = pageurl.ToLower();
            PGEnt = (PAGES)PGSer.GetSingle(PGEnt);
            if (PGEnt != null)
            {
                pageid = PGEnt.PK_ID;
                string permission = hf.getPagePermission(pageid, groupid);

                if (permission.Substring(0, 1) == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public Boolean VoucherCheckAccess(string pageurl, string groupid)
        {
            string pageid = "";
            PAGES PGEnt = new PAGES();
            PAGESService PGSer = new PAGESService();
            PGEnt.PAGENAME = pageurl.ToLower();
            PGEnt = (PAGES)PGSer.GetSingle(PGEnt);
            if (PGEnt != null)
            {
                pageid = PGEnt.PK_ID;
                string permission = hf.getPagePermission(pageid, groupid);

                if (permission.Substring(1, 1) == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public Boolean VoucherApproveAccess(string pageurl, string groupid)
        {
            string pageid = "";
            PAGES PGEnt = new PAGES();
            PAGESService PGSer = new PAGESService();
            PGEnt.PAGENAME = pageurl.ToLower();
            PGEnt = (PAGES)PGSer.GetSingle(PGEnt);
            if (PGEnt != null)
            {
                pageid = PGEnt.PK_ID;
                string permission = hf.getPagePermission(pageid, groupid);

                if (permission.Substring(2, 1) == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public EntityList getBranchList()
        {
            EntityList theList = new EntityList();
            EntityList newList = new EntityList();
            OEnt = new OFFICE();
            theList = OSer.GetAll(OEnt);
            if (theList.Count > 0)
            {
                foreach (OFFICE O in theList)
                {
                    OFFICE tempOffice = new OFFICE();
                    tempOffice.PK_ID = O.PK_ID;
                    tempOffice = (OFFICE)OSer.GetSingle(tempOffice);
                    if (tempOffice != null)
                    {
                        tempOffice.OFFICENAME = ED.Decrypt(tempOffice.OFFICENAME) + "-" + tempOffice.STREET;
                        newList.Add(tempOffice);
                    }

                }

            }
            return newList;
        }


    }

}


public class PhyeGanDate
{
    ADBS ADBSEnt = new ADBS();
    ADBSService ADBSSer = new ADBSService();

    #region date Functions
    public string CheckDate(string str)
    {
        string[] shortdate = str.Split(' ');
        return shortdate[0];
    }
    public int NepaliMonthsLastDay(string NepaliYear, string NepaliMonth)
    {
        Boolean Found = false;
        int i = 32;

        while (Found == false)
        {
            ADBSEnt = new ADBS();
            ADBSEnt.BS_DAY = i.ToString();
            ADBSEnt.BS_MONTH = NepaliMonth;
            ADBSEnt.BS_YEAR = NepaliYear;
            ADBSEnt = (ADBS)ADBSSer.GetSingle(ADBSEnt);
            if (ADBSEnt != null)
                Found = true;
            else
            {
                i = i - 1;
            }
        }
        return i;

    }

    public int EnglishMonthsLastDay(string EngYear, string EngMonth)
    {
        Boolean Found = false;
        int i = 31;

        while (Found == false)
        {
            ADBSEnt = new ADBS();
            ADBSEnt.AD_DAY = i.ToString();
            ADBSEnt.AD_MONTH = EngMonth;
            ADBSEnt.AD_YEAR = EngYear;
            ADBSEnt = (ADBS)ADBSSer.GetSingle(ADBSEnt);
            if (ADBSEnt != null)
                Found = true;
            else
            {
                i = i - 1;
            }
        }
        return i;

    }
    public string checkFiscalYear(string month, string year)
    {
        string FY = "";
        try
        {
            int MM, CY, AY;

            MM = Int32.Parse(month);
            CY = Int32.Parse(year);
            if (MM < 4)
            {
                AY = CY - 1;
                FY = AY.ToString() + "/" + CY.ToString().Substring(2, 2);

            }
            else if (MM >= 4)
            {
                AY = CY + 1;
                FY = CY.ToString() + "/" + AY.ToString().Substring(2, 2);
            }
            else
            { }
        }
        catch
        {

        }
        return FY;
    }

    public string CBMSFY(string FY)
    {
        //returrn format "2073.074"
        string return_value = "";
        try
        {
            string[] year = FY.Split('/');
            return_value = year[0] + ".0" + year[1];
        }
        catch { }
        return return_value;
    }

    public string ConvertNepaliTOEnglish(string day, string month, string year)
    {
        string EDate = "";
        ADBSEnt = new ADBS();
        ADBSEnt.BS_DAY = day;
        ADBSEnt.BS_MONTH = month;
        ADBSEnt.BS_YEAR = year;
        ADBSEnt = (ADBS)ADBSSer.GetSingle(ADBSEnt);
        if (ADBSEnt != null)
            EDate = ADBSEnt.AD_DATE_F;

        return EDate;
    }
    public string GetEnglishDateFromNepali(string nepalidate, string format)
    {
        string return_date = "";
        DateTime newdate;
        if (format == "dd/mm/yyyy")
        {
            try
            {
                string[] date = nepalidate.Split('/');
                return_date = this.ConvertNepaliTOEnglish(date[0], date[1], date[2]);

            }
            catch//(Exception kbhayobhayo)
            { }
        }
        else if (format == "dd-MMM-yyyy")
        {
            try
            {
                string[] date = nepalidate.Split('/');
                string englishdate = this.ConvertNepaliTOEnglish(date[0], date[1], date[2]);
                newdate = DateTime.ParseExact(englishdate, "dd/MM/yyyy", null);
                return_date = newdate.ToString("dd-MMM-yyyy");
            }
            catch//(Exception kbhayobhayo)
            { }
        }
        return return_date;
    }
    public string GetNepaliDateFromEnglish(string EnglishDate, string format)
    {
        string NDate = "";
        ADBSEnt = new ADBS();
        ADBSEnt.AD_DATE = EnglishDate;
        ADBSEnt = (ADBS)ADBSSer.GetSingle(ADBSEnt);
        if (ADBSEnt != null)
        {
            if (format == "dd/mm/yyyy")
            {
                NDate = ADBSEnt.BS_DATE;
            }
            else if (format == "yyyy/mm/dd")
            {
                NDate = ADBSEnt.BS_YEAR + "/" + Convert.ToInt16(ADBSEnt.BS_MONTH).ToString("00") + "/" + Convert.ToInt16(ADBSEnt.BS_DAY).ToString("00");
            }
        }

        return NDate;
    }
    public string NepaliDay()
    {
        string NDay = "";
        DateTime englishDate = DateTime.Today;
        ADBSEnt = new ADBS();
        ADBSEnt.AD_DATE = englishDate.ToString("dd/MM/yyyy");
        ADBSEnt = (ADBS)ADBSSer.GetSingle(ADBSEnt);
        if (ADBSEnt != null)
            NDay = Convert.ToInt16(ADBSEnt.BS_DAY).ToString("00");
        return NDay;
    }
    public string NepaliMonth()
    {
        string NMonth = "";
        DateTime englishDate = DateTime.Today;
        ADBSEnt = new ADBS();
        ADBSEnt.AD_DATE = englishDate.ToString("dd/MM/yyyy");
        ADBSEnt = (ADBS)ADBSSer.GetSingle(ADBSEnt);
        if (ADBSEnt != null)
            NMonth = Convert.ToInt16(ADBSEnt.BS_MONTH).ToString("00");
        return NMonth;
    }
    public string NepaliYear()
    {
        string NYear = "";
        DateTime englishDate = DateTime.Today;
        ADBSEnt = new ADBS();
        ADBSEnt.AD_DATE = englishDate.ToString("dd/MM/yyyy");
        ADBSEnt = (ADBS)ADBSSer.GetSingle(ADBSEnt);
        if (ADBSEnt != null)
            NYear = ADBSEnt.BS_YEAR;
        return NYear;
    }
    public string GetTodayDate(string format)
    {
        string todaydate = "";
        DateTime today = DateTime.Today;
        if (format == "dd/mm/yyyy")
            todaydate = today.ToString("dd/MM/yyyy");
        else if (format == "dd-MMM-yyyy")
            todaydate = today.ToString("dd-MMM-yyyy");
        return todaydate;
    }
    public string GetTodayNepaliDate()
    {
        string NDay = "";
        DateTime englishDate = DateTime.Today;
        ADBSEnt = new ADBS();
        ADBSEnt.AD_DATE = englishDate.ToString("dd/MM/yyyy");
        ADBSEnt = (ADBS)ADBSSer.GetSingle(ADBSEnt);
        if (ADBSEnt != null)
            NDay = Convert.ToInt16(ADBSEnt.BS_DAY).ToString("00") + "/" + Convert.ToInt16(ADBSEnt.BS_MONTH).ToString("00") + "/" + ADBSEnt.BS_YEAR;
        return NDay;
    }

    public string SwapNepaliDate(string date, string format) //input==dd/mm/yyyy
    {
        string new_date = "";
        string[] date_to_change = date.Split('/');
        if (format == "yyyy/mm/dd")
            new_date = date_to_change[2] + "/" + date_to_change[1] + "/" + date_to_change[0];
        if (format == "mm/dd/yyyy")
            new_date = date_to_change[1] + "/" + date_to_change[0] + "/" + date_to_change[2];
        return new_date;
    }
    public string NepaliMonths(string calMth)
    {
        //current month name
        string MonthName = "";
        if (calMth == "01")
        {
            MonthName = "वैशाख";
        }
        else if (calMth == "02")
        {
            MonthName = "जेठ";
        }
        else if (calMth == "03")
        {
            MonthName = "असार";
        }
        else if (calMth == "04")
        {
            MonthName = "साउन";
        }
        else if (calMth == "05")
        {
            MonthName = "भदौ";
        }
        else if (calMth == "06")
        {
            MonthName = "असोज";
        }
        else if (calMth == "07")
        {
            MonthName = "कार्तिक";
        }
        else if (calMth == "08")
        {
            MonthName = "मङ्‌सिर";
        }
        else if (calMth == "09")
        {
            MonthName = "पुस";
        }
        else if (calMth == "10")
        {
            MonthName = "माघ";
        }
        else if (calMth == "11")
        {
            MonthName = "फागुन";
        }
        else if (calMth == "12")
        {
            MonthName = "चैत";
        }
        return MonthName;
    }
    public string SplitNepaliDateToEnglish(string nepalidate)
    {
        string[] shortdates = nepalidate.Split('/');
        string day = shortdates[0];
        string month = shortdates[1];
        string year = shortdates[2];
        string englishdate;
        englishdate = ConvertNepaliTOEnglish(day, month, year);
        return englishdate;
    }
    public string getEnglishMonth(string Month)
    {
        if (Month == "1")
            return "January";
        else if (Month == "2")
            return "Febuary";
        else if (Month == "3")
            return "March";
        else if (Month == "4")
            return "April";
        else if (Month == "5")
            return "May";
        else if (Month == "6")
            return "June";
        else if (Month == "7")
            return "July";
        else if (Month == "8")
            return "August";
        else if (Month == "9")
            return "September";
        else if (Month == "10")
            return "October";
        else if (Month == "11")
            return "November";
        else if (Month == "12")
            return "December";
        else
            return "";
    }
    public string getNepaliMonth(string Month)
    {
        if (Month == "1")
            return "Baisakh";
        else if (Month == "2")
            return "Jestha";
        else if (Month == "3")
            return "Asadh";
        else if (Month == "4")
            return "Shrawan";
        else if (Month == "5")
            return "Bhadra";
        else if (Month == "6")
            return "Ashoj";
        else if (Month == "7")
            return "Kartik";
        else if (Month == "8")
            return "Mangshir";
        else if (Month == "9")
            return "Poush";
        else if (Month == "10")
            return "Magh";
        else if (Month == "11")
            return "Falgun";
        else if (Month == "12")
            return "Chaitra";
        else
            return "";
    }

    public string ConvertToOrdinalDateFormat(string englishDate)
    {
        string result = "";
        try
        {
            // Strip any time portion if present (e.g. "09/04/2026 12:00:00 AM")
            string datePart = englishDate.Split(' ')[0];

            string[] parts = datePart.Split('/');
            int month = Convert.ToInt32(parts[1]);
            int day = Convert.ToInt32(parts[0]);
            string year = parts[2];

            string monthName = getEnglishMonth(month.ToString());
            string suffix = GetDaySuffix(day);

            result = day + suffix + " " + monthName + ", " + year;
        }
        catch //(Exception ex)
        { }
        return result;
    }

    private string GetDaySuffix(int day)
    {
        if (day >= 11 && day <= 13)
            return "th";

        switch (day % 10)
        {
            case 1: return "st";
            case 2: return "nd";
            case 3: return "rd";
            default: return "th";
        }
    }

    public string getPrevious_FiscalYear(string currentFY)
    {
        string[] splitFV = currentFY.Split('/').ToArray();
        int yr1 = Convert.ToInt32(splitFV[0]) - 1;
        int yr2 = Convert.ToInt32(splitFV[1]) - 1;
        string prevFY = (yr1 + "/" + yr2);

        return prevFY;
    }
    public string getFiscalYearEndDateEng(string fiscalYear)
    {
        ADBSEnt = new ADBS();
        string[] year = fiscalYear.Split('/');
        ADBSEnt.BS_YEAR = "20" + year[1];
        ADBSEnt.BS_MONTH = "3";
        EntityList Al = ADBSSer.GetAll(ADBSEnt);

        var maxRecord = Al.Cast<ADBS>()
                          .OrderByDescending(x => x.BS_DATE)     // sort by BS_DAY descending
                          .FirstOrDefault();                    // take the first (max)

        return maxRecord != null ? maxRecord.AD_DATE : "";
    }
    public string getFiscalYearEndDateNep(string fiscalYear)
    {
        ADBSEnt = new ADBS();
        string[] year = fiscalYear.Split('/');
        ADBSEnt.BS_YEAR = "20" + year[1];
        ADBSEnt.BS_MONTH = "3";
        EntityList Al = ADBSSer.GetAll(ADBSEnt);

        var maxRecord = Al.Cast<ADBS>()
                          .OrderByDescending(x => x.BS_DATE)     // sort by BS_DAY descending
                          .FirstOrDefault();                    // take the first (max)

        return maxRecord != null ? maxRecord.BS_DATE : "";
    }
    public string getFiscalYearStartDateEng(string fiscalYear)
    {
        ADBSEnt = new ADBS();
        string[] year = fiscalYear.Split('/');
        ADBSEnt.BS_YEAR = year[0];
        ADBSEnt.BS_MONTH = "4";
        EntityList Al = ADBSSer.GetAll(ADBSEnt);

        var maxRecord = Al.Cast<ADBS>()
                          .OrderBy(x => x.BS_DATE)     // sort by BS_DAY ascending
                          .FirstOrDefault();                    // take the first (max)

        return maxRecord != null ? maxRecord.AD_DATE : "";
    }
    public string getFiscalYearStartDateNep(string fiscalYear)
    {
        ADBSEnt = new ADBS();
        string[] year = fiscalYear.Split('/');
        ADBSEnt.BS_YEAR = year[0];
        ADBSEnt.BS_MONTH = "4";
        EntityList Al = ADBSSer.GetAll(ADBSEnt);

        var maxRecord = Al.Cast<ADBS>()
                          .OrderBy(x => x.BS_DATE)     // sort by BS_DAY ascending
                          .FirstOrDefault();                    // take the first (max)

        return maxRecord != null ? maxRecord.BS_DATE : "";
    }

    #endregion

}
public class PhyeGanProductSetting
{
    PRODUCT_SETTING PSEnt = new PRODUCT_SETTING();
    PRODUCT_SETTINGService PSSer = new PRODUCT_SETTINGService();

    public bool ProductBatch()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.BATCH_NUMBER == "1")
                ret = true;
        }
        return ret;
    }
    public bool ProductExpDate()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.EXPIRY_DATE == "1")
                ret = true;
        }
        return ret;
    }


    public bool ProductManufacture()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.PRODUCT_MANUFACTURER == "1")
                ret = true;
        }
        return ret;
    }

    public bool ProductColor()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.PRODUCT_COLOUR == "1")
                ret = true;
        }
        return ret;
    }
    public bool ProductSize()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.PRODUCT_SIZE == "1")
                ret = true;
        }
        return ret;
    }

    public bool DualQuantity()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.DUAL_QUANTITY == "1")
                ret = true;
        }
        return ret;
    }
    public bool ShowDualQuantity()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.SHOW_DUAL_QUANTITY == "1")
                ret = true;
        }
        return ret;
    }
    public bool ItemWiseDiscount()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.ITEM_WISE_DISCOUNT == "1")
                ret = true;
        }
        return ret;
    }
    public bool MultipleRate()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.MULTIPLE_RATE == "1")
                ret = true;
        }
        return ret;
    }
    public bool SHOW_AVAILABILITY()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.SHOW_AVAILABILITY == "1")
                ret = true;
        }
        return ret;
    }
    public bool ShowPO()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.SHOW_PO_NUMBER == "1")
                ret = true;
        }
        return ret;
    }
    public bool RoundOff()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.ROUND_OFF == "1")
                ret = true;
        }
        return ret;

    }
    public bool OnlyStockSales()
    {
        bool ret = false;
        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.ONLY_STOCK_SALES == "1")
                ret = true;
        }
        return ret;
    }
}
