# dayson.logapi

API de consulta y limpieza de registros de DaysOn, con almacenamiento SQL y acceso mediante Dapper.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

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
