using System;

namespace KAS.Uploading.Models.Abstract
{
    public class Auditable : IAuditable
    {
        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

    }
}