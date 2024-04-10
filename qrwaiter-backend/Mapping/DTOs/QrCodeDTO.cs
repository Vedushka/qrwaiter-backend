using shortid;

namespace qrwaiter_backend.Mapping.DTOs
{
    public class QrCodeDTO
    {
        public Guid Id { get; set; }
        public Guid IdTable { get; set; }
        public string ClientLink { get; set; } = ShortId.Generate();
        public string WaiterLink { get; set; } = ShortId.Generate();
        public string Title { get; set; } = string.Empty;
    }
}
