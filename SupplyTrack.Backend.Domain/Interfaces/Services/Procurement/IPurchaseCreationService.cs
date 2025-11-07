using SupplyTrack.Backend.Domain.Aggregates;
using SupplyTrack.Backend.Domain.Enums.Procurement;
using SupplyTrack.Backend.Shared.Models;
namespace SupplyTrack.Backend.Domain.Interfaces.Services.Procurement
{
    public interface IPurchaseCreationService
    {
        public Result<PurchaseOrder> CreatePurchaseOrder(int supplierId,
            int employeeId,
            int companyId,
            DateTime? expectedDeliveryDate = null
            );
    }
}
