# The Artifact-Driven Agentic SDLC Framework**

## Introduction

This framework is intentionally optimized around three ideas that keep delivery objective, ordered, and lean:

**1) Definitions of Done are per-artifact (and per-maturity gate)**  
“Done” is not a single universal statement. Each artifact type (e.g., User Story, UI Prototype, API Contract, Source Code, Test Suite, Runbook) has its own **granular Definition of Done (DoD)** that is explicit at the maturity level being claimed (e.g., “This artifact is ready for implementation” vs “This artifact is release-ready”). Progress is therefore **binary at each gate**: either the artifact meets its DoD for that maturity milestone, or it doesn’t.

**2) Progressive elaboration follows a clear dependency path**  
Artifacts mature in a deliberate sequence. Each artifact should declare its **prerequisites** (which upstream artifacts must exist, and at what maturity) before it is allowed to move forward. This creates a visible dependency path from trigger → decision → implementation → verification, and prevents downstream artifacts from “running ahead” of unresolved upstream uncertainty.

**3) Waste elimination comes from only working in a JIT slot**  
Artifacts do not mature just because they exist in a backlog. Work only proceeds when the Accountable Human places a specific artifact into a **Just-in-Time (JIT) slot** (a scheduled, time-boxed window to mature it to the next gate). Anything not in a JIT slot remains intentionally under-elaborated (often at M1), avoiding speculative detail and preventing inventory of half-finished artifacts.


## 1. The Paradigm Shift: From Verbs to Nouns

Traditional SDLC models focus on **activities** (what people are doing: Designing, Developing, Testing).  

This framework shifts the focus to **Artifacts** (what is being produced).

**The Problem**  
Activity-based progress is subjective. Statements like “I am 80% done coding” provide no tangible evidence of value.

**The Solution**  
Progress is measured by the **maturity** and **sum of delivered artifacts**. Work is only considered “Done” when the required set of artifacts meets their specific Definition of Done (DoD).

## 2. Human-Centered Accountability (The Decision Authority)

In this agentic model, humans are removed from repetitive “doing” but elevated to “deciding.” Humans remain **always and only** the Accountable party.

**Human Decisions:**
- **Approval**: Signing off on an artifact’s maturity (e.g., “The User Story is now ready for development”).
- **Prioritization**: Deciding which artifacts need to mature first based on business value.
- **Scheduling**: Determining deadlines for downstream triggers and release windows.

## 3. The RASCI-A Matrix (Role & Agent Classification)

To manage concurrent development, an adapted RASCI model is used where Agents and Services specialize in specific intersections.

| Role        | Entity                  | Functional Description |
|-------------|-------------------------|------------------------|
| **Accountable** | Human | Sole decision authority. Approves maturity, sets priority, and handles overrides. |
| **Responsible** | Agent (Writer) | The primary “doer.” Creates the draft and iterates based on feedback. |
| **Support**     | Existing Infra / MCP / Worker | Provides the “How.” Includes MCP Servers, centralized services, Inner-source libraries, and internal Runbooks. |
| **Consulted**   | Agent (Oracle) | The “Critic & Scout.” Compares artifacts against standards and discovers existing Support artifacts to prevent re-work. |
| **Informed**    | Orchestrator | Downstream agents or humans notified when an artifact hits a specific milestone. |

## 4. Artifact Maturity Levels (Illustrative)

Maturity levels provide a common language for progress. Different artifact types require tailored scales.

### Standard Illustrative Scale
- **M1: Ideation/Trigger (Vague)** – Initial concepts, automated alerts, or audit findings.
- **M2: Drafted (Structured)** – Formulated into a standard format by a Writer Agent.
- **M3: Critiqued & Discovered (Analyzed)** – The Oracle has scored the artifact and identified existing Support artifacts for reuse.
- **M4: Validated (Human Approved)** – The Human (Accountable) has reviewed the Oracle’s score and approved the path forward.
- **M5: Verified (Realized)** – The artifact is implemented and verified against outcome metrics.

### Example: Specialized Code Artifact Maturity
For technical artifacts like Source Code, maturity is determined by different metrics:
- **Level 1**: Compiled – Syntactically correct and builds in the environment.
- **Level 2**: Unit Tested – Core logic is covered by passing automated tests.
- **Level 3**: Coverage Qualified – Meets the organization's threshold for code coverage (e.g., 80%+).
- **Level 4**: Complexity Checked – Cyclomatic complexity and maintainability scores are within acceptable limits.
- **Level 5**: Peer/Oracle Approved – Security scans and logic reviews are passed.

## 5. Waste Elimination via Just-in-Time (JIT) Maturity

Traditional SDLCs suffer from **pre-emptive over-production** (building massive Epics and User Stories that are later abandoned).

The Artifact-Driven model eliminates this through:

- **Just-in-Time (JIT) Maturation**: Artifacts only move from M1 to M2 when the Accountable Human schedules them. This prevents the “Story Graveyard.”
- **The Feedback Anchor**: Downstream artifacts (e.g., UI Prototype) begin maturing early, allowing real-time adjustment of upstream artifacts.
- **Continuous Pruning**: If an Oracle discovers an existing Support Artifact, downstream work is avoided entirely.
- **Binary Progress**: Work is either maturing toward a Definition of Done or it isn’t — no sunk cost in half-finished activities.

## 6. Agentic Loop Examples

### Example 1: The Idea as a Trigger for a New User Story
- **M1 (Trigger)**: Business user logs an idea → SDLC Orchestrator invokes User Story Orchestrator.
- **Oracle Search**: Scans library via MCP for existing solutions.
- **M2 (Writing)**: User Story Writer formulates the story in Jira.
- **M3 (Review)**: User Story Oracle/Reviewer analyzes and scores it.
- **M4 (Human Decision)**: Product Owner approves the story.
- **Parallelism**: UI Orchestrator is triggered to start a Prototype UI Artifact for early feedback.

### Example 2: The Alert as a Trigger for Performance Improvement
- **M1 (Trigger)**: Monitoring alert (high latency) creates a Performance Artifact.
- **Oracle Search**: Scans Runbooks and previous Incident Reports.
- **M2 (Writing)**: Remediation Writer drafts configuration change or indexing strategy.
- **M3 (Review)**: Performance Reviewer assesses predicted impact.
- **M4 (Human Decision)**: SRE Lead approves the fix and schedules deployment.
- **Parallelism**: Load Test Orchestrator prepares a Benchmark Artifact for verification.

### Example 3: The Security Audit as a Trigger for a Security Intervention
- **M1 (Trigger)**: Automated scan identifies a vulnerability.
- **Oracle Search**: Checks Compliance Library for regulatory requirements.
- **M2 (Writing)**: Security Patch Agent drafts the fix and impact statement.
- **M3 (Review)**: Security Reviewer scores the patch.
- **M4 (Human Decision)**: Security Architect approves for immediate release.
- **Parallelism**: Compliance Orchestrator updates the Audit Log Artifact automatically.

### Example 4: The Production Bug as a Trigger for a Validation Fix
- **M1 (Trigger)**: Server-side exception (500 error) detected in production.
- **Oracle Discovery**: Identifies missing backend validation.
- **M2 (Writing)**: Bug Writer drafts fix using standard Validation Library.
- **M3 (Review)**: Bug Reviewer verifies fix against stack trace.
- **M4 (Human Decision)**: Engineering Lead approves hotfix.
- **Parallelism**: QA Orchestrator creates a Regression Test Artifact.

## 7. Business Value & Consistency

- **Asset Reuse**: Oracle agents prevent rebuilding existing solutions by discovering Support artifacts.
- **Agentification via MCP**: Secure bridging between LLM reasoning and proprietary internal data.
- **Tangible Progress**: Clear, measurable advancement shown by the number of artifacts reaching M4/M5.
- **Outcome-focused**: Progress is directly tied to business value rather than activity volume.

