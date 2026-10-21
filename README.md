# dayson-platform

Aplicación DaysOn/HrPlan: identidad, empresas, vacaciones, mensajería, registro y clientes web.

## Contenido

| Directorio | Función |
| --- | --- |
| [`dayson.auth/`](dayson.auth/) | Servicio de identidad de DaysOn/HrPlan: registro, inicio de sesión, emisión de tokens, activación de cuentas, recuperación de contraseñas y asignación de roles relacionados con equipos. |
| [`dayson.common/`](dayson.common/) | Bibliotecas e infraestructura compartidas por los componentes de DaysOn/HrPlan. |
| [`dayson.company/`](dayson.company/) | Servicio de organizaciones de DaysOn/HrPlan: empresas, empleados, equipos y publicación de sus eventos de negocio. |
| [`dayson.facade/`](dayson.facade/) | Fachada API de DaysOn/HrPlan: ofrece operaciones orientadas a la interfaz y coordina llamadas a autenticación, empresas y vacaciones. |
| [`dayson.functions/`](dayson.functions/) | Procesamiento de eventos y operaciones de apoyo de DaysOn/HrPlan: validación de solicitudes de vacaciones, cálculo de días laborables y envío de correos. |
| [`dayson.holidays/`](dayson.holidays/) | Servicio de vacaciones y ausencias de DaysOn/HrPlan: solicitudes, aprobaciones, reglas de empresa, calendario laboral y saldos anuales. |
| [`dayson.log/`](dayson.log/) | Interfaz web de consulta de registros de DaysOn, hecha con AngularJS, jQuery y Bootstrap. |
| [`dayson.logapi/`](dayson.logapi/) | API de consulta y limpieza de registros de DaysOn, con almacenamiento SQL y acceso mediante Dapper. |
| [`dayson.tower/`](dayson.tower/) | Carpeta de repositorio DaysOn sin archivos de trabajo: únicamente se encontró el directorio .git. |
| [`dayson.ui/`](dayson.ui/) | Interfaz de DaysOn/HrPlan para autenticación, administración de empresas/equipos/empleados y consulta o aprobación de vacaciones. Incluye dos generaciones del frontend. |

Los directorios dayson.* son microservicios y bibliotecas de una misma aplicación. Cada uno conserva su solución y su configuración. dayson.tower no contenía implementación.

## Requisitos y preparación

.NET Framework, ASP.NET Web API/OWIN, IIS, Azure Functions, NuGet y clientes web.

Esta es una copia histórica. Consultar los archivos de proyecto y los README de cada componente para sus versiones y dependencias. Restaurar NuGet/npm desde la carpeta correspondiente; las bibliotecas internas pueden necesitar reconstrucción. Preparar las bases de datos y los servicios externos con cuentas propias.

Soluciones conservadas:

- `dayson.auth/src/DaysOn.Auth.sln`
- `dayson.common/src/DaysOn.Common.sln`
- `dayson.company/src/DaysOn.CompanyBc.sln`
- `dayson.facade/src/DaysOn.Facade.sln`
- `dayson.functions/src/DaysOn.Functions.sln`
- `dayson.holidays/src/DaysOn.HolidaysBc.sln`
- `dayson.log/src/DaysonLog.sln`
- `dayson.logapi/src/DaysOn.Log.IISHost.sln`

Paquetes JavaScript principales:

- `dayson.ui/src/package.json`
- `dayson.ui/v2/package.json`

## Configuración y seguridad

Los secretos originales se han eliminado o sustituido por `REPLACE_WITH_YOUR_VALUE`. Buscar este marcador antes de ejecutar y aportar los valores mediante configuración local o variables de entorno. Las sesiones, claves privadas, certificados de firma, bases de datos, registros, perfiles de despliegue y paquetes binarios no se publican. El `.gitignore` protege estos archivos locales.

Se publica un historial nuevo: los commits antiguos permanecen únicamente en la copia local original. Los valores de ejemplo no permiten conectarse a los servicios originales.

## Estado de validación

Se realizó una revisión estática y un escaneo de secretos antes de publicar. No se ejecutaron aplicaciones, operaciones de trading, despliegues ni pruebas que contacten servicios externos. No se garantiza la compilación con herramientas actuales. Algunos proyectos necesitan dependencias históricas y activos que no están incluidos.

## Licencias y procedencia

Se conservan las licencias y atribuciones de terceros presentes en el código. No se añade una licencia global a los componentes que no la tenían. Esta importación conserva fuentes locales y no afirma ser una versión actual del proyecto original.
