# UX Design — Layout System

**Version**: 2.1  
**Last Updated**: February 15, 2026

## 3. Layout System

### 3.1 Application Structure

The application uses a **sidebar navigation pattern** (not a marketing site layout):

```
┌─────────────┬──────────────────────────────────────┐
│             │  Top Bar (sticky)                    │
│  Sidebar    ├──────────────────────────────────────┤
│  Navigation │                                      │
│  (250px)    │  Main Content Area                   │
│             │  - Centered container (max 800px)    │
│  Fixed on   │  - Calculator components             │
│  desktop    │  - Generous padding                  │
│             │                                      │
│  Collapsible│                                      │
│  on mobile  │                                      │
└─────────────┴──────────────────────────────────────┘
```

**Desktop (≥641px):**
- Sidebar: 250px fixed width, full height, sticky position
- Main content: Flexible width with centered container
- Top bar: Sticky with 3.5rem height

**Mobile (<640px):**
- Sidebar: Collapsed into hamburger menu toggle
- Main content: Full width with reduced padding
- Single column layout

### 3.2 Content Container
```
.calculator-container {
  max-width: 800px
  margin: 0 auto
  padding: 32px (desktop), 16px (mobile)
}
```

**Container Rationale:**
- 800px max width prevents excessive line length
- Centered for easy scanning
- Responsive padding maintains usability on all screens

### 3.3 Grid and Alignment Principles

- **Flexbox for components:** Justify-between for label-value pairs
- **No Bootstrap grid:** Custom layouts for full control
- **Consistent gutters:** Use spacing scale tokens
- **Vertical rhythm:** All spacing in multiples of 8px
