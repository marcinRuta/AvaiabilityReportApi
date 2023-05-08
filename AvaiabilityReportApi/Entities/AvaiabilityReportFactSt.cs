using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaiabilityReportApi.Entities
{
    
    public class AvaiabilityReportFactSt
    {
        [Key]
        public int Id { get; set; }
        public TimeSpan Timestamp { get; set; }
        public int FactId { get; set; }
        public string Occupancy { get; set; }
        public DateTime Date { get; set; }
        public int GymRoomId { get; set; }
        public int MachineId { get; set; }


    }
}
