**Languages:** [English](README.md) · [Español](README.es.md)

# dayson-platform

Aplicación DaysOn/HrPlan: identidad, empresas, vacaciones, mensajería, registro y clientes web.

<!-- BEGIN project-context -->

## Antigüedad y evidencia

**Año de referencia de esta versión: 2018.** Se toma el archivo de código o configuración más reciente con una fecha histórica recuperable; no se ha podido confirmar la fecha exacta de creación.

- Archivo más reciente datable: **2018-07-02**, [`dayson.auth/src/DaysOn.Auth.Web/Providers/CustomOAuthProvider.cs`](dayson.auth/src/DaysOn.Auth.Web/Providers/CustomOAuthProvider.cs); fecha del commit que modificó el archivo, recuperada del Git local.
- Primer registro entre los repositorios Git originales incluidos: **2015-07-12**. Puede corresponder a una importación; no prueba cuándo se escribió por primera vez.
- Dependencia fechada más reciente entre las comprobadas: [Castle.Core 4.3.1](https://www.nuget.org/packages/Castle.Core/4.3.1), publicada el **2018-06-20** (versión declarada), en [`dayson.auth/src/DaysOn.Auth.Acceptance/packages.config`](dayson.auth/src/DaysOn.Auth.Acceptance/packages.config).
- Referencia adicional: destino `v4.6.1` en [`dayson.common/src/DaysOn.Common.ServiceBase.Test/DaysOn.Common.ServiceBase.Test.csproj`](dayson.common/src/DaysOn.Common.ServiceBase.Test/DaysOn.Common.ServiceBase.Test.csproj); su [versión estable se publicó el 2015-11-30](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). Es una pista de generación tecnológica; pudieron existir versiones preliminares anteriores.

La publicación de una dependencia sitúa la configuración conservada a partir de esa versión, si la referencia es válida. Una actualización de paquetes puede ser mucho más reciente que el inicio del proyecto. No se restauraron paquetes para comprobarlo.

Se descartaron las marcas de tiempo de la copia de octubre de 2026 y los documentos generados durante esta revisión. Las fechas recuperables no garantizan que no existan cambios posteriores sin datar.

Años de referencia por componente (archivo más reciente datable):

| Componente | Año |
| --- | --- |
| [`dayson.auth`](dayson.auth/) | 2018 |
| [`dayson.common`](dayson.common/) | 2018 |
| [`dayson.company`](dayson.company/) | 2018 |
| [`dayson.facade`](dayson.facade/) | 2018 |
| [`dayson.functions`](dayson.functions/) | 2018 |
| [`dayson.holidays`](dayson.holidays/) | 2018 |
| [`dayson.log`](dayson.log/) | 2016 |
| [`dayson.logapi`](dayson.logapi/) | 2016 |
| [`dayson.tower`](dayson.tower/) | Sin determinar |
| [`dayson.ui`](dayson.ui/) | 2018 |

## Por qué merece la pena mirarlo

El recorrido solicitud de vacaciones → eventos → cálculo de días laborables → aprobación conecta varios microservicios con un problema de negocio reconocible. El calendario del frontend ayuda a ver el resultado de esa separación de responsabilidades.

Puntos de entrada al código:

- [`dayson.holidays/src/DaysOn.HolidaysBc.Domain`](dayson.holidays/src/DaysOn.HolidaysBc.Domain/)
- [`dayson.functions/src/DaysOn.Functions.Api/BusEventHandlers`](dayson.functions/src/DaysOn.Functions.Api/BusEventHandlers/)
- [`dayson.ui/v2/src/app/holidays/calendar`](dayson.ui/v2/src/app/holidays/calendar/)

<!-- END project-context -->

## Contenido

| Directorio | Función |
| --- | --- |
| [`dayson.auth/`](dayson.auth/) | Servicio de identidad de DaysOn/HrPlan: registro, inicio de sesión, emisión de tokens, activación de cuentas, recuperación de contraseñas y asignación de roles relacionados con equipos. |
| [`dayson.common/`](dayson.common/) | Bibliotecas e infraestructura compartidas por los componentes de DaysOn/HrPlan. |
| [`dayson.company/`](dayson.company/) | Servicio de organizaciones de DaysOn/HrPlan: empresas, empleados, equipos y publicación de sus eventos de negocio. |
| [`dayson.facade/`](dayson.facade/) | Fachada API de DaysOn/HrPlan: ofrece operaciones orientadas a la interfaz y coordina llamadas a autenticación, empresas y vacaciones. |
| [`dayson.functions/`](dayson.functions/) | Procesamiento de eventos y operaciones de apoyo de DaysOn/HrPlan: validación de solicitudes de vacaciones, cálculo de días laborables y envío de correos. |
| [`dayson.holidays/`](dayson.holidays/) | Servicio de vacaciones y ausencias de DaysOn/HrPlan: solicitudes, aprobaciones, reglas de empresa, calendario laboral y saldos anuales. |
| [`dayson.log/`](dayson.log/) | Interfaz web de consulta de registros de DaysOn, hecha con AngularJS, jQuery y Bootstrap. |
| [`dayson.logapi/`](dayson.logapi/) | API de consulta y limpieza de registros de DaysOn, con almacenamiento SQL y acceso mediante Dapper. |
| [`dayson.tower/`](dayson.tower/) | Carpeta de repositorio DaysOn sin archivos de trabajo: únicamente se encontró el directorio .git. |
| [`dayson.ui/`](dayson.ui/) | Interfaz de DaysOn/HrPlan para autenticación, administración de empresas/equipos/empleados y consulta o aprobación de vacaciones. Incluye dos generaciones del frontend. |

Los directorios dayson.* son microservicios y bibliotecas de una misma aplicación. Cada uno conserva su solución y su configuración. dayson.tower no contenía implementación.

## Requisitos y preparación

.NET Framework, ASP.NET Web API/OWIN, IIS, Azure Functions, NuGet y clientes web.

Esta es una copia histórica. Consultar los archivos de proyecto y los README de cada componente para sus versiones y dependencias. Restaurar NuGet/npm desde la carpeta correspondiente; las bibliotecas internas pueden necesitar reconstrucción. Preparar las bases de datos y los servicios externos con cuentas propias.

Soluciones conservadas:

- `dayson.auth/src/DaysOn.Auth.sln`
- `dayson.common/src/DaysOn.Common.sln`
- `dayson.company/src/DaysOn.CompanyBc.sln`
- `dayson.facade/src/DaysOn.Facade.sln`
- `dayson.functions/src/DaysOn.Functions.sln`
- `dayson.holidays/src/DaysOn.HolidaysBc.sln`
- `dayson.log/src/DaysonLog.sln`
- `dayson.logapi/src/DaysOn.Log.IISHost.sln`

Paquetes JavaScript principales:

- `dayson.ui/src/package.json`
- `dayson.ui/v2/package.json`

## Configuración y seguridad

Los secretos originales se han eliminado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Buscar este marcador antes de ejecutar y aportar los valores mediante configuración local o variables de entorno. Las sesiones, claves privadas, certificados de firma, bases de datos, registros, perfiles de despliegue y paquetes binarios no se publican. El `.gitignore` protege estos archivos locales.

Se publica un historial nuevo: los commits antiguos permanecen únicamente en la copia local original. Los valores de ejemplo no permiten conectarse a los servicios originales.

## Estado de validación

Se realizó una revisión estática y un escaneo de secretos antes de publicar. No se ejecutaron aplicaciones, operaciones de trading, despliegues ni pruebas que contacten servicios externos. No se garantiza la compilación con herramientas actuales. Algunos proyectos necesitan dependencias históricas y activos que no están incluidos.

## Licencias y procedencia

Se conservan las licencias y atribuciones de terceros presentes en el código. No se añade una licencia global a los componentes que no la tenían. Esta importación conserva fuentes locales y no afirma ser una versión actual del proyecto original.
