using Luveck.Service.Administration.DTO.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IReportService
    {
        Task<string> GetReportGeneral(DateTime start, DateTime end);
        Task<List<PurchaseResponseDto>> GetPurchaseByUser(string userId, DateTime start, DateTime end);
    }
}
