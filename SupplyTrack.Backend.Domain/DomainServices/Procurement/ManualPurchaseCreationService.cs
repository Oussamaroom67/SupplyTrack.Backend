using SupplyTrack.Backend.Domain.Aggregates;
using SupplyTrack.Backend.Domain.Interfaces.Services.Procurement;
using SupplyTrack.Backend.Shared.Models;

public class ManualPurchaseCreationService : IPurchaseCreationService
{
    public Result<PurchaseOrder> CreatePurchaseOrder(
        int supplierId,
        int employeeId,
        int companyId,
        DateTime? expectedDeliveryDate = null)
    {
        // Validation métier spécifique aux commandes manuelles
        if (expectedDeliveryDate == null)
        {
            return Result<PurchaseOrder>.Failure(
                "La date de livraison prévue est obligatoire pour une commande manuelle.");
        }

        try
        {
            var order = new PurchaseOrder(
                supplierId,
                DateTime.UtcNow,
                employeeId,
                companyId,
                expectedDeliveryDate);

            return Result<PurchaseOrder>.Success(order);
        }
        catch (ArgumentException ex)
        {
            return Result<PurchaseOrder>.Failure(ex.Message);
        }
    }
}