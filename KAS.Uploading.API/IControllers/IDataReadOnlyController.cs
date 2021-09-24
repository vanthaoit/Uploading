using KAS.Uploading.Models.Structs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAS.Uploading.API.IControllers
{
    public interface IDataReadOnlyController<in TEntity> where TEntity : class
    {
        [HttpGet("GetAll")]
        Task<IActionResult> GetAll();

        [HttpGet("Get/{id}")]
        Task<IActionResult> Get(int id);

        [HttpPost("GetWithProcedure/{storedProcedure}")]
        Task<IActionResult> GetWithProcedure(string storedProcedure, [FromBody] params ParameterDefinition[] parameters);

        [HttpPost("GetByFilter")]
        Task<IActionResult> GetByFilter([FromBody] params ParameterDefinition[] parameters);
    }
}
