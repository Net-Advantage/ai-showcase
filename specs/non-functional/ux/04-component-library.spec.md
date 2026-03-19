# UX Design — Component Library

**Version**: 2.1  
**Last Updated**: February 15, 2026

## 4. Component Library

### 4.1 Buttons

**Primary Button** (`.btn-primary`)
```
Background:     var(--color-accent-primary)  #2f81f7
Text:           #ffffff (white)
Padding:        0.75rem 1.5rem (12px 24px)
Border radius:  var(--radius-md)  8px
Font weight:    500 (medium)
Transition:     All properties 150ms

States:
- Hover:    Background → #539bf5 (lighter blue)
- Active:   Background → #6ca6ff (lightest blue)
- Focus:    Blue ring (3px at 30% opacity)
- Disabled: Opacity 60%, cursor not-allowed
```

**Loading State:**
- Spinner icon (rotating border animation, 750ms linear)
- Text changes to "Calculating..."
- Button remains disabled during loading

**Usage:**
- Primary call-to-action (Calculate button)
- Maximum one primary button per section

### 4.2 Form Controls

**Input Field** (`.form-control`)
```
Background:     #161b22 (secondary)
Text:           #e6edf3 (primary text)
Border:         1px solid #30363d
Border radius:  8px
Padding:        0.75rem 1rem
Font size:      1rem (16px)
Transition:     Border and shadow 150ms

States:
- Focus:   Border #2f81f7, blue ring (3px at 10% opacity)
- Invalid: Border #f85149 (red)
- Disabled: Opacity 60%
```

**Currency Input** (`.salary-input`)
```
Additional styles:
- Font family: Monospace
- Text align: Right
- Font size: 1.125rem (18px, larger for numbers)
- Prefix: "$" symbol (absolute positioned, left: 1rem)
```

**Form Labels:**
```
Font weight:   500 (medium)
Color:         #e6edf3 (primary text)
Margin bottom: 0.5rem
Display:       block
```

**Help Text** (`.form-text`)
```
Font size:     0.875rem (14px)
Color:         #9198a1 (secondary text)
Margin top:    0.25rem
```

**Validation Message** (`.validation-message`)
```
Font size:     0.875rem (14px)
Color:         #f85149 (error red)
Font weight:   500 (medium)
Margin top:    0.25rem
Display:       block
```

### 4.3 Cards and Panels

**Results Card** (`.results-card`)
```
Background:     #161b22 (secondary)
Border:         1px solid #30363d
Border radius:  12px (large)
Padding:        32px (desktop), 16px (mobile)
Box shadow:     Medium elevation
```

**Section Heading in Card:**
```
Border bottom:  2px solid #21262d
Padding bottom: 0.5rem
Margin bottom:  1.5rem
```

### 4.4 Results Display

**Results List** (`.results-list`)
```
Layout: Definition list (<dl>)
Structure:
  - Reset list styles (padding: 0, margin: 0)
  - Each row: <div class="result-row">

Result Row:
  Display:         Flex
  Justify content: Space between
  Padding:         1rem 0
  Border bottom:   1px solid #21262d (muted)
  
  Last row: Border none
```

**Result Labels** (`<dt>`)
```
Font weight: 500 (medium)
Color:       #e6edf3 (primary text)
```

**Result Values** (`<dd>`)
```
Font family: Monospace
Font size:   1.125rem (18px)
Font weight: 600 (semibold)
Color:       #e6edf3 (primary text)
Margin:      0

Modifiers:
- .deduction dd:  Color #f85149 (red)
- .take-home dd:  Color #3fb950 (green), font-size 1.5rem (24px)
```

**Take-Home Row Special Treatment:**
```
Border top:    2px solid #30363d (emphasized separator)
Padding top:   1.5rem
Margin top:    0.5rem
Border bottom: none
```

### 4.5 Collapsible Details

**Annual Breakdown** (`<details class="annual-breakdown">`)
```
Border top:    1px solid #21262d
Padding top:   1rem

<summary>:
  Cursor:      Pointer
  Color:       #2f81f7 (accent blue)
  Font weight: 500 (medium)
  Padding:     0.5rem
  Transition:  Color 150ms

<summary>::before:
  Content:    "▶" (right arrow)
  Transform:  rotate(90deg) when open
  Transition: 150ms

Hover:
  Color:      #539bf5 (lighter blue)
```

### 4.6 Alerts

**Danger Alert** (`.alert-danger`)
```
Background:     #3b1219 (dark red tint)
Color:          #f85149 (error red)
Border:         1px solid #f85149
Border radius:  8px
Padding:        1rem 1.5rem
Margin bottom:  1rem
Position:       Relative (for close button)
```

**Close Button** (`.btn-close`)
```
Position:     Absolute, top-right
Padding:      0
Background:   Transparent
Color:        Inherit
Opacity:      0.5
Font size:    1.5rem
Content:      "×" (multiplication sign)

Hover:
  Opacity:    1
```

### 4.7 Navigation (Sidebar)

**Sidebar** (`.sidebar`)
```
Width:       250px (desktop)
Height:      100vh
Position:    Sticky, top: 0
Background:  Linear gradient (slate to darker slate)
             linear-gradient(180deg, #0f172a 0%, #1e293b 70%)

Mobile:
  Hidden by default
  Toggle button reveals/collapses
```

**Nav Item** (`.nav-item`)
```
Font size:   0.9rem
Padding:     0.5rem bottom

<a>:
  Color:         #d7d7d7 (light grey)
  Height:        3rem
  Display:       Flex, align-items: center
  Border radius: 4px
  Transition:    Background 150ms

  .active:
    Background:  rgba(255,255,255,0.37) (white overlay)
    Color:       white

  :hover:
    Background:  rgba(255,255,255,0.1)
    Color:       white
```

**Navbar Toggle** (Mobile)
```
Width:      3.5rem
Height:     2.5rem
Position:   Absolute, top-right
Border:     1px solid rgba(255,255,255,0.1)
Background: Hamburger icon SVG
```
