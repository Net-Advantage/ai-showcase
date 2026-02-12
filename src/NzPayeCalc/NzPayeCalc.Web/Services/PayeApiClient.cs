using NzPayeCalc.Web.Models;
using System.Net.Http.Json;

namespace NzPayeCalc.Web.Services;

/// <summary>
/// Client for communicating with the PAYE calculation API
/// </summary>
public class PayeApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PayeApiClient> _logger;

    public PayeApiClient(HttpClient httpClient, ILogger<PayeApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Calculate PAYE and deductions for the given annual salary
    /// </summary>
    /// <param name="annualSalary">Annual gross salary in NZD</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Calculation results with annual and monthly breakdowns</returns>
    public async Task<PayeCalculationResponse?> CalculateAsync(
        decimal annualSalary,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Calculating PAYE for salary: {Salary:C}", annualSalary);

            var request = new PayeCalculationRequest { AnnualSalary = annualSalary };

            var response = await _httpClient.PostAsJsonAsync(
                "/api/paye/calculate",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PayeCalculationResponse>(
                cancellationToken: cancellationToken);

            _logger.LogInformation("PAYE calculation successful");

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "API request failed for salary: {Salary:C}", annualSalary);
            throw new ApplicationException("Unable to calculate PAYE. Please try again.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error calculating PAYE");
            throw;
        }
    }
}
