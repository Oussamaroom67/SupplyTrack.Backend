using SupplyTrack.Backend.Domain.Aggregates;
using SupplyTrack.Backend.Domain.Interfaces.Services.Procurement;
using SupplyTrack.Backend.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Domain.DomainServices.Procurement
{
    internal class AutoPurchaseCreationService : IPurchaseCreationService
    {
        public Result<PurchaseOrder> CreatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            throw new NotImplementedException();
        }
    }
}
