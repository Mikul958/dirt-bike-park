namespace DirtBikePark.Models
{
    public class Booking
    {
        public string bookingID { get; set; }
        public string parkID { get; set; }
        public string cartID { get; set; }
        public int guestNumber { get; set; }
        public decimal totalPrice { get; set; }
        public List<string> bookingDates { get; set; }

        public Booking()
        {
            // TODO
        }
    }
}
