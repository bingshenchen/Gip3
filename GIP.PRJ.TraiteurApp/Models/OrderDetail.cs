
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Aantal moet groter zijn dan 0.")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Prijs mag niet negatief zijn.")]
        public decimal Price { get; set; }
        public virtual Order Order { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
