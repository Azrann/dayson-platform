# dayson.ui

Interfaz de DaysOn/HrPlan para autenticación, administración de empresas/equipos/empleados y consulta o aprobación de vacaciones. Incluye dos generaciones del frontend.

Documentación basada en los archivos locales revisados el 21 de octubre de 2026.

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
