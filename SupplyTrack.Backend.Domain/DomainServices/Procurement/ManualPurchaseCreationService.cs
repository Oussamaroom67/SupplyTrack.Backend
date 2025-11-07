using SupplyTrack.Backend.Domain.Aggregates;
using SupplyTrack.Backend.Domain.Enums.Procurement;
using SupplyTrack.Backend.Domain.Interfaces.Services.Procurement;
using SupplyTrack.Backend.Shared.Models;

namespace SupplyTrack.Backend.Domain.DomainServices.Procurement
{
    public class ManualPurchaseCreationService : IPurchaseCreationService
    {

        public Result<PurchaseOrder> CreatePurchaseOrder(int supplierId, int employeeId, DateTime? expectedDeliveryDate = null)
        {
            if (expectedDeliveryDate == null)
            {
                return Result<PurchaseOrder>.Failure("GERE");
            }
            if (supplierId <= 0)
            {
                return Result<PurchaseOrder>.Failure("er");
            }
            if (employeeId <= 0)
            {
                return Result<PurchaseOrder>.Failure("hh");
            }
            PurchaseOrder order =new(supplierId, DateTime.Now,employeeId,expectedDeliveryDate);


            return Result<PurchaseOrder>.Failure("");
        }
    }
}
