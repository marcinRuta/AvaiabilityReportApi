using AvaiabilityReportApi.Dtos;
using AvaiabilityReportApi.Entities;

namespace AvaiabilityReportApi.Repositories
{
    public interface IAvaiabilityReportRepository
    {

        Task<Machine> GetMachine(string deviceEUI);
        Task<AvaiabilityReport> AddAvaiabilityReport(AvaiabilityReportDto avaiabilityReportDto);
    }
}
