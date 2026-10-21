# dayson.facade

Fachada API de DaysOn/HrPlan: ofrece operaciones orientadas a la interfaz y coordina llamadas a autenticación, empresas y vacaciones.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

## Estructura y responsabilidades

| Ruta | Responsabilidad |
| --- | --- |
| `src/DaysOn.Facade/` | Controladores de autenticación, empresa, empleados, vacaciones, reglas, aprobadores y vistas web. |
| `src/DaysOn.Facade.Common/` | Clientes HTTP de los servicios, DTOs y utilidades de integración. |
| `src/DaysOn.Facade.Models/` | Modelos consumibles por otros proyectos. |
| `src/DaysOn.Facade.IISHost/` | Host IIS. |
| `src/DaysOn.Facade.Tests.Acceptance/, src/DaysOn.Facade.Tests.Integration/` | Pruebas de coordinación de servicios. |

## Tecnologías y requisitos

Windows, Visual Studio/MSBuild, paquetes NuGet y herramientas del .NET Framework declarado por cada proyecto. Los servicios HTTP usan Web API/OWIN y los hosts IIS requieren IIS Express o IIS. Hay dependencias DaysOn.* propias cuya disponibilidad en los feeds originales no se ha comprobado.

Los servicios y bases de datos necesarios dependen de la configuración privada del componente.

Destinos leídos de los proyectos: `v4.6`.

## Preparación y arranque

Restaurar src/DaysOn.Facade.sln y seleccionar DaysOn.Facade.IISHost para IIS Express. Configurar primero los servicios Auth, Company y Holidays y adaptar sus URLs base.

Soluciones presentes: `src/DaysOn.Facade.sln`.

## Configuración e integraciones

Configurar URLs de los servicios, JWT y mensajería en los archivos de configuración y clientes. Las pruebas incluyen credenciales de acceso y tokens literales.

## Relación con la aplicación DaysOn

Esta carpeta es un componente de DaysOn/HrPlan, dentro de una aplicación distribuida.

- [dayson.auth](../dayson.auth/Readme.md): Identidad, tokens y cuentas.
- [dayson.company](../dayson.company/Readme.md): Empresas, equipos y empleados.
- [dayson.holidays](../dayson.holidays/Readme.md): Vacaciones, reglas, aprobaciones y saldos.
- [dayson.ui](../dayson.ui/Readme.md): Frontend de la aplicación.

El [índice general](../README.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

Es el punto de integración principal para dayson.ui; su funcionamiento depende de que los servicios de dominio estén disponibles.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
