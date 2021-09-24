using Dapper;
using KAS.Uploading.API.IControllers;
using KAS.Uploading.BusinessFunctions.IServices;
using KAS.Uploading.BusinessFunctions.Services;
using KAS.Uploading.Models.Structs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static KAS.Uploading.Models.Entities.AuditTrail;

namespace KAS.Uploading.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditTrailController : ControllerBase, IDataAccessController<AuditTrails>
    {
        private readonly IDataAccessService<AuditTrails> _dataDapperService;

        public AuditTrailController(IConfiguration config)
        {
            _dataDapperService = new DapperDataService<AuditTrails>(config);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var resp = await _dataDapperService.GetAll();
            return new OkObjectResult(resp);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var resp = await _dataDapperService.Get(id);
            return new OkObjectResult(resp);
        }

        [HttpPost("GetWithProcedure/{storedProcedure}")]
        public async Task<IActionResult> GetWithProcedure(string storedProcedure, [FromBody] params ParameterDefinition[] parameters)
        {
            var dbParams = new DynamicParameters();
            if (parameters.Length > 0 && !string.IsNullOrEmpty(parameters[0].Key))
                foreach (var p in parameters)
                    dbParams.Add(p.Key, p.Value);

            var resp = await _dataDapperService.GetDataWithProc(storedProcedure, dbParams);
            return new OkObjectResult(resp);
        }

        [HttpPost("GetByFilter")]
        public async Task<IActionResult> GetByFilter([FromBody] params ParameterDefinition[] parameters)
        {
            var resp = await _dataDapperService.GetDataByFilter(parameters);
            return new OkObjectResult(resp);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] AuditTrails request)
        {
            try
            {
                var resp = await _dataDapperService.Insert(request);
                return new OkObjectResult(resp);
            }
            catch (Exception e)
            {
                Console.WriteLine("error create AuditTrails  " + e);
                return new OkObjectResult(null);
            }
        }

        [HttpPost("InsertMany")]
        public async Task<IActionResult> InsertMany([FromBody] IEnumerable<AuditTrails> request)
        {
            try
            {
                var resp = await _dataDapperService.Insert(request);
                return new OkObjectResult(resp);
            }
            catch (Exception e)
            {
                Console.WriteLine("error create AuditTrails  " + e);
                return new OkObjectResult(null);
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] AuditTrails request)
        {
            try
            {
                var resp = await _dataDapperService.Update(request);

                return new OkObjectResult(resp);
            }
            catch (Exception e)
            {
                Console.WriteLine("error create AuditTrails  " + e);
                return new OkObjectResult(null);
            }
        }

        [HttpPost("UpdateMany")]
        public async Task<IActionResult> UpdateMany([FromBody] IEnumerable<AuditTrails> request)
        {
            try
            {
                var resp = await _dataDapperService.Update(request);

                return new OkObjectResult(resp);
            }
            catch (Exception e)
            {
                Console.WriteLine("error create AuditTrails  " + e);
                return new OkObjectResult(null);
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] AuditTrails request)
        {
            try
            {
                var resp = await _dataDapperService.Delete(request);

                return new OkObjectResult(resp);
            }
            catch (Exception e)
            {
                Console.WriteLine("error create AuditTrails  " + e);
                return new OkObjectResult(null);
            }
        }

        [HttpPost("DeleteMany")]
        public async Task<IActionResult> DeleteMany([FromBody] IEnumerable<AuditTrails> request)
        {
            try
            {
                var resp = await _dataDapperService.Delete(request);

                return new OkObjectResult(resp);
            }
            catch (Exception e)
            {
                Console.WriteLine("error create AuditTrails  " + e);
                return new OkObjectResult(null);
            }
        }
    }
}