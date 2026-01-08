using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Application.Mappings;

/// <summary>
/// AutoMapper profile configuration for entity to DTO mappings.
/// Defines mapping rules between domain entities and data transfer objects.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// Configures all entity to DTO mapping rules.
    /// </summary>
    public MappingProfile()
    {
        ConfigureTestTypeMappings();
        ConfigureTestSetupMappings();
        ConfigureTestEntryMappings();
        ConfigurePaymentMappings();
        ConfigureUserMappings();
    }

    /// <summary>
    /// Configures mappings for TestType entity and its DTOs.
    /// </summary>
    private void ConfigureTestTypeMappings()
    {
        // Entity to DTO mapping
        CreateMap<TestType, TestTypeDto>();

        // Create DTO to Entity mapping
        CreateMap<TestTypeCreateDto, TestType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.TestSetups, opt => opt.Ignore());

        // Update DTO to Entity mapping
        CreateMap<TestTypeUpdateDto, TestType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.TestSetups, opt => opt.Ignore());
    }

    /// <summary>
    /// Configures mappings for TestSetup entity and its DTOs.
    /// </summary>
    private void ConfigureTestSetupMappings()
    {
        // Entity to DTO mapping
        CreateMap<TestSetup, TestSetupDto>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.TestType != null ? src.TestType.Name : string.Empty));

        // Create DTO to Entity mapping
        CreateMap<TestSetupCreateDto, TestSetup>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.TestType, opt => opt.Ignore())
            .ForMember(dest => dest.TestEntries, opt => opt.Ignore());

        // Update DTO to Entity mapping
        CreateMap<TestSetupUpdateDto, TestSetup>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.TestType, opt => opt.Ignore())
            .ForMember(dest => dest.TestEntries, opt => opt.Ignore());
    }

    /// <summary>
    /// Configures mappings for TestEntry entity and its DTOs.
    /// </summary>
    private void ConfigureTestEntryMappings()
    {
        // Entity to DTO mapping
        CreateMap<TestEntry, TestEntryDto>()
            .ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.TestSetup != null ? src.TestSetup.Name : string.Empty));

        // Create DTO to Entity mapping
        CreateMap<TestEntryCreateDto, TestEntry>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.TestSetup, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore())
            .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => 0m)); // Initialize paid amount to 0

        // Update DTO to Entity mapping
        CreateMap<TestEntryUpdateDto, TestEntry>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.TestSetup, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore());
    }

    /// <summary>
    /// Configures mappings for Payment entity and its DTOs.
    /// </summary>
    private void ConfigurePaymentMappings()
    {
        // Entity to DTO mapping
        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.TestEntry != null ? src.TestEntry.PatientName : string.Empty))
            .ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.TestEntry != null && src.TestEntry.TestSetup != null ? src.TestEntry.TestSetup.Name : string.Empty));

        // Create DTO to Entity mapping
        CreateMap<PaymentCreateDto, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.TestEntry, opt => opt.Ignore())
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate != default ? src.PaymentDate : DateTime.UtcNow));

        // Update DTO to Entity mapping
        CreateMap<PaymentUpdateDto, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.TestEntry, opt => opt.Ignore());
    }

    /// <summary>
    /// Configures mappings for User entity and its DTOs.
    /// </summary>
    private void ConfigureUserMappings()
    {
        // Entity to DTO mapping - map PasswordHash to Password
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));

        // Create DTO to Entity mapping - password will be hashed in the service
        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Will be set in service after hashing
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

        // Update DTO to Entity mapping - password will be hashed in the service if provided
        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Will be set in service after hashing if password is provided
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
