using AEMOSAPI.Models;

namespace AEMOSAPI.DTO
{
    public class SensorDTO
    {
        public long Id { set; get; }
        public string? Name { set; get; }
        public string? Description { set; get; }
        public string? Area { set; get; }
        public Guid? uuid { set; get; }
        public string? Protocol { set; get; }
        public long AreaId { set; get; }
        public IList<TelemetryDatum>? telemetryData { set; get; }

    }
}
