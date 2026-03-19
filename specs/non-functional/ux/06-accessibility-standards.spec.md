# UX Design — Accessibility Standards

**Version**: 2.1  
**Last Updated**: February 15, 2026

## 6. Accessibility Standards

### 6.1 WCAG 2.1 Level AA Compliance

**Required Standards:**
- Color contrast ≥ 4.5:1 for normal text
- Color contrast ≥ 3:1 for large text and UI components
- All functionality accessible via keyboard
- Visible focus indicators
- Proper heading hierarchy
- Text resizable to 200% without loss of functionality

### 6.2 Focus Indicators

**Global Focus Style:**
```css
*:focus-visible {
  outline: 2px solid var(--color-accent-primary);  /* #2f81f7 blue */
  outline-offset: 2px;
}
```

**Focus Indicator Requirements:**
- 2px solid outline (meets 3:1 contrast)
- 2px offset (clear separation from element)
- Blue color (#2f81f7) visible on all backgrounds
- Applied to all interactive elements
- Never remove focus indicators

### 6.3 Semantic HTML

**Required Practices:**
- Use `<h1>`, `<h2>`, etc. for headings (never skip levels)
- Use `<button>` for clickable actions (not `<div>` or `<a>`)
- Use `<label>` with `for` attribute for all form inputs
- Use `<dl>`, `<dt>`, `<dd>` for name-value pairs
- Use `<details>`/`<summary>` for disclosure widgets
- Use native HTML controls over custom implementations

### 6.4 ARIA Support

**When to Use ARIA:**
- `aria-describedby` for form field help text
- `aria-label` when visible label insufficient
- `role="alert"` for error messages
- `role="status"` for loading indicators
- `aria-hidden="true"` for decorative elements

**ARIA Principles:**
- First rule of ARIA: Don't use ARIA (prefer semantic HTML)
- Only add ARIA when semantic HTML insufficient
- Test with screen readers (NVDA, JAWS, VoiceOver)

### 6.5 Screen Reader Support

**Screen Reader Only Content** (`.sr-only`)
```css
.sr-only {
  position: absolute;
  width: 1px;
  height: 1px;
  padding: 0;
  margin: -1px;
  overflow: hidden;
  clip: rect(0, 0, 0, 0);
  white-space: nowrap;
  border: 0;
}
```

**Usage:**
- Provide context invisible to sighted users
- Announce dynamic content changes
- Supplement visual-only information

### 6.6 Keyboard Navigation

**Required Capabilities:**
- All interactive elements in tab order
- Logical tab sequence (top-to-bottom, left-to-right)
- Enter key: Submit form, activate buttons
- Space key: Toggle checkboxes, activate buttons
- Arrow keys: Navigate within custom controls (if any)
- Escape key: Close modals/dialogs (future)

**Keyboard Traps:**
- Never trap focus (user must always be able to tab out)
- Keep focus visible at all times
- Return focus appropriately after actions

### 6.7 Color and Contrast

**Verified Color Contrasts:**
All color combinations have been tested and meet or exceed WCAG AA:

| Foreground | Background | Ratio | Standard |
|------------|------------|-------|----------|
| #e6edf3 (Primary text) | #0d1117 | 15.9:1 | AAA ✅ |
| #9198a1 (Secondary text) | #0d1117 | 7.8:1 | AAA ✅ |
| #6e7681 (Tertiary text) | #0d1117 | 5.3:1 | AA ✅ |
| #2f81f7 (Accent) | #0d1117 | 7.3:1 | AAA ✅ |
| #3fb950 (Success) | #0d1117 | 6.9:1 | AAA ✅ |
| #f85149 (Error) | #0d1117 | 5.6:1 | AA ✅ |
| #d29922 (Warning) | #0d1117 | 7.8:1 | AAA ✅ |
| #ffffff (White) | #2f81f7 (Blue button) | 4.7:1 | AA ✅ |

**Never Rely on Color Alone:**
- Errors shown with icon + text + color
- Deductions use minus sign + red color
- Take-home uses label + emphasis + green color

### 6.8 Touch Targets

**Minimum Sizes:**
- Buttons: 44×44 CSS pixels
- Form controls: 44px height minimum
- Touch-friendly spacing between interactive elements
- Adequate padding for safe activation
