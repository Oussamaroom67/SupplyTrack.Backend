using SupplyTrack.Backend.Domain.Events.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Domain.Events.Procurement
{
    /// <summary>
    /// indique que la commande vient d’être initiée, mais pas encore validée (draft)
    /// </summary>
    internal class PurchaseOrderInitializedEvent(int purchaseOrderId, int supplierId):IDomainEvent
    {
        public int PurchaseOrderId { get; } = purchaseOrderId;
        public int SupplierId { get; } = supplierId;
        public DateTime CreatedTime { get; } = DateTime.UtcNow;
    }
}
