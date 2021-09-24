using System;
using System.Collections.Generic;
using System.Text;

namespace KAS.Uploading.Models.Entities
{
    public class Error
    {
        public int ID { set; get; }

        public string Message { set; get; }

        public string StackTrace { set; get; }

        public DateTime CreatedDate { set; get; }
    }
}
