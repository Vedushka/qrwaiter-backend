using AutoMapper;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Mapping.DTOs;

namespace qrwaiter_backend.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() { 
            CreateMap<Restaurant, RestaurantDTO>();
            CreateMap<RestaurantDTO, Restaurant>();

            CreateMap<Table, TableDTO>();
            CreateMap<TableDTO, Table>();

            CreateMap<QrCode, QrCodeDTO>();
            CreateMap<QrCodeDTO, QrCode>();
        }
    }
}
