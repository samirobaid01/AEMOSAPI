using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AEMOS_IdentityA.Models;
using AEMOS_IdentityA.DTO;

namespace AEMOS_IdentityA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly AemosCoreContext _context;

        public DevicesController()
        {
            _context = new AemosCoreContext();
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
           //   IList<Device> devicelist= await _context.Devices.Select(dev=> new Device() { Id=dev.Id,Name= dev.Name, Uuid= dev.Uuid}).ToListAsync();
           var deviceList = await _context.Devices.Select(dev => new{ dev.Id, dev.Name, dev.Uuid, dev.UpdatedAt, dev.Status }).ToListAsync();
            return Ok(deviceList);
        }

        //GET: api/Devices/5
        [HttpGet]
        [Route("GetDevice/{id}")]
        public async Task<ActionResult<Device>> GetDevice(long id)
        {
            var device = await _context.Devices.FindAsync(id);//.(dev => new { dev.Id, dev.Name });
                if (device == null)
            {
                return NotFound();
            }

            return Ok(new {device.Id, device.Name, device.Uuid, device.UpdatedAt,device.Description});
        }

        // PUT: api/Devices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(long id, Device device)
        {
            if (id != device.Id)
            {
                return BadRequest();
            }

            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Devices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateDevice")]
        public async Task<ActionResult<Device>> CreateDevice(Device device)
        {
            device.Uuid = new Guid();
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevice", new { id = device.Id }, device);
        }

        [HttpPost("DeviceTelemetry")]
        public async Task<ActionResult<TelemetryDatum>> DeviceTelemetry(DeviceTelemetry dev)
        {
            try
            {
                Device? _device = (_context.Devices.Where(d => d.Uuid == dev.UUID)).SingleOrDefault();
                if (_device != null)
                {
                    TelemetryDatum? _telemetryDataum = (_device.TelemetryData.Where(td => td.DeviceId == _device.Id)).SingleOrDefault();
                    if (_telemetryDataum != null)
                    {
                        DataStream _dataStream = new DataStream() { Value = dev.value, TelemetryDataId=_telemetryDataum.Id  };
                        _context.Add(_dataStream);
                        await _context.SaveChangesAsync();
                        return CreatedAtAction("DeviceTelemetry", dev);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Device may not have defined parameters" });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Device may not exist" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to send telemetry data");
            }
        }

        [HttpGet]
        [Route("LatestTelemetry/{UUID}")]
        public async Task<ActionResult<List<SingleDataStream>>> LatestTelemetry(string UUID)
        {
            try
            {
                Device? _device = await (_context.Devices.Where(d => d.Uuid == new Guid(UUID))).SingleOrDefaultAsync();
                if (_device != null)
                {
                    IList<TelemetryDatum> _telemetryDataumList = (_device.TelemetryData.Where(td => td.DeviceId == _device.Id)).ToList();
                    SingleDataStream _dataStream = new SingleDataStream();
                    List<SingleDataStream> deviceLatestDataStream = new List<SingleDataStream>();
                    foreach (TelemetryDatum _dataInst in _telemetryDataumList)
                    {
                        _dataStream.Variable = _dataInst.VariableName;
                        _dataStream.Type = _dataInst.Datatype;
                        _dataStream.Value = _dataInst.DataStreams.Last().Value;
                        deviceLatestDataStream.Add(_dataStream);
                    }
                    return Ok(deviceLatestDataStream);
                }
                return StatusCode(StatusCodes.Status404NotFound, "Probably no device exist");
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get telemetry data");
            }
        }
        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(long id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceExists(long id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}
