**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.facade

DaysOn/HrPlan API facade: exposes interface-oriented operations and coordinates calls to authentication, company, and leave services.

Documentation based on local files reviewed on 21 October 2026.

<!-- BEGIN project-context -->

## Historical dates and evidence

**Reference year for this version: 2018.** This uses the most recent source or configuration file with a recoverable historical date; the exact creation date could not be confirmed.

- Most recent datable file: **2018-06-30**, [`dayson.facade/src/DaysOn.Facade/packages.config`](src/DaysOn.Facade/packages.config); date of the commit modifying the file, recovered from local Git history.
- First record in the original Git repository containing this code: **2015-07-12**. This may be an import and does not establish when the code was first written.
- Most recently published dependency among those checked: [Castle.Core 4.3.1](https://www.nuget.org/packages/Castle.Core/4.3.1), published on **2018-06-20** (declared version), in [`dayson.facade/src/DaysOn.Facade/packages.config`](src/DaysOn.Facade/packages.config).
- Additional reference: target `v4.6` in [`dayson.facade/src/DaysOn.Facade/DaysOn.Facade.csproj`](src/DaysOn.Facade/DaysOn.Facade.csproj); its [stable release was published on 2015-07-20](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). This indicates the technology generation; earlier preview releases may have existed.

A dependency's publication places the preserved configuration at or after that version, if the reference is valid. Package updates may be much newer than the project's beginnings. Packages were not restored to verify this.

October 2026 copy timestamps and documents generated during this review were excluded. Recoverable dates do not guarantee that there are no later, undated changes.

## Why this is worth exploring

The facade translates web client needs into identity, company, and leave service calls. Following a controller explains a coordination API's role and the difference between exposing an operation and implementing its business rules.

Code entry points:

- [`src/DaysOn.Facade/Controllers/Factory.cs`](src/DaysOn.Facade/Controllers/Factory.cs)
- [`src/DaysOn.Facade/Controllers/HolidayController.cs`](src/DaysOn.Facade/Controllers/HolidayController.cs)
- [`src/DaysOn.Facade/Controllers/CompanyController.cs`](src/DaysOn.Facade/Controllers/CompanyController.cs)

<!-- END project-context -->

## Structure and responsibilities

| Path | Responsibility |
| --- | --- |
| `src/DaysOn.Facade/` | Controllers for authentication, companies, employees, leave, rules, approvers, and web views. |
| `src/DaysOn.Facade.Common/` | Service HTTP clients, DTOs, and integration utilities. |
| `src/DaysOn.Facade.Models/` | Models consumed by other projects. |
| `src/DaysOn.Facade.IISHost/` | IIS host. |
| `src/DaysOn.Facade.Tests.Acceptance/, src/DaysOn.Facade.Tests.Integration/` | Service coordination tests. |

## Technologies and requirements

Windows, Visual Studio/MSBuild, NuGet packages, and tools for the .NET Framework version declared by each project. HTTP services use Web API/OWIN, and IIS hosts require IIS Express or IIS. Availability of the internal DaysOn.* dependencies in their original feeds has not been checked.

Required services and databases depend on the component's private configuration.

Targets read from project files: `v4.6`.

## Setup and startup

Restore src/DaysOn.Facade.sln and select DaysOn.Facade.IISHost for IIS Express. Configure Auth, Company, and Holidays first and adjust their base URLs.

Solutions present: `src/DaysOn.Facade.sln`.

## Configuration and integrations

Configure service URLs, JWT, and messaging in configuration files and clients. Tests include login credentials and literal tokens.

## Relationship to the DaysOn application

This folder is a DaysOn/HrPlan component within a distributed application.

- [dayson.auth](../dayson.auth/Readme.md): Identity, tokens, and accounts.
- [dayson.company](../dayson.company/Readme.md): Companies, teams, and employees.
- [dayson.holidays](../dayson.holidays/Readme.md): Leave, rules, approvals, and balances.
- [dayson.ui](../dayson.ui/Readme.md): Application frontend.

The [main index](../README.md) includes a map of the application. Relationships described here come from visible controllers, clients, contracts, and configuration; a running deployment has not been verified.

## Status of this copy

This is the main integration point for dayson.ui; it depends on the domain services being available.

The review was static: dependencies were not restored, and applications and tests were neither built nor run. The commands above are setup instructions, not validation results. Listed versions describe the preserved code.


## Public copy

Original credentials have been removed or replaced with `REPLACE_WITH_YOUR_VALUE`. Local data, sessions, private certificates, and deployment profiles are omitted. Configure your own services and credentials before running. Building and operation with external services have not been validated.
