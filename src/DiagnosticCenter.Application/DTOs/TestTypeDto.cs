namespace DiagnosticCenter.Application.DTOs;

public class TestTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class TestTypeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class TestTypeUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
