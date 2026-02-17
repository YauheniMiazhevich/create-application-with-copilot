namespace BackendApi.Models
{
    public class Company
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CompanySite { get; set; } = string.Empty;

        // Navigation property
        public Owner Owner { get; set; } = null!;
    }
}
