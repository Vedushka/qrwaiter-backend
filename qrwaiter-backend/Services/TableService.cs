using AutoMapper;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Mapping.DTOs;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Services.Interfaces;

namespace qrwaiter_backend.Services
{
    public class TableService : ITableService 
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;
        public TableService(ITableRepository tableRepository, IMapper mapper)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task<List<TableWithWaitersDTO>> GetTablesWithWaiterByRestaurantLink(string restaurantLink)
        {
            var tables = await _tableRepository.GetTablesWithQrCodesWithDevicesByRestaurantLink(restaurantLink);
            var tablesDto = new List<TableWithWaitersDTO>();
            foreach (var table in tables)
            {
                tablesDto.Add(new TableWithWaitersDTO
                {
                    Id = table.Id,
                    Name = table.Name,
                    Number = table.Number,
                    WaiterLink = table.QrCode.WaiterLink,
                    Waiters = _mapper.Map<List<WaiterDTO>>(table.QrCode.NotifyDevices)
                });
            }
            return tablesDto.ToList();
        }


    }
}
