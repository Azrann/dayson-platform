# dayson.functions

Procesamiento de eventos y operaciones de apoyo de DaysOn/HrPlan: validación de solicitudes de vacaciones, cálculo de días laborables y envío de correos.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

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

- [dayson.holidays](../dayson.holidays/Readme.md): Vacaciones, reglas, aprobaciones y saldos.
- [dayson.common](../dayson.common/Readme.md): Bibliotecas, contratos, datos y mensajería compartidos.

El [índice general](../README.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

El proyecto utiliza servicios y contratos DaysOn.HolidaysBc y DaysOn.Common.Events para procesar solicitudes.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
