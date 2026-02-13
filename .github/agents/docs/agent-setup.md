# Agent Setup

## Models

| Sub-agent                                               | Best default model    | Why this model fits best                                                                                                                                                                                                                                           |
| ------------------------------------------------------- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Product Owner (Orchestrator)                            | **Claude Sonnet 4.5** | Copilot explicitly positions Sonnet 4.5 as strong for “general-purpose coding and agent tasks,” which matches orchestration, delegation, and keeping multi-threaded work coherent. ([GitHub Docs][1])                                                              |
| Domain Expert                                           | **Claude Opus 4.6**   | Domain extraction from specs benefits from maximum reasoning quality and careful synthesis. Copilot lists Opus 4.6 as Anthropic’s most powerful option for complex problem-solving. ([GitHub Docs][1])                                                             |
| Task Planner                                            | **Gemini 2.5 Pro**    | Planning is structured reasoning across many constraints and dependencies. Copilot recommends Gemini 2.5 Pro for complex reasoning, debugging, and research-like workflows, which aligns with producing robust plans and traceability matrices. ([GitHub Docs][1]) |
| Architect                                               | **GPT-5.2**           | Architecture work needs deep reasoning, trade-off evaluation, and multi-step analysis. Copilot recommends GPT-5.2 for complex reasoning and technical decision-making. ([GitHub Docs][1])                                                                          |
| UX Designer (Design system, no code)                    | **GPT-5.2**           | This role is heavy on clear writing, systematisation (tokens, components, states), and consistency. Copilot positions GPT-5.2 as strong for general-purpose coding and writing with fast, accurate explanations. ([GitHub Docs][1])                                |
| Frontend Developer (Blazor, MVVM, CSS)                  | **GPT-5.1-Codex**     | Frontend implementation is lots of “ship code” work: components, refactors, wiring, and fixes. Copilot recommends GPT-5.1-Codex for higher-quality code on complex engineering tasks like features, tests, debugging, and refactors. ([GitHub Docs][1])            |
| Backend Developer (.NET, ASP.NET, EF Core, SQL, Aspire) | **GPT-5.1-Codex**     | Same reasoning as frontend, plus more multi-file changes (API, EF mappings, migrations, Aspire wiring). GPT-5.1-Codex is explicitly recommended for complex engineering tasks across features and debugging. ([GitHub Docs][1])                                    |
| Quality Assurance (scenarios, permutations, Playwright) | **Gemini 3 Pro**      | QA needs deep reasoning over permutations and observable behaviour, plus solid automation output. Copilot positions Gemini 3 Pro for complex reasoning and research-like workflows, which fits scenario design and permutation coverage. ([GitHub Docs][1])        |
| Implementation Engineer (docs in `/docs`)               | **GPT-5 mini**        | Docs benefit from fast, accurate writing and structured output without “overthinking.” Copilot recommends GPT-5 mini as a reliable default for coding and writing tasks. ([GitHub Docs][1])                                                                        |

[1]: https://docs.github.com/en/copilot/reference/ai-models/model-comparison "AI model comparison - GitHub Docs"


## Tools

| Sub-agent                        | Suggested Tools                                      | Why These Tools                                                                           |
| -------------------------------- | ---------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| **Product Owner (Orchestrator)** | `search`, `read`, `agent`, `memory`                  | Search for files, Read files, orchestrate other agents, keep the process in mind.         |
| **Domain Expert**                | `read`, `search`, `memory`                           | Only reads `/specs` and extracts requirements. Should never edit files.                   |
| **Task Planner**                 | `vscode`, `search`, `read`, `edit`, `memory`, `todo` | Produces planning output but does not modify repo directly in v1.                         |
| **Architect**                    | `vscode`, `search`, `read`, `edit`, `memory`, `todo` | Designs architecture, references codebase structure, but does not implement changes.      |
| **UX Designer**                  | `vscode`, `search`, `read`, `edit`, `memory`, `todo` | Works from specs and produces structured design output. No direct code edits in v1.       |
| **Frontend Developer**           | `vscode`, `search`, `read`, `edit`, `memory`, `todo` | Must implement Blazor, MVVM, CSS. Needs write capability.                                 |
| **Backend Developer**            | `vscode`, `search`, `read`, `edit`, `memory`, `todo` | Must implement .NET API, EF Core, migrations. Needs write capability.                     |
| **Quality Assurance**            | `vscode`, `search`, `read`, `edit`, `memory`, `todo` | Writes Playwright tests and test artifacts. Needs write capability.                       |
| **Implementation Engineer**      | `vscode`, `search`, `read`, `edit`, `memory`, `todo` | Must create documentation in `/docs`. Needs write capability.                             |


## Tool Inventory

The following table shows a consolidated list of tools that we can use in our agent setup.

| Tool Name              | Category                    | Where It Comes From                                  | What It Is Used For |
|-----------------------|----------------------------|------------------------------------------------------|---------------------|
| search                | Codebase / Workspace       | VS Code agent / codebase provider                    | Search files, symbols, or text across the repo |
| read                  | Codebase / Workspace       | VS Code agent                                        | Read files and understand project structure |
| editFiles             | Codebase / Workspace       | VS Code agent                                        | Modify or create files in the repository |
| changes               | Codebase / Workspace       | VS Code agent                                        | Inspect diffs or changes in files |
| usages                | Codebase / Workspace       | VS Code agent                                        | Find references/usages of functions or symbols |
| problems              | Diagnostics                | VS Code language server                              | Surface compile errors, lint issues |
| testFailure           | Diagnostics                | VS Code test integration                             | Identify failing tests |
| findTestFiles         | Testing                    | VS Code agent                                        | Locate test files in the repo |
| searchResults         | Codebase / Workspace       | VS Code agent                                        | Structured results from search operations |
| vscodeAPI             | IDE Integration            | VS Code extension API                                | Interact with editor features and extensions |
| extensions            | IDE Integration            | VS Code                                              | Discover installed extensions |
| openSimpleBrowser     | Browser / UI               | VS Code                                              | Open a simple browser view for content |
| fetch                 | Web / MCP                  | Built-in web tool (headless browser)                 | Fetch web pages as markdown for research :contentReference[oaicite:1]{index=1} |
| web                   | External / MCP             | MCP servers                                          | Search internet or external sources |
| githubRepo            | External / MCP             | GitHub MCP server                                    | Interact with repositories |
| github/search_code    | External / MCP             | GitHub MCP server                                    | Search code on GitHub |
| github/search_issues  | External / MCP             | GitHub MCP server                                    | Search issues |
| github/search_repositories | External / MCP       | GitHub MCP server                                    | Discover repositories |
| runCommands           | Execution                  | VS Code terminal                                     | Run CLI commands |
| runTasks              | Execution                  | VS Code tasks                                        | Run defined tasks (build, test, etc.) |
| terminalSelection     | Execution                  | VS Code terminal                                     | Access selected terminal output |
| terminalLastCommand   | Execution                  | VS Code terminal                                     | Inspect last executed command |
| runNotebooks          | Execution                  | VS Code notebooks                                    | Execute notebook cells |
| new                   | File Management            | VS Code agent                                        | Create new files or resources |
| todo                  | Agent Workflow             | Copilot agent system                                 | Maintain task lists for progress tracking |
| agent                 | Multi-agent orchestration  | Copilot agent system                                 | Call other agents |
| memory                | Agent state                | Copilot agent system                                 | Persist or recall structured context |
| context7/*            | MCP / Knowledge            | Context7 MCP server                                  | Retrieve curated docs and structured knowledge :contentReference[oaicite:2]{index=2} |


### Where the Tools Come From

| Source Category          | Where It Comes From                              | Description                                           | What They Do                                                                                 | Requires Setup |
|------------------------|--------------------------------------------------|-------------------------------------------------------|---------------------------------------------------------------------------------------------|----------------|
| VS Code Built-in Tools | VS Code / Copilot Agent runtime                  | Tools exposed from the editor environment             | Read files, search code, edit files, run commands, inspect problems, navigate workspace     | No |
| Codebase Provider      | VS Code language services & indexer              | Structured access to repository context               | Symbol search, references, diagnostics, test discovery, code understanding                  | No |
| Execution Environment  | VS Code terminal & task system                   | Access to runtime environment                         | Run CLI commands, build projects, run tests, execute notebooks                              | No |
| Copilot Agent Runtime  | GitHub Copilot agent framework                   | Internal orchestration capabilities                   | Delegate to other agents, manage tasks, store memory, coordinate workflows                  | No |
| Web Tooling            | Built-in web/fetch providers                     | Safe browsing capability for agents                   | Fetch documentation, scrape web pages, retrieve external knowledge                          | No |
| MCP Servers            | Model Context Protocol ecosystem                 | External tool providers via a standard protocol       | Extend agents with APIs, data sources, documentation, GitHub access, custom capabilities    | Yes |
| GitHub MCP Server      | GitHub-provided MCP server                       | GitHub integration via MCP                            | Read repos, search code, manage issues/PRs, automate workflows                              | Yes |
| Context Providers      | MCP servers (e.g. context7)                      | Curated knowledge sources                             | Retrieve structured docs, API references, domain knowledge                                  | Yes |
| Custom MCP Servers     | Your own services / APIs exposed via MCP         | Custom tool layer for your organisation               | Expose business logic, workflows, internal systems to agents                                | Yes |
| External MCP Registry  | MCP marketplaces / registries                    | Discoverable third-party MCP servers                  | Install new tools (e.g. YouTube, GitHub, data APIs, cloud services)                         | Yes |

**Setup Requirements**

| Source Category       | Setup Type                  | What You Need To Do                                         | Key Config / Example                                       | Notes |
|----------------------|----------------------------|-------------------------------------------------------------|-------------------------------------------------------------|------|
| MCP Servers (General) | Install + Configure         | Add MCP servers to your agent environment                   | mcp.json config with command or URL                         | Foundation for all external tools |
| GitHub MCP Server     | Install + Auth              | Install GitHub MCP server and authenticate                  | Use npx or package install + GitHub token                    | Enables repo, PR, issue access |
| Context Providers     | Install + Configure         | Install documentation MCP servers (e.g. context7)           | Add server to mcp.json                                       | Used for trusted documentation |
| Custom MCP Servers    | Build + Host                | Create your own MCP server exposing APIs                     | Implement MCP protocol + run locally or host                 | Your strategic advantage |
| MCP Registry          | Discover + Install          | Browse and install third-party MCP servers                   | Use VS Code UI or CLI                                        | Optional but powerful |
| Remote MCP Servers    | Configure Endpoint          | Connect to hosted MCP services                              | Provide URL + authentication                                | Useful for shared services |
| Authenticated APIs    | Secrets / Credentials       | Provide API keys, tokens, or OAuth credentials              | Environment variables or secure storage                      | Required for most real integrations |


