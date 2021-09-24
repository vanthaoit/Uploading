using System;
using System.Collections.Generic;
using System.Text;

namespace KAS.Uploading.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
