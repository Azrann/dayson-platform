# dayson.holidays

Servicio de vacaciones y ausencias de DaysOn/HrPlan: solicitudes, aprobaciones, reglas de empresa, calendario laboral y saldos anuales.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

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

- [dayson.company](../dayson.company/Readme.md): Empresas, equipos y empleados.
- [dayson.functions](../dayson.functions/Readme.md): Procesamiento de eventos, días laborables y correo.
- [dayson.auth](../dayson.auth/Readme.md): Identidad, tokens y cuentas.
- [dayson.facade](../dayson.facade/Readme.md): API de coordinación para clientes web.

El [índice general](../README.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

El saldo y los días disponibles dependen de reglas de empresa, año de vacaciones, días laborables y fechas cerradas.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
