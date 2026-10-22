**Languages:** [English](README.md) · [Español](README.es.md)

# dayson-platform

DaysOn/HrPlan application: identity, companies, leave, messaging, logging, and web clients.

<!-- BEGIN project-context -->

## Historical dates and evidence

**Reference year for this version: 2018.** This uses the most recent source or configuration file with a recoverable historical date; the exact creation date could not be confirmed.

- Most recent datable file: **2018-07-02**, [`dayson.auth/src/DaysOn.Auth.Web/Providers/CustomOAuthProvider.cs`](dayson.auth/src/DaysOn.Auth.Web/Providers/CustomOAuthProvider.cs); date of the commit modifying the file, recovered from local Git history.
- Earliest record among the original Git repositories included: **2015-07-12**. This may be an import and does not establish when the code was first written.
- Most recently published dependency among those checked: [Castle.Core 4.3.1](https://www.nuget.org/packages/Castle.Core/4.3.1), published on **2018-06-20** (declared version), in [`dayson.auth/src/DaysOn.Auth.Acceptance/packages.config`](dayson.auth/src/DaysOn.Auth.Acceptance/packages.config).
- Additional reference: target `v4.6.1` in [`dayson.common/src/DaysOn.Common.ServiceBase.Test/DaysOn.Common.ServiceBase.Test.csproj`](dayson.common/src/DaysOn.Common.ServiceBase.Test/DaysOn.Common.ServiceBase.Test.csproj); its [stable release was published on 2015-11-30](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). This indicates the technology generation; earlier preview releases may have existed.

A dependency's publication places the preserved configuration at or after that version, if the reference is valid. Package updates may be much newer than the project's beginnings. Packages were not restored to verify this.

October 2026 copy timestamps and documents generated during this review were excluded. Recoverable dates do not guarantee that there are no later, undated changes.

Reference years by component (most recent datable file):

| Component | Year |
| --- | --- |
| [`dayson.auth`](dayson.auth/) | 2018 |
| [`dayson.common`](dayson.common/) | 2018 |
| [`dayson.company`](dayson.company/) | 2018 |
| [`dayson.facade`](dayson.facade/) | 2018 |
| [`dayson.functions`](dayson.functions/) | 2018 |
| [`dayson.holidays`](dayson.holidays/) | 2018 |
| [`dayson.log`](dayson.log/) | 2016 |
| [`dayson.logapi`](dayson.logapi/) | 2016 |
| [`dayson.tower`](dayson.tower/) | Undetermined |
| [`dayson.ui`](dayson.ui/) | 2018 |

## Why this is worth exploring

The leave request → events → business day calculation → approval flow connects several microservices to a recognizable business problem. The frontend calendar shows the result of that separation of responsibilities.

Code entry points:

- [`dayson.holidays/src/DaysOn.HolidaysBc.Domain`](dayson.holidays/src/DaysOn.HolidaysBc.Domain/)
- [`dayson.functions/src/DaysOn.Functions.Api/BusEventHandlers`](dayson.functions/src/DaysOn.Functions.Api/BusEventHandlers/)
- [`dayson.ui/v2/src/app/holidays/calendar`](dayson.ui/v2/src/app/holidays/calendar/)

<!-- END project-context -->

## Contents

| Directory | Function |
| --- | --- |
| [`dayson.auth/`](dayson.auth/) | DaysOn/HrPlan identity service: registration, login, token issuance, account activation, password recovery, and team-related role assignment. |
| [`dayson.common/`](dayson.common/) | Shared libraries and infrastructure for DaysOn/HrPlan components. |
| [`dayson.company/`](dayson.company/) | DaysOn/HrPlan organization service: companies, employees, teams, and publication of their business events. |
| [`dayson.facade/`](dayson.facade/) | DaysOn/HrPlan API facade: exposes interface-oriented operations and coordinates calls to authentication, company, and leave services. |
| [`dayson.functions/`](dayson.functions/) | DaysOn/HrPlan event processing and supporting operations: leave request validation, business day calculation, and email delivery. |
| [`dayson.holidays/`](dayson.holidays/) | DaysOn/HrPlan leave and absence service: requests, approvals, company rules, work calendar, and annual balances. |
| [`dayson.log/`](dayson.log/) | DaysOn log viewer built with AngularJS, jQuery, and Bootstrap. |
| [`dayson.logapi/`](dayson.logapi/) | DaysOn log querying and cleanup API, using SQL storage and Dapper data access. |
| [`dayson.tower/`](dayson.tower/) | DaysOn repository folder without working files: only the .git directory was found. |
| [`dayson.ui/`](dayson.ui/) | DaysOn/HrPlan interface for authentication, company/team/employee administration, and leave viewing or approval. Includes two frontend generations. |

The dayson.* directories are microservices and libraries belonging to a single application. Each retains its solution and configuration. dayson.tower contained no implementation.

## Requirements and setup

.NET Framework, ASP.NET Web API/OWIN, IIS, Azure Functions, NuGet, and web clients.

This is a historical copy. Consult each component's project files and README for versions and dependencies. Restore NuGet/npm from the appropriate folder; internal libraries may need rebuilding. Prepare databases and external services using your own accounts.

Preserved solutions:

- `dayson.auth/src/DaysOn.Auth.sln`
- `dayson.common/src/DaysOn.Common.sln`
- `dayson.company/src/DaysOn.CompanyBc.sln`
- `dayson.facade/src/DaysOn.Facade.sln`
- `dayson.functions/src/DaysOn.Functions.sln`
- `dayson.holidays/src/DaysOn.HolidaysBc.sln`
- `dayson.log/src/DaysonLog.sln`
- `dayson.logapi/src/DaysOn.Log.IISHost.sln`

Main JavaScript packages:

- `dayson.ui/src/package.json`
- `dayson.ui/v2/package.json`

## Configuration and security

Original secrets have been removed or replaced with `REPLACE_WITH_YOUR_VALUE`. Search for this marker before running and provide values through local configuration or environment variables. Sessions, private keys, signing certificates, databases, logs, deployment profiles, and binary packages are not published. The `.gitignore` protects these local files.

A fresh history is published: old commits remain only in the original local copy. Example values cannot connect to the original services.

## Validation status

A static review and secret scan were performed before publication. Applications, trading operations, deployments, and tests contacting external services were not run. Building with current tools is not guaranteed. Some projects need historical dependencies and assets that are not included.

## Licenses and provenance

Third-party licenses and attributions present in the code are retained. No blanket license is added to components that lacked one. This import preserves local sources and does not claim to be a current version of the original project.
