**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.logapi

DaysOn log querying and cleanup API, using SQL storage and Dapper data access.

Documentation based on local files reviewed on 21 October 2026.

<!-- BEGIN project-context -->

## Historical dates and evidence

**Reference year for this version: 2016.** This uses the most recent source or configuration file with a recoverable historical date; the exact creation date could not be confirmed.

- Most recent datable file: **2016-12-18**, [`dayson.logapi/src/DaysOn.Log.IISHost/Web.config`](src/DaysOn.Log.IISHost/Web.config); date of the commit modifying the file, recovered from local Git history.
- First record in the original Git repository containing this code: **2016-11-20**. This may be an import and does not establish when the code was first written.
- Most recently published dependency among those checked: [Dapper 1.50.2](https://www.nuget.org/packages/Dapper/1.50.2), published on **2016-07-22** (declared version), in [`dayson.logapi/src/DaysOn.Log.Web/packages.config`](src/DaysOn.Log.Web/packages.config).
- Additional reference: target `v4.6` in [`dayson.logapi/src/DaysOn.Log.IISHost/DaysOn.Log.IISHost.csproj`](src/DaysOn.Log.IISHost/DaysOn.Log.IISHost.csproj); its [stable release was published on 2015-07-20](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). This indicates the technology generation; earlier preview releases may have existed.

A dependency's publication places the preserved configuration at or after that version, if the reference is valid. Package updates may be much newer than the project's beginnings. Packages were not restored to verify this.

October 2026 copy timestamps and documents generated during this review were excluded. Recoverable dates do not guarantee that there are no later, undated changes.

## Why this is worth exploring

LogService centralizes log reading, error selection, and cleanup through Dapper. It is a compact example of an observability API with SQL persistence and an independent web client.

Code entry points:

- [`src/DaysOn.Log.Web/Services/LogService.cs`](src/DaysOn.Log.Web/Services/LogService.cs)
- [`src/DaysOn.Log.Web/Controllers/HomeController.cs`](src/DaysOn.Log.Web/Controllers/HomeController.cs)

<!-- END project-context -->

## Structure and responsibilities

| Path | Responsibility |
| --- | --- |
| `src/DaysOn.Log.Web/` | Web API/OWIN, Home and Version controllers, and LogService. |
| `src/DaysOn.Log.Web/Services/LogService.cs` | Lists logs, filters errors, and clears the log table. |
| `src/DaysOn.Log.IISHost/` | IIS host, connection configuration, and publish profiles. |

## Technologies and requirements

Windows, Visual Studio/MSBuild, NuGet packages, and tools for the .NET Framework version declared by each project. HTTP services use Web API/OWIN, and IIS hosts require IIS Express or IIS. Availability of the internal DaysOn.* dependencies in their original feeds has not been checked.

Required services and databases depend on the component's private configuration.

Targets read from project files: `v4.6`.

## Setup and startup

Restore src/DaysOn.Log.IISHost.sln and select DaysOn.Log.IISHost in IIS Express. Prepare the logging database and adapt the dayson.log client.

Solutions present: `src/DaysOn.Log.IISHost.sln`.

## Configuration and integrations

dbConnection and other SQL connection strings in the host's Web.config. .pubxml.user profiles may contain encrypted passwords tied to the local user.

## Relationship to the DaysOn application

This folder is a DaysOn/HrPlan component within a distributed application.

- [dayson.log](../dayson.log/Readme.md): Log viewing interface.

The [main index](../README.md) includes a map of the application. Relationships described here come from visible controllers, clients, contracts, and configuration; a running deployment has not been verified.

## Status of this copy

Review access to query and deletion operations before exposing this component.

The review was static: dependencies were not restored, and applications and tests were neither built nor run. The commands above are setup instructions, not validation results. Listed versions describe the preserved code.


## Public copy

Original credentials have been removed or replaced with `REPLACE_WITH_YOUR_VALUE`. Local data, sessions, private certificates, and deployment profiles are omitted. Configure your own services and credentials before running. Building and operation with external services have not been validated.
