using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace SalesInvoiceApi
{
    public class DetailCreateDto
    {
        public string PRODUCT_ID { get; set; }
        public string QUANTITY { get; set; }
        public string RATE { get; set; }
        public string SCHEME_DISCOUNT { get; set; }
    }
}
