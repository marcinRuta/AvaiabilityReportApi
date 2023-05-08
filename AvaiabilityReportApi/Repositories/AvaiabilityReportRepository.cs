using AvaiabilityReportApi.Data;
using AvaiabilityReportApi.Dtos;
using AvaiabilityReportApi.Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;


namespace AvaiabilityReportApi.Repositories
{
    public class AvaiabilityReportRepository : IAvaiabilityReportRepository

    {

        private readonly GymAvaiabilityDbContext gymAvaiabilityDbContext;

        public AvaiabilityReportRepository(GymAvaiabilityDbContext gymAvaiabilityDbContext)
        {
            this.gymAvaiabilityDbContext = gymAvaiabilityDbContext;
        }

        public async Task<AvaiabilityReport> AddAvaiabilityReport(AvaiabilityReportDto avaiabilityReportDto)
        {

            var lastAvaiabilityReport = await  (from report in this.gymAvaiabilityDbContext.AvaiabilityReports where report.Machine == avaiabilityReportDto.Machine 
                                                orderby report.Timestamp descending
                                                select new AvaiabilityReport
                                                {
                                                    Machine = report.Machine,
                                                    Id = report.Id,
                                                    CurrentState= report.CurrentState,
                                                    PreviousState= report.PreviousState,
                                                    Timestamp= report.Timestamp,
                                                } ).FirstOrDefaultAsync();

            var newReport = new AvaiabilityReport
            {
                Id = avaiabilityReportDto.Id,
                CurrentState = avaiabilityReportDto.CurrentState,
                Timestamp = avaiabilityReportDto.Timestamp,
                MachineId = avaiabilityReportDto.Machine.Id
            };

            if (lastAvaiabilityReport != null) 
            {
                
                newReport.PreviousState = lastAvaiabilityReport.CurrentState;

            }

            var result = await this.gymAvaiabilityDbContext.AvaiabilityReports.AddAsync(newReport);
            await this.gymAvaiabilityDbContext.SaveChangesAsync();
            return result.Entity;

        }


        public async Task<Machine> GetMachine(string deviceEUI)
        {
            return await (from machine in this.gymAvaiabilityDbContext.Machines
                                         where machine.DeviceEUI == deviceEUI
                                         select new Machine
                                         {
                                             Id = machine.Id,
                                             DeviceEUI = machine.DeviceEUI,
                                             ImageFileLink = machine.ImageFileLink,
                                             Description = machine.Description,
                                             Name = machine.Name,
                                         }).SingleOrDefaultAsync();
            
        }
        public async Task<List<AvaiabilityReportFactSt>> LoadAvaiabilityFactSt()
        {
            var reportsFacts = await (from report in this.gymAvaiabilityDbContext.AvaiabilityReports
                          join machine in this.gymAvaiabilityDbContext.Machines
                            on report.MachineId equals machine.Id
                          join machinePlacement in this.gymAvaiabilityDbContext.MachinePlacements
                            on machine.Id equals machinePlacement.MachineId
                          where report.Timestamp >= (DateTime.Today.AddDays(-1))
                          select new AvaibilityReportFactDto
                          {
                              FactId = report.Id,
                              MachineId = machine.Id,
                              GymRoomId = machinePlacement.GymRoomId,
                              Timestamp = report.Timestamp.TimeOfDay,
                              Occupancy = report.CurrentState,
                              PrevOccupancy = report.PreviousState,
                              Date = report.Timestamp.Date,
                          }
                          ).ToListAsync();
            var dict = reportsFacts.GroupBy(o => o.MachineId)
                  .ToDictionary(g => g.Key, g => g.ToList());
            //loop over key and value in dictionary
            List<AvaiabilityReportFactSt> resultArray = new List<AvaiabilityReportFactSt>();
            foreach (KeyValuePair<int, List<AvaibilityReportFactDto>> item in dict)
            {
                var facts = item.Value;
                var lastTimestamp = TimeSpan.FromHours(6);
                // loop over list using indexes
                for (int i = 1; i < facts.Count; i++)
                {
                    var fact = facts[i];
                    foreach( var time in Intervals(lastTimestamp, fact.Timestamp, TimeSpan.FromMinutes(1)))
                    {
                        resultArray.Add(new AvaiabilityReportFactSt { FactId = fact.FactId, MachineId = fact.MachineId, Occupancy = fact.PrevOccupancy ?? "false", Timestamp = time, GymRoomId = fact.GymRoomId, Date = fact.Date });
                    }
                    lastTimestamp = new TimeSpan(fact.Timestamp.Hours,fact.Timestamp.Minutes, 0);

                    if (i == facts.Count - 1)
                    {
                        foreach (var time in Intervals(lastTimestamp, TimeSpan.FromHours(23), TimeSpan.FromMinutes(1)))
                        {
                            resultArray.Add(new AvaiabilityReportFactSt { FactId = fact.FactId, MachineId = fact.MachineId, Occupancy = fact.Occupancy ?? "false", Timestamp = time, GymRoomId = fact.GymRoomId, Date = fact.Date });
                        }
                    }
                    
                }



            }
            await this.gymAvaiabilityDbContext.AvaiabilityReportFactSt.AddRangeAsync(resultArray);
            await this.gymAvaiabilityDbContext.SaveChangesAsync();

            return resultArray;
        }
        public static IEnumerable<TimeSpan> Intervals(TimeSpan inclusiveStart, TimeSpan exclusiveEnd, TimeSpan increment)
        {
            for (var time = inclusiveStart; time < exclusiveEnd; time += increment)
                yield return time;
        }
    }
}
