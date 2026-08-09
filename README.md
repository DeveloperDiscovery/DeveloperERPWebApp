# ERP KONSolutions · Aplicación Web (Blazor WebAssembly .NET 9)

Aplicación web que consume la API REST de ERP KONSolutions. Cumple los requisitos:
seguridad por JWT de la API, log de errores, log de accesos (página/módulo/evento/
parámetros), menú lateral, workspace con favoritos (clic derecho), y módulo de Usuarios
completo.

## Stack
- Blazor WebAssembly (.NET 9)
- MudBlazor (UI)
- Blazored.LocalStorage (token + favoritos)
- Proyecto Shared con los DTOs

## Requisitos cumplidos

### Seguridad de la API (JWT)
- `Login.razor` → `POST /api/v1/auth/login`.
- `JwtMessageHandler` añade `Bearer {token}` a cada request automáticamente.
- `JwtAuthStateProvider` controla sesión y expiración.

### Log de errores
- `ErrorLogService`: registra en consola del navegador y envía a
  `api/v1/developer/error-log` (programa, clase/página, método, mensaje, detalle, parámetros).

### Log de accesos
- `AccessLogService`: registra navegación y eventos con sus parámetros, a cada
  página/módulo/evento, y los envía a `api/v1/developer/access-log`.
- Se invoca en cada acción: navegación, búsqueda, CRUD, reset, password, estado,
  agregar/quitar favorito, login/logout.

### Menú lateral
- `MainLayout.razor`: menú lateral que carga las **opciones autorizadas** del rol
  mediante `MenuService` → SP `PROC_SEGURIDAD_ROLES_USUARIOS_APLICACIONES_OPCIONES_SHOWALL`.

### Workspace con favoritos (tiles)
- `Workspace.razor`: tiles tipo favorito en la zona central.
- **Clic derecho** sobre una opción → menú contextual propio → "Agregar a favoritos".
- Sobre un favorito → "Quitar de favoritos".
- Clic izquierdo abre la opción. Favoritos persistidos en localStorage.

### Botones autorizados
- `MenuService.GetBotonesAsync` → SP
  `PROC_SEGURIDAD_ROLES_USUARIOS_APLICACIONES_OPCIONES_BOTONES_SEARCH`.

### Módulo de Usuarios (SEGURIDAD.USUARIOS)
- CRUD completo (grilla `Usuarios.razor` + `UsuarioDialog`).
- **Búsqueda** con textbox → `PROC_SEGURIDAD_USUARIOS_SEARCH`.
- Iconos por fila: **Ver, Editar (Update), Cambiar contraseña, Reset Intentos,
  Cambiar Estado, Eliminar (Delete)**.
- Reset intentos → `PROC_SEGURIDAD_USUARIOS_RESETINTENTOS`.
- Cambiar contraseña → `PROC_SEGURIDAD_USUARIOS_PASSWORD`.
- Cambiar estado → `PROC_SEGURIDAD_USUARIOS_ESTADOS`.

## Estructura
```
KONSolutions.sln
├── KONSolutions.Shared/        DTOs (Auth, Menu, Usuario) + ApiResponse
├── KONSolutions.Web/
│   ├── Services/                Auth, JWT, ErrorLog, AccessLog, Menu, Favoritos, Usuario
│   ├── Layout/                  MainLayout (menú lateral), EmptyLayout (login)
│   └── Pages/
│       ├── Auth/Login.razor
│       ├── Workspace.razor      (favoritos + menú contextual)
│       └── Seguridad/           Usuarios + 4 diálogos
└── _BackendReferencia/          Endpoints a integrar en la API
    ├── MenuControllers.cs       opciones + botones autorizados
    └── DeveloperLogController.cs  error-log + access-log
```

## Antes de ejecutar

1. **Integrar en la API** los controllers de `_BackendReferencia/`:
   - `MenuControllers.cs` → endpoints de opciones y botones de menú.
   - `DeveloperLogController.cs` → recibe los logs (opcional; si no, el front loguea en consola).
   - Recuerda que la API ya debe tener el `AuthController` real y los endpoints
     especiales de usuarios (password/estados/resetintentos) de las entregas anteriores.

2. **URL de la API**: en `wwwroot/appsettings.json`, ajustar `ApiBaseUrl` al puerto real.

3. **CORS**: la API debe permitir el origen del front (ya tiene política `AllowAll`).

## Ejecutar
```bash
dotnet restore
dotnet run --project KONSolutions.Web
```

## Notas honestas
- `COD_APLICACION` está fijado en 1 en `MenuService`. Ajustar al código real de esta aplicación.
- El mapeo opción→ruta en el menú usa `DES_FORMULARIO` por convención (si contiene
  "usuario" → /seguridad/usuarios). Ajustar según tus nombres de formulario reales.
- Asumí rutas de combobox `seguridad/perfiles-usuario/combobox` y `estandar/estados/combobox`.
  Si difieren, se ajustan en `UsuarioDialog` y `CambiarEstadoDialog`.
- No se pudo compilar en el entorno de generación (sin SDK); validación estática. Revisa el
  primer `dotnet build` por ajustes menores de versiones NuGet.

---

## Módulos de seguridad agregados (catálogos)

Se agregaron 4 módulos CRUD nuevos, accesibles desde el menú lateral:

| Módulo | Ruta | SP combobox usado |
|--------|------|-------------------|
| Perfiles | /seguridad/perfiles | PROC_SEGURIDAD_PERFILES_USUARIO_SHOWCOMBOX |
| Roles | /seguridad/roles | PROC_SEGURIDAD_ROLES_USUARIO_SHOWCOMBOX |
| Aplicaciones | /seguridad/aplicaciones | PROC_SEGURIDAD_APLICACIONES_SHOWCOMBOX |
| Complejidad contraseña | /seguridad/complejidad-password | PROC_SEGURIDAD_COMPLEJIDAD_PASSWORD_SHOWCOMBOX |

### Validación de contraseña por complejidad
- `PasswordPolicyService` valida la contraseña contra las reglas de la tabla
  SEGURIDAD.COMPLEJIDAD_PASSWORD (longitud mín/máx, exigir mayúsculas, minúsculas,
  números y/o especiales según lo definido).
- Al cambiar la contraseña de un usuario, se resuelve la regla de complejidad
  a partir de su perfil (COD_PERFIL_USUARIO → COD_PASSWORD_COMPLEJIDAD) y se valida
  antes de enviar al servidor. Si no cumple, muestra los errores y no graba.

### CatalogoService genérico
Los 4 módulos comparten `CatalogoService<TEntidad, TDto>`, un servicio CRUD genérico
reutilizable (GetAll, Combobox, GetById, Guardar, Eliminar) con log de accesos y errores.

### Combobox: corrección importante
Los SP _SHOWCOMBOX devuelven las columnas reales (COD_x, DES_x), no Id/Texto. En la API,
los repositorios de combobox se ajustaron para proyectar COD_x→Id y DES_x→Texto. Por eso
ahora el combo de perfiles del formulario de usuarios muestra datos.

---

## Pauta de diálogos CRUD (IMPORTANTE)

Los diálogos CRUD usan SIEMPRE componentes de diálogo SEPARADOS con
DialogService.ShowAsync<XDialog>, NO el patrón inline @bind-Visible="_open".

Patrón correcto (confiable, cierra con Cancelar y Guardar):
- Componente XDialog.razor con [CascadingParameter] MudDialogInstance MudDialog
- Cancelar: MudDialog.Cancel()
- Guardar OK: MudDialog.Close(DialogResult.Ok(true))
- La página llama: await DialogService.ShowAsync<XDialog>(titulo, parameters, options)
  y refresca con: if (!result.Canceled) await Load();

El patrón inline @bind-Visible="_open" NO cierra de forma fiable en MudBlazor 7.15
y NO debe usarse. Aplica a: Roles, Aplicaciones, ComplejidadPassword (ya migrados),
y a TODOS los CRUD futuros.

appsettings.json front: "ApiBaseUrl": "https://localhost:62599/"

---
## Entrega 2: Imágenes, Estados Estándar y tabs de Usuarios

### Manejo de imágenes (ImagePicker mejorado)
Componente Pages/Shared/ImagePicker.razor con: Seleccionar, Ver (ampliar en diálogo),
Borrar, Copiar (la imagen del CRUD al portapapeles) y Pegar (desde el portapapeles).
Requiere el helper JS window.imageClipboard en wwwroot/index.html.
Usado en: Aplicaciones (IMG_ICONO), Tipos de Estados, Estados, Estados Estándar (IMG_PICTURE).

### Opción Ver en todas las pantallas
Diálogo genérico Pages/Shared/VerRegistroDialog.razor (solo lectura, muestra campos
como pares etiqueta-valor, con preview de imágenes byte[]). Botón Visibility en las grillas.

### Estados Estándar (ESTANDAR.ESTADOS_ESTANDAR)
CRUD completo con imagen y colores. Ruta /estandar/estados-estandar.

### Estados: combo de Tipos de Motivos + importar desde estándar
- Combo PROC_ESTANDAR_TIPOS_MOTIVOS_SHOWCOMBOX (acepta null).
- Botón "Importar de estándar": selección múltiple de Estados Estándar e inserción
  con el SP PROC_ESTANDAR_ESTADOS_FROM_ESTADOS_ESTANDAR_INSERT (incluir en la BD).

### Tabs en la pantalla de Usuarios (sección inferior al hacer clic en una fila)
- Tab 1: Roles asignados (PROC_SEGURIDAD_ROLES_USUARIO_USUARIOS_SHOWALL).
- Tab 2: Opciones (TreeView) con botones por opción
  (check verde si FLG_HABILITADO/FLG_ACCESO_PERMITIDO, rojo si no).
  Selector "Por Rol" (un árbol por rol) / "Todos los Roles" (distinct de opciones).
  SP: ROLES_USUARIOS_APLICACIONES_OPCIONES_SHOWALL y ..._BOTONES_SHOWALL.
  Endpoints API: /seguridad/usuario-asignaciones/{roles|opciones|botones}/...

NOTA: la aplicación por defecto para opciones/botones es COD_APLICACION="1"
(constante en AsignacionService). Ajustar si corresponde.
