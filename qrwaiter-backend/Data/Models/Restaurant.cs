using Microsoft.AspNetCore.Identity;

namespace qrwaiter_backend.Data.Models
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public Guid IdIdentityUser { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
        public bool NotifyMe { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int TimeZoneMinutes { get; set; } = 0;
        public IdentityUser IdentityUser { get; set; } = null!;
        public ICollection<Table> Tabels { get; set; } = new List<Table>();

    }
    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
