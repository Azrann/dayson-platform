**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.auth

DaysOn/HrPlan identity service: registration, login, token issuance, account activation, password recovery, and team-related role assignment.

Documentation based on local files reviewed on 21 October 2026.

<!-- BEGIN project-context -->

## Historical dates and evidence

**Reference year for this version: 2018.** This uses the most recent source or configuration file with a recoverable historical date; the exact creation date could not be confirmed.

- Most recent datable file: **2018-07-02**, [`dayson.auth/src/DaysOn.Auth.Web/Providers/CustomOAuthProvider.cs`](src/DaysOn.Auth.Web/Providers/CustomOAuthProvider.cs); date of the commit modifying the file, recovered from local Git history.
- First record in the original Git repository containing this code: **2015-07-22**. This may be an import and does not establish when the code was first written.
- Most recently published dependency among those checked: [Castle.Core 4.3.1](https://www.nuget.org/packages/Castle.Core/4.3.1), published on **2018-06-20** (declared version), in [`dayson.auth/src/DaysOn.Auth.Acceptance/packages.config`](src/DaysOn.Auth.Acceptance/packages.config).
- Additional reference: target `v4.6` in [`dayson.auth/src/DaysOn.Auth.Acceptance/DaysOn.Auth.Acceptance.csproj`](src/DaysOn.Auth.Acceptance/DaysOn.Auth.Acceptance.csproj); its [stable release was published on 2015-07-20](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). This indicates the technology generation; earlier preview releases may have existed.

A dependency's publication places the preserved configuration at or after that version, if the reference is valid. Package updates may be much newer than the project's beginnings. Packages were not restored to verify this.

October 2026 copy timestamps and documents generated during this review were excluded. Recoverable dates do not guarantee that there are no later, undated changes.

## Why this is worth exploring

Changes to teams and managers translate into role changes, alongside registration and token issuance. That relationship between business events and permissions is a concrete identity example within a distributed application.

Code entry points:

- [`src/DaysOn.Auth.Domain/Events/CheckAddManagerRoleEventHandler.cs`](src/DaysOn.Auth.Domain/Events/CheckAddManagerRoleEventHandler.cs)
- [`src/DaysOn.Auth.Domain/Application/AuthService.cs`](src/DaysOn.Auth.Domain/Application/AuthService.cs)

<!-- END project-context -->

## Structure and responsibilities

| Path | Responsibility |
| --- | --- |
| `src/DaysOn.Auth.Web/` | Auth, Password, and Version controllers, OWIN configuration, and validators. |
| `src/DaysOn.Auth.Domain/` | ASP.NET Identity users, authentication, activation, team services, and events. |
| `src/DaysOn.Auth.IISHost/` | IIS host and service configuration. |
| `src/DaysOn.Auth.Models/` | Commands and models shared with clients. |
| `src/DaysOn.Auth.Test/, src/DaysOn.Auth.Test.Integration/, src/DaysOn.Auth.Acceptance/` | Tests at different levels. |

## Technologies and requirements

Windows, Visual Studio/MSBuild, NuGet packages, and tools for the .NET Framework version declared by each project. HTTP services use Web API/OWIN, and IIS hosts require IIS Express or IIS. Availability of the internal DaysOn.* dependencies in their original feeds has not been checked.

Required services and databases depend on the component's private configuration.

Targets read from project files: `v4.5.2`, `v4.6`.

## Setup and startup

Select DaysOn.Auth.IISHost in Visual Studio/IIS Express after restoring src/DaysOn.Auth.sln and preparing the identity and NoSQL databases.

Solutions present: `src/DaysOn.Auth.sln`.

## Configuration and integrations

AuthDB and NoSQL, JWT.Issuer/JWT.Audience/JWT.Secret, AuthServer, Service Bus, and PubNub keys. JWT configuration must be consistent with the other services.

## Relationship to the DaysOn application

This folder is a DaysOn/HrPlan component within a distributed application.

- [dayson.common](../dayson.common/Readme.md): Shared libraries, contracts, data, and messaging.
- [dayson.company](../dayson.company/Readme.md): Companies, teams, and employees.
- [dayson.facade](../dayson.facade/Readme.md): Coordination API for web clients.

The [main index](../README.md) includes a map of the application. Relationships described here come from visible controllers, clients, contracts, and configuration; a running deployment has not been verified.

## Status of this copy

Part of a single application with the other dayson.* projects. Logs and tests also contain sensitive material.

The review was static: dependencies were not restored, and applications and tests were neither built nor run. The commands above are setup instructions, not validation results. Listed versions describe the preserved code.


## Public copy

Original credentials have been removed or replaced with `REPLACE_WITH_YOUR_VALUE`. Local data, sessions, private certificates, and deployment profiles are omitted. Configure your own services and credentials before running. Building and operation with external services have not been validated.
