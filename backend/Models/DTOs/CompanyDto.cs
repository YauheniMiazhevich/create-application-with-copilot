namespace BackendApi.Models.DTOs
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CompanySite { get; set; } = string.Empty;
        public OwnerDto? Owner { get; set; }
    }
}
