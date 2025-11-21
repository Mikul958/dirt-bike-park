namespace DirtBikePark.Models
{
    public class ParkInputDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal PricePerAdult { get; set; }
        public decimal PricePerChild { get; set; }
        public int GuestLimit { get; set; }

        // Creates a new Booking from the provided DTO information.
        // NOTE: Using this alone will create a new park. For PUTs, pass in the parkId of the park to update to the controller.
        public Park FromInputDTO(ParkInputDTO parkDTO)
        {
            Park parkModel = new Park()
            {
                Id = 0,
                Name = parkDTO.Name,
                Location = parkDTO.Location,
                Description = parkDTO.Description,
                ImageUrl = parkDTO.ImageUrl,
                PricePerAdult = parkDTO.PricePerAdult,
                PricePerChild = parkDTO.PricePerChild,
                GuestLimit = parkDTO.GuestLimit
            };
            return parkModel;
        }
    }
}
