namespace DirtBikePark.Models
{
    public class BookingInputDTO
    {
        public int Id { get; set; }
        public int NumDays { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }

        // Creates a new Booking from the provided DTO information.
        // NOTE: Only expected to be used on POSTs, PUTs reference BookingId only
        public Booking FromInputDTO(BookingInputDTO bookingDTO)
        {
            Booking bookingModel = new Booking()
            {
                Id = bookingDTO.Id,
                CartId = null,
                ParkId = 0,
                NumDays = bookingDTO.NumDays,
                NumAdults = bookingDTO.NumAdults,
                NumChildren = bookingDTO.NumChildren,
                TotalPrice = 0,
                IsPaidFor = false
            };
            return bookingModel;
        }
    }
}
