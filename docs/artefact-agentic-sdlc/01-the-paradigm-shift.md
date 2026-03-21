# 01. The Paradigm Shift: From Verbs to Nouns

## Executive Summary (for executives to developers)

Traditional SDLCs primarily describe **verbs**: what teams do (plan, design, build, test, deploy). Progress is therefore reported in *activity language* ("we’re 80% through development") which is often subjective and hard to audit.

The Artifact-Driven Agentic SDLC describes **nouns**: what the delivery system produces (User Story, UI Prototype, API Contract, Source Code, Test Suite, Runbook, Release Notes, etc.). Progress is measured by the **existence and maturity of those artifacts**, each with an explicit Definition of Done (DoD) at a given maturity gate.

This is knowledge work. Humans bring judgment and creativity, but also have limited context, biases, and shifts in focus that can pull delivery away from the outcome. This system is designed to keep **outputs** anchored to the **outcome** and the **business objective**, while using agents to reduce toil.

This is not “another tool.” It is a **complete delivery system**:

- **Humans stay in the loop** as the accountable decision-makers: they define and approve artifact maturity, set priorities, and decide what ships.
- **Agents reduce toil and increase consistency** by drafting, critiquing, discovering reuse, and maintaining artifacts.
- The operating mindset shifts from **toil-based progress** with occasional creativity → to **continuous creative work** assisted by automation, with toil systematically automated.

---

## How to review this (three parallel tasks)

This content is intentionally structured so it can be evolved in three parallel streams:

- **Task A — Contrast & mindset shift:** Section 2 (current SDLC vs artifact-driven SDLC, and what “output-driven while objective-driven” means).
- **Task B — Definition (system, not tool):** Section 3 (what the new SDLC is, including the specialist-agent “inception” concept).
- **Task C — Benefits:** Section 4 (business benefits, role benefits, customer benefits).

## 1) What is changing (the simple articulation)

- **Old framing (verbs):** “We are doing the work.” Activities and effort are used as proxies for progress.
- **New framing (nouns):** “We are producing outputs.” Artifacts are the primary unit of progress and accountability.

In practice this also clarifies roles:

- Humans provide intent, constraints, approvals, and trade-offs.
- Agents provide scalable, repeatable production and critique of delivery outputs.

Where this gets sharper is the distinction between:

- **Outputs:** The individual artifacts produced by the SDLC (documents, code, tests, runbooks, etc.).
- **Outcome:** The **deployable** that results from accumulating and integrating the required outputs to the target maturity (e.g., a releasable service, a product increment, a compliant change).
- **Business objective:** The **change** the outcome is intended to achieve (for the business customer and other stakeholders), measured by outcome metrics.

In this paradigm:

- The SDLC’s day-to-day movement is **output-driven** (artifact maturity).
- The SDLC’s purpose stays **objective-driven** (measured business change).
- The deployable outcome is the **accumulation** of its artifacts — not a separate thing that appears at the end.

The human-in-the-loop element is what prevents “automation drift”: the system automates toil and standardizes outputs, while humans remain responsible for direction, approval, and alignment to business objectives.

---

## 2) Contrast with the current SDLC (and the mindset shift)

### The current SDLC (typical pattern)

Most SDLCs are managed as a sequence of activities:

- plan → design → implement → test → deploy → operate

This structure is familiar and useful, but it has a predictable failure mode: the system measures motion more easily than it measures tangible evidence.

- Activity status ("in development") is not proof.
- Partial progress is hard to validate.
- Quality, security, and operational readiness are frequently discovered late because they are not “first-class outputs.”

In knowledge work, another failure mode appears:

- Delivery focus can drift due to limited context, bias, and shifting attention across stakeholders.
- “Busy work” (toil) expands to fill uncertainty, and progress gets reported as activity rather than evidence.

### The Artifact-Driven SDLC (what is different)

The delivery system is reorganized around the artifacts that must exist for a deployable outcome to be credible.

- Work is not “done” when activity feels complete.
- Work is “done” when the required artifacts meet the DoD for the claimed maturity gate.

Humans and agents work as a loop:

- **Humans decide**: what matters, what is acceptable, what is prioritized, and what ships.
- **Agents do the repeatable knowledge work**: draft, critique, cross-check, discover reuse, and maintain artifacts to standards.

**Mindset change required:**

- From **effort reporting** (“hours spent” / “percent complete”)…
- To **evidence reporting** (“these artifacts exist and are at M4/M5”).

**Important nuance:** being output-driven does *not* mean losing outcomes.

- Outputs are how the SDLC proves it is converging.
- Outcomes and objectives are how the SDLC proves it matters.

Agents exist to remove toil, not remove accountability. The system is “human-led, agent-assisted” so that creativity and judgment are continuous, and repetitive effort becomes automation.

### A practical comparison

In a traditional verb-centric SDLC, the primary unit of progress is the activity or phase. Status is often described as “in dev” or “in test,” evidence is indirect, and gaps tend to surface late. Control is commonly exercised through schedules, ticket states, and phase gates, and the deployable can be treated as something that “shows up at the end.”

In the Artifact-Driven Agentic SDLC, the primary unit of progress is the artifact and its maturity. Status is described as “Artifact X is M3/M4 and meets its DoD,” evidence is direct and auditable, and gaps show up early because missing or immature artifacts are visible. Control is exercised through prerequisites and Just-in-Time maturation slots, and the deployable outcome is the integrated accumulation of artifacts.

---

## 3) What the new SDLC is (not a tool — a system)

This framework is not a product or a platform you “buy.” It is a **delivery operating system** that combines:

- **Standards:** artifact definitions, templates, and per-artifact DoD (per maturity gate)
- **Flow control:** prerequisite chains, dependency paths, and Just-in-Time (JIT) maturation slots
- **Accountability:** humans are accountable for decisions; agents are responsible for producing and maturing artifacts
- **Specialization at scale:** a continuous production of agents that specialize in specific artifact outputs

Put simply: this SDLC is **human-led and agent-assisted**. Humans supply purpose (outcomes/objectives), judgment, and accountability. Agents supply repeatable execution and critique so the system can stay focused even when human attention shifts or bias creeps in.

### Why “system” matters

If the unit of delivery is the artifact, then:

- Every artifact type needs **repeatable production quality** (templates, standards, critique loops).
- Every artifact needs **maturity and maintenance** over time.

So it is not sufficient to only create a code module or a document and then depend on generalist agents (or generalist humans) to keep it coherent indefinitely.

### The specialist-agent model (and the inception problem)

In this SDLC:

- Many artifacts should have a corresponding **specialist agent** that can maintain and evolve that artifact type.
- In some cases, an output of an authoring agent **is itself a specialist agent**.

That “inception” is intentional:

- You don’t just produce a policy document; you may produce a **Policy Maintainer Agent** that keeps the document aligned with standards and changes.
- You don’t just produce a runbook; you may produce a **Runbook Maintainer Agent** that keeps it accurate as systems evolve.
- You don’t just produce a test suite; you may produce a **Test Maintainer Agent** that keeps coverage relevant as the domain changes.

The system therefore continuously grows a library of maintained outputs (artifacts) and maintained maintainers (specialist agents) — with human accountability on decisions and release.

---

## 4) Benefits (business, roles, customers)

### Business benefits

- **More reliable visibility:** progress is evidenced by artifacts meeting DoD, not subjective completion estimates.
- **Better governance:** artifact maturity gates create auditable controls for security, compliance, and readiness.
- **Earlier risk exposure:** missing artifacts or low maturity becomes visible before deployment pressure.
- **Less rework:** oracle/critic functions increase reuse by discovering existing support artifacts.
- **More predictable delivery:** JIT maturation + prerequisites reduce churn and premature elaboration.

### Benefits to delivery roles (from executives to developers)

- **Executives / Sponsors:** clearer investment tracking (what outputs exist, what outcome is ready, and what objective is targeted).
- **Product Owners:** better control over scope and sequencing via artifact prerequisites and DoD-based approvals.
- **Architects / Tech Leads:** fewer hidden dependencies; earlier alignment through explicit contracts and standards.
- **Developers:** higher-quality inputs (stories, contracts, designs) reduce ambiguity and thrash; clearer “done.”
- **QA / Test:** testing becomes a first-class artifact with explicit maturity, not a phase that is squeezed.
- **SRE / Operations:** runbooks, monitoring, and operational readiness are explicit outputs, not afterthoughts.
- **Security / Compliance:** evidence is produced continuously as artifacts mature, rather than compiled at the end.

### Customer benefits

- **Faster delivery of usable value:** less time wasted on speculative work and late-cycle surprises.
- **Higher quality and reliability:** readiness artifacts (tests, runbooks, monitoring) mature alongside code.
- **More consistent outcomes:** objective-driven measurement improves alignment between what’s shipped and what customers need.
