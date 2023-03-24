using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaiabilityReportApi.Entities
{
    public class AvaiabilityReport
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string CurrentState { get; set; }
        public string? PreviousState { get; set; }

        [ForeignKey("MachineId")]
        public int MachineId { get; set; }
        public Machine Machine { get; set; }

    }
}
