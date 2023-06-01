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

        [Display(Name = "Besteld op")]
        public DateTime OrderedOn { get; set; }

        [Display(Name = "Tijdsslot")]
        public string TimeSlot { get; set; }

        [Display(Name = "KlantId")]
        public int CustomerId { get; set; }

        [Display(Name = "Totaal")]
        public decimal Total { get; set; }

        [Display(Name = "Korting")]
        public int Reduction { get; set; }

        [Display(Name = "Is betaald")]
        public bool IsPaid { get; set; }
        /// <summary>
        /// int? omdat een bestelling niet onmiddellijk aan een kok wordt toegewezen
        /// </summary>
        /// 
        [Display(Name = "KokId")]
        public int? CookId { get; set; }

        [Display(Name = "Status")]
        public OrderStatus Status { get; set; }

        [Display(Name = "Kok")]
        public virtual Cook Cook { get; set; }

        [Display(Name = "Klant")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "Order details")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [Display(Name = "Herinnering")]
        public bool ReminderSent { get; set; }

    }

}
