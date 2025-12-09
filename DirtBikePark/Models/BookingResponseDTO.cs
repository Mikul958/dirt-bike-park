namespace DirtBikePark.Models
{
    public class BookingResponseDTO
    {
        public int Id { get; set; }
        public ParkResponseDTO? Park { get; set; }
        public DateOnly Date { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public decimal TotalPrice { get; set; }

        // Constructs a new BookingResponseDTO with information provided in a Booking
        public BookingResponseDTO(Booking bookingModel)
        {
            Id = bookingModel.Id;
            Park = bookingModel.Park != null ? new ParkResponseDTO(bookingModel.Park) : null;
            Date = bookingModel.Date;
            NumAdults = bookingModel.NumAdults;
            NumChildren = bookingModel.NumChildren;
            TotalPrice = bookingModel.TotalPrice;
        }

        // Takes an existing BookingResponseDTO and supplies it with information in a Booking
        public void UpdateWith(Booking bookingModel)
        {
            Id = bookingModel.Id;
            Park = bookingModel.Park != null ? new ParkResponseDTO(bookingModel.Park) : null;
            Date = bookingModel.Date;
            NumAdults = bookingModel.NumAdults;
            NumChildren = bookingModel.NumChildren;
            TotalPrice = bookingModel.TotalPrice;
        }
    }
}
