**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.company

DaysOn/HrPlan organization service: companies, employees, teams, and publication of their business events.

Documentation based on local files reviewed on 21 October 2026.

<!-- BEGIN project-context -->

## Historical dates and evidence

**Reference year for this version: 2018.** This uses the most recent source or configuration file with a recoverable historical date; the exact creation date could not be confirmed.

- Most recent datable file: **2018-07-02**, [`dayson.company/src/DaysOn.CompanyBc.sln`](src/DaysOn.CompanyBc.sln); date of the commit modifying the file, recovered from local Git history.
- First record in the original Git repository containing this code: **2015-07-24**. This may be an import and does not establish when the code was first written.
- Most recently published dependency among those checked: [Castle.Core 4.3.1](https://www.nuget.org/packages/Castle.Core/4.3.1), published on **2018-06-20** (declared version), in [`dayson.company/src/DaysOn.CompanyBc.Api/packages.config`](src/DaysOn.CompanyBc.Api/packages.config).
- Additional reference: target `v4.6` in [`dayson.company/src/DaysOn.CompanyBc.Api/DaysOn.CompanyBc.Api.csproj`](src/DaysOn.CompanyBc.Api/DaysOn.CompanyBc.Api.csproj); its [stable release was published on 2015-07-20](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). This indicates the technology generation; earlier preview releases may have existed.

A dependency's publication places the preserved configuration at or after that version, if the reference is valid. Package updates may be much newer than the project's beginnings. Packages were not restored to verify this.

October 2026 copy timestamps and documents generated during this review were excluded. Recoverable dates do not guarantee that there are no later, undated changes.

## Why this is worth exploring

The company, team, and employee model publishes events consumed by other DaysOn services. It shows how an organizational change propagates to identity and leave management without concentrating every responsibility in one service.

Code entry points:

- [`src/DaysOn.CompanyBc.Domain/Events`](src/DaysOn.CompanyBc.Domain/Events/)
- [`src/DaysOn.CompanyBc.Api/Controllers/CompanyController.cs`](src/DaysOn.CompanyBc.Api/Controllers/CompanyController.cs)

<!-- END project-context -->

## Structure and responsibilities

| Path | Responsibility |
| --- | --- |
| `src/DaysOn.CompanyBc.Api/` | Company and Version controllers and Web API/OWIN startup. |
| `src/DaysOn.CompanyBc.Domain/` | Company, Employee, and Team entities, persistence, search, and team services. |
| `src/DaysOn.CompanyBc.Models/, src/DaysOn.CompanyBc.SharedKernel/` | Commands, DTOs, and domain support. |
| `src/DaysOn.CompanyBc.Host/` | IIS host. |
| `src/DaysOn.CompanyBc.Test.*/` | Unit, integration, and acceptance tests. |

## Technologies and requirements

Windows, Visual Studio/MSBuild, NuGet packages, and tools for the .NET Framework version declared by each project. HTTP services use Web API/OWIN, and IIS hosts require IIS Express or IIS. Availability of the internal DaysOn.* dependencies in their original feeds has not been checked.

Required services and databases depend on the component's private configuration.

Targets read from project files: `v4.6`.

## Setup and startup

Restore src/DaysOn.CompanyBc.sln and select DaysOn.CompanyBc.Host as the IIS Express startup project. Prepare connections and event exchange with Auth and Holidays.

Solutions present: `src/DaysOn.CompanyBc.sln`.

## Configuration and integrations

The host's Web.config and API/domain App.config files include database, bus, and JWT settings. Review Web.Production.config and Web.Release.config transformations and publish profiles.

## Relationship to the DaysOn application

This folder is a DaysOn/HrPlan component within a distributed application.

- [dayson.auth](../dayson.auth/Readme.md): Identity, tokens, and accounts.
- [dayson.holidays](../dayson.holidays/Readme.md): Leave, rules, approvals, and balances.
- [dayson.common](../dayson.common/Readme.md): Shared libraries, contracts, data, and messaging.
- [dayson.facade](../dayson.facade/Readme.md): Coordination API for web clients.

The [main index](../README.md) includes a map of the application. Relationships described here come from visible controllers, clients, contracts, and configuration; a running deployment has not been verified.

## Status of this copy

Company, employee, and team events feed other DaysOn contexts; Holidays includes dedicated handlers for them.

The review was static: dependencies were not restored, and applications and tests were neither built nor run. The commands above are setup instructions, not validation results. Listed versions describe the preserved code.


## Public copy

Original credentials have been removed or replaced with `REPLACE_WITH_YOUR_VALUE`. Local data, sessions, private certificates, and deployment profiles are omitted. Configure your own services and credentials before running. Building and operation with external services have not been validated.
