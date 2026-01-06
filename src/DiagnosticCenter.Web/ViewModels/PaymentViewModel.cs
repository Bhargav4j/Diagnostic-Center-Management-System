using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bill Number is required")]
        [StringLength(50, ErrorMessage = "Bill number cannot be longer than 50 characters")]
        public string BillNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile Number is required")]
        [Phone(ErrorMessage = "Invalid mobile number")]
        [StringLength(15, ErrorMessage = "Mobile number cannot be longer than 15 characters")]
        public string MobileNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Total Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Total Amount must be a positive number")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Paid Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Paid Amount must be a positive number")]
        [DataType(DataType.Currency)]
        public decimal PaidAmount { get; set; }

        [Required(ErrorMessage = "Payment Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Test Entry is required")]
        public int TestEntryId { get; set; }

        // Calculated property
        [Display(Name = "Due Amount")]
        [DataType(DataType.Currency)]
        public decimal DueAmount => TotalAmount - PaidAmount;

        // List for dropdown
        public List<TestEntryDropdownViewModel>? TestEntries { get; set; }
    }

    // Helper ViewModel for TestEntry Dropdown
    public class TestEntryDropdownViewModel
    {
        public int Id { get; set; }
        public string BillNo { get; set; } = string.Empty;
    }
}