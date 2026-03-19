using System.ComponentModel.DataAnnotations;

namespace NzPayeCalc.ApiService.Models;

/// <summary>
/// Request model for PAYE calculation
/// </summary>
public class PayeCalculationRequest
{
    /// <summary>
    /// Annual gross salary in NZD
    /// </summary>
    [Required(ErrorMessage = "Annual salary is required")]
    [Range(0.01, 999999999.99, ErrorMessage = "Annual salary must be between $0.01 and $999,999,999.99")]
    public decimal AnnualSalary { get; set; }

    /// <summary>
    /// KiwiSaver employee contribution rate (must be one of: 0, 0.03, 0.04, 0.06, 0.08, 0.10)
    /// </summary>
    [Required(ErrorMessage = "KiwiSaver rate is required")]
    [AllowedKiwiSaverRate(ErrorMessage = "KiwiSaver rate must be one of: 0%, 3%, 4%, 6%, 8%, or 10%")]
    public decimal KiwiSaverRate { get; set; } = 0.03m;

    /// <summary>
    /// Indicates whether the person has a student loan requiring repayment
    /// </summary>
    [Required(ErrorMessage = "HasStudentLoan is required")]
    public bool HasStudentLoan { get; set; } = false;
}

/// <summary>
/// Validation attribute for allowed KiwiSaver rates
/// </summary>
public class AllowedKiwiSaverRateAttribute : ValidationAttribute
{
    private static readonly decimal[] AllowedRates = [0m, 0.03m, 0.04m, 0.06m, 0.08m, 0.10m];

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is decimal rate && AllowedRates.Contains(rate))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage ?? "Invalid KiwiSaver rate");
    }
}
