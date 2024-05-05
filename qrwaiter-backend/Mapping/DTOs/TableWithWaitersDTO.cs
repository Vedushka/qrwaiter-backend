using qrwaiter_backend.Data.Models;
using System.Composition;

namespace qrwaiter_backend.Mapping.DTOs
{
    public class TableWithWaitersDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; } = 0;
        public string WaiterLink { get; set; } = null!;
        public List<WaiterDTO> Waiters { get; set; } = new List<WaiterDTO>();
    }
}
    public class WaiterDTO
{
    public string Name { get; set; } = string.Empty;
    public string DeviceToken { get; set; } = string.Empty;

}