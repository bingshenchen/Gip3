using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "")]
        public DateTime OrderedOn { get; set; }

        [Display(Name = "")]
        public string TimeSlot { get; set; }

        [Display(Name = "")]
        public int CustomerId { get; set; }

        [Display(Name = "")]
        public decimal Total { get; set; }

        [Display(Name = "")]
        public int Reduction { get; set; }

        [Display(Name = "")]
        public bool IsPaid { get; set; }
        /// <summary>
        /// int? omdat een bestelling niet onmiddellijk aan een kok wordt toegewezen
        /// </summary>
        /// 
        [Display(Name = "")]
        public int? CookId { get; set; }

        [Display(Name = "")]
        public OrderStatus Status { get; set; }

        [Display(Name = "")]
        public virtual Cook Cook { get; set; }

        [Display(Name = "")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [Display(Name = "")]
        public bool ReminderSent { get; set; }

    }

}
