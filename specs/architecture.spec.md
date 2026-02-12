# Architecture Specification

**Version**: 1.0  
**Last Updated**: February 13, 2026

This specification defines the mandated technology stack, architectural patterns, and technical constraints for all applications in this repository. All architectural decisions must align with these standards.

---

## 1. Mandated Technology Stack

All systems must use the following technology stack. These choices are fixed and may not be changed without explicit approval.

### 1.1 Hosting and Infrastructure
- **Cloud Platform**: Microsoft Azure
- **Hosting**: Azure Container Apps
- **Infrastructure as Code**: Bicep
- **Container Registry**: Azure Container Registry (ACR)

### 1.2 Backend Stack
- **Runtime**: .NET 10
- **API Style**: REST (RESTful HTTP APIs)
- **ORM**: Entity Framework Core
- **Database**: SQL Server (Azure SQL Database for production)
- **Local Orchestration**: .NET Aspire

### 1.3 Frontend Stack
- **Framework**: Blazor
- **Pattern**: MVVM (Model-View-ViewModel)
- **Rendering Mode**: As defined per feature requirements
- **Styling**: Custom CSS (Bootstrap-inspired patterns without heavy dependency)

### 1.4 DevOps and CI/CD
- **Version Control**: Git (GitHub)
- **CI/CD**: GitHub Actions
- **Build Tools**: .NET SDK, Docker
- **Testing Framework**: xUnit (backend), Playwright (end-to-end)

---

## 2. Architectural Patterns and Constraints

### 2.1 Layered Architecture (Backend)

All backend code must follow clean layering principles:

#### **API Layer**
- Exposes REST endpoints
- Handles HTTP concerns (routing, serialization, status codes)
- Uses DTOs (Data Transfer Objects) only
- No business logic
- Maps between DTOs and application layer contracts

#### **Application Layer**
- Orchestrates use cases
- Coordinates domain objects
- Manages transactions
- Handles application-level validation
- Exposes application services to API layer

#### **Domain Layer**
- Contains business invariants and rules
- Pure domain models (entities, value objects, domain services)
- Independent of infrastructure concerns
- No dependencies on ORM, database, or external services

#### **Infrastructure Layer**
- EF Core DbContext and configurations
- Repository implementations (if used)
- External service integrations
- File system, email, messaging concerns
- Database migrations

**Dependency Rules:**
- API → Application → Domain
- Infrastructure → Domain (for implementations)
- Domain has no dependencies on other layers
- Infrastructure details never leak to Domain

### 2.2 Frontend Architecture (Blazor MVVM)

All frontend code must follow MVVM pattern:

#### **Views (.razor files)**
- Contain markup only
- Bind to ViewModel properties and commands
- No business logic
- No direct API calls
- Minimal code-behind (only UI-specific concerns)

#### **ViewModels**
- Contain UI state and logic
- Expose properties for data binding
- Expose commands for user actions
- Call backend services
- Handle UI validation and error states
- Implement INotifyPropertyChanged where needed

#### **Models**
- DTOs received from API
- Client-side domain models (if applicable)
- No business rules (rules live in backend domain)

**Rules:**
- Views never contain domain logic
- ViewModels never contain markup
- Domain rules enforced server-side, not in UI
- API calls isolated in service classes or ViewModels

### 2.3 Database Strategy

#### **Entity Framework Core Patterns**
- Code-first approach
- Migrations for all schema changes
- Explicit entity configurations (FluentAPI over attributes)
- No lazy loading (explicit eager or select loading)
- Query optimization via projection

#### **Transaction Management**
- Use EF Core transactions
- Transaction boundaries in Application Layer
- Idempotency for critical operations

#### **Migration Strategy**
- All migrations in source control
- Migrations applied via CI/CD or startup (development)
- Backward-compatible migrations for zero-downtime deployments

---

## 3. API Design Standards

### 3.1 REST Conventions
- Use HTTP verbs correctly (GET, POST, PUT, DELETE, PATCH)
- Resource-oriented URLs (`/api/orders`, `/api/orders/{id}`)
- Plural resource names
- HTTP status codes follow semantics:
  - 200 OK (success with body)
  - 201 Created (resource created)
  - 204 No Content (success without body)
  - 400 Bad Request (validation failure)
  - 404 Not Found (resource not found)
  - 500 Internal Server Error (unhandled errors)

### 3.2 Request/Response Contracts
- All endpoints use DTOs
- DTOs versioned with API
- Validation attributes on request DTOs
- Error responses follow consistent format
- ISO 8601 for dates
- JSON camelCase for property names

### 3.3 Versioning
- URL versioning (`/api/v1/orders`) when needed
- Maintain backward compatibility where possible
- Deprecation notices in response headers

---

## 4. Security Standards

### 4.1 Authentication and Authorization
- Use Azure AD / Microsoft Entra ID when authentication required
- JWT tokens for API authentication
- Role-based access control (RBAC) where applicable
- Secrets stored in Azure Key Vault (production) or user secrets (development)

### 4.2 Data Protection
- HTTPS only (TLS 1.2+)
- Input validation on all endpoints
- SQL injection prevention via parameterized queries (EF Core default)
- XSS protection (Blazor automatic escaping)
- CORS policies explicitly defined

---

## 5. Infrastructure as Code (Bicep)

### 5.1 Structure
- Modular Bicep templates
- Separate modules for:
  - Container Apps
  - SQL Database
  - Networking
  - Application Insights
  - Key Vault

### 5.2 Environment Strategy
- Separate resource groups per environment (dev, test, prod)
- Environment-specific parameter files
- Naming conventions include environment suffix

### 5.3 Provisioning
- Bicep deployed via GitHub Actions
- Idempotent deployments
- Infrastructure changes code-reviewed like application code

---

## 6. Observability and Monitoring

### 6.1 Logging
- Structured logging (JSON format)
- Log levels: Trace, Debug, Information, Warning, Error, Critical
- Correlation IDs for request tracing
- PII (Personally Identifiable Information) excluded from logs

### 6.2 Application Insights
- All applications instrumented
- Custom telemetry for business events
- Performance monitoring
- Dependency tracking

### 6.3 Health Checks
- `/health` endpoint for all services
- Liveness and readiness probes for Container Apps
- Database connectivity checks

---

## 7. Testing Strategy

### 7.1 Unit Tests
- xUnit framework
- Test domain logic in isolation
- Mock external dependencies
- High coverage of business rules

### 7.2 Integration Tests
- Test API endpoints
- Use in-memory database or test containers
- Validate full request/response cycles

### 7.3 End-to-End Tests
- Playwright for UI automation
- Page Object Model pattern
- Test critical user journeys
- Run against deployed environments

---

## 8. Performance and Scalability

### 8.1 Performance Targets
- API response time: < 200ms (median)
- Page load time: < 2 seconds
- Database queries optimized (avoid N+1)

### 8.2 Scalability Assumptions
- Stateless services (horizontal scaling)
- Database connection pooling
- Caching strategy where appropriate (in-memory, Redis)

### 8.3 Resource Limits
- Container Apps: Define CPU and memory limits
- SQL Database: Right-size DTUs/vCores per environment

---

## 9. Configuration Management

### 9.1 Configuration Sources
- `appsettings.json` (default settings)
- `appsettings.{Environment}.json` (environment overrides)
- Environment variables (Container Apps)
- Azure Key Vault (secrets)
- User Secrets (local development)

### 9.2 Configuration Validation
- Required settings validated at startup
- Options pattern for strongly-typed configuration
- Fail fast on misconfiguration

---

## 10. Deployment Strategy

### 10.1 CI/CD Pipeline (GitHub Actions)
- **Build Stage**:
  - Restore dependencies
  - Build all projects
  - Run unit tests
  - Run integration tests

- **Package Stage**:
  - Build Docker images
  - Push to Azure Container Registry
  - Tag with commit SHA and version

- **Deploy Stage**:
  - Deploy Bicep infrastructure (if changed)
  - Apply EF Core migrations
  - Deploy container to Azure Container Apps
  - Run smoke tests

### 10.2 Rollback Strategy
- Container Apps revision management
- Blue-green deployment capability
- Database migration rollback plan

---

## 11. Development Workflow

### 11.1 Local Development
- .NET Aspire for local orchestration
- SQL Server in Docker or LocalDB
- User Secrets for local configuration
- Hot reload enabled for rapid iteration

### 11.2 Branch Strategy
- `main` branch protected
- Feature branches for new work
- Pull requests required
- All checks must pass before merge

### 11.3 Code Review Requirements
- Architectural alignment verified
- Tests included for all changes
- No domain logic in infrastructure or UI
- Requirement IDs referenced in commits

---

## 12. Technology-Specific Guidelines

### 12.1 .NET 10
- Use latest C# language features
- Async/await for I/O operations
- Dependency injection for all services
- Minimal APIs or controllers (as appropriate)

### 12.2 Entity Framework Core
- No tracking queries for read-only operations
- Explicit loading over lazy loading
- Use compiled queries for hot paths
- Index foreign keys and frequently queried columns

### 12.3 Blazor MVVM
- ViewModels implement observable patterns
- Use `@bind` for two-way binding
- Error boundaries for fault isolation
- Dispose of subscriptions in ViewModels

### 12.4 Azure Container Apps
- Use managed identity for Azure service access
- Configure auto-scaling rules
- Health probes configured
- Resource limits defined

---

## 13. Constraints and Non-Negotiables

The following are **fixed constraints** and cannot be changed without Product Owner approval:

1. All hosting must be Azure Container Apps (no VMs, no App Service)
2. All databases must be SQL Server (no PostgreSQL, MongoDB, etc.)
3. All frontend must be Blazor using MVVM (no React, Angular, Vue)
4. All backend must be .NET 10 REST APIs (no GraphQL, no gRPC without approval)
5. All IaC must be Bicep (no Terraform, ARM templates)
6. All CI/CD must be GitHub Actions (no Azure DevOps without approval)

Optimizations within these boundaries are permitted. Architectural patterns may be refined as long as core constraints are maintained.

---

## 14. Architectural Decision Records (ADR)

Major architectural decisions should be documented in `/docs/architecture` with:
- Context (requirement IDs)
- Decision
- Rationale
- Alternatives considered
- Consequences

Decisions must not conflict with this specification.

---

## 15. Compliance and Standards

### 15.1 Accessibility
- WCAG 2.1 Level AA compliance (minimum)
- Keyboard navigation support
- Screen reader compatibility

### 15.2 Code Quality
- No compiler warnings in production builds
- Static analysis tools enabled
- Consistent formatting (EditorConfig)

### 15.3 Documentation
- XML comments for public APIs
- README in each project
- Architecture diagrams in `/docs`

---

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | Feb 13, 2026 | System | Initial architecture specification |
