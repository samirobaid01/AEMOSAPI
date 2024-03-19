using System.Collections;

namespace AEMOSAPI.DTO
{
    public class DeviceDTO
    {
        public DeviceDTO()
        {
            Telemetrydate = new List<string>();
        }
        public long Id { set; get; }
        public string? Name { set; get; }
        public string? Area { set; get; }
        public Guid? uuid { set; get; }
        public string? Description { set; get; }
        public long AreaId { set; get; }

        public string? State { set; get; }
        public IList<string> Telemetrydate { set; get; }
    }
}
