# UX Design — Implementation Guidelines

**Version**: 2.1  
**Last Updated**: February 15, 2026

## 8. Implementation Guidelines

### 8.1 CSS Architecture

**File Structure:**
```
wwwroot/
├── app.css                        (Main design system)
Components/Layout/
├── MainLayout.razor.css           (Page structure)
└── NavMenu.razor.css              (Navigation)
```

**CSS Organization:**
1. Design tokens (CSS custom properties in `:root`)
2. Base styles (html, body, headings, links)
3. Component styles (alphabetical)
4. Utility classes (spacing, text, etc.)
5. Media queries (at end of file)

**Best Practices:**
- Use CSS custom properties for all design tokens
- Prefer classes over element selectors
- Keep specificity low (avoid nesting beyond 2 levels)
- Group related properties (layout → visual → text)
- Comment major sections

### 8.2 Naming Conventions

**Component Classes:**
- `.component-name` (block)
- `.component-name-element` (element)
- `.component-name--modifier` (modifier)

**State Classes:**
- `.is-active`, `.is-loading`, `.is-invalid`
- Prefixed with `is-` or `has-`

**Utility Classes:**
- `.mt-2` (margin-top spacing level 2)
- `.text-muted` (semantic text color)
- `.sr-only` (screen reader only)

### 8.3 Browser Support

**Target Browsers:**
- Chrome: Last 2 versions
- Firefox: Last 2 versions
- Safari: Last 2 versions
- Edge: Last 2 versions

**Required CSS Features:**
- CSS Custom Properties (variables)
- Flexbox
- Media Queries
- Transforms and Transitions
- `:focus-visible` pseudo-class

**Vendor Prefix Requirements:**
- All CSS properties that require vendor prefixes for the target browser matrix **must** include them.
- `-webkit-` prefixes are required for any property not yet unprefixed in the last 2 versions of Safari and Chrome.
- When a standard property and its prefixed variant are both needed, the **unprefixed property must appear last** (progressive enhancement).
- Common properties requiring `-webkit-` prefix consideration:
  - `appearance` → `-webkit-appearance`
  - `backdrop-filter` → `-webkit-backdrop-filter`
  - `text-size-adjust` → `-webkit-text-size-adjust`
  - `user-select` → `-webkit-user-select`
  - Pseudo-elements like `::-webkit-details-marker`, `::-webkit-scrollbar`
- If a build tool (e.g., Autoprefixer, PostCSS) is used, document it in the project. If no build tool is used (e.g., vanilla CSS), prefixes must be applied manually.
- The frontend developer is responsible for verifying vendor prefix coverage against the target browser matrix before marking work as complete.

**Graceful Degradation:**
- Browsers without `:focus-visible` use `:focus`
- No critical functionality depends on CSS
- Core content accessible without CSS
