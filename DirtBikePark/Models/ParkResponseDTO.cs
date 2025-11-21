namespace DirtBikePark.Models
{
    public class ParkResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal PricePerAdult { get; set; }
        public decimal PricePerChild { get; set; }
        public int GuestLimit { get; set; }

        // Constructs a new ParkResponseDTO with information provided in a Park
        public ParkResponseDTO(Park parkModel)
        {
            Id = parkModel.Id;
            Name = parkModel.Name;
            Location = parkModel.Location;
            Description = parkModel.Description;
            ImageUrl = parkModel.ImageUrl;
            PricePerAdult = parkModel.PricePerAdult;
            PricePerChild = parkModel.PricePerChild;
            GuestLimit = parkModel.GuestLimit;
        }

        // Takes an existing ParkResponseDTO and supplies it with information in a Park
        public void UpdateWith(Park parkModel)
        {
            Id = parkModel.Id;
            Name = parkModel.Name;
            Location = parkModel.Location;
            Description = parkModel.Description;
            ImageUrl = parkModel.ImageUrl;
            PricePerAdult = parkModel.PricePerAdult;
            PricePerChild = parkModel.PricePerChild;
            GuestLimit = parkModel.GuestLimit;
        }
    }
}
