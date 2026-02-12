namespace NzPayeCalc.ApiService.Services;

/// <summary>
/// Implementation of PAYE calculation service using 2024/2025 NZ tax rates
/// </summary>
public class PayeCalculationService : IPayeCalculationService
{
    private readonly ILogger<PayeCalculationService> _logger;

    // Tax brackets for 2024/2025 tax year
    private static readonly (decimal Lower, decimal Upper, decimal Rate)[] TaxBrackets =
    [
        (0m, 14000m, 0.105m),           // $0 - $14,000 @ 10.5%
        (14000m, 48000m, 0.175m),       // $14,001 - $48,000 @ 17.5%
        (48000m, 70000m, 0.30m),        // $48,001 - $70,000 @ 30%
        (70000m, 180000m, 0.33m),       // $70,001 - $180,000 @ 33%
        (180000m, decimal.MaxValue, 0.39m)  // Over $180,000 @ 39%
    ];

    private const decimal KiwiSaverRate = 0.03m;  // 3% employee contribution
    private const decimal AccRate = 0.0153m;      // 1.53% ACC Earners' Levy
    private const decimal AccMaxEarnings = 139384m; // ACC maximum earnings threshold

    public PayeCalculationService(ILogger<PayeCalculationService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public PayeCalculationResult Calculate(decimal annualSalary)
    {
        _logger.LogInformation("Calculating PAYE for annual salary: {AnnualSalary:C}", annualSalary);

        try
        {
            var payeTax = CalculateProgressiveTax(annualSalary);
            var kiwiSaver = CalculateKiwiSaver(annualSalary);
            var accLevy = CalculateAccLevy(annualSalary);

            var result = new PayeCalculationResult
            {
                AnnualSalary = annualSalary,
                AnnualPayeTax = RoundCurrency(payeTax),
                AnnualKiwiSaver = RoundCurrency(kiwiSaver),
                AnnualAccLevy = RoundCurrency(accLevy)
            };

            _logger.LogDebug(
                "PAYE calculation completed. Salary: {Salary:C}, Tax: {Tax:C}, KiwiSaver: {KS:C}, ACC: {ACC:C}, TakeHome: {TH:C}",
                result.AnnualSalary,
                result.AnnualPayeTax,
                result.AnnualKiwiSaver,
                result.AnnualAccLevy,
                result.AnnualTakeHome);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating PAYE for salary: {AnnualSalary:C}", annualSalary);
            throw;
        }
    }

    /// <summary>
    /// Calculate progressive PAYE tax across all tax brackets
    /// </summary>
    private decimal CalculateProgressiveTax(decimal annualSalary)
    {
        decimal totalTax = 0m;

        foreach (var (lower, upper, rate) in TaxBrackets)
        {
            if (annualSalary <= lower)
                break;

            var taxableInBracket = Math.Min(annualSalary, upper) - lower;
            var taxInBracket = taxableInBracket * rate;
            totalTax += taxInBracket;

            _logger.LogTrace(
                "Tax bracket: ${Lower:N0}-${Upper:N0} @ {Rate:P1}: Taxable ${Taxable:N2} = ${Tax:N2}",
                lower, upper, rate, taxableInBracket, taxInBracket);

            if (annualSalary <= upper)
                break;
        }

        return totalTax;
    }

    /// <summary>
    /// Calculate KiwiSaver employee contribution (3% of gross salary)
    /// </summary>
    private decimal CalculateKiwiSaver(decimal annualSalary)
    {
        return annualSalary * KiwiSaverRate;
    }

    /// <summary>
    /// Calculate ACC Earners' Levy (1.53% of gross salary, capped at maximum earnings)
    /// </summary>
    private decimal CalculateAccLevy(decimal annualSalary)
    {
        var cappedSalary = Math.Min(annualSalary, AccMaxEarnings);
        return cappedSalary * AccRate;
    }

    /// <summary>
    /// Round currency to 2 decimal places using Banker's Rounding (Round-to-Even)
    /// </summary>
    private static decimal RoundCurrency(decimal value) =>
        Math.Round(value, 2, MidpointRounding.ToEven);
}
