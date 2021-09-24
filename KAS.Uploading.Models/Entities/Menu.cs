using Dapper.Contrib.Extensions;
using KAS.Uploading.Models.SharedKernal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace KAS.Uploading.Models.Entities
{
    [Table("kas.Menus")]
    public class Menu:DomainEntity<int>
    {

        public string Name { set; get; }

        public string URL { set; get; }

        public int? DisplayOrder { set; get; }

        public string Target { set; get; }

        public bool Status { set; get; }

        public int? hasChild { set; get; }
    }
}
