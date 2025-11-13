using SupplyTrack.Backend.Domain.Events.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Domain.Events.Procurement
{
    internal class PurchaseOrderCanceledEvent(int purchaseOrderId, int supplierId) : IDomainEvent
    {
        public int PurchaseOrderId { get; } = purchaseOrderId;
        public int SupplierId { get; } = supplierId;
        public DateTime CancelledAt { get; } = DateTime.UtcNow;
    }
}
