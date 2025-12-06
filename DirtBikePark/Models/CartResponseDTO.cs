namespace DirtBikePark.Models
{
    public class CartResponseDTO
    {
        public Guid Id { get; set; }
        public decimal TaxRate { get; set; }
        public List<BookingResponseDTO> Bookings { get; set; } = new List<BookingResponseDTO>();
        public decimal TotalPrice;

        public CartResponseDTO(Cart cartModel)
        {
            Id = cartModel.Id;
            TaxRate = cartModel.TaxRate;

            // Map Bookings to BookingResponseDTOs and get subtotal for price calculation
            decimal subtotal = 0.00m;
            foreach (Booking bookingModel in cartModel.Bookings)
            {
                Bookings.Add(new BookingResponseDTO(bookingModel));
                subtotal += bookingModel.TotalPrice;
            }

            TotalPrice = subtotal + subtotal * TaxRate;
        }

        public void UpdateWith(Cart cartModel)
        {
            Id = cartModel.Id;
            TaxRate = cartModel.TaxRate;

            // Map Bookings to BookingResponseDTOs and get subtotal for price calculation
            decimal subtotal = 0.00m;
            foreach (Booking bookingModel in cartModel.Bookings)
            {
                Bookings.Add(new BookingResponseDTO(bookingModel));
                subtotal += bookingModel.TotalPrice;
            }

            TotalPrice = subtotal + subtotal * TaxRate;
        }
    }
}
