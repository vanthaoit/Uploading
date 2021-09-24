using Dapper.Contrib.Extensions;
using KAS.Uploading.Models.SharedKernal;
using System;
using System.Collections.Generic;
using System.Text;

namespace KAS.Uploading.Models.Entities
{
    [Table("kas.XProduct_ProductCategory")]
    public class XProduct_ProductCategory:DomainEntity<long>
    {
        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }

    }
}
