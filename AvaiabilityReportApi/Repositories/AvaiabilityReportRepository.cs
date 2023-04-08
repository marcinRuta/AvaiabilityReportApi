using AvaiabilityReportApi.Data;
using AvaiabilityReportApi.Dtos;
using AvaiabilityReportApi.Entities;
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
    }
}
