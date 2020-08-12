using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
            .ForMember(x => x.ProductBrand, o => o.MapFrom(r => r.ProductBrand.Name))
            .ForMember(x => x.ProductType, o => o.MapFrom(r => r.ProductType.Name))
            .ForMember(x => x.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemsDto, BasketItem>();
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(x => x.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(x => x.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(x => x.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(x => x.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(x => x.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());


        }
    }
}