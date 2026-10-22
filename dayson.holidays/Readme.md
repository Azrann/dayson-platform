**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.holidays

DaysOn/HrPlan leave and absence service: requests, approvals, company rules, work calendar, and annual balances.

Documentation based on local files reviewed on 21 October 2026.

<!-- BEGIN project-context -->

## Historical dates and evidence

**Reference year for this version: 2018.** This uses the most recent source or configuration file with a recoverable historical date; the exact creation date could not be confirmed.

- Most recent datable file: **2018-06-30**, [`dayson.holidays/src/DaysOn.HolidaysBc.Test.Unit/packages.config`](src/DaysOn.HolidaysBc.Test.Unit/packages.config); date of the commit modifying the file, recovered from local Git history.
- First record in the original Git repository containing this code: **2015-11-24**. This may be an import and does not establish when the code was first written.
- Most recently published dependency among those checked: [Castle.Core 4.3.1](https://www.nuget.org/packages/Castle.Core/4.3.1), published on **2018-06-20** (declared version), in [`dayson.holidays/src/DaysOn.HolidaysBc.Api/packages.config`](src/DaysOn.HolidaysBc.Api/packages.config).
- Additional reference: target `v4.6` in [`dayson.holidays/src/DaysOn.HolidaysBc.Api/DaysOn.HolidaysBc.Api.csproj`](src/DaysOn.HolidaysBc.Api/DaysOn.HolidaysBc.Api.csproj); its [stable release was published on 2015-07-20](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). This indicates the technology generation; earlier preview releases may have existed.

A dependency's publication places the preserved configuration at or after that version, if the reference is valid. Package updates may be much newer than the project's beginnings. Packages were not restored to verify this.

October 2026 copy timestamps and documents generated during this review were excluded. Recoverable dates do not guarantee that there are no later, undated changes.

## Why this is worth exploring

Company rules, approvals, closed dates, and annual balances form a domain richer than simple CRUD. It is worth following how these concepts combine and how employees and teams synchronize through events.

Code entry points:

- [`src/DaysOn.HolidaysBc.Domain`](src/DaysOn.HolidaysBc.Domain/)
- [`src/DaysOn.HolidaysBc.Api/BusEventHandlers`](src/DaysOn.HolidaysBc.Api/BusEventHandlers/)
- [`src/DaysOn.HolidaysBc.Api/Controllers`](src/DaysOn.HolidaysBc.Api/Controllers/)

<!-- END project-context -->

## Structure and responsibilities

| Path | Responsibility |
| --- | --- |
| `src/DaysOn.HolidaysBc.Api/` | APIs for requests, approvers, rules, administration, open/closed dates, and annual calculations. |
| `src/DaysOn.HolidaysBc.Api/BusEventHandlers/` | Company, employee, and team synchronization through events. |
| `src/DaysOn.HolidaysBc.Domain/` | Repositories and services for employees, teams, requests, rules, and calculations. |
| `src/DaysOn.HolidaysBc.Models/` | Commands and DTOs. |
| `src/DaysOn.HolidaysBc.Host/` | IIS host. |
| `src/DaysOn.HolidaysBc.Test.*/` | Unit, integration, and acceptance tests. |

## Technologies and requirements

Windows, Visual Studio/MSBuild, NuGet packages, and tools for the .NET Framework version declared by each project. HTTP services use Web API/OWIN, and IIS hosts require IIS Express or IIS. Availability of the internal DaysOn.* dependencies in their original feeds has not been checked.

Required services and databases depend on the component's private configuration.

Targets read from project files: `v4.6`.

## Setup and startup

Restore src/DaysOn.HolidaysBc.sln, prepare SQL/NoSQL, and select DaysOn.HolidaysBc.Host. Align events with Company and validation with Functions.

Solutions present: `src/DaysOn.HolidaysBc.sln`.

## Configuration and integrations

Review host and domain connections, JWT, NoSQL, bus settings, publish profiles, and deployment transformations. Tests reuse private configuration.

## Relationship to the DaysOn application

This folder is a DaysOn/HrPlan component within a distributed application.

- [dayson.company](../dayson.company/Readme.md): Companies, teams, and employees.
- [dayson.functions](../dayson.functions/Readme.md): Event processing, business days, and email.
- [dayson.auth](../dayson.auth/Readme.md): Identity, tokens, and accounts.
- [dayson.facade](../dayson.facade/Readme.md): Coordination API for web clients.

The [main index](../README.md) includes a map of the application. Relationships described here come from visible controllers, clients, contracts, and configuration; a running deployment has not been verified.

## Status of this copy

Balances and available days depend on company rules, the leave year, business days, and closed dates.

The review was static: dependencies were not restored, and applications and tests were neither built nor run. The commands above are setup instructions, not validation results. Listed versions describe the preserved code.


## Public copy

Original credentials have been removed or replaced with `REPLACE_WITH_YOUR_VALUE`. Local data, sessions, private certificates, and deployment profiles are omitted. Configure your own services and credentials before running. Building and operation with external services have not been validated.
