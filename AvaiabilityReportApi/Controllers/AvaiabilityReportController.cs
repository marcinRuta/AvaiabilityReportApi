using AvaiabilityReportApi.Contracts;
using AvaiabilityReportApi.Dtos;
using AvaiabilityReportApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AvaiabilityReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaiabilityReportController : ControllerBase
    {
        private readonly IAvaiabilityReportRepository AvaiabilityReportRepository;

        public AvaiabilityReportController(IAvaiabilityReportRepository avaiabilityReportRepository)
        {
            AvaiabilityReportRepository = avaiabilityReportRepository;
        }

        [HttpPost]
        public async Task<ActionResult> PostStatus([FromBody] AvaiabilityReportContract avaiabilityReport)
        {
            try
            {
                var machine = await this.AvaiabilityReportRepository.GetMachine(avaiabilityReport.dev_eui);
                if (machine == null)
                {
                    return BadRequest("Device has not been added into the system");
                }

                var newAvaiabilityReport = new AvaiabilityReportDto { CurrentState = avaiabilityReport.decoded.payload.occupancy.ToString(), Machine = machine, Timestamp = DateTime.Now };
                var newReportStatus = await this.AvaiabilityReportRepository.AddAvaiabilityReport(newAvaiabilityReport);
                
                if(newReportStatus == null)
                {
                    return NoContent();
                }

                return Ok(newReportStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpGet]
        public async Task<ActionResult> GetStatus()
        {
            try
            {
                var result = await this.AvaiabilityReportRepository.LoadAvaiabilityFactSt();
                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }

   
    
}
