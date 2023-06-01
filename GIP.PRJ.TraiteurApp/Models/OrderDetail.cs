
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Display(Name = "")]
        public int OrderId { get; set; }

        [Display(Name = "")]
        public int MenuItemId { get; set; }

        [Display(Name = "")]
        [Range(1, int.MaxValue, ErrorMessage = "Aantal moet groter zijn dan 0.")]
        public int Quantity { get; set; }

        [Display(Name = "")]
        [Range(0, double.MaxValue, ErrorMessage = "Prijs mag niet negatief zijn.")]
        public decimal Price { get; set; }

        [Display(Name = "")]
        public virtual Order Order { get; set; }

        [Display(Name = "")]
        public virtual MenuItem MenuItem { get; set; }
    }
}
