using GIP.PRJ.TraiteurApp.Services.DTO;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceTotals> CalculateTotals(int orderId, int reduction);
    }
}
