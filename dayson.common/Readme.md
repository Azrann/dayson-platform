**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.common

Shared libraries and infrastructure for DaysOn/HrPlan components.

Documentation based on local files reviewed on 21 October 2026.

<!-- BEGIN project-context -->

## Historical dates and evidence

**Reference year for this version: 2018.** This uses the most recent source or configuration file with a recoverable historical date; the exact creation date could not be confirmed.

- Most recent datable file: **2018-07-02**, [`dayson.common/src/DaysOn.Common/packages.config`](src/DaysOn.Common/packages.config); date of the commit modifying the file, recovered from local Git history.
- First record in the original Git repository containing this code: **2016-01-25**. This may be an import and does not establish when the code was first written.
- Most recently published dependency among those checked: [JustEat.StatsD 3.0.0](https://www.nuget.org/packages/JustEat.StatsD/3.0.0), published on **2018-06-20** (declared version), in [`dayson.common/src/DaysOn.Common/packages.config`](src/DaysOn.Common/packages.config).
- Additional reference: target `v4.6.1` in [`dayson.common/src/DaysOn.Common.ServiceBase.Test/DaysOn.Common.ServiceBase.Test.csproj`](src/DaysOn.Common.ServiceBase.Test/DaysOn.Common.ServiceBase.Test.csproj); its [stable release was published on 2015-11-30](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). This indicates the technology generation; earlier preview releases may have existed.

A dependency's publication places the preserved configuration at or after that version, if the reference is valid. Package updates may be much newer than the project's beginnings. Packages were not restored to verify this.

October 2026 copy timestamps and documents generated during this review were excluded. Recoverable dates do not guarantee that there are no later, undated changes.

## Why this is worth exploring

Bus abstractions, shared contracts, and infrastructure utilities show what the microservices share and where they remain separate. AzureServiceBus is a good entry point for following event-based communication.

Code entry points:

- [`src/DaysOn.Common/AzureBus/AzureServiceBus.cs`](src/DaysOn.Common/AzureBus/AzureServiceBus.cs)
- [`src/DaysOn.Common/AzureBus/IServiceBus.cs`](src/DaysOn.Common/AzureBus/IServiceBus.cs)
- [`src/DaysOn.Common/MessageHandlers`](src/DaysOn.Common/MessageHandlers/)

<!-- END project-context -->

## Structure and responsibilities

| Path | Responsibility |
| --- | --- |
| `src/DaysOn.Common/` | Azure Service Bus and PubNub messaging and message handlers. |
| `src/DaysOn.Common.Events/` | Cross-service event contracts. |
| `src/DaysOn.Common.DataAccess/, src/DaysOn.Common.SqlDataAccess/` | Persistence and data access utilities. |
| `src/DaysOn.Common.Domain/, src/DaysOn.Common.ServiceBase/, src/DaysOn.Common.Utilities/` | Domain events, OWIN startup, dependency injection, and utilities. |
| `src/DaysOn.Common.CloudSites/, src/DaysOn.Common.CronService/, src/Scale/` | GearHost hosting and database integration and auxiliary tools. |
| `src/WindowsService1/` | Auxiliary Windows service project. |

## Technologies and requirements

Windows, Visual Studio/MSBuild, NuGet packages, and tools for the .NET Framework version declared by each project. HTTP services use Web API/OWIN, and IIS hosts require IIS Express or IIS. Availability of the internal DaysOn.* dependencies in their original feeds has not been checked.

Required services and databases depend on the component's private configuration.

Targets read from project files: `v4.5.2`, `v4.6`, `v4.6.1`.

## Setup and startup

Restore and build src/DaysOn.Common.sln. Other repositories consume the libraries through DaysOn.Common.* packages; review .nuspec files before preparing local packages.

Solutions present: `src/DaysOn.Common.sln`.

## Configuration and integrations

Subscriber tests use NoSQL, PubNub, Ably, and Service Bus. Hosting tools read gearhostkey; HrPlanCloudService also contains database credentials in code.

## Relationship to the DaysOn application

This folder is a DaysOn/HrPlan component within a distributed application.

- [dayson.auth](../dayson.auth/Readme.md): Identity, tokens, and accounts.
- [dayson.company](../dayson.company/Readme.md): Companies, teams, and employees.
- [dayson.holidays](../dayson.holidays/Readme.md): Leave, rules, approvals, and balances.
- [dayson.functions](../dayson.functions/Readme.md): Event processing, business days, and email.
- [dayson.facade](../dayson.facade/Readme.md): Coordination API for web clients.

The [main index](../README.md) includes a map of the application. Relationships described here come from visible controllers, clients, contracts, and configuration; a running deployment has not been verified.

## Status of this copy

Hosting administration functions can create or modify infrastructure. Scaling tools, cron jobs, and connected tests were not run.

The review was static: dependencies were not restored, and applications and tests were neither built nor run. The commands above are setup instructions, not validation results. Listed versions describe the preserved code.


## Public copy

Original credentials have been removed or replaced with `REPLACE_WITH_YOUR_VALUE`. Local data, sessions, private certificates, and deployment profiles are omitted. Configure your own services and credentials before running. Building and operation with external services have not been validated.
