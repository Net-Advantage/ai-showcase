# Specifications Directory

This directory contains all authoritative specifications for the project. All development work must be traceable to specifications in this directory.

## Organization

Specifications are organized into two categories:

### 1. Non-Functional Specifications (`/non-functional`)

**Non-functional specifications** define cross-cutting concerns, architectural standards, and design systems that apply across all features.

**Required Specifications (Must Exist Before Any Work Begins):**

- **[architecture.spec.md](non-functional/architecture.spec.md)** - Mandated technology stack, architectural patterns, API design standards, security requirements, infrastructure as code guidelines, testing strategy, and performance targets.

- **[ux-design.spec.md](non-functional/ux-design.spec.md)** - UX design system index (design tokens, components, interaction patterns, accessibility, responsive design).
- **[non-functional/ux/](non-functional/ux/)** - Detailed UX sub-specs (split by concern for easier navigation and targeted updates).

**Characteristics:**
- Apply to all features universally
- Define "how we build" rather than "what we build"
- Changes affect entire system
- Rarely change once established
- Must be consulted by Architect and UX Designer for every feature

---

### 2. Functional Specifications (`/functional`)

**Functional specifications** define specific features, user stories, and business capabilities.

**Current Specifications:**

- **[PAYE-calculator.spec.md](functional/PAYE-calculator.spec.md)** - NZ PAYE tax calculator feature specification including tax rates, calculations, user inputs, and API contracts.

**Characteristics:**
- Define specific features or capabilities
- Describe "what we build"
- One spec per feature or bounded context
- Can evolve as features are added
- Must reference non-functional specs where applicable

---

## Mandatory Pre-Flight Checks

**Before any development work begins, the following specifications MUST exist:**

### ✅ Required Non-Functional Specifications
- [ ] `/specs/non-functional/architecture.spec.md`
- [ ] `/specs/non-functional/ux-design.spec.md`

### ✅ Required Functional Specification
- [ ] At least one feature specification in `/specs/functional/`

**If any required specification is missing, work MUST NOT proceed.**

---

## Specification Lifecycle

### Creating a New Specification

1. **Determine Category:**
   - Non-functional: Cross-cutting concern (architecture, security, UX, performance)
   - Functional: Specific feature or business capability

2. **Create in Appropriate Folder:**
   - Non-functional: `/specs/non-functional/<name>.spec.md`
   - Functional: `/specs/functional/<feature-name>.spec.md`

3. **Follow Specification Template:**
   - Version number
   - Last updated date
   - Clear sections (Overview, Requirements, Acceptance Criteria)
   - Glossary of domain terms (functional specs)
   - Traceability markers (requirement IDs)

### Updating a Specification

1. **Version Control:**
   - Increment version number
   - Update "Last Updated" date
   - Document changes in revision history

2. **Impact Assessment:**
   - Non-functional spec changes affect all features
   - Functional spec changes affect specific feature only

3. **Review Process:**
   - Domain Expert reviews for accuracy
   - All affected work must be re-validated against updated spec

---

## Specification Standards

### All Specifications Must Include

- **Header Matter:**
  - Title
  - Version number
  - Last updated date
  
- **Content Sections:**
  - Overview/Purpose
  - Detailed requirements
  - Acceptance criteria (testable)
  
- **Traceability:**
  - Requirement IDs (e.g., REQ-001)
  - Reference markers for subsections

### Non-Functional Specifications Must Also Include

- **Scope statement** - What aspects of the system are covered
- **Constraints and non-negotiables** - What cannot be changed
- **Standards and compliance** - External standards being followed

### Functional Specifications Must Also Include

- **Glossary** - Domain terms with definitions
- **API contracts** - If applicable
- **Business rules** - Clearly stated and traceable
- **User interaction flows** - Where UI is involved

---

## Specification Integrity Rules

### 1. Single Source of Truth
Each concept is defined in exactly **one** specification file. No redundancy between specs.

### 2. Non-Functional Specs Are Authoritative
When functional specs reference architecture or UX patterns, they defer to non-functional specs rather than redefining them.

**Example:**
- ❌ **Bad:** Functional spec redefines button styles
- ✅ **Good:** Functional spec references UX design spec for button usage

### 3. Functional Specs Define Domain
When implementation needs domain clarification, consult the functional spec. Non-functional specs are domain-agnostic.

### 4. Specs Drive Documentation
Implementation documentation in `/docs` references specifications but never duplicates them. The spec is authoritative.

---

## Working with Specifications

### For Product Owners
- Verify all required specs exist before initiating work
- Route spec gaps back to specification authoring
- Ensure no work proceeds without spec backing

### For Domain Experts
- Extract requirements exclusively from functional specs
- Flag ambiguities and gaps
- Propose spec amendments for unclear requirements

### For Architects
- Consult `/specs/non-functional/architecture.spec.md` for all technical decisions
- Flag conflicts between requirements and architectural constraints
- Architecture outputs must not contradict architecture spec

### For UX Designers
- Consult `/specs/non-functional/ux-design.spec.md` for all design decisions
- Design outputs must not contradict UX design spec
- Propose spec amendments for new patterns

### For Developers (Frontend & Backend)
- Every implementation decision must trace to a spec
- When in doubt, ask Domain Expert for functional specs
- Consult Architect/UX Designer for non-functional specs

### For QA
- Every test must map to an acceptance criterion in a spec
- Use requirement IDs for traceability
- Gaps in testability indicate spec gaps

### For Implementation Engineers
- Documentation references specs as source of truth
- Installation guides reference architecture spec
- User guides reference functional specs

---

## Specification Naming Conventions

### Non-Functional Specifications
- Format: `<concern>.spec.md`
- Examples:
  - `architecture.spec.md`
  - `ux-design.spec.md`
  - `security.spec.md` (future)
  - `performance.spec.md` (future)

### Functional Specifications
- Format: `<feature-name>.spec.md`
- Use kebab-case for multi-word features
- Examples:
  - `PAYE-calculator.spec.md`
  - `user-authentication.spec.md` (future example)
  - `reporting-dashboard.spec.md` (future example)

---

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | Feb 13, 2026 | Product Owner | Initial specification organization and pre-flight requirements |
