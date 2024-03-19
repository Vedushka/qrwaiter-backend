namespace qrwaiter_backend.Data.Models
{
    public class Table
    {
        public Guid Id { get; set; }
        public Guid IdResaurant { get; set; }
        public Guid IdQrCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public QrCode QrCode { get; set; } = null!;
        public Restaurant Restaurant { get; set; } = null!;

    }
}
