namespace GIP.PRJ.TraiteurApp.Services.DTO
{
    public class InvoiceTotals
    {
        public decimal TotalInclVAT { get; set; }
        public decimal TotalReduction { get; set; }
        public decimal TotalExclVAT { get; set; }
        public decimal TotalVAT { get; set; }
        public decimal ToPay { get; set; }
    }
}
