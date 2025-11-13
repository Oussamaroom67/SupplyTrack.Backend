using SupplyTrack.Backend.Domain.Events.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Domain.Events.Procurement
{
    internal class PurchaseOrderFullyReceivedEvent : IDomainEvent
    {
        public int PurchaseOrderId { get; }
        public int SupplierId { get; }
        public decimal TotalAmount { get; }
        public DateTime? CompletedAt { get; }
        public PurchaseOrderFullyReceivedEvent(int purchaseOrderId, int supplierId, decimal totalAmount, DateTime? completedAt)
        {
            PurchaseOrderId = purchaseOrderId;
            SupplierId = supplierId;
            TotalAmount = totalAmount;
            CompletedAt = completedAt;
        }
    }
}
