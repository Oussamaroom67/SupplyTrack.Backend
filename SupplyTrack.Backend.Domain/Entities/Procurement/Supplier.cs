using SupplyTrack.Backend.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SupplyTrack.Backend.Domain.Entities.Procurement
{
    internal class Supplier
    {
        #region Properties
        public int Id { get; private set; }
        public string Name { get; private set; }
        public ContactInfo ContactInfo { get; private set; }
        public bool Approved { get; private set; }
        public List<SupplierProduct> Products { get; private set; } = new();
        #endregion

        #region Constructors
        private Supplier() { }

        public Supplier(string name, string email, string phoneNumber, string address, bool approved = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Le nom du fournisseur ne peut pas être vide.", nameof(name));
            }
            ContactInfo = new ContactInfo(email, phoneNumber, address);
            Name = name;
            Approved = approved;
        }

        public Supplier(int id, string name, string email, string phoneNumber, string address, bool approved = false)
            : this(name, email, phoneNumber, address, approved)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'identifiant du fournisseur doit être supérieur à zéro.", nameof(id));
            }
            Id = id;
        }
        #endregion

        #region methods
        public void Approve()
        {
            Approved = true;
        }

        public void Disapprove()
        {
            Approved = false;
        }

        public void UpdateContactInfo(string email, string phoneNumber, string address)
        {
            ContactInfo = new ContactInfo(email, phoneNumber, address);
        }

        public void AddProduct(int productId, decimal unitPrice, string referenceCode, int minimumOrderQty = 1, int leadTimeDays = 0)
        {
            var existingProduct = Products.FirstOrDefault(p => p.ProductId == productId);
            if (existingProduct != null)
            {
                throw new InvalidOperationException("Le produit est déjà associé à ce fournisseur.");
            }

            var supplierProduct = new SupplierProduct(this.Id, productId, unitPrice, referenceCode, minimumOrderQty, leadTimeDays);
            Products.Add(supplierProduct);
        }
        #endregion
    }
}