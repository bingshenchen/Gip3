namespace GIP.PRJ.TraiteurApp.Models
{
    public enum OrderStatus
    { 
        ToBeAssigned,
        Assigned,
        Started,
        Finished
    }
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderedOn { get; set; }
        public string TimeSlot { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
        public int Reduction { get; set; }
        public bool IsPaid { get; set; }
        /// <summary>
        /// int? omdat een bestelling niet onmiddellijk aan een kok wordt toegewezen
        /// </summary>
        public int? CookId { get; set; }
        public OrderStatus Status { get; set; }

        public virtual Cook Cook { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
