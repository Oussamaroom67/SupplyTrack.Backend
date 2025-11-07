using SupplyTrack.Backend.Domain.Enums.Procurement;
using SupplyTrack.Backend.Domain.Entities.Procurement;

namespace SupplyTrack.Backend.Domain.Aggregates
{
    public class PurchaseOrder
    {
        #region Properties
        public int Id { get; private set; }
        public int SupplierId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public DateTime? ExpectedDeliveryDate { get; private set; }
        public PurchaseOrderStatus Status { get; private set; }
        public int CompanyId { get; private set; }

        /// <summary>
        /// ID de l'employé qui a créé la commande.
        /// 0 = Système (pour les commandes automatiques)
        /// </summary>
        public int EmployeeId { get; private set; }

        public List<PurchaseLine> PurchaseLines { get; private set; } = new();
        public decimal TotalAmount => PurchaseLines.Sum(line => line.TotalPrice);

        #endregion

        #region Constructors
        // Constructeur pour EF Core
        private PurchaseOrder() { }

        /// <summary>
        /// Crée une nouvelle commande d'achat
        /// </summary>
        public PurchaseOrder(
            int supplierId,
            DateTime orderDate,
            int employeeId,
            int companyId,
            DateTime? expectedDeliveryDate = null
           )
        {
            // Validation commune
            if (supplierId <= 0)
            {
                throw new ArgumentException("L'identifiant du fournisseur doit être supérieur à zéro.", nameof(supplierId));
            }

            // Validation selon le mode
            if ( employeeId <= 0)
            {
                throw new ArgumentException(
                    "Une commande manuelle doit être créée par un employé valide (EmployeeId > 0).",
                    nameof(employeeId));
            }

            SupplierId = supplierId;
            OrderDate = orderDate;
            EmployeeId = employeeId;
            ExpectedDeliveryDate = expectedDeliveryDate;
            Status = PurchaseOrderStatus.Draft;
            CompanyId = companyId;
        }

        /// <summary>
        /// Constructeur pour reconstruire une commande existante (depuis la DB)
        /// </summary>
        public PurchaseOrder(
            int id,
            int supplierId,
            DateTime orderDate,
            int employeeId,
            int companyId,
            DateTime? expectedDeliveryDate = null
            )
            : this(supplierId, orderDate, employeeId,companyId,expectedDeliveryDate)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'identifiant de la commande doit être supérieur à zéro.", nameof(id));
            }
            Id = id;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Ajoute une ligne à la commande
        /// </summary>
        public void AddPurchaseLine(PurchaseLine line)
        {
            if (line == null)
            {
                throw new ArgumentNullException(nameof(line), "La ligne d'achat ne peut pas être nulle.");
            }

            if (Status != PurchaseOrderStatus.Draft)
            {
                throw new InvalidOperationException(
                    "Impossible d'ajouter des lignes à une commande qui n'est plus en brouillon.");
            }

            PurchaseLines.Add(line);
        }

        /// <summary>
        /// Valide la commande
        /// </summary>
        public void ValidateOrder()
        {
            if (Status != PurchaseOrderStatus.Draft)
            {
                throw new InvalidOperationException(
                    $"Impossible de valider une commande avec le statut {Status}.");
            }

            if (PurchaseLines.Count == 0)
            {
                throw new InvalidOperationException(
                    "Impossible de valider une commande sans lignes d'achat.");
            }

            Status = PurchaseOrderStatus.Validated;
        }

        /// <summary>
        /// Annule la commande
        /// </summary>
        public void CancelOrder()
        {
            if (Status == PurchaseOrderStatus.Closed)
            {
                throw new InvalidOperationException(
                    "Impossible d'annuler une commande déjà complétée.");
            }

            if (Status == PurchaseOrderStatus.Canceled)
            {
                throw new InvalidOperationException("La commande est déjà annulée.");
            }

            Status = PurchaseOrderStatus.Canceled;
        }

        /// <summary>
        /// Enregistre une livraison partielle
        /// </summary>
        public void ReceivePartialDelivery(int lineId, int quantityReceived)
        {
            if (Status != PurchaseOrderStatus.Validated)
            {
                throw new InvalidOperationException(
                    "Impossible de recevoir une livraison pour une commande non validée.");
            }

            var line = PurchaseLines.FirstOrDefault(l => l.Id == lineId) ?? throw new ArgumentException(
                    "La ligne d'achat spécifiée n'existe pas dans cette commande.",
                    nameof(lineId));
            if (quantityReceived <= 0)
            {
                throw new ArgumentException(
                    "La quantité reçue doit être supérieure à zéro.",
                    nameof(quantityReceived));
            }

            if (quantityReceived > line.QuantityOrdered - line.QuantityReceived)
            {
                throw new ArgumentException(
                    $"La quantité reçue ({quantityReceived}) dépasse la quantité restante à recevoir ({line.QuantityOrdered - line.QuantityReceived}).",
                    nameof(quantityReceived));
            }

            line.UpdateQuantityReceived(quantityReceived);

            // Vérifier si toutes les lignes sont complètement reçues
            if (IsFullyReceived(lineId))
            {
                Status = PurchaseOrderStatus.Closed;
            }
        }


        /// <summary>
        /// Vérifie si la commande peut être modifiée
        /// </summary>
        public bool CanBeModified()
        {
            return Status == PurchaseOrderStatus.Draft;
        }

        /// <summary>
        /// Vérifie si la commande peut être validée
        /// </summary>
        public bool CanBeValidated()
        {
            return Status == PurchaseOrderStatus.Draft && PurchaseLines.Count != 0;
        }

        /// <summary>
        /// Vérifie si la commande peut être annulée
        /// </summary>
        public bool CanBeCanceled()
        {
            return Status != PurchaseOrderStatus.Closed && Status != PurchaseOrderStatus.Canceled;
        }

        /// <summary>
        /// Vérifie si une ligne est partiellement reçue (quantité reçue > 0 mais < quantité commandée)
        /// </summary>
        /// <param name="lineId">ID de la ligne</param>
        /// <returns>true si partiellement reçue, false sinon</returns>
        public bool IsLinePartiallyReceived(int lineId)
        {
            var line = PurchaseLines.FirstOrDefault(l => l.Id == lineId)
                       ?? throw new ArgumentException($"Aucune ligne trouvée avec l'ID {lineId}", nameof(lineId));

            return line.QuantityReceived > 0 && line.QuantityReceived < line.QuantityOrdered;
        }

        /// <summary>
        /// Vérifie si une ligne est entièrement reçue (quantité reçue = quantité commandée)
        /// </summary>
        /// <param name="lineId">ID de la ligne</param>
        /// <returns>true si entièrement reçue, false sinon</returns>
        public bool IsLineFullyReceived(int lineId)
        {
            var line = PurchaseLines.FirstOrDefault(l => l.Id == lineId)
                       ?? throw new ArgumentException($"Aucune ligne trouvée avec l'ID {lineId}", nameof(lineId));

            return line.QuantityReceived == line.QuantityOrdered;
        }

        /// <summary>
        /// Vérifie si toute la commande est entièrement reçue (toutes les lignes sont complètes)
        /// </summary>
        /// <returns>true si toutes les lignes sont entièrement reçues, false sinon</returns>
        public bool IsFullyReceived()
        {
            return PurchaseLines.All(line => line.QuantityReceived == line.QuantityOrdered);
        }

    
        #endregion
    }
}