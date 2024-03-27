using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AEMOSAPI.Models;
using AEMOSAPI.DTO;

namespace AEMOSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private AemosCoreContext _context;

        public DevicesController()
        {
            _context = new AemosCoreContext();
        }

        [HttpPut]
        [Route("TriggerState/{id}")]
        public async Task<IActionResult> TriggerState(long id, Device d)
        {
            var _deviceInstance = await _context.Devices.FindAsync(id);
            if (id != d.Id)
            {
                return BadRequest();
            }
            if (_deviceInstance != null)
            {
                _deviceInstance.State = d.State;
                _context.Entry(_deviceInstance).State = EntityState.Modified;
            }
            else
            {
                return BadRequest();
            }
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

        //GET: api/Devices
        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            try
            {
                _context = new AemosCoreContext();
                List<Device> _tem = _context.Devices.ToList();
                List<DeviceDTO> _deviceList = await (from d in _context.Devices
                                                     join _areaDev in _context.AreaDevices on d.Id equals _areaDev.DeviceId
                                                     join _area in _context.Areas on _areaDev.AreaId equals _area.Id
                                                     where d.Status == true  & d.Type.ToString() == "Device"
                                                     select
                                                     new DeviceDTO() { Id = d.Id, Name = d.Name, State = d.State, Area = _area.Name, uuid = d.Uuid }).ToListAsync();
                // IList<Device> devicelist = await _context.Devices.Select(dev => new Device() { Id = dev.Id, Name = dev.Name, Uuid = dev.Uuid }).ToListAsync();
                return Ok(_deviceList);
            }
            catch(Exception Ex)
            {
                return BadRequest(Ex.Message +Ex.StackTrace);
            }
        }

        //GET: api/Devices
        [HttpGet]
        [Route("GetSensors")]
        public async Task<IActionResult> GetSensors()
        {
            try
            {
                _context = new AemosCoreContext();
                List<Device> _tem = _context.Devices.ToList();
                List<SensorDTO> _sensorList = await (from d in _context.Devices
                                                     join _areaDev in _context.AreaDevices on d.Id equals _areaDev.DeviceId
                                                     join _area in _context.Areas on _areaDev.AreaId equals _area.Id
                                                     where d.Status == true & d.Type.ToString()=="Sensor" 
                                                     select
                                                     new SensorDTO() { Id = d.Id, Name = d.Name, Area = _area.Name, AreaId=_area.Id, uuid = d.Uuid }).ToListAsync();
                return Ok(_sensorList);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message + Ex.StackTrace);
            }
        }

        // GET: api/Devices/5
        [HttpGet]
        [Route("GetDevice/{id}")]
        public async Task<ActionResult<Device>> GetDevice(long id)
        {
            var device = await _context.Devices.FindAsync(id);//.(dev => new { dev.Id, dev.Name });
            if (device == null)
            {
                return NotFound();
            }

            return Ok(new { device.Id, device.Name, device.Uuid, device.UpdatedAt, device.Description, device.State });
        }

        [HttpGet]
        [Route("GetDeviceState/{uuid}")]
        public async Task<ActionResult<Device>> GetDeviceState(String uuid)
        {
            Guid _tempId = new Guid(uuid);
            try
            {
                Device dev = await _context.Devices.Where(d => d.Uuid == _tempId).SingleOrDefaultAsync();
                if (dev != null)
                {
                    return Ok(dev.State);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

       // PUT: api/Devices/5
       //  To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(long id, Device device)
        {
            if (id != device.Id)
            {
                return BadRequest();
            }

            var _deviceInstance = _context.Devices.Find(id);
            if (_deviceInstance != null)
            {
                _deviceInstance.Name = device.Name;
                _context.Entry(_deviceInstance).State = EntityState.Modified;
            }
            else
            {
                return BadRequest();
            }
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

        [HttpPut]
        [Route("UpdateSensor/{id}")]
        public async Task<IActionResult> UpdateSensor(long id, SensorDTO sensor)
        {
            if (id != sensor.Id)
            {
                return BadRequest();
            }

            var _deviceInstance = _context.Devices.Find(id);
            if (_deviceInstance != null)
            {

                AreaDevice _areaDevInst = (from ad in _context.AreaDevices where ad.DeviceId == _deviceInstance.Id select ad).SingleOrDefault();
                _context.AreaDevices.Remove(_areaDevInst);
                _context.SaveChanges();
                _deviceInstance.Name = sensor.Name;
                _deviceInstance.AreaDevices.Add(new AreaDevice() { AreaId = sensor.AreaId, DeviceId = sensor.Id });
                _context.Entry(_deviceInstance).State = EntityState.Modified;
            }
            else
            {
                return BadRequest();
            }
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
        //POST: api/Devices
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateDevice")]
        public async Task<ActionResult<Device>> CreateDevice(DeviceDTO device)
        {
            device.uuid = Guid.NewGuid();
            Device _deviceInst = new Device();
            _deviceInst.Uuid = device.uuid;
            _deviceInst.Name = device.Name;
            _deviceInst.Description = device.Description;
            _deviceInst.Status = true;
            _deviceInst.State = "off";
            _deviceInst.Type = "Device";
            // _deviceInst.CreatedAt = DateTime.UtcNow.short;
            _context.Devices.Add(_deviceInst);
            _context.SaveChanges();
            _deviceInst = _context.Devices.Where(dev => dev.Uuid == device.uuid).SingleOrDefault();
            _deviceInst.AreaDevices.Add(new AreaDevice() { AreaId = device.AreaId, DeviceId = device.Id });
            _context.Entry(_deviceInst).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDevice", new { id = device.Id }, device);
        }

        [HttpPost("CreateSensor")]
        public async Task<ActionResult<Device>> CreateSensor(SensorDTO sensor)
        {
            try
            {
                Device _deviceInst = new Device();
                _deviceInst.Uuid = Guid.NewGuid();
                _deviceInst.Name = sensor.Name;
                _deviceInst.Type = "Sensor";
                _deviceInst.Description = sensor.Description;
                _deviceInst.Status = true;
                _deviceInst.TelemetryData = sensor.telemetryData;
                _context.Devices.Add(_deviceInst);
                _context.SaveChanges();
                Device _device = _context.Devices.Where(d => d.Uuid == _deviceInst.Uuid).SingleOrDefault();
                _deviceInst = _context.Devices.Where(dev => dev.Uuid == _deviceInst.Uuid).SingleOrDefault();
                _deviceInst.AreaDevices.Add(new AreaDevice() { AreaId = sensor.AreaId, DeviceId = _deviceInst.Id });
                _context.Entry(_deviceInst).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction("GetSensor",new SensorDTO());
        }

        [HttpPost("DeviceTelemetry")]
        public async Task<ActionResult<TelemetryDatum>> DeviceTelemetry(DeviceTelemetry dev)
        {
            try
            {
                Device? _device = (_context.Devices.Where(d => d.Uuid == dev.UUID)).SingleOrDefault();
                if (_device != null)
                {
                    TelemetryDatum? _telemetryDataum = (_context.TelemetryData.Where(td => td.DeviceId == _device.Id & td.VariableName == dev.Name)).SingleOrDefault();
                    if (_telemetryDataum != null)
                    {
                        DataStream _dataStream = new DataStream() { Value = dev.value, TelemetryDataId = _telemetryDataum.Id };
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
            catch (Exception e)
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
            device.Status = false;
            _context.Entry(device).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceExists(long id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}
