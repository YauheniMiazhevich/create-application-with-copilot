namespace BackendApi.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompanyContact { get; set; } = false;

        // Navigation properties
        public ICollection<Company> Companies { get; set; } = new List<Company>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
