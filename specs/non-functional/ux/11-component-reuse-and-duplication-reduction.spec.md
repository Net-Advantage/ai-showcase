# UX Design — Component Reuse and Duplication Reduction

**Version**: 2.1  
**Last Updated**: February 15, 2026

This specification section provides guidance for building reusable UI components that reduce duplication across the codebase. It covers universal principles and platform-specific patterns for both Blazor and vanilla Web implementations.

## 11. Component Reuse and Duplication Reduction

### 11.1 General Principles

#### Single Source of Truth
- Each visual pattern defined in § 4 (Component Library) **must** be implemented as a single reusable component or template — never duplicated inline.
- Design tokens (§ 2) are the sole source of styling values. Components must reference tokens, not hard-coded values.
- If a pattern appears in two or more places, extract it into a shared component.

#### Component Granularity
Organize components into three tiers:

| Tier | Description | Examples |
|------|-------------|----------|
| **Primitives** | Smallest reusable atoms that map directly to design tokens | Button, Input, Label, HelpText, ValidationMessage |
| **Composites** | Combine primitives into meaningful groups | FormField (Label + Input + HelpText + Validation), ResultRow (Label + Value) |
| **Features** | Page-level assemblies composed of composites | CalculatorForm, ResultsCard, Sidebar |

**Rules:**
- Primitives accept design-token-aligned props/attributes (variant, size, state).
- Composites orchestrate primitives — they do **not** redefine primitive styles.
- Features wire composites to application data and behaviour.

#### Parameterisation over Duplication
When components vary in minor ways, prefer parameterisation:
- Use **variants** (e.g., `variant="primary"`, `variant="danger"`) rather than creating separate components.
- Use **slots/children** for content projection rather than duplicating wrapper markup.
- Use **modifier classes** (§ 8.2) following the naming convention `.component-name--modifier`.

#### Shared CSS
- All design tokens live in a single shared stylesheet (e.g., `app.css` or `:root` block).
- Component-specific styles are scoped to their component (CSS isolation in Blazor, BEM in Web).
- Never duplicate token definitions or base reset styles across component files.

#### Documentation
- Every reusable component must include a brief summary of its purpose, accepted parameters/attributes, and usage example.
- Keep documentation collocated with the component (inline comments or a companion `README.md` in the component folder).

### 11.2 Blazor Component Reuse (MVVM)

#### Component Structure
```
Components/
├── Shared/                        (Reusable primitives and composites)
│   ├── AppButton.razor            (Primary/secondary/danger button)
│   ├── AppButton.razor.css
│   ├── FormField.razor            (Label + Input + HelpText + Validation)
│   ├── FormField.razor.css
│   ├── CurrencyInput.razor        (Monospace $ input)
│   ├── ResultRow.razor            (Label-value pair in results list)
│   ├── ResultsCard.razor          (Card wrapper with heading)
│   ├── AlertDanger.razor          (Dismissible error alert)
│   └── CollapsibleSection.razor   (Details/summary pattern)
├── Layout/
│   ├── MainLayout.razor
│   └── NavMenu.razor
└── Pages/
    └── Calculator.razor           (Feature: assembles composites)
```

#### Parameter Design
Use Blazor `[Parameter]` attributes aligned to design tokens and component variants:

```csharp
// AppButton.razor
[Parameter] public string Variant { get; set; } = "primary";   // primary | danger
[Parameter] public bool IsLoading { get; set; }
[Parameter] public bool IsDisabled { get; set; }
[Parameter] public RenderFragment ChildContent { get; set; }    // Slot
[Parameter] public EventCallback OnClick { get; set; }
```

```csharp
// FormField.razor
[Parameter] public string Label { get; set; }
[Parameter] public string HelpText { get; set; }
[Parameter] public string ValidationMessage { get; set; }
[Parameter] public RenderFragment ChildContent { get; set; }    // Input slot
```

#### CSS Isolation
- Use Blazor CSS isolation (`.razor.css` files) to scope component styles automatically.
- Component CSS files reference shared design tokens via `var(--token-name)` — they do **not** re-declare token values.
- Global base styles and token definitions remain in `wwwroot/app.css`.

#### ViewModel Binding
- Components expose parameters for display data and `EventCallback` for user actions.
- ViewModels (per MVVM, § copilot-instructions) own UI state; components are stateless display units wherever possible.
- Avoid embedding business logic or domain rules inside shared components.

#### RenderFragment for Content Projection
Prefer `RenderFragment` (child content / slots) over adding many string parameters:

```razor
<!-- Usage -->
<ResultsCard Heading="Fortnightly Results">
    <ResultRow Label="Gross Pay" Value="@Model.GrossFortnightly" />
    <ResultRow Label="PAYE" Value="@Model.PayeFortnightly" Variant="deduction" />
    <ResultRow Label="Take-Home" Value="@Model.TakeHomeFortnightly" Variant="take-home" />
</ResultsCard>
```

This keeps markup in the parent while the card handles layout, borders, and spacing.

#### Cascading Values for Theming
Use `CascadingValue` for cross-cutting concerns (e.g., display period, currency symbol) to avoid prop-drilling through many layers:

```razor
<CascadingValue Value="@currentPeriod" Name="DisplayPeriod">
    <ResultsCard ... />
</CascadingValue>
```

### 11.3 Web Component Reuse (HTML / CSS / JavaScript)

#### File Structure
```
src/mvp/
├── components/                    (Reusable modules)
│   ├── app-button.js              (Button factory / Web Component)
│   ├── form-field.js              (Label + input + help + validation)
│   ├── currency-input.js          (Monospace $ input)
│   ├── result-row.js              (Label-value pair)
│   ├── results-card.js            (Card wrapper)
│   ├── alert-danger.js            (Dismissible alert)
│   └── collapsible-section.js     (Details/summary pattern)
├── shared/
│   └── design-tokens.css          (All CSS custom properties)
├── app.js                         (Feature wiring)
├── styles.css                     (Global base + utilities)
└── index.html
```

#### Strategy: Template Functions or Web Components
Choose one approach per project and apply it consistently:

**Option A — Template Functions (lightweight, no build step)**
```javascript
// components/result-row.js
export function createResultRow({ label, value, variant = 'default' }) {
  const row = document.createElement('div');
  row.className = `result-row ${variant !== 'default' ? variant : ''}`.trim();
  row.innerHTML = `
    <dt>${label}</dt>
    <dd>${value}</dd>
  `;
  return row;
}
```

**Option B — Web Components (encapsulated, framework-agnostic)**
```javascript
// components/result-row.js
class ResultRow extends HTMLElement {
  static get observedAttributes() { return ['label', 'value', 'variant']; }
  connectedCallback() { this.render(); }
  attributeChangedCallback() { this.render(); }
  render() {
    const variant = this.getAttribute('variant') || 'default';
    this.innerHTML = `
      <div class="result-row ${variant !== 'default' ? variant : ''}">
        <dt>${this.getAttribute('label')}</dt>
        <dd>${this.getAttribute('value')}</dd>
      </div>
    `;
  }
}
customElements.define('result-row', ResultRow);
```

**Selection Guidance:**
- Use **Template Functions** for simple projects with no build pipeline (like `src/mvp/`).
- Use **Web Components** when components need encapsulation or will be shared across multiple HTML pages.

#### CSS Sharing
- Extract all design token definitions into a dedicated `design-tokens.css` file.
- Each HTML page includes `design-tokens.css` **before** any component or page styles.
- Component-specific CSS uses BEM naming (§ 8.2) scoped by the component's block class.
- Never inline token values — always reference `var(--token-name)`.

#### Avoiding Markup Duplication
- Identify repeated HTML structures (e.g., result rows, form field groups, card wrappers).
- Replace duplicated markup with calls to component factory functions or custom element tags.
- Keep `index.html` as a thin shell that mounts components programmatically.

#### Event Delegation
When multiple instances of a component exist on a page, use event delegation on a shared parent rather than attaching individual listeners to each instance:

```javascript
document.querySelector('.results-list').addEventListener('click', (e) => {
  const row = e.target.closest('.result-row');
  if (row) { /* handle */ }
});
```

### 11.4 Cross-Platform Consistency Checklist

When implementing a component in either Blazor or Web, verify:

| # | Check | Reference |
|---|-------|-----------|
| 1 | Component references design tokens, not hard-coded values | § 2 |
| 2 | Component maps to exactly one entry in the Component Library | § 4 |
| 3 | All interactive states are implemented (hover, focus, disabled, loading) | § 5.1 |
| 4 | Accessibility requirements met (semantic HTML, ARIA, focus indicators) | § 6 |
| 5 | Responsive behaviour matches mobile adaptation rules | § 7.2 |
| 6 | CSS naming follows conventions (BEM / component-name pattern) | § 8.2 |
| 7 | No duplicate markup or styles exist elsewhere for the same pattern | § 11.1 |
| 8 | Component is parameterised for known variants, not forked | § 11.1 |
| 9 | Component documentation (purpose, parameters, example) exists | § 11.1 |
| 10 | Vendor prefixes applied per browser support matrix | § 8.3 |
