namespace DiagnosticCenter.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a duplicate entity
/// </summary>
public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string entityName, string fieldName, object value)
        : base($"Entity '{entityName}' with {fieldName} '{value}' already exists.")
    {
    }

    public DuplicateEntityException(string message)
        : base(message)
    {
    }

    public DuplicateEntityException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
