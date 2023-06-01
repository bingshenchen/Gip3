using System.ComponentModel.DataAnnotations;
namespace GIP.PRJ.TraiteurApp.Models
{
    public class MenuItemCategory
    {
        public int Id { get; set; }

        [Display(Name = "")]
        public string Name { get; set; }

        [Display(Name = "")]
        public decimal VAT { get; set; }

        [Display(Name = "")]
        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
