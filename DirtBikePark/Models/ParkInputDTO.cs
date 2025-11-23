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
