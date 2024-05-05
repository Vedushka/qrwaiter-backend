using shortid;

namespace qrwaiter_backend.Mapping.DTOs
{
    public class QrCodeAndTableDTO
    {
        public string ClientLink { get; set; } = string.Empty;
        public string WaiterLink { get; set; } = string.Empty;
        public string QrTitle { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public int TableNumber { get; set; }
        public bool subscribed { get; set; } = false;
    }
}
