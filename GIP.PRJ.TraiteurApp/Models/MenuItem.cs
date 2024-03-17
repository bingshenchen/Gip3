using System.ComponentModel.DataAnnotations;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Display(Name = "Naam")]
        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Name { get; set; }

        [Display(Name = "Omschrijving")]
        [Required(ErrorMessage = "Omschrijving is verplicht.")]
        public string Description { get; set; }

        [Display(Name = "Prijs")]
        [Range(0, double.MaxValue, ErrorMessage = "Prijs mag niet negatief zijn.")]
        public decimal Price { get; set; }

        [Display(Name = "MenuItemCategorieId")]
        public int MenuItemCategoryId { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual MenuItemCategory MenuItemCategory { get; set; }
    }
}
