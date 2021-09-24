using System;
using System.Collections.Generic;
using System.Text;

namespace KAS.Uploading.Models.Abstract
{
    public interface IAvailable
    {
        bool IsActive { get; set; }
    }
}
