using System;

namespace AEMOSAPI.DTO
{
    public class TelemetryDTO
    {
        public long Id { set; get; }
        public Guid? Uuid { set; get; }
        public string? SensorName { set; get; }
        public string? Variable { set; get; }
        public string? Data { set; get; }
        public string? OrgId { set; get; }
        public string? OrgName { set; get; }
    }
}
