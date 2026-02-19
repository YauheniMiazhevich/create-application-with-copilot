namespace BackendApi.Models.DTOs
{
    public class UpdatePropertyDto
    {
        public int? OwnerId { get; set; }
        public int? PropertyTypeId { get; set; }
        public decimal? PropertyLength { get; set; }
        public decimal? PropertyCost { get; set; }
        public DateTime? DateOfBuilding { get; set; }
        public string? Description { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
    }
}
