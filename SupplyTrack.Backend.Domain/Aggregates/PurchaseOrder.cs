using SupplyTrack.Backend.Domain.Enums.Procurement;
using SupplyTrack.Backend.Domain.Entities.Procurement;
namespace SupplyTrack.Backend.Domain.Aggregates
{
    internal class PurchaseOrder
    {
        #region properties
        public int Id { get;private set; }
        public int SupplierId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public DateTime? ExpectedDeliveryDate { get; private set; }
        public PurchaseOrderStatus Status { get; private set; }
        //The employee who created the order
        public int EmployeeId { get; private set; }
        public List<PurchaseLine> PurchaseLines { get; private set; }=new();
        public decimal TotalAmount => PurchaseLines.Sum(line => line.TotalPrice);

        #endregion
        #region Constructors
        private PurchaseOrder() { }
        public PurchaseOrder(int supplierId, DateTime orderDate, int employeeId, DateTime? expectedDeliveryDate = null)
        {
            if (supplierId <= 0)
            {
                throw new ArgumentException("L'identifiant du fournisseur doit être supérieur à zéro.", nameof(supplierId));
            }
            if (employeeId <= 0)
            {
                throw new ArgumentException("L'identifiant de l'employé doit être supérieur à zéro.", nameof(employeeId));
            }
            SupplierId = supplierId;
            OrderDate = orderDate;
            EmployeeId = employeeId;
            ExpectedDeliveryDate = expectedDeliveryDate;
            Status = PurchaseOrderStatus.Draft;
        }
        public PurchaseOrder(int id, int supplierId, DateTime orderDate, int employeeId, DateTime? expectedDeliveryDate = null)
            : this(supplierId, orderDate, employeeId, expectedDeliveryDate)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'identifiant de la commande doit être supérieur à zéro.", nameof(id));
            }
            Id = id;
        }
        #endregion
        #region methods
        public void AddPurchaseLine(PurchaseLine line)
        {
            if (line == null)
            {
                throw new ArgumentNullException(nameof(line), "La ligne d'achat ne peut pas être nulle.");
            }
            PurchaseLines.Add(line);
        }
        public void ValidateOrder()
        {
            this.Status = PurchaseOrderStatus.Validated;
        }
        public void CancelOrder()
        {
            this.Status = PurchaseOrderStatus.Canceled;
        }
        public void ReceivePartialDelivery(int LineId,int quatityReceived)
        {
            var line=PurchaseLines.FirstOrDefault(l => l.Id == LineId);
            if (line == null)
            {
                throw new ArgumentException("La ligne d'achat spécifiée n'existe pas dans cette commande.", nameof(LineId));
            }
            if (quatityReceived <= 0 || quatityReceived > line.QuantityOrdered)
            {
                throw new ArgumentException("La quantité reçue doit être supérieure à zéro et ne peut pas dépasser la quantité commandée.", nameof(quatityReceived));
            }
            line.UpdateQuantityReceived(quatityReceived);
        }



        #endregion
    }
}
