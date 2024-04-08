using qrwaiter_backend.Data.Models;

namespace qrwaiter_backend.Mapping.DTOs
{
    public class AddTableDTO
    {
        public Guid IdResaurant { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; } = 0;
    }
}
