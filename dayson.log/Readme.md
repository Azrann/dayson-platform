# dayson.log

Interfaz web de consulta de registros de DaysOn, hecha con AngularJS, jQuery y Bootstrap.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

## Estructura y responsabilidades

| Ruta | Responsabilidad |
| --- | --- |
| `src/DaysonLog/index.html` | Vista de registros. |
| `src/DaysonLog/js/daysonlogapisApp.js` | Carga registros y errores desde la API y contiene una llamada de limpieza. |
| `src/DaysonLog/lib/, src/DaysonLog/Content/` | Dependencias y estilos. |
| `src/DaysonLog.sln` | Solución de sitio web para publicación desde Visual Studio. |

## Tecnologías y requisitos

Un servidor HTTP estático y dayson.logapi disponible; las bibliotecas del navegador están incluidas.

## Preparación y arranque

Servir src/DaysonLog por HTTP y adaptar las URLs de la API en daysonlogapisApp.js. Verificar CORS y autorización en el backend.

Soluciones presentes: `src/DaysonLog.sln`.

## Configuración e integraciones

La conexión al backend está fijada en el JavaScript. Los perfiles de publicación contienen datos del destino de alojamiento.

## Relación con la aplicación DaysOn

Esta carpeta es un componente de DaysOn/HrPlan, dentro de una aplicación distribuida.

- [dayson.logapi](../dayson.logapi/Readme.md): API y persistencia de logs.

El [índice general](../README.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

Es una interfaz del sistema DaysOn. La operación de limpieza puede borrar registros en el servicio al que apunte.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
