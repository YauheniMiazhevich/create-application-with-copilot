namespace BackendApi.Models.DTOs
{
    public class CreatePropertyDto
    {
        public int OwnerId { get; set; }
        public int PropertyTypeId { get; set; }
        public decimal PropertyLength { get; set; }
        public decimal PropertyCost { get; set; }
        public DateTime DateOfBuilding { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}
