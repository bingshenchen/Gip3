using System.ComponentModel.DataAnnotations;
namespace GIP.PRJ.TraiteurApp.Models
{
    public class MenuItemCategory
    {
        public int Id { get; set; }

        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Display(Name = "BTW")]
        public decimal VAT { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
