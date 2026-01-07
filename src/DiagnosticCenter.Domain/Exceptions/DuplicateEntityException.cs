namespace DiagnosticCenter.Domain.Exceptions;

public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string entityName, string fieldName, string value)
        : base($"{entityName} with {fieldName} '{value}' already exists.")
    {
    }
}
