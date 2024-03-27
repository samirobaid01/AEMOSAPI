using AEMOSAPI.DTO;
using AEMOSAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AEMOSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelemetryController : ControllerBase
    {
        private AemosCoreContext _context;

        public TelemetryController()
        {
            _context = new AemosCoreContext();
        }
        //GET: api/Telemetry/GetTelemetry
        [HttpGet]
        [Route("GetTelemetry")]
        public async Task<IActionResult> GetTelemetry()
        {
            try
            {
                List<TelemetryDTO> _telemetryList = new List<TelemetryDTO>();
                List<Device> _deviceList =await _context.Devices.Where(d => d.Type.ToString() == "Sensor" & d.Status==true).ToListAsync();
                if (_deviceList != null && _deviceList.Count > 0)
                {
                    foreach (Device d in _deviceList)
                    {
                        List<TelemetryDatum> _telemetryData = _context.TelemetryData.Where(td => td.DeviceId == d.Id).ToList();
                        if (_telemetryData != null && _telemetryData.Count > 0)
                        {
                            foreach (TelemetryDatum datum in _telemetryData)
                            {
                                DataStream ds = _context.DataStreams.Where(dt => dt.TelemetryDataId == datum.Id).OrderByDescending(dt=>dt.Id).FirstOrDefault();
                                if (ds != null)
                                {
                                    TelemetryDTO inst = new TelemetryDTO() { Id = d.Id, SensorName = d.Name, Uuid = d.Uuid, Variable = datum.VariableName, Data = ds.Value };
                                    _telemetryList.Add(inst);
                                }
                            }
                        }
                    }
                }                
                return Ok(_telemetryList);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<TelemetryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TelemetryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TelemetryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TelemetryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
