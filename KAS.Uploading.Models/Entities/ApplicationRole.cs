using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;


namespace KAS.Uploading.Models.Entities
{
    [Table("ApplicationRoles")]
    public class ApplicationRole : IdentityRole<Guid>
    {
        //public ApplicationRole() : base()
        //{

        //}
        //public ApplicationRole(string name, string description) : base(name)
        //{
        //    this.Description = description;
        //}

        //[StringLength(250)]
        //public string Description { get; set; }
    }
}
