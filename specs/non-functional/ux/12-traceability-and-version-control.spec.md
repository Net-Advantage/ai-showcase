# UX Design — Traceability and Version Control

**Version**: 2.1  
**Last Updated**: February 15, 2026

## 12. Traceability and Version Control

(Renumbered from former § 10)

### 12.1 Design Token Mapping

All 41 design tokens in `app.css` are documented in this specification:
- § 2.1: Color palette (16 tokens)
- § 2.2: Typography (13 tokens)
- § 2.3: Spacing (7 tokens)
- § 2.4: Border radius (3 tokens)
- § 2.5: Shadows (3 tokens)
- § 2.6: Animation timing (2 tokens)

### 12.2 Component Coverage

All implemented components documented:
- § 4.1: Buttons
- § 4.2: Form controls
- § 4.3: Cards and panels
- § 4.4: Results display
- § 4.5: Collapsible details
- § 4.6: Alerts
- § 4.7: Navigation

### 12.3 Accessibility Compliance

WCAG 2.1 Level AA requirements:
- § 6.1: Standards overview
- § 6.2: Focus indicators
- § 6.3: Semantic HTML
- § 6.4: ARIA support
- § 6.5: Screen reader support
- § 6.6: Keyboard navigation
- § 6.7: Color contrast (verified ratios)
- § 6.8: Touch targets

**Test Coverage:** AccessibilityTests.cs validates all requirements

---

## Document Control

**Version History:**

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | [Original] | [Original Author] | Initial style direction specification |
| 2.0 | 2026-02-13 | Product Owner (AI) | Comprehensive update to reflect implementation:<br>• All 41 design tokens documented with values<br>• Complete color palette with verified contrast ratios<br>• Typography system fully specified<br>• All components cataloged with CSS specifications<br>• Accessibility features documented (WCAG AA)<br>• Clarified sidebar navigation (not top nav)<br>• Clarified app layout (not marketing site)<br>• Added implementation guidelines<br>• Added testing and QA section<br>• Added complete traceability |
| 2.1 | 2026-02-15 | Product Owner (AI) | Added § 11 Component Reuse and Duplication Reduction:<br>• General principles (single source of truth, component tiers, parameterisation)<br>• Blazor-specific guidance (component structure, parameter design, CSS isolation, RenderFragment, CascadingValue)<br>• Web-specific guidance (template functions, Web Components, CSS sharing, event delegation)<br>• Cross-platform consistency checklist<br>• Renumbered former § 10 → § 12 |

**Review Status**: ✅ Ready for stakeholder review

**Implementation Status**: ✅ Fully implemented and production-ready

---

*End of Site Style Specification v2.1*
