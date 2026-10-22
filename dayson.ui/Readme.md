**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.ui

DaysOn/HrPlan interface for authentication, company/team/employee administration, and leave viewing or approval. Includes two frontend generations.

Documentation based on local files reviewed on 21 October 2026.

<!-- BEGIN project-context -->

## Historical dates and evidence

**Reference year for this version: 2018.** This uses the most recent source or configuration file with a recoverable historical date; the exact creation date could not be confirmed.

- Most recent datable file: **2018-06-24**, [`dayson.ui/v2/src/index.html`](v2/src/index.html); date of the commit modifying the file, recovered from local Git history.
- First record in the original Git repository containing this code: **2015-10-19**. This may be an import and does not establish when the code was first written.
- Most recently published dependency among those checked: [@angular/core 5.2.10](https://www.npmjs.com/package/@angular/core/v/5.2.10), published on **2018-04-16** (version pinned in the lockfile), in [`dayson.ui/v2/package.json`](v2/package.json). Cross-checked against the version in [`dayson.ui/v2/package-lock.json`](v2/package-lock.json).

A dependency's publication places the preserved configuration at or after that version, if the reference is valid. Package updates may be much newer than the project's beginnings. Packages were not restored to verify this.

October 2026 copy timestamps and documents generated during this review were excluded. Recoverable dates do not guarantee that there are no later, undated changes.

## Why this is worth exploring

The calendar is divided into day, month, and year components, alongside team views and approvals. It is useful for studying how a leave management domain becomes visual components and user actions.

Code entry points:

- [`v2/src/app/holidays/calendar/calendar-year.component.ts`](v2/src/app/holidays/calendar/calendar-year.component.ts)
- [`v2/src/app/holidays/calendar/calendar-month.component.ts`](v2/src/app/holidays/calendar/calendar-month.component.ts)
- [`v2/src/app/holidays/approve-view/approve-view.component.ts`](v2/src/app/holidays/approve-view/approve-view.component.ts)

<!-- END project-context -->

## Structure and responsibilities

| Path | Responsibility |
| --- | --- |
| `src/` | AngularJS 1.4 version, Angular Route, translations, Bootstrap 3, and Grunt/Bower tasks. |
| `v2/` | Angular 5.2 version with Angular Material, RxJS, moment, and PubNub. |
| `v2/src/app/admin/` | Company, team, and user administration. |
| `v2/src/app/holidays/` | Calendars, requests, and approvals. |
| `v2/src/app/login/` | Registration, login, and password recovery. |

## Technologies and requirements

Node/npm compatible with Angular CLI 1.x for v2. The earlier version needs Grunt 0.4 and Bower. No new Node version was pinned during this review.

## Setup and startup

Configure DaysOn service URLs, starting with the facade and authentication. npm run build compiles v2; npm test, npm run lint, and npm run e2e are declared but were not run.

Setup commands, run from this folder after reviewing private configuration:

```text
cd v2
npm install
npm start
```

## Configuration and integrations

Review HTTP services and environment files before opening the interface. gruntfile.js contains deployment details, and sign-in.component.ts contains PubNub browser keys.

## Relationship to the DaysOn application

This folder is a DaysOn/HrPlan component within a distributed application.

- [dayson.facade](../dayson.facade/Readme.md): Coordination API for web clients.
- [dayson.auth](../dayson.auth/Readme.md): Identity, tokens, and accounts.

The [main index](../README.md) includes a map of the application. Relationships described here come from visible controllers, clients, contracts, and configuration; a running deployment has not been verified.

## Status of this copy

Review browser publish/subscribe keys according to their permissions; their presence alone does not establish that a secret PubNub key has been found.

The review was static: dependencies were not restored, and applications and tests were neither built nor run. The commands above are setup instructions, not validation results. Listed versions describe the preserved code.


## Public copy

Original credentials have been removed or replaced with `REPLACE_WITH_YOUR_VALUE`. Local data, sessions, private certificates, and deployment profiles are omitted. Configure your own services and credentials before running. Building and operation with external services have not been validated.
