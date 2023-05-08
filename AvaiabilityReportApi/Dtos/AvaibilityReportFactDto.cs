
namespace AvaiabilityReportApi.Dtos
{
    public class AvaibilityReportFactDto
    {
        public TimeSpan Timestamp { get; set; }
        public int FactId { get; set; }
        public string Occupancy { get; set; }
        public string PrevOccupancy { get; set; }
        public DateTime Date { get; set; }
        public int GymRoomId { get; set; }
        public int MachineId { get; set; }
    }
}
