using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class BusinessHours
    {
        public int Id { get; set; }

        [Display(Name = "Day of Week")]
        public int Id { get; set; }
        [Display(Name = "")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Opening Time")]
        [Display(Name = "Sluitingsdagen")]
        public string ClosingDays { get; set; }

        [Display(Name = "")]
        public TimeSpan OpeningTime { get; set; }

        [Display(Name = "Closing Time")]
        public TimeSpan ClosingTime { get; set; }

        [Display(Name = "")]
        [NotMapped]
        public ICollection<DateTime> Holidays { get; set; }

        [Display(Name = "")]
        public bool IsClosed { get; set; }
    }
}