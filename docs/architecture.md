# ARCHITECTURE DOCUMENT
## NZ PAYE Calculator Application

**Version:** 1.0  
**Date:** February 12, 2026  
**Status:** Draft

---

## 1. Architecture Outline

### 1.1 High-Level Component Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    .NET Aspire AppHost                      │
│                  (Orchestration Layer)                      │
└─────────────────────────────────────────────────────────────┘
                            │
        ┌───────────────────┴───────────────────┐
        │                                       │
        ▼                                       ▼
┌──────────────────┐                  ┌──────────────────┐
│   Web Frontend   │◄────HTTPS────────►│   API Service    │
│  (Blazor Server) │                   │  (ASP.NET Core)  │
└──────────────────┘                   └──────────────────┘
        │                                       │
        │                                       │
        ▼                                       ▼
┌──────────────────┐                  ┌──────────────────┐
│  UI Components   │                  │ Service Layer    │
│  - Calculator    │                  │ - PayeService    │
│  - Display       │                  │ - Validators     │
│  - Layout        │                  │                  │
└──────────────────┘                  └──────────────────┘
                                               │
                                               ▼
                                      ┌──────────────────┐
                                      │  Domain Layer    │
                                      │ - Tax Calculator │
                                      │ - Models         │
                                      │ - Constants      │
                                      └──────────────────┘
```

### 1.2 Technology Stack Decisions

#### Backend (NzPayeCalc.ApiService)
- **Framework:** ASP.NET Core 10.0 (Minimal APIs)
  - *Rationale:* Lightweight, performant, aligns with .NET Aspire best practices
  - *Traceability:* Existing project structure, developer-focused requirement
  
- **API Documentation:** OpenAPI/Swagger
  - *Rationale:* Standard API documentation, developer-friendly
  - *Traceability:* Already configured in project
  
- **Validation:** FluentValidation (to be added)
  - *Rationale:* Clean, testable validation logic separation
  - *Traceability:* REQ-020 (input validation)

#### Frontend (NzPayeCalc.Web)
- **Framework:** Blazor Server (.NET 10.0)
  - *Rationale:* Full-stack .NET, minimal JavaScript, strong typing
  - *Traceability:* Existing project structure, developer-focused requirement
  
- **Component Model:** Interactive Server Components
  - *Rationale:* Real-time updates, minimal client payload
  - *Traceability:* Already configured in project
  
- **Styling:** CSS (custom, Bootstrap-inspired)
  - *Rationale:* Aligns with site-style.spec.md inspiration
  - *Traceability:* Site style spec sections 3, 4, 5, 6

#### Infrastructure
- **.NET Aspire:** Service orchestration and discovery
  - *Rationale:* Built-in observability, service discovery, configuration
  - *Traceability:* Existing project structure
  
- **ServiceDefaults:** Shared configuration (health checks, OpenTelemetry)
  - *Ratability:* Centralized cross-cutting concerns
  - *Traceability:* Existing project structure

### 1.3 API Architecture

#### Endpoint Design (RESTful)

**Primary Endpoint:**
```
POST /api/paye/calculate
```

**Request Model:**
```csharp
{
  "annualSalary": decimal (required, > 0)
}
```

**Response Model:**
```csharp
{
  "annualSalary": decimal,
  "annualBreakdown": {
    "grossSalary": decimal,
    "payeTax": decimal,
    "kiwiSaver": decimal,
    "accLevy": decimal,
    "takeHomePay": decimal
  },
  "monthlyBreakdown": {
    "grossSalary": decimal,
    "payeTax": decimal,
    "kiwiSaver": decimal,
    "accLevy": decimal,
    "takeHomePay": decimal
  },
  "calculationDate": datetime
}
```

**Health Check Endpoints:**
```
GET /health
GET /alive
```

**API Versioning Strategy:**
- Initial version: v1 (implicit in route)
- Future versioning: URL-based (/api/v2/paye/calculate)
- *Rationale:* Simple, explicit, widely understood

#### Service Layer Structure

```
NzPayeCalc.ApiService/
├── Program.cs                    # Minimal API setup
├── Services/
│   ├── IPayeCalculationService.cs
│   ├── PayeCalculationService.cs # Core business logic
│   └── TaxBracketService.cs      # Tax bracket logic
├── Models/
│   ├── CalculationRequest.cs
│   ├── CalculationResponse.cs
│   ├── AnnualBreakdown.cs
│   └── MonthlyBreakdown.cs
├── Domain/
│   ├── TaxBracket.cs
│   ├── TaxConstants.cs           # PAYE rates, ACC, KiwiSaver
│   └── PayeCalculator.cs         # Pure calculation logic
└── Validators/
    └── CalculationRequestValidator.cs
```

### 1.4 Frontend Architecture

#### Component Structure

```
NzPayeCalc.Web/
├── Program.cs                    # App configuration
├── Components/
│   ├── App.razor
│   ├── Routes.razor
│   ├── _Imports.razor
│   ├── Layout/
│   │   ├── MainLayout.razor      # Global layout
│   │   ├── NavMenu.razor         # Navigation
│   │   └── Footer.razor          # Footer with legal info
│   └── Pages/
│       ├── Home.razor            # Landing/calculator page
│       ├── Calculator.razor      # Calculator component
│       ├── Results.razor         # Results display component
│       └── Error.razor
├── Services/
│   └── PayeApiClient.cs          # HTTP client for API
└── wwwroot/
    ├── app.css                   # Global styles
    └── css/
        ├── layout.css            # Layout-specific styles
        ├── calculator.css        # Calculator component styles
        └── components.css        # Reusable component styles
```

#### Component Hierarchy

```
App
└── MainLayout
    ├── NavMenu
    ├── Home (Calculator Page)
    │   ├── Calculator (Input Component)
    │   └── Results (Display Component)
    └── Footer
```

#### State Management
- **Component-level state:** For calculator input
- **Service-injection:** For API communication
- **No global state management needed:** Simple single-page calculator
- *Rationale:* YAGNI principle, simple requirements don't warrant complex state management

---

## 2. Interfaces and Boundaries

### 2.1 API Contracts

#### Request Models

**CalculationRequest.cs**
```csharp
public record CalculationRequest
{
    public decimal AnnualSalary { get; init; }
}
```

**Validation Rules:**
- AnnualSalary > 0
- AnnualSalary <= 10,000,000 (reasonable upper bound)
- Must be a valid decimal

#### Response Models

**CalculationResponse.cs**
```csharp
public record CalculationResponse
{
    public decimal AnnualSalary { get; init; }
    public AnnualBreakdown Annual { get; init; }
    public MonthlyBreakdown Monthly { get; init; }
    public DateTime CalculationDate { get; init; }
}

public record AnnualBreakdown
{
    public decimal GrossSalary { get; init; }
    public decimal PayeTax { get; init; }
    public decimal KiwiSaver { get; init; }
    public decimal AccLevy { get; init; }
    public decimal TakeHomePay { get; init; }
}

public record MonthlyBreakdown
{
    public decimal GrossSalary { get; init; }
    public decimal PayeTax { get; init; }
    public decimal KiwiSaver { get; init; }
    public decimal AccLevy { get; init; }
    public decimal TakeHomePay { get; init; }
}
```

**Error Response Model**
```csharp
// Uses standard ProblemDetails (RFC 7807)
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "AnnualSalary": ["Annual salary must be greater than 0"]
    }
}
```

### 2.2 Service Interfaces

#### IPayeCalculationService
```csharp
public interface IPayeCalculationService
{
    /// <summary>
    /// Calculates PAYE tax, KiwiSaver, ACC levy, and take-home pay
    /// </summary>
    /// <param name="annualSalary">Gross annual salary in NZD</param>
    /// <returns>Calculation results with annual and monthly breakdowns</returns>
    CalculationResponse Calculate(decimal annualSalary);
}
```

#### ITaxBracketService
```csharp
public interface ITaxBracketService
{
    /// <summary>
    /// Calculates total PAYE tax using progressive tax brackets
    /// </summary>
    /// <param name="annualSalary">Gross annual salary in NZD</param>
    /// <returns>Total annual PAYE tax</returns>
    decimal CalculatePayeTax(decimal annualSalary);
}
```

### 2.3 Component Interfaces (Blazor)

#### Calculator Component
```razor
@* Input: None (page-level component) *@
@* Output: Displays results via Results component *@

<Calculator OnCalculate="HandleCalculation" />

@code {
    private CalculationResponse? _result;
    
    private async Task HandleCalculation(decimal salary)
    {
        _result = await PayeApiClient.CalculateAsync(salary);
    }
}
```

#### Results Component
```razor
@* Input: CalculationResponse *@

<Results Data="@_result" />

@code {
    [Parameter]
    public CalculationResponse? Data { get; set; }
}
```

### 2.4 HTTP Client Interface

**PayeApiClient.cs**
```csharp
public interface IPayeApiClient
{
    /// <summary>
    /// Calls the PAYE calculation API
    /// </summary>
    Task<CalculationResponse?> CalculateAsync(decimal annualSalary, CancellationToken cancellationToken = default);
}
```

---

## 3. Cross-Cutting Concerns

### 3.1 Error Handling Strategy

#### API Layer (NzPayeCalc.ApiService)

**Validation Errors (400 Bad Request)**
- Use FluentValidation for input validation
- Return ProblemDetails with validation errors
- Example: Invalid salary value

**Business Logic Errors (422 Unprocessable Entity)**
- For domain-specific validation failures
- Example: Salary exceeds reasonable bounds

**Unexpected Errors (500 Internal Server Error)**
- Global exception handler middleware
- Return generic ProblemDetails without sensitive info
- Log full exception details server-side

**Implementation:**
```csharp
// In Program.cs
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;
        
        // Log exception (see Logging section)
        logger.LogError(exception, "Unhandled exception occurred");
        
        // Return problem details
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred processing your request.",
            Type = "https://httpstatuses.com/500"
        };
        
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});
```

#### Frontend Layer (NzPayeCalc.Web)

**API Communication Errors**
- Try-catch around HTTP calls
- Display user-friendly error messages
- Maintain form state on error

**Validation Errors**
- Client-side validation using Blazor validation
- Display inline validation messages
- Prevent submission of invalid data

**Implementation Pattern:**
```csharp
try
{
    _result = await PayeApiClient.CalculateAsync(salary);
    _errorMessage = null;
}
catch (HttpRequestException ex)
{
    _errorMessage = "Unable to connect to the calculation service. Please try again.";
    logger.LogError(ex, "API communication failed");
}
catch (Exception ex)
{
    _errorMessage = "An unexpected error occurred. Please try again.";
    logger.LogError(ex, "Unexpected error during calculation");
}
```

### 3.2 Logging Approach

#### Logging Strategy
- **Framework:** ASP.NET Core ILogger (built-in)
- **Sink:** Console (via .NET Aspire dashboard)
- **Structured Logging:** Yes (JSON format)
- **Correlation:** Request correlation IDs (via ServiceDefaults)

#### Log Levels

**Critical:** Application crash scenarios
**Error:** Exceptions, failed calculations
**Warning:** Validation failures, unusual inputs
**Information:** Calculation requests, API calls
**Debug:** Detailed calculation steps (dev only)
**Trace:** Not used

#### Implementation

**API Service:**
```csharp
public class PayeCalculationService : IPayeCalculationService
{
    private readonly ILogger<PayeCalculationService> _logger;
    
    public CalculationResponse Calculate(decimal annualSalary)
    {
        _logger.LogInformation(
            "Calculating PAYE for annual salary: {AnnualSalary:C}",
            annualSalary);
        
        try
        {
            // Calculation logic
            var result = PerformCalculation(annualSalary);
            
            _logger.LogDebug(
                "Calculation completed: PAYE={PayeTax:C}, KiwiSaver={KiwiSaver:C}, ACC={AccLevy:C}",
                result.Annual.PayeTax,
                result.Annual.KiwiSaver,
                result.Annual.AccLevy);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Calculation failed for salary: {AnnualSalary}", annualSalary);
            throw;
        }
    }
}
```

**Frontend:**
```csharp
public class PayeApiClient : IPayeApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PayeApiClient> _logger;
    
    public async Task<CalculationResponse?> CalculateAsync(decimal annualSalary, CancellationToken ct = default)
    {
        _logger.LogInformation("Requesting PAYE calculation for {AnnualSalary:C}", annualSalary);
        
        // HTTP call logic
    }
}
```

#### Observability via .NET Aspire
- Logs automatically collected and displayed in Aspire dashboard
- Correlation IDs automatically added via ServiceDefaults
- Distributed tracing for cross-service calls

### 3.3 Input Validation

#### Validation Layers

**Client-Side (Blazor)**
- Purpose: Immediate user feedback
- Implementation: Blazor EditForm + DataAnnotations
- Rules:
  - Required field
  - Numeric input only
  - Greater than 0

**API-Side (FluentValidation)**
- Purpose: Security, data integrity
- Implementation: FluentValidation library
- Rules:
  - AnnualSalary > 0
  - AnnualSalary <= 10,000,000
  - Valid decimal format

#### Validation Rules

**CalculationRequestValidator.cs**
```csharp
public class CalculationRequestValidator : AbstractValidator<CalculationRequest>
{
    public CalculationRequestValidator()
    {
        RuleFor(x => x.AnnualSalary)
            .GreaterThan(0)
            .WithMessage("Annual salary must be greater than 0")
            .LessThanOrEqualTo(10_000_000)
            .WithMessage("Annual salary must not exceed $10,000,000");
    }
}
```

**Client-Side Model:**
```csharp
public class CalculatorInputModel
{
    [Required(ErrorMessage = "Annual salary is required")]
    [Range(0.01, 10_000_000, ErrorMessage = "Annual salary must be between $0.01 and $10,000,000")]
    public decimal AnnualSalary { get; set; }
}
```

#### Security Considerations
- Never trust client-side validation alone
- API validates all inputs independently
- Prevent SQL injection: N/A (no database)
- Prevent XSS: Blazor auto-escapes outputs
- Prevent over-posting: Use specific DTOs

### 3.4 Configuration Management

#### Configuration Sources
1. **appsettings.json** - Default configuration
2. **appsettings.Development.json** - Development overrides
3. **Environment variables** - Container/deployment settings
4. **Aspire configuration** - Service discovery, orchestration

#### Configuration Structure

**ApiService appsettings.json:**
```json
{
  "TaxConfiguration": {
    "TaxYear": "2024/2025",
    "KiwiSaverRate": 0.03,
    "AccLevyRate": 0.0153,
    "AccMaxEarnings": 139384,
    "TaxBrackets": [
      { "Upper": 14000, "Rate": 0.105 },
      { "Upper": 48000, "Rate": 0.175 },
      { "Upper": 70000, "Rate": 0.30 },
      { "Upper": 180000, "Rate": 0.33 },
      { "Upper": null, "Rate": 0.39 }
    ]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Web appsettings.json:**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

#### Configuration Access Pattern
```csharp
// Register configuration
builder.Services.Configure<TaxConfiguration>(
    builder.Configuration.GetSection("TaxConfiguration"));

// Inject via IOptions
public class PayeCalculationService : IPayeCalculationService
{
    private readonly TaxConfiguration _config;
    
    public PayeCalculationService(IOptions<TaxConfiguration> config)
    {
        _config = config.Value;
    }
}
```

#### Configuration Advantages
- Easy to update tax rates without code changes
- Different rates per environment (if needed)
- Testable via mock IOptions
- Strongly-typed configuration classes

### 3.5 Testing Strategy

#### Test Pyramid

```
        ╱╲
       ╱  ╲     E2E Tests (5%)
      ╱────╲    - Full app workflow
     ╱      ╲   Integration Tests (15%)
    ╱────────╲  - API endpoint tests
   ╱          ╲ Unit Tests (80%)
  ╱────────────╲ - Business logic tests
```

#### Unit Tests (NzPayeCalc.Tests)

**Framework:** xUnit
**Mocking:** Moq
**Coverage Target:** >80% for business logic

**Test Coverage:**
- PayeCalculator (pure calculation logic)
- TaxBracketService (progressive tax calculation)
- Validators (input validation rules)
- Extension methods and utilities

**Example:**
```csharp
public class PayeCalculatorTests
{
    [Theory]
    [InlineData(60000, 10300)]  // Example from spec
    [InlineData(14000, 1470)]   // Lower bracket
    [InlineData(180000, 48020)] // Upper bracket
    public void CalculatePayeTax_VariousSalaries_ReturnsCorrectTax(
        decimal salary, decimal expectedTax)
    {
        // Arrange
        var calculator = new PayeCalculator();
        
        // Act
        var actualTax = calculator.CalculatePayeTax(salary);
        
        // Assert
        Assert.Equal(expectedTax, actualTax, 2); // 2 decimal places
    }
}
```

#### Integration Tests

**Framework:** WebApplicationFactory (ASP.NET Core)
**Scope:** API endpoints

**Test Coverage:**
- POST /api/paye/calculate - Success scenarios
- POST /api/paye/calculate - Validation failures
- Health check endpoints

**Example:**
```csharp
public class PayeApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public PayeApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Calculate_ValidSalary_ReturnsOk()
    {
        // Arrange
        var request = new CalculationRequest { AnnualSalary = 60000 };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/paye/calculate", request);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CalculationResponse>();
        Assert.NotNull(result);
        Assert.Equal(60000, result.AnnualSalary);
    }
}
```

#### E2E Tests (Optional/Future)

**Framework:** Playwright or Selenium
**Scope:** Critical user workflows
**Priority:** Low (simple single-page calculator)

**Test Scenarios:**
- User enters salary, sees calculation results
- Validation errors display correctly
- Responsive design works on mobile

#### Testing Best Practices
- Arrange-Act-Assert pattern
- One assertion per test (where practical)
- Descriptive test names
- Test edge cases and boundaries
- Mock external dependencies
- Use test data builders for complex objects

#### Continuous Testing
- Run unit tests on every build
- Run integration tests on PR
- Required for merge to main
- Code coverage reports in CI/CD

---

## 4. Risks and Mitigations

### 4.1 Technical Risks

#### Risk 1: Tax Rate Changes
**Severity:** Medium  
**Likelihood:** Medium (annual budget changes)  
**Impact:** Incorrect calculations, user distrust

**Mitigation:**
- Store tax rates in configuration (not hardcoded)
- Version configuration by tax year
- Add TaxYear field to response
- Document update process
- Add admin warning if rates are >1 year old

**Traceability:** REQ-004 to REQ-006 (PAYE rates)

---

#### Risk 2: Decimal Precision Issues
**Severity:** High  
**Likelihood:** Low  
**Impact:** Incorrect calculations, rounding errors

**Mitigation:**
- Use `decimal` type throughout (not `double` or `float`)
- Round to 2 decimal places at presentation layer only
- Maintain full precision during calculations
- Add unit tests for edge cases (e.g., $0.01, $9,999,999.99)

**Traceability:** All calculation requirements

---

#### Risk 3: Performance Degradation
**Severity:** Low  
**Likelihood:** Low  
**Impact:** Slow user experience

**Analysis:**
- Simple calculation (no database, no complex algorithms)
- Expected response time: <100ms
- No performance concerns anticipated

**Mitigation:**
- Monitor via .NET Aspire telemetry
- Add response time logging
- Set performance budget: 200ms p99

**Traceability:** Developer-focused UX requirement

---

#### Risk 4: Accessibility Compliance
**Severity:** Medium  
**Likelihood:** Medium (if not tested)  
**Impact:** Excludes users with disabilities, potential legal issues

**Mitigation:**
- Follow WCAG AA guidelines from inception
- Use semantic HTML
- Ensure keyboard navigation
- Test with screen readers (NVDA/JAWS)
- Add ARIA labels where needed
- Regular accessibility audits

**Traceability:** Site style spec section 8 (WCAG AA)

---

#### Risk 5: Browser Compatibility
**Severity:** Low  
**Likelihood:** Low (modern browsers)  
**Impact:** UI breaks on older browsers

**Mitigation:**
- Support last 2 versions of major browsers (Chrome, Firefox, Safari, Edge)
- Test on mobile browsers (Safari iOS, Chrome Android)
- Use standard CSS (avoid cutting-edge features)
- Blazor Server has broad compatibility

**Traceability:** Site style spec section 9 (responsive)

---

#### Risk 6: Service Discovery Failure
**Severity:** Medium  
**Likelihood:** Low  
**Impact:** Frontend can't communicate with API

**Mitigation:**
- Use .NET Aspire service discovery (built-in resilience)
- Implement retry policies with Polly
- Display user-friendly error messages
- Add health checks to detect issues early

**Traceability:** .NET Aspire architecture

---

### 4.2 Business/Requirement Risks

#### Risk 7: Misunderstood Requirements
**Severity:** Medium  
**Likelihood:** Low (clear spec)  
**Impact:** Wrong calculations, rework

**Mitigation:**
- Verify calculations with external sources (IRD website)
- Add example test cases from spec
- Get stakeholder review before deployment
- Document all calculation logic

**Traceability:** All PAYE spec requirements

---

#### Risk 8: Scope Creep
**Severity:** Low  
**Likelihood:** Medium  
**Impact:** Delayed delivery

**Mitigation:**
- Clearly defined requirements in spec
- No database layer (prevents "save calculations" feature creep)
- Focus on core calculator functionality
- Document future enhancements separately

**Traceability:** Project scope definition

---

### 4.3 Security Risks

#### Risk 9: Input Validation Bypass
**Severity:** Low  
**Likelihood:** Low  
**Impact:** Invalid calculations, potential DOS

**Mitigation:**
- Multi-layer validation (client + server)
- API validates all inputs independently
- Use FluentValidation for robust rules
- Add upper bounds to prevent extreme values

**Traceability:** Input validation section 3.3

---

#### Risk 10: Sensitive Data Exposure
**Severity:** Low  
**Likelihood:** Low  
**Impact:** Privacy concerns

**Analysis:**
- No personal data collected (just salary amount)
- No data persistence
- No authentication/authorization needed

**Mitigation:**
- Don't log sensitive salary values in production
- Use HTTPS (enforced by .NET Aspire)
- No cookies or persistent storage

**Traceability:** Privacy-conscious design

---

## 5. Requirement Conflicts

### 5.1 Analysis of Requirements

After thorough review of the PAYE Calculator Specification and Site Style Specification, **no conflicts have been identified** between requirements.

### 5.2 Specification Gaps Addressed

The following areas required architectural decisions not explicitly specified in requirements:

#### Gap 1: Tax Rate Versioning
**Gap:** Specification provides 2024/2025 tax rates but no guidance on updates  
**Decision:** Store rates in configuration with TaxYear field  
**Rationale:** Enables future updates without code changes  
**Risk:** Low (common practice)

#### Gap 2: Upper Salary Bound
**Gap:** No maximum salary specified for validation  
**Decision:** Set reasonable upper bound at $10,000,000  
**Rationale:** Prevents abuse, still handles extreme high earners  
**Risk:** Very low (edge case)

#### Gap 3: Rounding Strategy
**Gap:** No specification of rounding method or precision  
**Decision:** Round to 2 decimal places (cents) for display  
**Rationale:** Standard currency display, maintains precision internally  
**Risk:** Low (standard practice)

#### Gap 4: Error Message Tone
**Gap:** Site style spec defines brand tone but not error messages  
**Decision:** Friendly, professional error messages ("Please enter a valid salary")  
**Rationale:** Aligns with "approachable" brand keyword  
**Risk:** None

#### Gap 5: Loading States
**Gap:** No specification for calculation in-progress state  
**Decision:** Show loading spinner during API calls  
**Rationale:** Standard UX pattern, prevents user confusion  
**Risk:** None

### 5.3 Assumptions Made

1. **Single Currency:** All amounts in NZD (no currency conversion)
2. **Single Tax Year:** Only current year rates (2024/2025)
3. **No Persistence:** Calculations not saved (stateless)
4. **No Authentication:** Public calculator, no user accounts
5. **Desktop-First:** Primary use case is desktop, but mobile-responsive
6. **Modern Browsers:** Last 2 versions of Chrome, Firefox, Safari, Edge

### 5.4 Recommendations for Clarification

The following items are recommended for stakeholder confirmation (though don't block architecture):

1. **Accessibility Testing:** Confirm budget/timeline for formal WCAG audit
2. **Analytics:** Confirm if usage analytics are required (currently not specified)
3. **Disclaimer:** Confirm if legal disclaimer text is required
4. **Future Features:** Confirm scope boundaries (e.g., student loan repayments, secondary tax)

---

## 6. Architectural Decision Records (ADRs)

### ADR-001: Use Minimal APIs over Controllers
**Status:** Accepted  
**Context:** Need to choose API implementation pattern  
**Decision:** Use ASP.NET Core Minimal APIs  
**Consequences:**
- ✅ Less boilerplate code
- ✅ Aligns with .NET Aspire patterns
- ✅ Sufficient for simple API (1 endpoint)
- ⚠️ Less structure if API grows significantly

---

### ADR-002: Use Blazor Server over Blazor WebAssembly
**Status:** Accepted  
**Context:** Already chosen in project structure  
**Rationale:**
- ✅ Smaller client payload
- ✅ Full .NET runtime on server
- ✅ No API CORS concerns (same-origin)
- ⚠️ Requires WebSocket connection

---

### ADR-003: No Database Layer
**Status:** Accepted  
**Context:** Calculator is stateless per requirements  
**Decision:** No persistence layer  
**Consequences:**
- ✅ Simpler architecture
- ✅ Faster development
- ✅ No data migration concerns
- ⚠️ Cannot track historical calculations
- ⚠️ No user preferences

---

### ADR-004: Configuration-Based Tax Rates
**Status:** Accepted  
**Context:** Tax rates change annually  
**Decision:** Store tax rates in appsettings.json  
**Consequences:**
- ✅ Update rates without code changes
- ✅ Environment-specific rates possible
- ✅ Easy to test with different rates
- ⚠️ Need to remember to update configuration

---

### ADR-005: FluentValidation over Data Annotations
**Status:** Accepted  
**Context:** Need robust API validation  
**Decision:** Use FluentValidation library  
**Consequences:**
- ✅ More testable validation logic
- ✅ Separation of concerns
- ✅ Complex validation rules easier
- ⚠️ Additional dependency

---

### ADR-006: CSS over CSS Framework (Bootstrap/Tailwind)
**Status:** Accepted  
**Context:** Site style spec references Bootstrap-inspired design  
**Decision:** Custom CSS, no framework dependency  
**Consequences:**
- ✅ Full design control
- ✅ Smaller payload
- ✅ No framework lock-in
- ⚠️ More manual CSS work
- ⚠️ Need to implement responsive grid

---

## 7. Future Considerations

### 7.1 Potential Enhancements (Out of Scope)

1. **Student Loan Repayments:** Additional deduction calculation
2. **Secondary Tax Rates:** Support for second jobs
3. **Historical Calculations:** Save/compare previous calculations
4. **PDF Export:** Download calculation results
5. **Tax Year Comparison:** Compare take-home across multiple years
6. **Advanced Scenarios:** Bonuses, overtime, KiwiSaver rate selection

### 7.2 Scalability Considerations

**Current Design:**
- Single API instance (via .NET Aspire)
- Stateless calculation
- No database

**If Scale Needed:**
- ✅ Stateless design enables horizontal scaling
- ✅ .NET Aspire can orchestrate multiple instances
- ✅ Load balancing straightforward
- ✅ No session affinity required

**Estimated Capacity:**
- Single instance: ~1000 req/sec
- Current needs: <10 req/sec (if that)
- Headroom: >99%

### 7.3 Deployment Considerations

**Current Environment:** Development (local)  

**Production Readiness Checklist:**
- [ ] Environment-specific configuration
- [ ] HTTPS enforced
- [ ] Health checks configured
- [ ] Logging configured for production sink
- [ ] Rate limiting (if public-facing)
- [ ] Container hardening
- [ ] Security scanning
- [ ] Performance testing

---

## 8. Traceability Matrix

| Architecture Decision | Traced to Requirement |
|-----------------------|----------------------|
| 5 Tax Brackets | PAYE Spec: Tax Rates 2024/2025 |
| 3% KiwiSaver | PAYE Spec: KiwiSaver section |
| 1.53% ACC (capped) | PAYE Spec: ACC Earners' Levy |
| Monthly breakdown | PAYE Spec: Output Display |
| ASP.NET Core API | Existing project structure |
| Blazor Server | Existing project structure |
| .NET Aspire | Existing project structure |
| Clean, modular UI | Site Style Spec: Section 1, 2 |
| Developer-focused | Site Style Spec: Brand Tone |
| Accessible (WCAG AA) | Site Style Spec: Section 8 |
| Responsive design | Site Style Spec: Section 9 |
| Validation (client+server) | Site Style Spec: Security |
| Structured logging | .NET Aspire best practices |
| ProblemDetails errors | ASP.NET Core standards |

---

## 9. Appendices

### Appendix A: Technology Versions

- .NET: 10.0
- ASP.NET Core: 10.0
- Blazor: .NET 10.0 (Server mode)
- .NET Aspire: Latest stable
- FluentValidation: Latest stable (13.x)

### Appendix B: Development Tools

- IDE: Visual Studio 2025 or Visual Studio Code
- API Testing: REST Client, Swagger UI
- Browser DevTools: Chrome DevTools, Firefox DevTools
- Accessibility Testing: axe DevTools, NVDA

### Appendix C: Reference Links

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- [Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models#blazor-server)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [WCAG AA Guidelines](https://www.w3.org/WAI/WCAG2AA-Conformance)
- [New Zealand IRD Tax Rates](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/tax-codes-and-tax-rates-for-individuals)

### Appendix D: Calculation Examples

**Example 1: $60,000 salary**
```
Annual Salary: $60,000

PAYE Calculation:
- $0 - $14,000 @ 10.5% = $1,470
- $14,001 - $48,000 @ 17.5% = $5,950
- $48,001 - $60,000 @ 30% = $3,600
Total PAYE: $11,020 / 12 = $918.33/month

KiwiSaver: $60,000 × 3% = $1,800 / 12 = $150/month

ACC: $60,000 × 1.53% = $918 / 12 = $76.50/month

Take-Home: $60,000 - $11,020 - $1,800 - $918 = $46,262
Monthly: $3,855.17
```

**Example 2: $100,000 salary**
```
Annual Salary: $100,000

PAYE Calculation:
- $0 - $14,000 @ 10.5% = $1,470
- $14,001 - $48,000 @ 17.5% = $5,950
- $48,001 - $70,000 @ 30% = $6,600
- $70,001 - $100,000 @ 33% = $9,900
Total PAYE: $23,920 / 12 = $1,993.33/month

KiwiSaver: $100,000 × 3% = $3,000 / 12 = $250/month

ACC: $100,000 × 1.53% = $1,530 / 12 = $127.50/month

Take-Home: $100,000 - $23,920 - $3,000 - $1,530 = $71,550
Monthly: $5,962.50
```

---

## Document Control

**Author:** AI Architect  
**Reviewers:** [To be assigned]  
**Approval:** [Pending]  
**Version History:**

| Version | Date | Author | Change Summary |
|---------|------|--------|----------------|
| 1.0 | 2026-02-12 | AI Architect | Initial architecture document |

---

**END OF ARCHITECTURE DOCUMENT**
