using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReportService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PurchaseResponseDto>> GetPurchaseByUser(string userId, DateTime start, DateTime end)
        {
            if(end != null && start != null)
            {
                bool range = diffDate(start, end);
                if (!range) throw new BusinessException("La busqueda super el rango de 6 meses.");
            }
            if (end == null) end = DateTime.Now;            
            if (start == null) start = end.AddMonths(-6);
            
            var report = _unitOfWork.PurchaseRepository.FindAll(x=> x.userId.ToLower().Equals(userId.ToLower())
            && x.DateShiped >= start && x.DateShiped <= end).Select(y=> new PurchaseResponseDto() { 
                DateShiped = y.DateShiped,
                NoPurchase = y.NoPurchase
            }).ToList();
            //return await (from purchase in _unitOfWork.PurchaseRepository.AsQueryable()
            //              select new PurchaseResponseDto()
            //              {
            //                  DateShiped = purchase.DateShiped,
            //                  NoPurchase = purchase.NoPurchase,                              
            //              } ).ToListAsync();

            return report;
        }

        public async Task<string> GetReportGeneral(DateTime start, DateTime end)
        {
            //var report = await (from);
            throw new NotImplementedException();
        }

        private bool diffDate(DateTime start, DateTime end)
        {
            TimeSpan ts = end.Subtract(start);
            if (ts.TotalDays > 180) return false;
            return true;
        }
    }
}
