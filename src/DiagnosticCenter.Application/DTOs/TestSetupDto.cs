namespace DiagnosticCenter.Application.DTOs;

public class TestSetupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

public class TestSetupCreateDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TypeId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public class TestSetupUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TypeId { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}
