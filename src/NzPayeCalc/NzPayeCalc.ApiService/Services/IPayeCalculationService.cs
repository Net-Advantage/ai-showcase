namespace NzPayeCalc.ApiService.Services;

/// <summary>
/// Service for calculating NZ PAYE tax, KiwiSaver, ACC levy, and take-home pay
/// </summary>
public interface IPayeCalculationService
{
    /// <summary>
    /// Calculate PAYE and deductions for a given annual salary
    /// </summary>
    /// <param name="annualSalary">Annual gross salary in NZD</param>
    /// <returns>Calculation result with annual and monthly breakdowns</returns>
    PayeCalculationResult Calculate(decimal annualSalary);
}

/// <summary>
/// Result of PAYE calculation
/// </summary>
public class PayeCalculationResult
{
    public decimal AnnualSalary { get; init; }
    public decimal AnnualPayeTax { get; init; }
    public decimal AnnualKiwiSaver { get; init; }
    public decimal AnnualAccLevy { get; init; }
    public decimal AnnualTakeHome => AnnualSalary - AnnualPayeTax - AnnualKiwiSaver - AnnualAccLevy;

    public decimal MonthlyGrossSalary => RoundCurrency(AnnualSalary / 12);
    public decimal MonthlyPayeTax => RoundCurrency(AnnualPayeTax / 12);
    public decimal MonthlyKiwiSaver => RoundCurrency(AnnualKiwiSaver / 12);
    public decimal MonthlyAccLevy => RoundCurrency(AnnualAccLevy / 12);
    public decimal MonthlyTakeHome => RoundCurrency(AnnualTakeHome / 12);

    private static decimal RoundCurrency(decimal value) => 
        Math.Round(value, 2, MidpointRounding.ToEven);
}
