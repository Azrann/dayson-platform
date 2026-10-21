# dayson.ui

Interfaz de DaysOn/HrPlan para autenticación, administración de empresas/equipos/empleados y consulta o aprobación de vacaciones. Incluye dos generaciones del frontend.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

<!-- BEGIN project-context -->

## Antigüedad y evidencia

**Año de referencia de esta versión: 2018.** Se toma el archivo de código o configuración más reciente con una fecha histórica recuperable; no se ha podido confirmar la fecha exacta de creación.

- Archivo más reciente datable: **2018-06-24**, [`dayson.ui/v2/src/index.html`](v2/src/index.html); fecha del commit que modificó el archivo, recuperada del Git local.
- Primer registro del repositorio Git original que contiene este código: **2015-10-19**. Puede corresponder a una importación; no prueba cuándo se escribió por primera vez.
- Dependencia fechada más reciente entre las comprobadas: [@angular/core 5.2.10](https://www.npmjs.com/package/@angular/core/v/5.2.10), publicada el **2018-04-16** (versión fijada en el lockfile), en [`dayson.ui/v2/package.json`](v2/package.json). Fecha contrastada con la versión de [`dayson.ui/v2/package-lock.json`](v2/package-lock.json).

La publicación de una dependencia sitúa la configuración conservada a partir de esa versión, si la referencia es válida. Una actualización de paquetes puede ser mucho más reciente que el inicio del proyecto. No se restauraron paquetes para comprobarlo.

Se descartaron las marcas de tiempo de la copia de octubre de 2026 y los documentos generados durante esta revisión. Las fechas recuperables no garantizan que no existan cambios posteriores sin datar.

## Por qué merece la pena mirarlo

El calendario está descompuesto en componentes de día, mes y año, junto con vistas por equipo y aprobaciones. Es interesante estudiar cómo un dominio de vacaciones termina expresado en componentes visuales y acciones de usuario.

Puntos de entrada al código:

- [`v2/src/app/holidays/calendar/calendar-year.component.ts`](v2/src/app/holidays/calendar/calendar-year.component.ts)
- [`v2/src/app/holidays/calendar/calendar-month.component.ts`](v2/src/app/holidays/calendar/calendar-month.component.ts)
- [`v2/src/app/holidays/approve-view/approve-view.component.ts`](v2/src/app/holidays/approve-view/approve-view.component.ts)

<!-- END project-context -->

## Estructura y responsabilidades

| Ruta | Responsabilidad |
| --- | --- |
| `src/` | Versión AngularJS 1.4, Angular Route, traducciones, Bootstrap 3 y tareas Grunt/Bower. |
| `v2/` | Versión Angular 5.2 con Angular Material, RxJS, moment y PubNub. |
| `v2/src/app/admin/` | Administración de empresa, equipos y usuarios. |
| `v2/src/app/holidays/` | Calendarios, solicitudes y aprobaciones. |
| `v2/src/app/login/` | Registro, acceso y recuperación de contraseña. |

## Tecnologías y requisitos

Node/npm compatible con Angular CLI 1.x para v2. La versión anterior necesita Grunt 0.4 y Bower. No se ha fijado una versión nueva de Node durante esta revisión.

## Preparación y arranque

Configurar las URLs de los servicios DaysOn, empezando por la fachada y autenticación. npm run build compila v2; npm test, npm run lint y npm run e2e están declarados pero no se ejecutaron.

Comandos de preparación, desde esta carpeta, después de revisar la configuración privada:

```text
cd v2
npm install
npm start
```

## Configuración e integraciones

Revisar los servicios HTTP y los archivos de entorno antes de abrir la interfaz. gruntfile.js contiene datos de despliegue y sign-in.component.ts claves PubNub para el navegador.

## Relación con la aplicación DaysOn

Esta carpeta es un componente de DaysOn/HrPlan, dentro de una aplicación distribuida.

- [dayson.facade](../dayson.facade/Readme.md): API de coordinación para clientes web.
- [dayson.auth](../dayson.auth/Readme.md): Identidad, tokens y cuentas.

El [índice general](../README.md) incluye el mapa del conjunto. Las relaciones descritas proceden de controladores, clientes, contratos y configuración visibles; no se ha verificado su despliegue en ejecución.

## Estado de esta copia

Las claves publish/subscribe del navegador deben revisarse según sus permisos; su presencia no equivale por sí sola a encontrar una clave secreta de PubNub.

La revisión fue estática: no se restauraron dependencias, no se compiló ni se ejecutaron aplicaciones o pruebas. Los comandos anteriores son instrucciones de preparación, no resultados de validación. Las versiones indicadas son las del código conservado.


## Copia pública

Las credenciales originales se han retirado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Los datos locales, sesiones, certificados privados y perfiles de despliegue se omiten. Configurar servicios y credenciales propios antes de ejecutar. La compilación y el funcionamiento con servicios externos no se han validado.
