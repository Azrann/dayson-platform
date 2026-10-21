# dayson.log

Interfaz web de consulta de registros de DaysOn, hecha con AngularJS, jQuery y Bootstrap.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

<!-- BEGIN project-context -->

## Antigüedad y evidencia

**Año de referencia de esta versión: 2016.** Se toma el archivo de código o configuración más reciente con una fecha histórica recuperable; no se ha podido confirmar la fecha exacta de creación.

- Archivo más reciente datable: **2016-11-26**, [`dayson.log/src/DaysonLog/js/daysonlogapisApp.js`](src/DaysonLog/js/daysonlogapisApp.js); fecha del commit que modificó el archivo, recuperada del Git local.
- Primer registro del repositorio Git original que contiene este código: **2016-10-08**. Puede corresponder a una importación; no prueba cuándo se escribió por primera vez.

Se descartaron las marcas de tiempo de la copia de octubre de 2026 y los documentos generados durante esta revisión. Las fechas recuperables no garantizan que no existan cambios posteriores sin datar.

## Por qué merece la pena mirarlo

Es una interfaz AngularJS pequeña para consultar registros y errores mediante una API separada. Su tamaño permite seguir de principio a fin el enlace entre un servicio HTTP, el controlador de pantalla y la presentación de los resultados.

Puntos de entrada al código:

- [`src/DaysonLog/js/daysonlogapisApp.js`](src/DaysonLog/js/daysonlogapisApp.js)
- [`src/DaysonLog/index.html`](src/DaysonLog/index.html)

<!-- END project-context -->

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
