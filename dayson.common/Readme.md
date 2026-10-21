# dayson.common

Bibliotecas e infraestructura compartidas por los componentes de DaysOn/HrPlan.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

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

- [dayson.auth](../dayson.auth/Readme.md): Identidad, tokens y cuentas.
- [dayson.company](../dayson.company/Readme.md): Empresas, equipos y empleados.
- [dayson.holidays](../dayson.holidays/Readme.md): Vacaciones, reglas, aprobaciones y saldos.
- [dayson.functions](../dayson.functions/Readme.md): Procesamiento de eventos, días laborables y correo.
- [dayson.facade](../dayson.facade/Readme.md): API de coordinación para clientes web.

El [índice general](../README.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

Las funciones de administración de alojamiento pueden crear o cambiar infraestructura. No se ejecutaron herramientas de escalado, cron ni pruebas conectadas.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
