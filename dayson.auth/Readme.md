# dayson.auth

Servicio de identidad de DaysOn/HrPlan: registro, inicio de sesión, emisión de tokens, activación de cuentas, recuperación de contraseñas y asignación de roles relacionados con equipos.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

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
