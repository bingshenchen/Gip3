using System.ComponentModel.DataAnnotations;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class BusinessHours
    {
        [Display(Name = "")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Sluitingsdagen")]
        public string ClosingDays { get; set; }

        [Display(Name = "")]
        public TimeSpan OpeningTime { get; set; }

        [Display(Name = "")]
        public TimeSpan ClosingTime { get; set; }

        [Display(Name = "")]
        public ICollection<DateTime> Holidays { get; set; }

        [Display(Name = "")]
        public bool IsClosed { get; set; }
    }
}
