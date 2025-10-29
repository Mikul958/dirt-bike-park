namespace DirtBikePark.Models
{
    public class Park
    {
        public string parkID { get; set; }
        public string name { get; set; }
        public List<Booking> bookings { get; set; }
        public Dictionary<string, bool> availableDates { get; set; }
        public int guestLimit { get; set; }
        public decimal pricePerAdult { get; set; }
        public decimal pricePerChild { get; set; }

        public Park()
        {
            // TODO
        }
    }
}
