using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GIP.PRJ.TraiteurApp.Models
{
    public class BusinessHours
    {
        public int Id { get; set; }

        [Display(Name = "Openingsdagen")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Sluitingsdagen")]
        public string ClosingDays { get; set; }

        [Display(Name = "Openingsuren")]
        public TimeSpan OpeningTime { get; set; }

        [Display(Name = "Sluitingstijd")]
        public TimeSpan ClosingTime { get; set; }

        [Display(Name = "Vakantie")]
        [NotMapped]
        public ICollection<Holiday> Holidays { get; set; } = new List<Holiday>();

        [Display(Name = "Is gesloten")]
        public bool IsClosed { get; set; } 

        public bool ForceClosed
        {
            get
            {
                var now = DateTime.Now.TimeOfDay;

                if (now < OpeningTime || now > ClosingTime)
                {
                    return true;
                }

                return false;
            }
        }
    }
}