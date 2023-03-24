using AvaiabilityReportApi.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaiabilityReportApi.Dtos
{
    public class AvaiabilityReportDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string CurrentState { get; set; }
        public string? PreviousState { get; set; }

        [ForeignKey("MachineId")]
        public Machine? Machine { get; set; }

    }
}
