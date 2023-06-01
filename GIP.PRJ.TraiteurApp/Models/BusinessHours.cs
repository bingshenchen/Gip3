using System.ComponentModel.DataAnnotations;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class BusinessHours
    {
        public int Id { get; set; }

        [Display(Name = "Day of Week")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Opening Time")]
        public TimeSpan OpeningTime { get; set; }

        [Display(Name = "Closing Time")]
        public TimeSpan ClosingTime { get; set; }

        [Display(Name = "Holidays")]
        public DateTime Holidays { get; set; }

        [Display(Name = "Is Closed")]
        public bool IsClosed { get; set; }
    }
}