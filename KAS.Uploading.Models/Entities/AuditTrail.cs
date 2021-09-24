using Dapper.Contrib.Extensions;
using System;


namespace KAS.Uploading.Models.Entities
{
    public class AuditTrail
    {
        public class AuditTrails
        {
            [Key]
            public int Id { get; set; }

            public string TableName { get; set; }

            public long RecordId { get; set; }

            public string Field { get; set; }

            public string OldValue { get; set; }

            public string NewValue { set; get; }

            public DateTime ChangeDateTime { set; get; }

            public string UserAction { set; get; }

            public Guid UserId { set; get; }

            [Computed]
            public string PageName { get; set; }

        }
    }
}
