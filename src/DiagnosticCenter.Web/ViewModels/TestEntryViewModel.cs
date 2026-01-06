using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.ViewModels
{
    public class TestEntryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [Phone(ErrorMessage = "Invalid mobile number")]
        [StringLength(15, ErrorMessage = "Mobile number cannot be longer than 15 characters")]
        public string MobileNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bill Number is required")]
        [StringLength(50, ErrorMessage = "Bill number cannot be longer than 50 characters")]
        public string BillNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Total Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Total Amount must be a positive number")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Due Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Paid Amount must be a positive number")]
        [DataType(DataType.Currency)]
        public decimal PaidAmount { get; set; }

        [Required(ErrorMessage = "Test is required")]
        public int TestId { get; set; }

        // List for dropdown
        public List<TestSetupDropdownViewModel>? TestSetups { get; set; }
    }

    // Helper ViewModel for TestSetup Dropdown
    public class TestSetupDropdownViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}