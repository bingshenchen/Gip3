namespace GIP.PRJ.TraiteurApp.Models
{
    public class Holiday
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int BusinessHoursId { get; set; }

        public BusinessHours BusinessHours { get; set; }
    }
}
