using AutoMapper;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TestType, TestTypeDto>();
        CreateMap<TestTypeCreateDto, TestType>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<TestSetup, TestSetupDto>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.TestType != null ? src.TestType.Name : string.Empty));
        CreateMap<TestSetupCreateDto, TestSetup>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<TestEntry, TestEntryDto>()
            .ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.Test != null ? src.Test.Name : string.Empty));
        CreateMap<TestEntryCreateDto, TestEntry>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<Payment, PaymentDto>();
        CreateMap<PaymentCreateDto, Payment>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
    }
}
