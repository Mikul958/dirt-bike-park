namespace DirtBikePark.Models
{
    public class BookingResponseDTO
    {
        public int Id { get; set; }
        public Park? Park { get; set; }
        public int NumDays { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public decimal TotalPrice { get; set; }

        // Creates a new BookingResponseDTO with information provided in a Booking
        public BookingResponseDTO(Booking bookingModel)
        {
            Id = bookingModel.Id;
            Park = bookingModel.Park;
            NumDays = bookingModel.NumDays;
            NumAdults = bookingModel.NumAdults;
            NumChildren = bookingModel.NumChildren;
            TotalPrice = bookingModel.TotalPrice;
        }

        // Takes an existing BookingDTO and supplies it with information in a Booking
        public BookingResponseDTO ToResponseDTO(Booking bookingModel)
        {
            Id = bookingModel.Id;
            Park = bookingModel.Park;
            NumDays = bookingModel.NumDays;
            NumAdults = bookingModel.NumAdults;
            NumChildren = bookingModel.NumChildren;
            TotalPrice = bookingModel.TotalPrice;
            return this;
        }
    }
}
