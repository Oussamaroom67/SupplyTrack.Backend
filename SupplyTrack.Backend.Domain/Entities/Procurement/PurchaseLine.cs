using System;

namespace SupplyTrack.Backend.Domain.Entities.Procurement
{
    public class PurchaseLine
    {
        #region Properties
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int QuantityOrdered { get; private set; }
        public int QuantityReceived { get; private set; }
        public decimal UnitPrice { get; private set; }

        public decimal TotalPrice => QuantityReceived * UnitPrice;
        #endregion

        #region Constructors
        private PurchaseLine() { }

        public PurchaseLine(int productId, int quantityOrdered,int quantityReceived, decimal unitPrice)
        {
            ValidateProductId(productId);
            ValidateQuantity(quantityOrdered);
            ValidateUnitPrice(unitPrice);

            ProductId = productId;
            UnitPrice = unitPrice;
            QuantityOrdered = quantityOrdered;
            QuantityReceived = quantityReceived;
        }

        public PurchaseLine(int id, int productId, int quantityOrdered,int quantityReceived, decimal unitPrice)
            : this(productId, quantityOrdered,quantityReceived, unitPrice)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'identifiant de la ligne d'achat doit être supérieur à zéro.", nameof(id));
            }
            Id = id;
        }
        #endregion

        #region Validation Methods
        private void ValidateProductId(int productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("L'identifiant du produit doit être supérieur à zéro.", nameof(productId));
            }
        }

        private void ValidateQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("La quantité doit être supérieure à zéro.", nameof(quantity));
            }
        }

        private void ValidateUnitPrice(decimal unitPrice)
        {
            if (unitPrice < 0)
            {
                throw new ArgumentException("Le prix unitaire ne peut pas être négatif.", nameof(unitPrice));
            }
        }
        #endregion

        #region Business Methods
        public void UpdateQuatityOrdred(int quantityOrdered)
        {
            ValidateQuantity(quantityOrdered);
            QuantityOrdered = quantityOrdered;
        }
        public void UpdateQuantityReceived(int quantityReceived)
        {
            ValidateQuantity(quantityReceived);
            QuantityReceived = quantityReceived;
        }

        public void UpdateUnitPrice(decimal newUnitPrice)
        {
            ValidateUnitPrice(newUnitPrice);
            UnitPrice = newUnitPrice;
        }
        #endregion
    }
}