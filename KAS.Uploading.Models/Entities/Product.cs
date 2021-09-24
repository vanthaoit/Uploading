using Dapper.Contrib.Extensions;
using KAS.Uploading.Models.Abstract;
using KAS.Uploading.Models.SharedKernal;

namespace KAS.Uploading.Models.Entities
{
    [Table("kas.Product")]
    public class Product : DomainEntity<int>, IAvailable
    {
        //[Key]
        //public int ID { set; get; }

        public string Name { set; get; }

        public string Alias { set; get; }

        public string Image { set; get; }

        public string MoreImages { set; get; }

        public string Description { set; get; }

        public string Content { set; get; }

        public bool? HomeFlag { set; get; }

        public bool? HotFlag { set; get; }

        public int? ViewCount { set; get; }
        public bool IsActive { get; set; }
    }
}