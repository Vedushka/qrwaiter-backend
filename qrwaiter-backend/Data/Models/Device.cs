namespace qrwaiter_backend.Data.Models
{
    public class Device
    {
        public Guid Id { get; set; }
        public string DeviceToken { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public ICollection<QrCode> QrCodes { get; set; } = new List<QrCode>();
    }
}
