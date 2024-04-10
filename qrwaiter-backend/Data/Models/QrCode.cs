using shortid;

namespace qrwaiter_backend.Data.Models
{
    public class QrCode
    {
        public Guid Id { get; set; }
        public Guid IdTable { get; set; }
        public string ClientLink { get; set; } = ShortId.Generate();
        public string WaiterLink { get; set; } = ShortId.Generate();
        public string Title {  get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public ICollection<NotifyDevice> NotifyDevices { get; set; } = new List<NotifyDevice>();
        public ICollection<StatisticQrCode> StatisticQrCodes { get; set; } = new List<StatisticQrCode>();
        public Table Table { get; set; } = null!; 

    }
}
