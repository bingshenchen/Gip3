namespace GIP.PRJ.TraiteurApp.Models
{
    public class MenuItemCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal VAT { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
