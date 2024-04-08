using qrwaiter_backend.Data.Models;
using shortid;

namespace qrwaiter_backend.Mapping.DTOs
{
    public class RestaurantDTO
    {
        public Guid Id { get; set; }
        public string WaiterLink { get; set; } = ShortId.Generate();

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
        public bool NotifyMe { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int TimeZoneMinutes { get; set; } = 0;
    }
}
