namespace GIP.PRJ.TraiteurApp.Models
{
    public class BusinessHours
    {
        public string DayOfWeek { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public ICollection<DateTime> Holidays { get; set; }
        public bool IsClosed { get; set; }
    }
}
