# dayson.company

Servicio de organizaciones de DaysOn/HrPlan: empresas, empleados, equipos y publicación de sus eventos de negocio.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

## Estructura y responsabilidades

| Ruta | Responsabilidad |
| --- | --- |
| `src/DaysOn.CompanyBc.Api/` | Controladores Company y Version y arranque Web API/OWIN. |
| `src/DaysOn.CompanyBc.Domain/` | Entidades Company, Employee y Team, persistencia y servicios de búsqueda y equipos. |
| `src/DaysOn.CompanyBc.Models/, src/DaysOn.CompanyBc.SharedKernel/` | Comandos, DTOs y soporte de dominio. |
| `src/DaysOn.CompanyBc.Host/` | Host IIS. |
| `src/DaysOn.CompanyBc.Test.*/` | Pruebas unitarias, de integración y aceptación. |

## Tecnologías y requisitos

Windows, Visual Studio/MSBuild, paquetes NuGet y herramientas del .NET Framework declarado por cada proyecto. Los servicios HTTP usan Web API/OWIN y los hosts IIS requieren IIS Express o IIS. Hay dependencias DaysOn.* propias cuya disponibilidad en los feeds originales no se ha comprobado.

Los servicios y bases de datos necesarios dependen de la configuración privada del componente.

Destinos leídos de los proyectos: `v4.6`.

## Preparación y arranque

Restaurar src/DaysOn.CompanyBc.sln y seleccionar DaysOn.CompanyBc.Host como inicio en IIS Express. Preparar las conexiones y el intercambio de eventos con Auth y Holidays.

Soluciones presentes: `src/DaysOn.CompanyBc.sln`.

## Configuración e integraciones

Web.config del host y App.config de API/dominio incluyen bases, bus y JWT. Revisar transformaciones Web.Production.config y Web.Release.config y los perfiles de publicación.

## Relación con la aplicación DaysOn

Esta carpeta es un componente de DaysOn/HrPlan, dentro de una aplicación distribuida.

- [dayson.auth](../dayson.auth/Readme.md): Identidad, tokens y cuentas.
- [dayson.holidays](../dayson.holidays/Readme.md): Vacaciones, reglas, aprobaciones y saldos.
- [dayson.common](../dayson.common/Readme.md): Bibliotecas, contratos, datos y mensajería compartidos.
- [dayson.facade](../dayson.facade/Readme.md): API de coordinación para clientes web.

El [índice general](../README.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

Los eventos de empresas, empleados y equipos alimentan otros contextos de DaysOn; Holidays incorpora manejadores específicos para ellos.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
