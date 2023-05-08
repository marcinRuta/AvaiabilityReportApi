using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaiabilityReportApi.Entities
{
    public class MachinePlacement
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int MachineId { get; set; }
        public int GymRoomId { get; set; }

    }
}
