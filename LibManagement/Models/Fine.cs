using System.ComponentModel;

namespace LibManagement.Models
{
    public class Fine
    {
        public int Id { get; set; }
        [DisplayName("Late Day Rate")]
        public double LateDayRate { get; set; }
        [DisplayName("Maximum Fine Amount")]
        public double MaxFine { get; set; }
    }
}
