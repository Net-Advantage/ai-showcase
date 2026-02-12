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
}
