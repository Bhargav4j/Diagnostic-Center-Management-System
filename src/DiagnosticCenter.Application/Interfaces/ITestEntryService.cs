using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Application.Interfaces;

/// <summary>
/// Defines the contract for test entry service operations.
/// </summary>
public interface ITestEntryService
{
    /// <summary>
    /// Retrieves all test entries asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of test entry DTOs.</returns>
    Task<IEnumerable<TestEntryDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific test entry by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the test entry.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the test entry DTO if found; otherwise, null.</returns>
    Task<TestEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new test entry asynchronously.
    /// </summary>
    /// <param name="createDto">The data transfer object containing the information needed to create a test entry.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created test entry DTO.</returns>
    Task<TestEntryDto> CreateAsync(TestEntryCreateDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing test entry asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the test entry to update.</param>
    /// <param name="updateDto">The data transfer object containing the updated information.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(int id, TestEntryUpdateDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a test entry asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the test entry to delete.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for test entries by patient name or bill number asynchronously.
    /// </summary>
    /// <param name="searchTerm">The search term to match against patient names or bill numbers.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of matching test entry DTOs.</returns>
    Task<IEnumerable<TestEntryDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
