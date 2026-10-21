# dayson.logapi

API de consulta y limpieza de registros de DaysOn, con almacenamiento SQL y acceso mediante Dapper.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

<!-- BEGIN project-context -->

## Antigüedad y evidencia

**Año de referencia de esta versión: 2016.** Se toma el archivo de código o configuración más reciente con una fecha histórica recuperable; no se ha podido confirmar la fecha exacta de creación.

- Archivo más reciente datable: **2016-12-18**, [`dayson.logapi/src/DaysOn.Log.IISHost/Web.config`](src/DaysOn.Log.IISHost/Web.config); fecha del commit que modificó el archivo, recuperada del Git local.
- Primer registro del repositorio Git original que contiene este código: **2016-11-20**. Puede corresponder a una importación; no prueba cuándo se escribió por primera vez.
- Dependencia fechada más reciente entre las comprobadas: [Dapper 1.50.2](https://www.nuget.org/packages/Dapper/1.50.2), publicada el **2016-07-22** (versión declarada), en [`dayson.logapi/src/DaysOn.Log.Web/packages.config`](src/DaysOn.Log.Web/packages.config).
- Referencia adicional: destino `v4.6` en [`dayson.logapi/src/DaysOn.Log.IISHost/DaysOn.Log.IISHost.csproj`](src/DaysOn.Log.IISHost/DaysOn.Log.IISHost.csproj); su [versión estable se publicó el 2015-07-20](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). Es una pista de generación tecnológica; pudieron existir versiones preliminares anteriores.

La publicación de una dependencia sitúa la configuración conservada a partir de esa versión, si la referencia es válida. Una actualización de paquetes puede ser mucho más reciente que el inicio del proyecto. No se restauraron paquetes para comprobarlo.

Se descartaron las marcas de tiempo de la copia de octubre de 2026 y los documentos generados durante esta revisión. Las fechas recuperables no garantizan que no existan cambios posteriores sin datar.

## Por qué merece la pena mirarlo

LogService concentra lectura de registros, selección de errores y limpieza mediante Dapper. Es útil como ejemplo compacto de una API de observabilidad con persistencia SQL y un cliente web independiente.

Puntos de entrada al código:

- [`src/DaysOn.Log.Web/Services/LogService.cs`](src/DaysOn.Log.Web/Services/LogService.cs)
- [`src/DaysOn.Log.Web/Controllers/HomeController.cs`](src/DaysOn.Log.Web/Controllers/HomeController.cs)

<!-- END project-context -->

## Estructura y responsabilidades

| Ruta | Responsabilidad |
| --- | --- |
| `src/DaysOn.Log.Web/` | Web API/OWIN, controladores Home y Version y servicio LogService. |
| `src/DaysOn.Log.Web/Services/LogService.cs` | Lista registros, filtra errores y limpia la tabla de logs. |
| `src/DaysOn.Log.IISHost/` | Host IIS, configuración de conexión y perfiles de publicación. |

## Tecnologías y requisitos

Windows, Visual Studio/MSBuild, paquetes NuGet y herramientas del .NET Framework declarado por cada proyecto. Los servicios HTTP usan Web API/OWIN y los hosts IIS requieren IIS Express o IIS. Hay dependencias DaysOn.* propias cuya disponibilidad en los feeds originales no se ha comprobado.

Los servicios y bases de datos necesarios dependen de la configuración privada del componente.

Destinos leídos de los proyectos: `v4.6`.

## Preparación y arranque

Restaurar src/DaysOn.Log.IISHost.sln y seleccionar DaysOn.Log.IISHost en IIS Express. Preparar la base de registros y adaptar el cliente dayson.log.

Soluciones presentes: `src/DaysOn.Log.IISHost.sln`.

## Configuración e integraciones

dbConnection y otras cadenas SQL del Web.config del host. Los perfiles .pubxml.user pueden contener contraseñas cifradas vinculadas al usuario local.

## Relación con la aplicación DaysOn

Esta carpeta es un componente de DaysOn/HrPlan, dentro de una aplicación distribuida.

- [dayson.log](../dayson.log/Readme.md): Interfaz de consulta de logs.

El [índice general](../README.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

Revisar acceso a las operaciones de consulta y borrado antes de exponer este componente.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
