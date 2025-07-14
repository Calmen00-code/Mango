using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTO;

namespace Mango.Services.OrderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeader, OrderHeaderDTO>().ReverseMap();
                config.CreateMap<OrderDetails, OrderDetailsDTO>().ReverseMap();
                config.CreateMap<OrderHeaderDTO, OrderHeader>()
                    .ForMember(dest => dest.OrderHeaderId, opt => opt.Ignore());
            });
            return mappingConfig;
        }
    }
}
