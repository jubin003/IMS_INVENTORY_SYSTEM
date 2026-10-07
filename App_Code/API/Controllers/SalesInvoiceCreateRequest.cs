using System.Collections.Generic;

namespace SalesInvoiceApi
{
    /// <summary>
    /// Shape of the JSON body accepted by POST api/sales. Groups the credentials,
    /// the customer, the invoice header, and its detail lines into one payload so
    /// the client can create a full invoice in a single request instead of three
    /// calls, and so this whole module can be dropped into any host project
    /// without needing OWIN/OAuth token plumbing to be set up first.
    /// </summary>
    public class SalesInvoiceCreateRequest
    {
        public CredentialsDto Credentials { get; set; }

        public int IsWalkIn { get; set; }
        public CustomerCreateDto Customer { get; set; }
        public InvoiceCreateDto Invoice { get; set; }
        public List<DetailCreateDto> Details { get; set; }
    }
}
