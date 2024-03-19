namespace qrwaiter_backend.Data.Models
{
    public class QrCode
    {
        public Guid Id { get; set; }
        public Guid IdTable { get; set; }
        public string Link { get; set; } = string.Empty;
        public string Title {  get; set; } = string.Empty;
        public int NotificationLifeTimeSecunds {  get; set; } = 3600; // 1h default life time of notification
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public ICollection<NotifyDevice> NotifyDevices { get; set; } = new List<NotifyDevice>();
        public ICollection<StatisticQrCode> StatisticQrCodes { get; set; } = new List<StatisticQrCode>();
        public Table Table { get; set; } = null!; 

    }
}
