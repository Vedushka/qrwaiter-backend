namespace qrwaiter_backend.Data.Models
{
    public class StatisticQrCode
    {
        public Guid Id { get; set; }
        public Guid IdQrCode { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public QrCode QrCode { get; set; } = null!;
    }
}
