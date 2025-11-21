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
    }
}
