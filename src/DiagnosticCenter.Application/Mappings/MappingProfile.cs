using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TestType mappings
        CreateMap<TestType, TestTypeDto>();
        CreateMap<TestTypeCreateDto, TestType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<TestTypeUpdateDto, TestType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // TestSetup mappings
        CreateMap<TestSetup, TestSetupDto>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.TestType.Name));
        CreateMap<TestSetupCreateDto, TestSetup>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<TestSetupUpdateDto, TestSetup>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // TestEntry mappings
        CreateMap<TestEntry, TestEntryDto>()
            .ForMember(dest => dest.TestSetupName, opt => opt.MapFrom(src => src.TestSetup.Name));
        CreateMap<TestEntryCreateDto, TestEntry>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());
        CreateMap<TestEntryUpdateDto, TestEntry>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BillNo, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // Payment mappings
        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.TestEntryBillNo, opt => opt.MapFrom(src => src.TestEntry.BillNo));
        CreateMap<PaymentCreateDto, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());
        CreateMap<PaymentUpdateDto, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
