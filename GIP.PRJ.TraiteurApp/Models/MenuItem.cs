using System.ComponentModel.DataAnnotations;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Display(Name = "")]
        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Name { get; set; }

        [Display(Name = "")]
        [Required(ErrorMessage = "Omschrijving is verplicht.")]
        public string Description { get; set; }

        [Display(Name = "")]
        [Range(0, double.MaxValue, ErrorMessage = "Prijs mag niet negatief zijn.")]
        public decimal Price { get; set; }

        [Display(Name = "")]
        public int MenuItemCategoryId { get; set; }

        [Display(Name = "")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [Display(Name = "")]
        public virtual MenuItemCategory MenuItemCategory { get; set; }
    }
}
