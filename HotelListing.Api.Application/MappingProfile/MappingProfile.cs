using AutoMapper;
using HotelListing.Api.Application.DTOs.Booking;
using HotelListing.Api.Domain;
using HotelListing.Api.Application.DTOs.Country;
using HotelListing.Api.Application.DTOs.Hotel;

namespace HotelListing.Api.Application.MappingProfile;

public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<Hotel,GetHotelDto>()
            .ForMember(dest => dest.CountryName, cfg => cfg.MapFrom<CountryNameResolver>());
        CreateMap<CreateHotelDto, Hotel>();

        CreateMap<UpdateHotelDto, Hotel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
CreateMap<Country, GetCountryDto>()
    .ForCtorParam(nameof(GetCountryDto.Id),
        opt => opt.MapFrom(s => s.CountryId))
    .ForCtorParam(nameof(GetCountryDto.Name),
        opt => opt.MapFrom(s => s.Name))
    .ForCtorParam(nameof(GetCountryDto.ShortName),
        opt => opt.MapFrom(s => s.shortName)).ForCtorParam(nameof(GetCountryDto.Hotels), opt => opt.MapFrom(s => s.Hotels));

        
            CreateMap<Country, GetCountriesDto>()
        .ForCtorParam(nameof(GetCountriesDto.Id),
        opt => opt.MapFrom(s => s.CountryId))
    .ForCtorParam(nameof(GetCountriesDto.Name),
        opt => opt.MapFrom(s => s.Name))
    .ForCtorParam(nameof(GetCountriesDto.ShortName),
        opt => opt.MapFrom(s => s.shortName));        CreateMap<CreateCountryDto, Country>();
    }
}

public class CountryNameResolver : IValueResolver<Hotel, GetHotelDto, string>
{
    public string Resolve(Hotel source, GetHotelDto destination, string destMember, ResolutionContext context)
    {
        return source.Country?.Name ?? string.Empty;
    }

}


public sealed class BookingMappingProfile : Profile
    {
        
        public BookingMappingProfile()
        {
            CreateMap<Booking, GetBookingDto>()
    .ForCtorParam("name",
        opt => opt.MapFrom(src => src.Hotel!.Name))
    .ForCtorParam("Status",
        opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateBookingDto, Booking>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.UserId, opt => opt.Ignore())
    .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
    .ForMember(dest => dest.Status, opt => opt.Ignore())
    .ForMember(dest => dest.Hotel, opt => opt.Ignore());

               CreateMap<UpdateBookingDto, Booking>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.UserId, opt => opt.Ignore())
               .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
               .ForMember(dest => dest.CheckIn, opt => opt.Ignore())
               .ForMember(dest => dest.CheckOut, opt => opt.Ignore())
               .ForMember(dest => dest.Guests, opt => opt.Ignore())
               .ForMember(dest => dest.Status, opt => opt.Ignore())
               .ForMember(dest => dest.Hotel, opt => opt.Ignore());

        }
}



}
