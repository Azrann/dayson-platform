**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.holidays

Servicio de vacaciones y ausencias de DaysOn/HrPlan: solicitudes, aprobaciones, reglas de empresa, calendario laboral y saldos anuales.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

<!-- BEGIN project-context -->

## Antigüedad y evidencia

**Año de referencia de esta versión: 2018.** Se toma el archivo de código o configuración más reciente con una fecha histórica recuperable; no se ha podido confirmar la fecha exacta de creación.

- Archivo más reciente datable: **2018-06-30**, [`dayson.holidays/src/DaysOn.HolidaysBc.Test.Unit/packages.config`](src/DaysOn.HolidaysBc.Test.Unit/packages.config); fecha del commit que modificó el archivo, recuperada del Git local.
- Primer registro del repositorio Git original que contiene este código: **2015-11-24**. Puede corresponder a una importación; no prueba cuándo se escribió por primera vez.
- Dependencia fechada más reciente entre las comprobadas: [Castle.Core 4.3.1](https://www.nuget.org/packages/Castle.Core/4.3.1), publicada el **2018-06-20** (versión declarada), en [`dayson.holidays/src/DaysOn.HolidaysBc.Api/packages.config`](src/DaysOn.HolidaysBc.Api/packages.config).
- Referencia adicional: destino `v4.6` en [`dayson.holidays/src/DaysOn.HolidaysBc.Api/DaysOn.HolidaysBc.Api.csproj`](src/DaysOn.HolidaysBc.Api/DaysOn.HolidaysBc.Api.csproj); su [versión estable se publicó el 2015-07-20](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). Es una pista de generación tecnológica; pudieron existir versiones preliminares anteriores.

La publicación de una dependencia sitúa la configuración conservada a partir de esa versión, si la referencia es válida. Una actualización de paquetes puede ser mucho más reciente que el inicio del proyecto. No se restauraron paquetes para comprobarlo.

Se descartaron las marcas de tiempo de la copia de octubre de 2026 y los documentos generados durante esta revisión. Las fechas recuperables no garantizan que no existan cambios posteriores sin datar.

## Por qué merece la pena mirarlo

Las reglas de empresa, aprobaciones, fechas cerradas y saldos anuales forman un dominio con más matices que un CRUD. Merece la pena seguir cómo se combinan esos conceptos y cómo se sincronizan empleados y equipos mediante eventos.

Puntos de entrada al código:

- [`src/DaysOn.HolidaysBc.Domain`](src/DaysOn.HolidaysBc.Domain/)
- [`src/DaysOn.HolidaysBc.Api/BusEventHandlers`](src/DaysOn.HolidaysBc.Api/BusEventHandlers/)
- [`src/DaysOn.HolidaysBc.Api/Controllers`](src/DaysOn.HolidaysBc.Api/Controllers/)

<!-- END project-context -->

## Estructura y responsabilidades

| Ruta | Responsabilidad |
| --- | --- |
| `src/DaysOn.HolidaysBc.Api/` | APIs de solicitudes, aprobadores, reglas, administración, fechas abiertas/cerradas y cálculos anuales. |
| `src/DaysOn.HolidaysBc.Api/BusEventHandlers/` | Sincronización de empresas, empleados y equipos mediante eventos. |
| `src/DaysOn.HolidaysBc.Domain/` | Repositorios y servicios de empleados, equipos, solicitudes, reglas y cálculos. |
| `src/DaysOn.HolidaysBc.Models/` | Comandos y DTOs. |
| `src/DaysOn.HolidaysBc.Host/` | Host IIS. |
| `src/DaysOn.HolidaysBc.Test.*/` | Pruebas unitarias, integración y aceptación. |

## Tecnologías y requisitos

Windows, Visual Studio/MSBuild, paquetes NuGet y herramientas del .NET Framework declarado por cada proyecto. Los servicios HTTP usan Web API/OWIN y los hosts IIS requieren IIS Express o IIS. Hay dependencias DaysOn.* propias cuya disponibilidad en los feeds originales no se ha comprobado.

Los servicios y bases de datos necesarios dependen de la configuración privada del componente.

Destinos leídos de los proyectos: `v4.6`.

## Preparación y arranque

Restaurar src/DaysOn.HolidaysBc.sln, preparar SQL/NoSQL y seleccionar DaysOn.HolidaysBc.Host. Alinear los eventos con Company y la validación con Functions.

Soluciones presentes: `src/DaysOn.HolidaysBc.sln`.

## Configuración e integraciones

Revisar las conexiones del host y del dominio, JWT, NoSQL, bus, perfiles y transformaciones de despliegue. Las pruebas reutilizan configuraciones privadas.

## Relación con la aplicación DaysOn

Esta carpeta es un componente de DaysOn/HrPlan, dentro de una aplicación distribuida.

- [dayson.company](../dayson.company/README.es.md): Empresas, equipos y empleados.
- [dayson.functions](../dayson.functions/README.es.md): Procesamiento de eventos, días laborables y correo.
- [dayson.auth](../dayson.auth/README.es.md): Identidad, tokens y cuentas.
- [dayson.facade](../dayson.facade/README.es.md): API de coordinación para clientes web.

El [índice general](../README.es.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

El saldo y los días disponibles dependen de reglas de empresa, año de vacaciones, días laborables y fechas cerradas.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
