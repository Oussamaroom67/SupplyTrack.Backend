using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Domain.Entities.Procurement
{
    internal class SupplierProduct
    {
        #region properties
        public int Id { get; private set; }
        public int SupplierId { get; private set; }
        public int ProductId { get; private set; }
        public decimal UnitPrice { get; private set; }
        public string ReferenceCode { get; private set; }
        public int MinimumOrderQty { get; private set; }
        public int LeadTimeDays { get; private set; }
        #endregion
        #region constructors
        public SupplierProduct(int supplierId, int productId, decimal unitPrice, string referenceCode, int minimumOrderQty = 1, int leadTimeDays = 0)
        {
            if (unitPrice < 0)
            {
                throw new ArgumentException("Le prix unitaire ne peut pas etre negatif.", nameof(unitPrice));
            }
            if (string.IsNullOrWhiteSpace(referenceCode))
            {
                throw new ArgumentException("Le code de reference est requis.", nameof(referenceCode));
            }
            if (leadTimeDays < 0)
            {
                throw new ArgumentException("Le delai de livraison ne peut pas etre negatif.", nameof(leadTimeDays));
            }
            SupplierId = supplierId;
            ProductId = productId;
            UnitPrice = unitPrice;
            ReferenceCode = referenceCode;
            MinimumOrderQty = minimumOrderQty;
            LeadTimeDays = leadTimeDays;
        }
        public SupplierProduct(int id, int supplierId, int productId, decimal unitPrice, string referenceCode, int minimumOrderQty = 1, int leadTimeDays = 0)
            : this(supplierId, productId, unitPrice, referenceCode, minimumOrderQty, leadTimeDays)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'identifiant du produit fournisseur doit etre superieur a zero.", nameof(id));
            }
            Id = id;
        }
        #endregion

        public void UpdatePrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException("Le prix unitaire ne peut pas etre negatif.", nameof(price));
            }
            this.UnitPrice = price;

        }
    }
}
