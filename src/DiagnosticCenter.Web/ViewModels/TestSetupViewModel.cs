using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.ViewModels
{
    public class TestSetupViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fee is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Fee must be a positive number")]
        [DataType(DataType.Currency)]
        public decimal Fee { get; set; }

        [Required(ErrorMessage = "Test Type is required")]
        public int TypeId { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }

        // List for dropdown
        public List<TestTypeDropdownViewModel>? TestTypes { get; set; }
    }

    // Helper ViewModel for TestType Dropdown
    public class TestTypeDropdownViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}