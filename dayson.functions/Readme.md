**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.functions

DaysOn/HrPlan event processing and supporting operations: leave request validation, business day calculation, and email delivery.

Documentation based on local files reviewed on 21 October 2026.

<!-- BEGIN project-context -->

## Historical dates and evidence

**Reference year for this version: 2018.** This uses the most recent source or configuration file with a recoverable historical date; the exact creation date could not be confirmed.

- Most recent datable file: **2018-06-30**, [`dayson.functions/src/DaysOn.Functions.sln`](src/DaysOn.Functions.sln); date of the commit modifying the file, recovered from local Git history.
- First record in the original Git repository containing this code: **2016-08-02**. This may be an import and does not establish when the code was first written.
- Most recently published dependency among those checked: [Castle.Core 4.3.1](https://www.nuget.org/packages/Castle.Core/4.3.1), published on **2018-06-20** (declared version), in [`dayson.functions/src/DaysOn.Functions.Api/packages.config`](src/DaysOn.Functions.Api/packages.config).
- Additional reference: target `v4.6` in [`dayson.functions/src/DaysOn.Functions.Api/DaysOn.Functions.Api.csproj`](src/DaysOn.Functions.Api/DaysOn.Functions.Api.csproj); its [stable release was published on 2015-07-20](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). This indicates the technology generation; earlier preview releases may have existed.

A dependency's publication places the preserved configuration at or after that version, if the reference is valid. Package updates may be much newer than the project's beginnings. Packages were not restored to verify this.

October 2026 copy timestamps and documents generated during this review were excluded. Recoverable dates do not guarantee that there are no later, undated changes.

## Why this is worth exploring

Handlers connect leave requests, business day calculations, and email delivery. This provides a concrete example of moving coordination work to event consumers and keeping it separate from the API receiving the request.

Code entry points:

- [`src/DaysOn.Functions.Api/BusEventHandlers/CalculateBusinessDaysEventHandler.cs`](src/DaysOn.Functions.Api/BusEventHandlers/CalculateBusinessDaysEventHandler.cs)
- [`src/DaysOn.Functions.Api/BusEventHandlers/AddHolidayRequestEventHandler.cs`](src/DaysOn.Functions.Api/BusEventHandlers/AddHolidayRequestEventHandler.cs)
- [`src/DaysOn.Functions.Domain/Services/CreateHolidayService.cs`](src/DaysOn.Functions.Domain/Services/CreateHolidayService.cs)

<!-- END project-context -->

## Structure and responsibilities

| Path | Responsibility |
| --- | --- |
| `src/DaysOn.Functions.Api/BusEventHandlers/` | AddHolidayRequest, CalculateBusinessDays, and SendEmail handlers. |
| `src/DaysOn.Functions.Domain/` | CreateHolidayService, period validation, and business day calculation. |
| `src/DaysOn.Functions.Models/` | Commands and results. |
| `src/DaysOn.Functions.Host/` | Component IIS/OWIN host. |
| `src/DaysOn.Functions.Test.*/, src/DaysOn.Functions.BoundedContextTests/` | Rule and event tests. |

## Technologies and requirements

Windows, Visual Studio/MSBuild, NuGet packages, and tools for the .NET Framework version declared by each project. HTTP services use Web API/OWIN, and IIS hosts require IIS Express or IIS. Availability of the internal DaysOn.* dependencies in their original feeds has not been checked.

Required services and databases depend on the component's private configuration.

Targets read from project files: `v4.6`.

## Setup and startup

Restore src/DaysOn.Functions.sln and select DaysOn.Functions.Host. Prepare the bus and Holidays data before processing events.

Solutions present: `src/DaysOn.Functions.sln`.

## Configuration and integrations

Database connections, JWT, and messaging are configured in the host and test settings. SendEmailEventHandler.cs and an integration test contain a literal SendGrid key.

## Relationship to the DaysOn application

This folder is a DaysOn/HrPlan component within a distributed application.

- [dayson.holidays](../dayson.holidays/Readme.md): Leave, rules, approvals, and balances.
- [dayson.common](../dayson.common/Readme.md): Shared libraries, contracts, data, and messaging.

The [main index](../README.md) includes a map of the application. Relationships described here come from visible controllers, clients, contracts, and configuration; a running deployment has not been verified.

## Status of this copy

The project uses DaysOn.HolidaysBc and DaysOn.Common.Events services and contracts to process requests.

The review was static: dependencies were not restored, and applications and tests were neither built nor run. The commands above are setup instructions, not validation results. Listed versions describe the preserved code.


## Public copy

Original credentials have been removed or replaced with `REPLACE_WITH_YOUR_VALUE`. Local data, sessions, private certificates, and deployment profiles are omitted. Configure your own services and credentials before running. Building and operation with external services have not been validated.
