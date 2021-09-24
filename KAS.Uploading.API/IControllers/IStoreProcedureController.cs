using KAS.Uploading.Models.Structs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAS.Uploading.API.IControllers
{
    public interface IStoreProcedureController
    {
        Task<IActionResult> GetWithProcedure(string storedProcedure, [FromBody] params ParameterDefinition[] parameters);
    }
}
