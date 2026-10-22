**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.functions

Procesamiento de eventos y operaciones de apoyo de DaysOn/HrPlan: validación de solicitudes de vacaciones, cálculo de días laborables y envío de correos.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

<!-- BEGIN project-context -->

## Antigüedad y evidencia

**Año de referencia de esta versión: 2018.** Se toma el archivo de código o configuración más reciente con una fecha histórica recuperable; no se ha podido confirmar la fecha exacta de creación.

- Archivo más reciente datable: **2018-06-30**, [`dayson.functions/src/DaysOn.Functions.sln`](src/DaysOn.Functions.sln); fecha del commit que modificó el archivo, recuperada del Git local.
- Primer registro del repositorio Git original que contiene este código: **2016-08-02**. Puede corresponder a una importación; no prueba cuándo se escribió por primera vez.
- Dependencia fechada más reciente entre las comprobadas: [Castle.Core 4.3.1](https://www.nuget.org/packages/Castle.Core/4.3.1), publicada el **2018-06-20** (versión declarada), en [`dayson.functions/src/DaysOn.Functions.Api/packages.config`](src/DaysOn.Functions.Api/packages.config).
- Referencia adicional: destino `v4.6` en [`dayson.functions/src/DaysOn.Functions.Api/DaysOn.Functions.Api.csproj`](src/DaysOn.Functions.Api/DaysOn.Functions.Api.csproj); su [versión estable se publicó el 2015-07-20](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). Es una pista de generación tecnológica; pudieron existir versiones preliminares anteriores.

La publicación de una dependencia sitúa la configuración conservada a partir de esa versión, si la referencia es válida. Una actualización de paquetes puede ser mucho más reciente que el inicio del proyecto. No se restauraron paquetes para comprobarlo.

Se descartaron las marcas de tiempo de la copia de octubre de 2026 y los documentos generados durante esta revisión. Las fechas recuperables no garantizan que no existan cambios posteriores sin datar.

## Por qué merece la pena mirarlo

Los handlers conectan solicitudes de vacaciones, cálculo de días laborables y envío de correo. Es un ejemplo concreto de cómo sacar trabajo de coordinación a consumidores de eventos y mantenerlo separado de la API que recibe la solicitud.

Puntos de entrada al código:

- [`src/DaysOn.Functions.Api/BusEventHandlers/CalculateBusinessDaysEventHandler.cs`](src/DaysOn.Functions.Api/BusEventHandlers/CalculateBusinessDaysEventHandler.cs)
- [`src/DaysOn.Functions.Api/BusEventHandlers/AddHolidayRequestEventHandler.cs`](src/DaysOn.Functions.Api/BusEventHandlers/AddHolidayRequestEventHandler.cs)
- [`src/DaysOn.Functions.Domain/Services/CreateHolidayService.cs`](src/DaysOn.Functions.Domain/Services/CreateHolidayService.cs)

<!-- END project-context -->

## Estructura y responsabilidades

| Ruta | Responsabilidad |
| --- | --- |
| `src/DaysOn.Functions.Api/BusEventHandlers/` | Manejadores AddHolidayRequest, CalculateBusinessDays y SendEmail. |
| `src/DaysOn.Functions.Domain/` | CreateHolidayService, validación de periodos y cálculo de días laborables. |
| `src/DaysOn.Functions.Models/` | Comandos y resultados. |
| `src/DaysOn.Functions.Host/` | Host IIS/OWIN del componente. |
| `src/DaysOn.Functions.Test.*/, src/DaysOn.Functions.BoundedContextTests/` | Pruebas de reglas y eventos. |

## Tecnologías y requisitos

Windows, Visual Studio/MSBuild, paquetes NuGet y herramientas del .NET Framework declarado por cada proyecto. Los servicios HTTP usan Web API/OWIN y los hosts IIS requieren IIS Express o IIS. Hay dependencias DaysOn.* propias cuya disponibilidad en los feeds originales no se ha comprobado.

Los servicios y bases de datos necesarios dependen de la configuración privada del componente.

Destinos leídos de los proyectos: `v4.6`.

## Preparación y arranque

Restaurar src/DaysOn.Functions.sln y seleccionar DaysOn.Functions.Host. Preparar el bus y los datos de Holidays antes de procesar eventos.

Soluciones presentes: `src/DaysOn.Functions.sln`.

## Configuración e integraciones

Conexiones a bases, JWT y mensajería en el host y las configuraciones de pruebas. SendEmailEventHandler.cs y una prueba de integración contienen una clave SendGrid literal.

## Relación con la aplicación DaysOn

Esta carpeta es un componente de DaysOn/HrPlan, dentro de una aplicación distribuida.

- [dayson.holidays](../dayson.holidays/README.es.md): Vacaciones, reglas, aprobaciones y saldos.
- [dayson.common](../dayson.common/README.es.md): Bibliotecas, contratos, datos y mensajería compartidos.

El [índice general](../README.es.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

El proyecto utiliza servicios y contratos DaysOn.HolidaysBc y DaysOn.Common.Events para procesar solicitudes.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
