namespace qrwaiter_backend.Data.Models
{
    public class NotifyDevice
    {
        public Guid Id { get; set; }
        public string DeviceToken { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ICollection<QrCode> QrCodes { get; set; } = new List<QrCode>();
    }
}
