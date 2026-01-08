using AutoMapper;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Application.Mappings;

/// <summary>
/// AutoMapper configuration for entity to DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TestType mappings
        CreateMap<TestType, TestTypeDto>();
        CreateMap<TestTypeCreateDto, TestType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.TestSetups, opt => opt.Ignore());

        // TestSetup mappings
        CreateMap<TestSetup, TestSetupDto>()
            .ForMember(dest => dest.TestTypeName, opt => opt.MapFrom(src => src.TestType.Name));
        CreateMap<TestSetupCreateDto, TestSetup>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.TestType, opt => opt.Ignore())
            .ForMember(dest => dest.TestEntries, opt => opt.Ignore());

        // TestEntry mappings
        CreateMap<TestEntry, TestEntryDto>()
            .ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.TestSetup.Name))
            .ForMember(dest => dest.DueAmount, opt => opt.MapFrom(src => src.DueAmount));
        CreateMap<TestEntryCreateDto, TestEntry>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.TestSetup, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore());

        // Payment mappings
        CreateMap<Payment, PaymentDto>();
        CreateMap<PaymentCreateDto, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.TestEntry, opt => opt.Ignore());
    }
}
