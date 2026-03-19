# UX Design — Design Tokens

**Version**: 2.1  
**Last Updated**: February 15, 2026

Design tokens are implemented as CSS custom properties (CSS variables) in `:root`, enabling consistent theming and easy maintenance.

## 2. Design Tokens

### 2.1 Color Palette

#### Background Colors
```
--color-bg-primary:    #0d1117  (Deep charcoal, primary background)
--color-bg-secondary:  #161b22  (Slate, cards and elevated surfaces)
--color-bg-accent:     #21262d  (Slightly lighter, subtle emphasis)
--color-bg-hover:      #30363d  (Interactive element hover state)
```

#### Text Colors
```
--color-text-primary:    #e6edf3  (Off-white, primary text)      Contrast: 15.9:1 (AAA)
--color-text-secondary:  #9198a1  (Medium grey, secondary text)  Contrast: 7.8:1 (AAA)
--color-text-tertiary:   #6e7681  (Muted grey, tertiary text)    Contrast: 5.3:1 (AA)
```

#### Accent Colors (Blue - Primary Interactive)
```
--color-accent-primary:        #2f81f7  (Developer blue)         Contrast: 7.3:1 (AAA)
--color-accent-primary-hover:  #539bf5  (Lighter blue, hover)
--color-accent-primary-active: #6ca6ff  (Lightest blue, active)
```

#### Semantic Colors
```
Success:
--color-success:     #3fb950  (Green)              Contrast: 6.9:1 (AAA)
--color-success-bg:  #1f3326  (Dark green tint)

Error:
--color-error:       #f85149  (Red)                Contrast: 5.6:1 (AA)
--color-error-bg:    #3b1219  (Dark red tint)

Warning:
--color-warning:     #d29922  (Amber)              Contrast: 7.8:1 (AAA)
--color-warning-bg:  #3d2e00  (Dark amber tint)
```

#### Border Colors
```
--color-border:        #30363d  (Default borders)
--color-border-muted:  #21262d  (Subtle dividers)
```

**Color Usage Guidelines:**
- All colors meet or exceed WCAG AA contrast requirements (4.5:1 for normal text, 3:1 for large text and UI components)
- Use semantic colors consistently (green for success, red for errors, amber for warnings)
- Background colors create visual hierarchy (primary → secondary → accent)
- Text colors indicate importance (primary → secondary → tertiary)

### 2.2 Typography

#### Font Families
```
--font-family-base: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Noto Sans', 
                    Helvetica, Arial, sans-serif
--font-family-mono: ui-monospace, 'Cascadia Code', 'SF Mono', Monaco, 
                    'Courier New', monospace
```

**System Font Stack Rationale:**
- Native system fonts for optimal performance and familiarity
- Monospace for numerical data (amounts, calculations)
- Sans-serif for all UI text

#### Font Size Scale (7 steps)
```
--font-size-xs:   0.75rem  (12px)  - Captions, small labels
--font-size-sm:   0.875rem (14px)  - Help text, secondary info
--font-size-base: 1rem     (16px)  - Body text (default)
--font-size-lg:   1.125rem (18px)  - Emphasized text, subtitles
--font-size-xl:   1.5rem   (24px)  - Subheadings (H2)
--font-size-2xl:  2rem     (32px)  - Page headings (H1)
--font-size-3xl:  3rem     (48px)  - Hero text (reserved for future use)
```

**Scale Rationale:** Mathematically consistent 1.125x–1.33x progression with clear purpose for each step.

#### Font Weights
```
--font-weight-normal:    400  - Body text
--font-weight-medium:    500  - Labels, emphasis
--font-weight-semibold:  600  - Headings, important values
--font-weight-bold:      700  - Strong emphasis (reserved)
```

#### Line Heights
```
--line-height-tight:   1.25  - Headings (compact)
--line-height-normal:  1.5   - Body text (readable)
--line-height-relaxed: 1.75  - Long-form content (future)
```

**Typography Usage:**
- Base font size: 16px (desktop), 14px (mobile)
- All sizes in rem units (respects user font size preferences)
- Monospace for monetary values and numbers
- Consistent line height for vertical rhythm

### 2.3 Spacing Scale (8 steps)
```
--spacing-xs:  0.25rem  (4px)   - Tight spacing (icon gaps)
--spacing-sm:  0.5rem   (8px)   - Close elements (label-to-input)
--spacing-md:  1rem     (16px)  - Standard gap (default)
--spacing-lg:  1.5rem   (24px)  - Section spacing
--spacing-xl:  2rem     (32px)  - Large gaps (card padding)
--spacing-2xl: 3rem     (48px)  - Major sections
--spacing-3xl: 4rem     (64px)  - Hero sections (future)
```

**Spacing Usage:**
- Consistent vertical rhythm (multiples of 8px/0.5rem)
- Mobile reduces spacing proportionally (XL becomes MD)
- Applied via padding and margin utilities

### 2.4 Border Radius
```
--radius-sm: 0.25rem  (4px)   - Subtle rounding
--radius-md: 0.5rem   (8px)   - Standard controls (buttons, inputs)
--radius-lg: 0.75rem  (12px)  - Cards, panels (containers)
```

### 2.5 Shadows (Dark Mode Optimized)
```
--shadow-sm: 0 0 0 1px rgba(255, 255, 255, 0.05)
             Subtle border highlight

--shadow-md: 0 0 0 1px rgba(255, 255, 255, 0.1), 
             0 8px 16px rgba(0, 0, 0, 0.4)
             Elevated surface (cards)

--shadow-lg: 0 0 0 1px rgba(255, 255, 255, 0.1), 
             0 16px 32px rgba(0, 0, 0, 0.6)
             Floating surface (modals, future)
```

**Shadow Strategy:**
- Light highlights instead of dark shadows (dark mode adaptation)
- Minimal use (only for elevation, not decoration)
- Always combined with subtle border for definition

### 2.6 Animation Timing
```
--transition-fast: 150ms ease-in-out  - Micro-interactions (hover, focus)
--transition-base: 250ms ease-in-out  - State changes, reveals
```

**Animation Principles:**
- Purposeful, not decorative
- Fast enough to feel responsive
- Ease-in-out for natural feeling
- GPU-accelerated properties only (transform, opacity)
