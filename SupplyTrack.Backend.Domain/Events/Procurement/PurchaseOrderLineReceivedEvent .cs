using SupplyTrack.Backend.Domain.Events.Common;

namespace SupplyTrack.Backend.Domain.Events.Procurement
{
    public class PurchaseOrderLineReceivedEvent(int purchaseOrderId, int purchaseOrderLineId, int receivedQuantity, DateTime receivedAt) : IDomainEvent
    {
        public int PurchaseOrderId { get; } = purchaseOrderId;
        public int PurchaseOrderLineId { get; } = purchaseOrderLineId;
        public int ReceivedQuantity { get; } = receivedQuantity;
        public DateTime ReceivedAt { get; } = receivedAt;
    }
}
