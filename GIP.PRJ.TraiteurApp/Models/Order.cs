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

        [Display(Name = "Tijdslot")]
        public string TimeSlot { get; set; }

        [Display(Name = "Klant Id")]
        public int CustomerId { get; set; }

        [Display(Name = "Totaal")]
        public decimal Total { get; set; }

        [Display(Name = "Korting")]
        public int Reduction { get; set; }

        [Display(Name = "Betaald")]
        public bool IsPaid { get; set; }
        /// <summary>
        /// int? omdat een bestelling niet onmiddellijk aan een kok wordt toegewezen
        /// </summary>
        /// 
        [Display(Name = "Kok Id")]
        public int? CookId { get; set; }

        [Display(Name = "Herinnering Verzonden")]
        public bool ReminderSent { get; set; }
        public OrderStatus Status { get; set; }
        public virtual Cook Cook { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        

    }

}
