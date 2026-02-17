namespace BackendApi.Models.DTOs
{
    public class CreateCompanyDto
    {
        public int OwnerId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CompanySite { get; set; } = string.Empty;
    }
}
