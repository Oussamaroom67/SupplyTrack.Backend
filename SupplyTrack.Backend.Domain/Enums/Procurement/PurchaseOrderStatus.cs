using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Domain.Enums.Procurement
{
    internal enum PurchaseOrderStatus
    {
        Draft,
        Validated,
        Received,
        Canceled,
        Closed
    }
}
