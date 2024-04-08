using qrwaiter_backend.Data.Models;

namespace qrwaiter_backend.Mapping.DTOs
{
    public class TableDTO
    {
        public Guid Id { get; set; }
        public Guid IdResaurant { get; set; }
        public Guid IdQrCode { get; set; } 
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
    }
}
