# Copilot Instructions

This repository follows a strict spec-driven, role-segmented delivery model.

All code, design, planning, and documentation must align with this structure.

---

## 1. Source of Truth

All functional behaviour originates from the `/specs` folder.

- Do not invent requirements.
- Do not assume behaviour not defined in `/specs`.
- If behaviour is unclear, surface the ambiguity.
- All changes must be traceable to a spec.

If a requested feature is not defined in `/specs`, suggest creating or updating a spec file before implementation.

---

## 2. Role Separation Model

This repository models work across explicit roles:

- Product Owner (orchestration)
- Domain Expert (extracts requirements from `/specs`)
- Task Planner (delivery sequencing)
- Architect (system design)
- UX Designer (design system + experience)
- Frontend Developer (Blazor MVVM + CSS)
- Backend Developer (.NET, ASP.NET Core, EF Core, SQL Server, Aspire)
- Quality Assurance (scenario + Playwright automation)
- Implementation Engineer (setup, deployment, operations documentation)

When generating content, respect role boundaries:

- Do not mix domain logic with UI.
- Do not embed business rules in infrastructure.
- Do not add requirements not defined in `/specs`.
- Do not blur architecture layers.

---

## 3. Architectural Constraints

All systems must use:

- Hosting: Azure Container Apps
- Backend: .NET 10
- API style: REST
- ORM: Entity Framework Core
- Database: SQL Server
- Frontend: Blazor using MVVM
- CI/CD: GitHub Actions
- Infrastructure as Code: Bicep
- Local orchestration: .NET Aspire

Do not propose alternative stacks unless explicitly asked.

---

## 4. Layering Rules

Backend must follow clean layering:

- API Layer
- Application Layer
- Domain Layer
- Infrastructure Layer

Rules:
- Domain layer contains business invariants.
- Application layer orchestrates use cases.
- Infrastructure layer contains EF Core and external concerns.
- API layer exposes DTOs only.

Frontend must follow MVVM:

- Views contain markup only.
- ViewModels contain UI state and logic.
- No domain rules in UI.

---

## 5. Traceability Expectations

When generating:

- Code
- Tests
- Documentation
- Architecture decisions

Always assume traceability to requirement IDs exists.

Prefer comments like:

```csharp
// Implements REQ-001: Order must transition after successful payment
