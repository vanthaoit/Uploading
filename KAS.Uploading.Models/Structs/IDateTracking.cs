using System;
using System.Collections.Generic;
using System.Text;

namespace KAS.Uploading.Models.Structs
{
	public interface IDateTracking
	{
		DateTime DateCreated { set; get; }

		DateTime DateModified { set; get; }
	}
}
