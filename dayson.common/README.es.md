**Languages:** [English](Readme.md) · [Español](README.es.md)

# dayson.common

Bibliotecas e infraestructura compartidas por los componentes de DaysOn/HrPlan.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

<!-- BEGIN project-context -->

## Antigüedad y evidencia

**Año de referencia de esta versión: 2018.** Se toma el archivo de código o configuración más reciente con una fecha histórica recuperable; no se ha podido confirmar la fecha exacta de creación.

- Archivo más reciente datable: **2018-07-02**, [`dayson.common/src/DaysOn.Common/packages.config`](src/DaysOn.Common/packages.config); fecha del commit que modificó el archivo, recuperada del Git local.
- Primer registro del repositorio Git original que contiene este código: **2016-01-25**. Puede corresponder a una importación; no prueba cuándo se escribió por primera vez.
- Dependencia fechada más reciente entre las comprobadas: [JustEat.StatsD 3.0.0](https://www.nuget.org/packages/JustEat.StatsD/3.0.0), publicada el **2018-06-20** (versión declarada), en [`dayson.common/src/DaysOn.Common/packages.config`](src/DaysOn.Common/packages.config).
- Referencia adicional: destino `v4.6.1` en [`dayson.common/src/DaysOn.Common.ServiceBase.Test/DaysOn.Common.ServiceBase.Test.csproj`](src/DaysOn.Common.ServiceBase.Test/DaysOn.Common.ServiceBase.Test.csproj); su [versión estable se publicó el 2015-11-30](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-framework). Es una pista de generación tecnológica; pudieron existir versiones preliminares anteriores.

La publicación de una dependencia sitúa la configuración conservada a partir de esa versión, si la referencia es válida. Una actualización de paquetes puede ser mucho más reciente que el inicio del proyecto. No se restauraron paquetes para comprobarlo.

Se descartaron las marcas de tiempo de la copia de octubre de 2026 y los documentos generados durante esta revisión. Las fechas recuperables no garantizan que no existan cambios posteriores sin datar.

## Por qué merece la pena mirarlo

Las abstracciones del bus, contratos compartidos y utilidades de infraestructura permiten ver qué comparten los microservicios y dónde se mantiene la separación entre ellos. AzureServiceBus es un buen punto de entrada para seguir la comunicación mediante eventos.

Puntos de entrada al código:

- [`src/DaysOn.Common/AzureBus/AzureServiceBus.cs`](src/DaysOn.Common/AzureBus/AzureServiceBus.cs)
- [`src/DaysOn.Common/AzureBus/IServiceBus.cs`](src/DaysOn.Common/AzureBus/IServiceBus.cs)
- [`src/DaysOn.Common/MessageHandlers`](src/DaysOn.Common/MessageHandlers/)

<!-- END project-context -->

## Estructura y responsabilidades

| Ruta | Responsabilidad |
| --- | --- |
| `src/DaysOn.Common/` | Mensajería mediante Azure Service Bus y PubNub y manejadores de mensajes. |
| `src/DaysOn.Common.Events/` | Contratos de eventos entre servicios. |
| `src/DaysOn.Common.DataAccess/, src/DaysOn.Common.SqlDataAccess/` | Persistencia y utilidades de acceso a datos. |
| `src/DaysOn.Common.Domain/, src/DaysOn.Common.ServiceBase/, src/DaysOn.Common.Utilities/` | Eventos de dominio, arranque OWIN, inyección de dependencias y utilidades. |
| `src/DaysOn.Common.CloudSites/, src/DaysOn.Common.CronService/, src/Scale/` | Integración de alojamiento y bases de GearHost y herramientas auxiliares. |
| `src/WindowsService1/` | Proyecto auxiliar de servicio Windows. |

## Tecnologías y requisitos

Windows, Visual Studio/MSBuild, paquetes NuGet y herramientas del .NET Framework declarado por cada proyecto. Los servicios HTTP usan Web API/OWIN y los hosts IIS requieren IIS Express o IIS. Hay dependencias DaysOn.* propias cuya disponibilidad en los feeds originales no se ha comprobado.

Los servicios y bases de datos necesarios dependen de la configuración privada del componente.

Destinos leídos de los proyectos: `v4.5.2`, `v4.6`, `v4.6.1`.

## Preparación y arranque

Restaurar y compilar src/DaysOn.Common.sln. Las bibliotecas se consumen desde otros repositorios a través de paquetes DaysOn.Common.*; revisar los .nuspec antes de preparar paquetes locales.

Soluciones presentes: `src/DaysOn.Common.sln`.

## Configuración e integraciones

Las pruebas de suscriptores usan NoSQL, PubNub, Ably y Service Bus. Las herramientas de alojamiento leen gearhostkey; HrPlanCloudService también contiene credenciales de base de datos en código.

## Relación con la aplicación DaysOn

Esta carpeta es un componente de DaysOn/HrPlan, dentro de una aplicación distribuida.

- [dayson.auth](../dayson.auth/README.es.md): Identidad, tokens y cuentas.
- [dayson.company](../dayson.company/README.es.md): Empresas, equipos y empleados.
- [dayson.holidays](../dayson.holidays/README.es.md): Vacaciones, reglas, aprobaciones y saldos.
- [dayson.functions](../dayson.functions/README.es.md): Procesamiento de eventos, días laborables y correo.
- [dayson.facade](../dayson.facade/README.es.md): API de coordinación para clientes web.

El [índice general](../README.es.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

Las funciones de administración de alojamiento pueden crear o cambiar infraestructura. No se ejecutaron herramientas de escalado, cron ni pruebas conectadas.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
