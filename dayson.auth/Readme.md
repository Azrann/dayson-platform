# dayson.auth

Servicio de identidad de DaysOn/HrPlan: registro, inicio de sesión, emisión de tokens, activación de cuentas, recuperación de contraseñas y asignación de roles relacionados con equipos.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

<!-- BEGIN project-context -->

## Antigüedad y evidencia

**Año de referencia de esta versión: 2018.** Se toma el archivo de código o configuración más reciente con una fecha histórica recuperable; no se ha podido confirmar la fecha exacta de creación.

- Archivo más reciente datable: **2018-07-02**, [`dayson.auth/src/DaysOn.Auth.Web/Providers/CustomOAuthProvider.cs`](src/DaysOn.Auth.Web/Providers/CustomOAuthProvider.cs); fecha del commit que modificó el archivo, recuperada del Git local.
- Primer registro del repositorio Git original que contiene este código: **2015-07-22**. Puede corresponder a una importación; no prueba cuándo se escribió por primera vez.
- Dependencia fechada más reciente entre las comprobadas: [Castle.Core 4.3.1](https://www.nuget.org/packages/Castle.Core/4.3.1), publicada el **2018-06-20** (versión declarada), en [`dayson.auth/src/DaysOn.Auth.Acceptance/packages.config`](src/DaysOn.Auth.Acceptance/packages.config).
- Referencia adicional: destino `v4.6` en [`dayson.auth/src/DaysOn.Auth.Acceptance/DaysOn.Auth.Acceptance.csproj`](src/DaysOn.Auth.Acceptance/DaysOn.Auth.Acceptance.csproj); su [versión estable se publicó el 2015-07-20](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). Es una pista de generación tecnológica; pudieron existir versiones preliminares anteriores.

La publicación de una dependencia sitúa la configuración conservada a partir de esa versión, si la referencia es válida. Una actualización de paquetes puede ser mucho más reciente que el inicio del proyecto. No se restauraron paquetes para comprobarlo.

Se descartaron las marcas de tiempo de la copia de octubre de 2026 y los documentos generados durante esta revisión. Las fechas recuperables no garantizan que no existan cambios posteriores sin datar.

## Por qué merece la pena mirarlo

Los cambios de equipos y responsables se traducen en cambios de roles, además del registro y la emisión de tokens. Esa relación entre eventos de negocio y permisos ofrece un caso concreto de identidad dentro de una aplicación distribuida.

Puntos de entrada al código:

- [`src/DaysOn.Auth.Domain/Events/CheckAddManagerRoleEventHandler.cs`](src/DaysOn.Auth.Domain/Events/CheckAddManagerRoleEventHandler.cs)
- [`src/DaysOn.Auth.Domain/Application/AuthService.cs`](src/DaysOn.Auth.Domain/Application/AuthService.cs)

<!-- END project-context -->

## Estructura y responsabilidades

| Ruta | Responsabilidad |
| --- | --- |
| `src/DaysOn.Auth.Web/` | Controladores Auth, Password y Version, configuración OWIN y validadores. |
| `src/DaysOn.Auth.Domain/` | Usuarios ASP.NET Identity, servicios de autenticación, activación, equipos y eventos. |
| `src/DaysOn.Auth.IISHost/` | Host IIS y configuración del servicio. |
| `src/DaysOn.Auth.Models/` | Comandos y modelos compartidos con clientes. |
| `src/DaysOn.Auth.Test/, src/DaysOn.Auth.Test.Integration/, src/DaysOn.Auth.Acceptance/` | Pruebas de distintos niveles. |

## Tecnologías y requisitos

Windows, Visual Studio/MSBuild, paquetes NuGet y herramientas del .NET Framework declarado por cada proyecto. Los servicios HTTP usan Web API/OWIN y los hosts IIS requieren IIS Express o IIS. Hay dependencias DaysOn.* propias cuya disponibilidad en los feeds originales no se ha comprobado.

Los servicios y bases de datos necesarios dependen de la configuración privada del componente.

Destinos leídos de los proyectos: `v4.5.2`, `v4.6`.

## Preparación y arranque

Seleccionar DaysOn.Auth.IISHost en Visual Studio/IIS Express después de restaurar src/DaysOn.Auth.sln y preparar las bases de identidad y NoSQL.

Soluciones presentes: `src/DaysOn.Auth.sln`.

## Configuración e integraciones

AuthDB y NoSQL, JWT.Issuer/JWT.Audience/JWT.Secret, AuthServer, Service Bus y claves PubNub. La configuración JWT debe ser coherente con los demás servicios.

## Relación con la aplicación DaysOn

Esta carpeta es un componente de DaysOn/HrPlan, dentro de una aplicación distribuida.

- [dayson.common](../dayson.common/Readme.md): Bibliotecas, contratos, datos y mensajería compartidos.
- [dayson.company](../dayson.company/Readme.md): Empresas, equipos y empleados.
- [dayson.facade](../dayson.facade/Readme.md): API de coordinación para clientes web.

El [índice general](../README.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

Parte de una aplicación única con los otros proyectos dayson.*. Los logs y las pruebas también contienen material sensible.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
