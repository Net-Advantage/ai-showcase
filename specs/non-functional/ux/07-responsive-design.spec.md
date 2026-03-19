# UX Design — Responsive Design

**Version**: 2.1  
**Last Updated**: February 15, 2026

## 7. Responsive Design

### 7.1 Breakpoints

**Single Major Breakpoint:**
```
Mobile:  < 640px
Desktop: ≥ 641px
```

**Rationale:** Simple calculator app doesn't require complex breakpoints. Mobile-first with single pivot point.

### 7.2 Mobile Adaptations (< 640px)

**Typography:**
- Base font size: 14px (down from 16px)
- H1: 1.5rem (down from 2rem)
- H2: 1.125rem (down from 1.5rem)

**Spacing:**
- Container padding: 1rem (down from 2rem)
- Card padding: 1rem (down from 2rem)
- Section gaps reduced proportionally

**Layout:**
- Sidebar: Collapsed to hamburger menu
- Navigation: Toggle to reveal
- Single column throughout
- Full-width containers

**Components:**
- Result values: 1rem font size (down from 1.125rem)
- Take-home value: 1.125rem (down from 1.5rem)
- Button padding: Maintained for touch targets

### 7.3 Responsive Principles

**Mobile First:**
- Base styles target mobile
- Desktop styles added via `@media (min-width: 641px)`
- Progressive enhancement approach

**Flexible Layouts:**
- Use flexbox for dynamic sizing
- Avoid fixed widths (use max-width)
- Allow content to reflow naturally

**Touch Optimization:**
- Minimum 44×44px touch targets
- Generous spacing between clickable elements
- Immediate visual feedback on tap

**Performance:**
- Single CSS bundle
- Minimal media queries
- No separate mobile/desktop stylesheets
