namespace DirtBikePark.Models
{
    public class BookingInputDTO
    {
        public int NumDays { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }

        // Creates a new Booking from information in the current DTO.
        // NOTE: Only expected to be used on POSTs, PUTs reference BookingId only. Assign parkID from controller parameter.
        public Booking FromInputDTO()
        {
            Booking bookingModel = new Booking()
            {
                Id = 0,
                CartId = null,
                ParkId = 0,
                NumDays = this.NumDays,
                NumAdults = this.NumAdults,
                NumChildren = this.NumChildren,
                TotalPrice = 0,
                IsPaidFor = false
            };
            return bookingModel;
        }
    }
}
