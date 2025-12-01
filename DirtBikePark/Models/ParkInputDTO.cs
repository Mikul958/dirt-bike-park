using System.ComponentModel.DataAnnotations;

namespace DirtBikePark.Models
{
    public class ParkInputDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        // The attributes here constrain user input to the following ranges, if the input doesn't match an automatic HTTP 400 Bad Request is sent in the Controller

        [Range(0.01, double.MaxValue, ErrorMessage = "The price per adults must be a positive value")]
        public decimal PricePerAdult { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "The price per child must be a positive value")]
        public decimal PricePerChild { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The guest limit must be at least 1")]
        public int GuestLimit { get; set; }

        // Creates a new Park from information in the current DTO.
        // NOTE: Using this alone will create a new park. For PUTs, pass in the parkId of the park to update to the controller.
        public Park FromInputDTO()
        {
            Park parkModel = new Park()
            {
                Id = 0,
                Name = this.Name,
                Location = this.Location,
                Description = this.Description,
                ImageUrl = this.ImageUrl,
                PricePerAdult = this.PricePerAdult,
                PricePerChild = this.PricePerChild,
                GuestLimit = this.GuestLimit
            };
            return parkModel;
        }
    }
}
