using System.ComponentModel.DataAnnotations;
namespace DirtBikePark.Models
{
    public class BookingInputDTO
    {
        // The attributes here constrain user input to the following ranges, if the input doesn't match an automatic HTTP 400 Bad Request is sent in the Controller

        // [Range(1, int.MaxValue, ErrorMessage = "The number of days must be at least 1")]
        // public int NumDays { get; set; }

        public DateTime Date {  get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The number of adults must be positive")]
        public int NumAdults { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The number of children must be positive")]
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
                Date = this.Date,
                NumAdults = this.NumAdults,
                NumChildren = this.NumChildren,
                TotalPrice = 0,
                IsPaidFor = false
            };
            return bookingModel;
        }
    }
}
