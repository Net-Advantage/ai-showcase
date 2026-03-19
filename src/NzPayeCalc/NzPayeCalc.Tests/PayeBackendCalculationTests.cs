using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging.Abstractions;
using NzPayeCalc.ApiService.Models;
using NzPayeCalc.ApiService.Services;

namespace NzPayeCalc.Tests;

public class PayeBackendCalculationTests
{
    [Fact]
    public void REQ_PAYE_001_RequestValidation_Accepts_ZeroPercent_KiwiSaver()
    {
        var request = new PayeCalculationRequest
        {
            AnnualSalary = 60000m,
            KiwiSaverRate = 0m,
            HasStudentLoan = false
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(request, new ValidationContext(request), validationResults, validateAllProperties: true);

        Assert.True(isValid);
    }

    [Fact]
    public void REQ_PAYE_001_Service_Calculates_ZeroPercent_KiwiSaver_As_ZeroDollars()
    {
        var service = new PayeCalculationService(NullLogger<PayeCalculationService>.Instance);

        var result = service.Calculate(60000m, 0m, hasStudentLoan: false);

        Assert.Equal(0.00m, result.AnnualKiwiSaver);
    }

    [Theory]
    [InlineData(60000, 918.00)]
    [InlineData(200000, 2132.58)]
    public void REQ_PAYE_003_Service_Calculates_ACC_With_Expected_Precision(decimal annualSalary, decimal expectedAnnualAcc)
    {
        var service = new PayeCalculationService(NullLogger<PayeCalculationService>.Instance);

        var result = service.Calculate(annualSalary, 0.03m, hasStudentLoan: false);

        Assert.Equal(expectedAnnualAcc, result.AnnualAccLevy);
    }
}
