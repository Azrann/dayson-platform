**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.log

DaysOn log viewer built with AngularJS, jQuery, and Bootstrap.

Documentation based on local files reviewed on 21 October 2026.

<!-- BEGIN project-context -->

## Historical dates and evidence

**Reference year for this version: 2016.** This uses the most recent source or configuration file with a recoverable historical date; the exact creation date could not be confirmed.

- Most recent datable file: **2016-11-26**, [`dayson.log/src/DaysonLog/js/daysonlogapisApp.js`](src/DaysonLog/js/daysonlogapisApp.js); date of the commit modifying the file, recovered from local Git history.
- First record in the original Git repository containing this code: **2016-10-08**. This may be an import and does not establish when the code was first written.

October 2026 copy timestamps and documents generated during this review were excluded. Recoverable dates do not guarantee that there are no later, undated changes.

## Why this is worth exploring

This small AngularJS interface queries logs and errors through a separate API. Its size makes it easy to follow the connection between an HTTP service, screen controller, and result presentation from end to end.

Code entry points:

- [`src/DaysonLog/js/daysonlogapisApp.js`](src/DaysonLog/js/daysonlogapisApp.js)
- [`src/DaysonLog/index.html`](src/DaysonLog/index.html)

<!-- END project-context -->

## Structure and responsibilities

| Path | Responsibility |
| --- | --- |
| `src/DaysonLog/index.html` | Log view. |
| `src/DaysonLog/js/daysonlogapisApp.js` | Loads logs and errors from the API and includes a cleanup call. |
| `src/DaysonLog/lib/, src/DaysonLog/Content/` | Dependencies and styles. |
| `src/DaysonLog.sln` | Website solution for publishing from Visual Studio. |

## Technologies and requirements

A static HTTP server and an available dayson.logapi instance; browser libraries are bundled.

## Setup and startup

Serve src/DaysonLog over HTTP and adapt API URLs in daysonlogapisApp.js. Verify backend CORS and authorization.

Solutions present: `src/DaysonLog.sln`.

## Configuration and integrations

The backend connection is hardcoded in JavaScript. Publish profiles contain hosting destination details.

## Relationship to the DaysOn application

This folder is a DaysOn/HrPlan component within a distributed application.

- [dayson.logapi](../dayson.logapi/Readme.md): Log API and persistence.

The [main index](../README.md) includes a map of the application. Relationships described here come from visible controllers, clients, contracts, and configuration; a running deployment has not been verified.

## Status of this copy

This is an interface for the DaysOn system. Its cleanup operation can delete records in the service it targets.

The review was static: dependencies were not restored, and applications and tests were neither built nor run. The commands above are setup instructions, not validation results. Listed versions describe the preserved code.


## Public copy

Original credentials have been removed or replaced with `REPLACE_WITH_YOUR_VALUE`. Local data, sessions, private certificates, and deployment profiles are omitted. Configure your own services and credentials before running. Building and operation with external services have not been validated.
