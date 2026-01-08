using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Application.Interfaces;

/// <summary>
/// Defines the contract for test type service operations.
/// </summary>
public interface ITestTypeService
{
    /// <summary>
    /// Retrieves all test types asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of test type DTOs.</returns>
    Task<IEnumerable<TestTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific test type by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the test type.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the test type DTO if found; otherwise, null.</returns>
    Task<TestTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new test type asynchronously.
    /// </summary>
    /// <param name="createDto">The data transfer object containing the information needed to create a test type.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created test type DTO.</returns>
    Task<TestTypeDto> CreateAsync(TestTypeCreateDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing test type asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the test type to update.</param>
    /// <param name="updateDto">The data transfer object containing the updated information.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(int id, TestTypeUpdateDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a test type asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the test type to delete.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for test types by name asynchronously.
    /// </summary>
    /// <param name="searchTerm">The search term to match against test type names.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of matching test type DTOs.</returns>
    Task<IEnumerable<TestTypeDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
