namespace AvaiabilityReportApi.Entities
{
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageFileLink { get; set; }
        public string? Description { get; set; }
        public string? DeviceEUI { get; set; }
    }
}
 