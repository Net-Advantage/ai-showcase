using NzPayeCalc.ApiService.Models;
using NzPayeCalc.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Register PAYE calculation service
builder.Services.AddScoped<IPayeCalculationService, PayeCalculationService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// API root endpoint
app.MapGet("/", () => "NZ PAYE Calculator API - Navigate to /api/paye/calculate to calculate PAYE");

// PAYE calculation endpoint
app.MapPost("/api/paye/calculate", (PayeCalculationRequest request, IPayeCalculationService calculationService) =>
{
    var result = calculationService.Calculate(request.AnnualSalary);

    var response = new PayeCalculationResponse
    {
        AnnualSalary = result.AnnualSalary,
        Annual = new AnnualBreakdown
        {
            GrossSalary = result.AnnualSalary,
            PayeTax = result.AnnualPayeTax,
            KiwiSaver = result.AnnualKiwiSaver,
            AccLevy = result.AnnualAccLevy,
            TakeHomePay = result.AnnualTakeHome
        },
        Monthly = new MonthlyBreakdown
        {
            GrossSalary = result.MonthlyGrossSalary,
            PayeTax = result.MonthlyPayeTax,
            KiwiSaver = result.MonthlyKiwiSaver,
            AccLevy = result.MonthlyAccLevy,
            TakeHomePay = result.MonthlyTakeHome
        }
    };

    return Results.Ok(response);
})
.WithName("CalculatePaye")
.Produces<PayeCalculationResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest);

app.MapDefaultEndpoints();

app.Run();
