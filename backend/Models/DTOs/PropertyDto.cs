namespace BackendApi.Models.DTOs
{
    public class PropertyDto
    {
        public int Id { get; set; }
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
        public OwnerDto? Owner { get; set; }
        public PropertyTypeDto? PropertyType { get; set; }
    }
}
