using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAS.Uploading.API.Models
{
    public class DMReportingModel
    {
        public string ClientName { get; set; }
        public string Payer { get; set; }
        public int VoucherNumber { get; set; }
        public bool DM { get; set; }
        public bool HighAcuity { get; set; }
        public int HasPayment { get; set; }
        public int Has0Balance { get; set; }
        public int BillingDate { get; set; }
        public int DateUpdated { get; set; }
        public int VoidDate { get; set; }
        public bool Populate270 { get; set; }
    }
}
