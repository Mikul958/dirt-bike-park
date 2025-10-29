namespace DirtBikePark.Models
{
    public class Cart
    {
        public string cartID { get; set; }
        public List<Booking> cartBookings { get; set; }

        public Cart()
        {
            // TODO
        }
    }
}
