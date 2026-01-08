using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Application.Interfaces;

/// <summary>
/// Defines the contract for test setup service operations.
/// </summary>
public interface ITestSetupService
{
    /// <summary>
    /// Retrieves all test setups asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of test setup DTOs.</returns>
    Task<IEnumerable<TestSetupDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific test setup by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the test setup.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the test setup DTO if found; otherwise, null.</returns>
    Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new test setup asynchronously.
    /// </summary>
    /// <param name="createDto">The data transfer object containing the information needed to create a test setup.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created test setup DTO.</returns>
    Task<TestSetupDto> CreateAsync(TestSetupCreateDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing test setup asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the test setup to update.</param>
    /// <param name="updateDto">The data transfer object containing the updated information.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(int id, TestSetupUpdateDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a test setup asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the test setup to delete.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for test setups by name asynchronously.
    /// </summary>
    /// <param name="searchTerm">The search term to match against test setup names.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of matching test setup DTOs.</returns>
    Task<IEnumerable<TestSetupDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
