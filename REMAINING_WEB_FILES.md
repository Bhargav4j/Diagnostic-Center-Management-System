# Remaining Web Layer Files for DiagnosticCenter

This document lists all remaining files that need to be created for the complete Web layer implementation.

## COMPLETED FILES

### PART 1: Configuration Files ✓
- appsettings.json
- appsettings.Development.json
- Program.cs

### PART 2: Layout and Shared Pages ✓
- Pages/Shared/_Layout.cshtml
- Pages/Shared/_ValidationScriptsPartial.cshtml
- Pages/_ViewImports.cshtml
- Pages/_ViewStart.cshtml
- Pages/Index.cshtml + Index.cshtml.cs (Login)
- Pages/Error.cshtml + Error.cshtml.cs
- Pages/Dashboard.cshtml + Dashboard.cshtml.cs

### PART 3: TestType CRUD Pages ✓
- Pages/TestTypes/Index.cshtml + .cs
- Pages/TestTypes/Create.cshtml + .cs
- Pages/TestTypes/Details.cshtml + .cs
- Pages/TestTypes/Edit.cshtml + .cs
- Pages/TestTypes/Delete.cshtml + .cs

### PART 4: TestSetup CRUD Pages ✓
- Pages/TestSetup/Index.cshtml + .cs
- Pages/TestSetup/Create.cshtml + .cs
- Pages/TestSetup/Details.cshtml + .cs
- Pages/TestSetup/Edit.cshtml + .cs
- Pages/TestSetup/Delete.cshtml + .cs

### PART 5: TestEntry CRUD Pages (PARTIAL)
- Pages/TestEntry/Index.cshtml + .cs ✓
- Pages/TestEntry/Create.cshtml + .cs ✓
- Pages/TestEntry/Details.cshtml + .cs (NEEDS TO BE CREATED)
- Pages/TestEntry/Edit.cshtml + .cs (NEEDS TO BE CREATED)
- Pages/TestEntry/Delete.cshtml + .cs (NEEDS TO BE CREATED)

### PART 7: Static Files ✓
- wwwroot/css/site.css
- wwwroot/js/site.js

## REMAINING FILES TO CREATE

### PART 5: TestEntry - Remaining Pages (3 files)

#### 1. Pages/TestEntry/Details.cshtml
Similar structure to TestSetup/Details.cshtml but with TestEntry fields:
- Patient Name, DOB, Mobile
- Bill No, Test Name
- Total Amount, Paid Amount, Due Amount
- Due Date, Status

#### 2. Pages/TestEntry/Details.cshtml.cs
PageModel that loads TestEntryDto by ID and displays details.

#### 3. Pages/TestEntry/Edit.cshtml
Form with fields:
- Patient Name (text)
- Date of Birth (date)
- Mobile Number (text)
- Bill Number (text)
- Test (dropdown from TestSetup)
- Total Amount (number)
- Paid Amount (number)
- Due Date (date)
- IsActive (checkbox)

#### 4. Pages/TestEntry/Edit.cshtml.cs
PageModel with:
- InputModel class with all TestEntry fields
- OnGetAsync: Load test entry and populate form
- OnPostAsync: Update test entry
- LoadTestsAsync: Load active tests for dropdown

#### 5. Pages/TestEntry/Delete.cshtml
Confirmation page showing TestEntry details before deletion.

#### 6. Pages/TestEntry/Delete.cshtml.cs
PageModel with delete confirmation and execution.

### PART 6: Payment CRUD Pages (10 files)

#### 1. Pages/Payments/Index.cshtml
Table showing:
- Bill No, Payment Date, Amount
- Patient Name, Test Name
- Status, Actions (Details/Edit/Delete)

#### 2. Pages/Payments/Index.cshtml.cs
PageModel listing all payments with search functionality.

#### 3. Pages/Payments/Create.cshtml
Form with fields:
- Bill Number (text)
- Test Entry (dropdown)
- Amount (number)
- Payment Date (date)
- IsActive (checkbox)

#### 4. Pages/Payments/Create.cshtml.cs
PageModel with:
- InputModel for Payment fields
- LoadTestEntriesAsync for dropdown
- Create payment logic

#### 5. Pages/Payments/Details.cshtml
Display payment details including:
- Payment ID, Bill No, Amount
- Payment Date
- Associated Test Entry and Patient info

#### 6. Pages/Payments/Details.cshtml.cs
PageModel to load and display PaymentDto.

#### 7. Pages/Payments/Edit.cshtml
Edit form for payment (similar to Create).

#### 8. Pages/Payments/Edit.cshtml.cs
PageModel with edit logic.

#### 9. Pages/Payments/Delete.cshtml
Confirmation page for payment deletion.

#### 10. Pages/Payments/Delete.cshtml.cs
PageModel with delete confirmation.

### Additional Recommended Pages

#### 1. Pages/Logout.cshtml.cs
```csharp
public class LogoutModel : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync("DiagnosticCenterAuth");
        HttpContext.Session.Clear();
        TempData["InfoMessage"] = "You have been successfully logged out.";
        return RedirectToPage("/Index");
    }
}
```

#### 2. Pages/AccessDenied.cshtml + .cs
Simple page showing "Access Denied" message.

## IMPLEMENTATION GUIDE

### For TestEntry Details, Edit, Delete

Follow the same pattern as TestSetup but with TestEntry-specific fields:
- Use TestEntryDto, TestEntryUpdateDto
- Include ITestSetupService for test dropdown
- Handle date fields (DateOfBirth, DueDate)
- Display calculated DueAmount (TotalAmount - PaidAmount)

### For Payments CRUD

Follow the same CRUD pattern:
- Use PaymentDto, PaymentCreateDto, PaymentUpdateDto
- Include ITestEntryService for test entry dropdown
- Filter active test entries for selection
- Display patient and test information from related TestEntry

### Common Patterns

All pages should:
1. Include [Authorize] attribute
2. Use proper error handling with try-catch
3. Log operations with ILogger
4. Show success/error messages via TempData
5. Use Bootstrap 5 styling
6. Include validation
7. Manual ViewModel mapping (no AutoMapper)

### Manual Mapping Example

```csharp
// DTO to InputModel (for Edit)
Input = new InputModel
{
    Id = dto.Id,
    Field1 = dto.Field1,
    Field2 = dto.Field2
};

// InputModel to CreateDto/UpdateDto
var createDto = new EntityCreateDto
{
    Field1 = Input.Field1,
    Field2 = Input.Field2,
    CreatedBy = userId
};
```

### Session User ID Retrieval

```csharp
var userIdString = HttpContext.Session.GetString("UserId");
int? userId = null;
if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out var parsedUserId))
{
    userId = parsedUserId;
}
```

## TESTING CHECKLIST

After creating all files:
1. Build the solution
2. Run migrations
3. Test login functionality
4. Test each CRUD operation for all entities
5. Verify validation works
6. Check error handling
7. Test search functionality
8. Verify navigation menu works
9. Test responsive design
10. Check authentication/authorization

## FILE STRUCTURE

```
src/DiagnosticCenter.Web/
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
├── DiagnosticCenter.Web.csproj
├── Pages/
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Index.cshtml + .cs (Login)
│   ├── Dashboard.cshtml + .cs
│   ├── Error.cshtml + .cs
│   ├── Logout.cshtml.cs
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── TestTypes/
│   │   ├── Index.cshtml + .cs
│   │   ├── Create.cshtml + .cs
│   │   ├── Details.cshtml + .cs
│   │   ├── Edit.cshtml + .cs
│   │   └── Delete.cshtml + .cs
│   ├── TestSetup/
│   │   ├── Index.cshtml + .cs
│   │   ├── Create.cshtml + .cs
│   │   ├── Details.cshtml + .cs
│   │   ├── Edit.cshtml + .cs
│   │   └── Delete.cshtml + .cs
│   ├── TestEntry/
│   │   ├── Index.cshtml + .cs ✓
│   │   ├── Create.cshtml + .cs ✓
│   │   ├── Details.cshtml + .cs (TODO)
│   │   ├── Edit.cshtml + .cs (TODO)
│   │   └── Delete.cshtml + .cs (TODO)
│   └── Payments/
│       ├── Index.cshtml + .cs (TODO)
│       ├── Create.cshtml + .cs (TODO)
│       ├── Details.cshtml + .cs (TODO)
│       ├── Edit.cshtml + .cs (TODO)
│       └── Delete.cshtml + .cs (TODO)
└── wwwroot/
    ├── css/
    │   └── site.css
    └── js/
        └── site.js
```

## NOTES

- All code uses .NET 8.0
- Bootstrap 5.3.2 for UI
- Serilog for logging
- Session-based authentication
- Manual ViewModel mapping (no AutoMapper in Web layer)
- Validation attributes on InputModel classes
- Production-ready error handling
- TempData for user notifications
