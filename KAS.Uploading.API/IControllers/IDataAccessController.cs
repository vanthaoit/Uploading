using KAS.Uploading.Models.Structs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAS.Uploading.API.IControllers
{
    public interface IDataAccessController<in TEntity> where TEntity : class
    {
        [HttpGet("GetAll")]
        Task<IActionResult> GetAll();

        [HttpGet("Get/{id}")]
        Task<IActionResult> Get(int id);

        [HttpPost("GetWithProcedure/{storedProcedure}")]
        Task<IActionResult> GetWithProcedure(string storedProcedure, params ParameterDefinition[] parameters);

        [HttpPost("GetByFilter")]
        Task<IActionResult> GetByFilter([FromBody] params ParameterDefinition[] parameters);

        [HttpPost("Insert")]
        Task<IActionResult> Insert([FromBody] TEntity request);

        [HttpPost("InsertMany")]
        Task<IActionResult> InsertMany([FromBody] IEnumerable<TEntity> request);

        [HttpPost("Update")]
        Task<IActionResult> Update([FromBody] TEntity request);

        [HttpPost("UpdateMany")]
        Task<IActionResult> UpdateMany([FromBody] IEnumerable<TEntity> request);

        [HttpPost("Delete")]
        Task<IActionResult> Delete([FromBody] TEntity request);

        [HttpPost("DeleteMany")]
        Task<IActionResult> DeleteMany([FromBody] IEnumerable<TEntity> request);
    }
}
