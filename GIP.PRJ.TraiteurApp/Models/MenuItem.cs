namespace GIP.PRJ.TraiteurApp.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int MenuItemCategoryId { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual MenuItemCategory MenuItemCategory { get; set; }
    }
}
