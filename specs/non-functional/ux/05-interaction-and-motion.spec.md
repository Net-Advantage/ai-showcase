# UX Design — Interaction and Motion

**Version**: 2.1  
**Last Updated**: February 15, 2026

## 5. Interaction and Motion

### 5.1 Interactive States

**All Interactive Elements:**
- Default state with appropriate cursor
- Hover state (lighter/brighter color)
- Active state (even lighter)
- Focus state (visible outline)
- Disabled state (reduced opacity)

**Transition Timing:**
- Hover/focus: 150ms (feels immediate)
- State changes: 250ms (visible but not slow)
- Easing: ease-in-out (natural acceleration/deceleration)

### 5.2 Animations

**Fade In** (`.calculator-results`)
```css
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
Duration: 250ms ease-in-out
```

**Spinner** (`.spinner-border`)
```css
@keyframes spinner-border {
  to { transform: rotate(360deg); }
}
Duration: 750ms linear infinite
Visual: Rotating border (right side transparent)
```

**Principles:**
- Use motion to direct attention (results appearing)
- Provide feedback for loading states (spinner)
- Never distract from content
- Always respect `prefers-reduced-motion`
