using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.DTO;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly TraiteurAppDbContext _context;

        public InvoiceService(TraiteurAppDbContext context)
        {
            _context = context;
        }
        public async Task<InvoiceTotals> CalculateTotals(int orderId, int reduction)
        {
            try
            {
                var details = await _context.OrderDetails.Where(od => od.OrderId == orderId)
                    .Include(od => od.MenuItem).ThenInclude(mi => mi.MenuItemCategory).ToListAsync();

                var incl = Math.Round(details.Sum(d => (d.Quantity * d.Price)), 2);
                var red = Math.Round(incl * reduction / 100, 2);

                var excl = Math.Round(details.Sum(d => (d.Quantity * ((d.Price * (1 - reduction / 100m)) / (d.MenuItem.MenuItemCategory.VAT / 100m + 1)))), 2);

                var vat = Math.Round(details.Sum(d =>
                                                    (d.Quantity * ((d.Price * (1 - reduction / 100m)))) - 
                                                    (d.Quantity * ((d.Price * (1 - reduction / 100m)) / (d.MenuItem.MenuItemCategory.VAT / 100m + 1)))
                                                ), 2);

                return new InvoiceTotals
                {
                    TotalInclVAT = incl,
                    TotalVAT = vat,
                    TotalExclVAT = excl,
                    TotalReduction = red,
                    ToPay = incl - red
                };
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"OrderDetailService > CalculateVAT: " +
                    $"An error occurred while calculate the VAT for order id {orderId}", ex);
            }
        }
    }
}
