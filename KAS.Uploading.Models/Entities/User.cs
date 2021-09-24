using Dapper.Contrib.Extensions;
using KAS.Uploading.Models.SharedKernal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KAS.Uploading.Models.Entities
{
    [Dapper.Contrib.Extensions.Table("kas.User")]
    public class User:DomainEntity<int>
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
