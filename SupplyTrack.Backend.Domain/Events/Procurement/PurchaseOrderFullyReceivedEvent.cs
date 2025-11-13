using SupplyTrack.Backend.Domain.Events.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Domain.Events.Procurement
{
    internal class PurchaseOrderFullyReceivedEvent(int purchaseOrderId, int supplierId, decimal totalAmount, DateTime? completedAt) : IDomainEvent
    {
        public int PurchaseOrderId { get; } = purchaseOrderId;
        public int SupplierId { get; } = supplierId;
        public decimal TotalAmount { get; } = totalAmount;
        public DateTime? CompletedAt { get; } = completedAt;
    }
}
