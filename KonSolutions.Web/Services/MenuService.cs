using System.Net.Http.Json;
using Blazored.LocalStorage;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Auth;
using KONSolutions.Shared.Models.Seguridad;

namespace KONSolutions.Web.Services;

/// <summary>
/// Carga roles, opciones de menú y botones autorizados del usuario logueado.
/// Diseño: al login (o al cambiar de rol activo) se hace UNA sola llamada al SP
/// TREEVIEW que trae todos los botones de todas las opciones del rol — el resto
/// del sistema filtra en memoria (PorOpcion) sin volver a llamar a la API.
/// </summary>
public class MenuService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    /// <summary>Botones del rol activo, cacheados en memoria tras el último Cargar/CambiarRol.</summary>
    private List<BotonTreeviewItem> _botones = new();
    private List<OpcionTreeviewItem> _opciones = new();

    /// <summary>FLG_TECLADO_VIRTUAL de una opción (pantalla). null si no se encuentra.</summary>
    public bool? TecladoVirtualOpcion(string? codAplicacion, string? codOpcion)
    {
        if (string.IsNullOrWhiteSpace(codAplicacion) || string.IsNullOrWhiteSpace(codOpcion)) return null;
        var o = _opciones.FirstOrDefault(x =>
            x.COD_APLICACION == codAplicacion && x.COD_OPCION_APLICACION == codOpcion);
        return o?.FLG_TECLADO_VIRTUAL;
    }

    /// <summary>FLG_EXIGE_PASSWORD de una opción (pantalla). null si no se encuentra.
    /// Usar antes de abrir CUALQUIER pantalla desde CUALQUIER punto de entrada (menú
    /// lateral, favoritos del Workspace, tiles, búsqueda, etc.) — todos deben pasar
    /// por aquí en vez de construir su propio objeto sin este flag.</summary>
    public bool? ExigePasswordOpcion(string? codAplicacion, string? codOpcion)
    {
        if (string.IsNullOrWhiteSpace(codAplicacion) || string.IsNullOrWhiteSpace(codOpcion)) return null;
        var o = _opciones.FirstOrDefault(x =>
            x.COD_APLICACION == codAplicacion && x.COD_OPCION_APLICACION == codOpcion);
        return o?.FLG_EXIGE_PASSWORD;
    }

    /// <summary>DES_OPCION_APLICACION configurado en el menú para esta pantalla — permite que
    /// el título visible en la página salga de la configuración (Seguridad › Opciones de
    /// Aplicaciones) en vez de quedar hardcodeado en el .razor. null si no se encuentra
    /// (p.ej. la pantalla se abrió sin CodAplicacion/CodOpcionAplicacion) — el llamador debe
    /// tener un texto de respaldo para ese caso.</summary>
    public string? DescripcionOpcion(string? codAplicacion, string? codOpcion)
    {
        if (string.IsNullOrWhiteSpace(codAplicacion) || string.IsNullOrWhiteSpace(codOpcion)) return null;
        var o = _opciones.FirstOrDefault(x =>
            x.COD_APLICACION == codAplicacion && x.COD_OPCION_APLICACION == codOpcion);
        return o?.DES_OPCION_APLICACION;
    }
    private List<RolDeUsuario> _roles = new();
    private string? _rolActivo;

    /// <summary>Se dispara cuando los íconos cargados en segundo plano (TreeviewLogos) llegan
    /// y ya están fusionados en _botones/_opciones — las pantallas de menú (tile, lateral,
    /// circular) deben suscribirse y reconstruir su vista (StateHasChanged) al recibirlo.</summary>
    public event Action? IconosActualizados;

    public MenuService(HttpClient http, ILocalStorageService localStorage)
    { _http = http; _localStorage = localStorage; }

    /// <summary>Roles asignados al usuario logueado. Vacío si aún no se cargó (llamar CargarAsync primero).</summary>
    public IReadOnlyList<RolDeUsuario> Roles => _roles;

    /// <summary>Rol actualmente activo (el que determina menú y permisos visibles).</summary>
    public string? RolActivo => _rolActivo;

    /// <summary>El selector de rol en la barra superior solo debe mostrarse con 2+ roles.</summary>
    public bool TieneVariosRoles => _roles.Count > 1;

    /// <summary>
    /// Carga inicial al login: roles del usuario + botones/opciones del rol activo
    /// (el primero de la lista, o el que ya estuviera guardado en sesión).
    /// </summary>
    public async Task CargarAsync()
    {
        var t0 = DateTime.Now;
        Console.WriteLine($"[TRACE] {t0:HH:mm:ss.fff} — CARGA DE MENU - MenuService: inicio MisRoles");
        try
        {
            var rRoles = await _http.GetFromJsonAsync<ApiResponse<List<RolDeUsuario>>>(ApiRoutes.Seguridad.MisRoles);
            _roles = rRoles?.Data ?? new();
        }
        catch { _roles = new(); }
        Console.WriteLine($"[TRACE] {DateTime.Now:HH:mm:ss.fff} — CARGA DE MENU - MenuService: fin MisRoles (+{(DateTime.Now-t0).TotalMilliseconds:0}ms)");

        // Rol activo: el que ya tenga la sesión (UsuarioSesion.COD_ROL_USUARIO),
        // o el primero de la lista de roles si no hay uno definido.
        var sesion = await _localStorage.GetItemAsync<UsuarioSesion>("usuarioSesion");
        _rolActivo = sesion?.COD_ROL_USUARIO;
        if (string.IsNullOrWhiteSpace(_rolActivo))
            _rolActivo = _roles.FirstOrDefault()?.COD_ROL_USUARIO;

        Console.WriteLine($"[TRACE] {DateTime.Now:HH:mm:ss.fff} — CARGA DE MENU - MenuService: inicio BotonesTreeview (rol={_rolActivo})");
        await CargarBotonesAsync();
        Console.WriteLine($"[TRACE] {DateTime.Now:HH:mm:ss.fff} — CARGA DE MENU - MenuService: fin BotonesTreeview (+{(DateTime.Now-t0).TotalMilliseconds:0}ms total)");
    }

    /// <summary>
    /// Cambia el rol activo: vuelve a pedir el treeview de botones para ese rol
    /// específico. El componente que llama esto debe, además, reconstruir el menú
    /// lateral y cerrar todas las pestañas salvo Workspace.
    /// </summary>
    public async Task CambiarRolAsync(string codRolUsuario)
    {
        // Limpiar caché del nuevo rol antes de cargarlo para evitar datos viejos
        var prevRol = _rolActivo;
        _rolActivo = codRolUsuario;
        try
        {
            await _localStorage.RemoveItemAsync($"botonesTreeview_{codRolUsuario}");
            await _localStorage.RemoveItemAsync($"botonesTreeview_{codRolUsuario}_ts");
        }
        catch { }
        await CargarBotonesAsync();
    }

    /// <summary>Fuerza una recarga del árbol de botones/opciones del rol activo, saltando la
    /// caché de localStorage (TTL 60 min) — se usa cuando SEGURIDAD.APLICACIONES_OPCIONES o
    /// SEGURIDAD.APLICACIONES_OPCIONES_BOTONES cambian (ver MainLayout.RefrescarMenuPorCambio),
    /// para no esperar hasta que venza la caché ni un nuevo login.</summary>
    public async Task ForzarRecargaAsync()
    {
        if (string.IsNullOrWhiteSpace(_rolActivo)) return;
        try
        {
            await _localStorage.RemoveItemAsync($"botonesTreeview_{_rolActivo}");
            await _localStorage.RemoveItemAsync($"botonesTreeview_{_rolActivo}_ts");
        }
        catch { }
        await CargarBotonesAsync();
    }

    private static readonly TimeSpan _cacheTtl = TimeSpan.FromMinutes(60);

    private async Task CargarBotonesAsync()
    {
        if (string.IsNullOrWhiteSpace(_rolActivo)) { _botones = new(); return; }

        var cacheKey = $"botonesTreeview_{_rolActivo}";
        var tsKey    = $"botonesTreeview_{_rolActivo}_ts";

        // ── Leer caché ──────────────────────────────────────────────────────
        // El SP ahora devuelve 3 resultados deduplicados (aplicaciones/opciones/botones,
        // cada ícono UNA sola vez) — eso es lo que se cachea, mucho más liviano que antes
        // (el ícono ya no viaja repetido por cada botón de la misma app/opción).
        TreeviewResult? cached = null;
        bool cacheVencida = true;
        try
        {
            var ts = await _localStorage.GetItemAsync<DateTime?>(tsKey);
            cached = await _localStorage.GetItemAsync<TreeviewResult>(cacheKey);
            cacheVencida = !ts.HasValue || DateTime.Now - ts.Value >= _cacheTtl;
        }
        catch { }

        // ── Stale-while-revalidate ──────────────────────────────────────────
        // Si hay datos en caché (aunque estén vencidos), úsalos YA para que el
        // menú aparezca de inmediato. Luego refresca en segundo plano.
        if (cached is { Botones.Count: > 0 })
        {
            _botones = Fusionar(cached);
            _opciones = cached.Opciones;
            Console.WriteLine($"[TRACE] {DateTime.Now:HH:mm:ss.fff} — CARGA DE MENU - BotonesTreeview desde CACHE ({_botones.Count} items, vencida={cacheVencida})");
            if (cacheVencida)
                _ = RefrescarBotonesEnSegundoPlanoAsync(cacheKey, tsKey);
            else
                _ = MergeFlagsExigePasswordAsync(cached.Opciones);
            _ = CargarLogosEnSegundoPlanoAsync();
            return;
        }

        // ── Sin caché: cargar bloqueante (primera vez) ──────────────────────
        await FetchBotonesAsync(cacheKey, tsKey);
    }

    private async Task RefrescarBotonesEnSegundoPlanoAsync(string cacheKey, string tsKey)
    {
        await FetchBotonesAsync(cacheKey, tsKey);
        Console.WriteLine($"[TRACE] {DateTime.Now:HH:mm:ss.fff} — CARGA DE MENU - BotonesTreeview refresco bg completado ({_botones.Count} items)");
    }

    /// <summary>
    /// El SP TREEVIEW no siempre trae FLG_EXIGE_PASSWORD en su resultado de opciones —
    /// se fusiona aquí desde el maestro plano GET /seguridad/aplicaciones-opciones, que
    /// sí la tiene siempre (misma columna que ya usa el diálogo de edición de opciones).
    /// Se llama tanto al cargar desde caché como al refrescar desde la API, para que el
    /// gate de contraseña funcione incluso en la primera pintura desde caché.
    /// </summary>
    private async Task MergeFlagsExigePasswordAsync(List<OpcionTreeviewItem> opciones)
    {
        try
        {
            // GET /seguridad/aplicaciones-opciones (sin parámetros) devuelve 400 en este
            // ambiente — se pide por app usando /by-app, el mismo endpoint que ya usa el
            // diálogo de edición de opciones y funciona bien.
            var flagsMapa = new Dictionary<(string, string), KONSolutions.Shared.Models.Seguridad.AplicacionesOpcionesFlagsRow>();
            var apps = opciones.Select(o => o.COD_APLICACION).Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();

            // Todas las apps en paralelo en lugar de secuencial (N+1 → 1 ronda) — antes cada
            // login/carga sin caché esperaba una ronda HTTP completa por CADA aplicación
            // distinta del menú, una detrás de la otra, sumando varios segundos.
            var tareas = apps.Select(app => _http.GetFromJsonAsync<ApiResponse<List<KONSolutions.Shared.Models.Seguridad.AplicacionesOpcionesFlagsRow>>>(
                $"{ApiRoutes.Seguridad.AplicacionesOpciones}/by-app?codAplicacion={Uri.EscapeDataString(app)}")).ToList();
            var resultados = await Task.WhenAll(tareas);
            foreach (var flagsResp in resultados)
                if (flagsResp?.Data is not null)
                    foreach (var f in flagsResp.Data)
                        flagsMapa[(f.COD_APLICACION, f.COD_OPCION_APLICACION)] = f;

            foreach (var o in opciones)
                if (flagsMapa.TryGetValue((o.COD_APLICACION, o.COD_OPCION_APLICACION), out var f))
                    o.FLG_EXIGE_PASSWORD = f.FLG_EXIGE_PASSWORD;

            // Ahora corre en segundo plano (ver FetchBotonesAsync) — avisa a las pantallas de
            // menú abiertas para que repinten el candado de "exige contraseña" si corresponde.
            IconosActualizados?.Invoke();
        }
        catch { /* si falla la fusión, el menú igual carga sin el gate de contraseña */ }
    }

    private async Task FetchBotonesAsync(string cacheKey, string tsKey)
    {
        try
        {
            var url = $"{ApiRoutes.Seguridad.BotonesTreeview}?codRolUsuario={Uri.EscapeDataString(_rolActivo!)}";
            var resp = await _http.GetFromJsonAsync<ApiResponse<TreeviewResult>>(url);
            var data = resp?.Data ?? new TreeviewResult();

            // Experimento: el primer render de tile/lateral/circular sale sin íconos (texto
            // ya alcanza para pintar el menú); los íconos llegan aparte y sin bloquear, vía
            // CargarLogosEnSegundoPlanoAsync() abajo — igual que Entidades y Opciones de
            // Aplicaciones. Se limpian aquí por si el SP TREEVIEW todavía los trae embebidos
            // (si el SP en BD todavía hace SELECT de esas columnas, esto NO evita que viajen
            // por la red desde SQL Server hasta la API — solo evita que crucen a memoria del
            // browser/UI. Para que el primer render sea realmente liviano de punta a punta,
            // el PROC_..._TREEVIEW en la base debe dejar de traer IMG_ICONO_APLICACION/
            // IMG_ICONO_OPCION — esos íconos ahora los trae el SP TREEVIEWLOGOS aparte).
            foreach (var a in data.Aplicaciones) a.IMG_ICONO_APLICACION = null;
            foreach (var o in data.Opciones)     o.IMG_ICONO_OPCION     = null;

            _botones = Fusionar(data);
            _opciones = data.Opciones;

            // FLG_EXIGE_PASSWORD y los íconos NO bloquean el primer render — ambos se piden
            // en paralelo y en segundo plano; antes MergeFlagsExigePasswordAsync se esperaba
            // aquí (una ronda HTTP por cada app distinta del menú) y retrasaba la aparición
            // del menú de tiles/lateral/circular tanto como el ícono mismo.
            _ = MergeFlagsExigePasswordAsync(data.Opciones);
            _ = CargarLogosEnSegundoPlanoAsync();
            try
            {
                // Los íconos (base64) NO se persisten en localStorage — solo viven en memoria
                // para esta sesión. Con muchas aplicaciones/opciones con ícono propio, esta
                // caché podía pesar varios MB y hacer que CUALQUIER otro guardado en
                // localStorage (ej. Configuración General con logos) fallara con
                // QuotaExceededError al superar el límite total del navegador para el origen.
                var paraCache = new TreeviewResult
                {
                    Botones = data.Botones,
                    Aplicaciones = data.Aplicaciones
                        .Select(a => new AplicacionTreeviewItem
                        {
                            COD_APLICACION = a.COD_APLICACION,
                            DES_APLICACION = a.DES_APLICACION,
                            DES_BACKCOLOR_APLICACION = a.DES_BACKCOLOR_APLICACION,
                            DES_FORECOLOR_APLICACION = a.DES_FORECOLOR_APLICACION,
                            IMG_ICONO_APLICACION = null
                        }).ToList(),
                    Opciones = data.Opciones
                        .Select(o => new OpcionTreeviewItem
                        {
                            COD_APLICACION = o.COD_APLICACION,
                            COD_TIPO_OPCION_APLICACION = o.COD_TIPO_OPCION_APLICACION,
                            DES_TIPO_OPCION_APLICACION = o.DES_TIPO_OPCION_APLICACION,
                            COD_OPCION_APLICACION_PADRE = o.COD_OPCION_APLICACION_PADRE,
                            COD_OPCION_APLICACION = o.COD_OPCION_APLICACION,
                            DES_OPCION_APLICACION = o.DES_OPCION_APLICACION,
                            DES_FORMULARIO = o.DES_FORMULARIO,
                            DES_BACKCOLOR_OPCION = o.DES_BACKCOLOR_OPCION,
                            DES_FORECOLOR_OPCION = o.DES_FORECOLOR_OPCION,
                            IMG_ICONO_OPCION = null,
                            FLG_TECLADO_VIRTUAL = o.FLG_TECLADO_VIRTUAL,
                            FLG_EXIGE_PASSWORD = o.FLG_EXIGE_PASSWORD
                        }).ToList()
                };
                await _localStorage.SetItemAsync(cacheKey, paraCache);
                await _localStorage.SetItemAsync(tsKey, DateTime.Now);
            }
            catch { }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] CARGA DE MENU - FetchBotonesAsync falló: {ex}");
            _botones = new();
        }
    }

    /// <summary>
    /// Experimento: pide los íconos de TODO el árbol del rol activo (apps/opciones/botones)
    /// en un endpoint aparte del texto (TreeviewLogos) — no bloquea el primer render de los
    /// menús tile/lateral/circular, que ya se pintaron solo con texto. Al llegar, fusiona los
    /// íconos en _botones/_opciones (en memoria, sin localStorage) y avisa via IconosActualizados
    /// para que las pantallas de menú abiertas se repinten.
    /// </summary>
    private async Task CargarLogosEnSegundoPlanoAsync()
    {
        if (string.IsNullOrWhiteSpace(_rolActivo)) return;
        var t0 = DateTime.Now;
        Console.WriteLine($"[TRACE] {t0:HH:mm:ss.fff} — CARGA DE ICONOS - MenuService: inicio TreeviewLogos (rol={_rolActivo})");
        try
        {
            var url = $"{ApiRoutes.Seguridad.BotonesTreeviewLogos}?codRolUsuario={Uri.EscapeDataString(_rolActivo)}";
            var resp = await _http.GetFromJsonAsync<ApiResponse<TreeviewLogosResult>>(url);
            Console.WriteLine($"[TRACE] {DateTime.Now:HH:mm:ss.fff} — CARGA DE ICONOS - MenuService: fin HTTP TreeviewLogos (+{(DateTime.Now - t0).TotalMilliseconds:0}ms, apps={resp?.Data?.Aplicaciones.Count ?? 0}, opciones={resp?.Data?.Opciones.Count ?? 0}, botones={resp?.Data?.Botones.Count ?? 0})");
            var data = resp?.Data;
            if (data is null) return;

            var iconosApp = data.Aplicaciones.ToDictionary(a => a.COD_APLICACION, a => a.IMG_ICONO_APLICACION);
            var iconosOpcion = data.Opciones.ToDictionary(o => (o.COD_APLICACION, o.COD_OPCION_APLICACION), o => o.IMG_ICONO_OPCION);
            var iconosBoton = data.Botones
                .Where(b => !string.IsNullOrWhiteSpace(b.COD_BOTON_OPCION))
                .ToDictionary(b => (b.COD_APLICACION, b.COD_OPCION_APLICACION, b.COD_BOTON_OPCION!), b => b.IMG_ICONO);

            foreach (var b in _botones)
            {
                if (b.IMG_ICONO_APLICACION is null && iconosApp.TryGetValue(b.COD_APLICACION, out var iconoApp))
                    b.IMG_ICONO_APLICACION = iconoApp;
                if (b.IMG_ICONO_OPCION is null && iconosOpcion.TryGetValue((b.COD_APLICACION, b.COD_OPCION_APLICACION), out var iconoOpcion))
                    b.IMG_ICONO_OPCION = iconoOpcion;
                if (b.IMG_ICONO_BOTON is null && !string.IsNullOrWhiteSpace(b.COD_BOTON_OPCION) &&
                    iconosBoton.TryGetValue((b.COD_APLICACION, b.COD_OPCION_APLICACION, b.COD_BOTON_OPCION), out var iconoBoton))
                    b.IMG_ICONO_BOTON = iconoBoton;
            }
            foreach (var o in _opciones)
                if (o.IMG_ICONO_OPCION is null && iconosOpcion.TryGetValue((o.COD_APLICACION, o.COD_OPCION_APLICACION), out var iconoOpcion))
                    o.IMG_ICONO_OPCION = iconoOpcion;

            IconosActualizados?.Invoke();
            Console.WriteLine($"[TRACE] {DateTime.Now:HH:mm:ss.fff} — CARGA DE ICONOS - MenuService: fusión + IconosActualizados (+{(DateTime.Now - t0).TotalMilliseconds:0}ms total)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] CARGA DE ICONOS - MenuService.CargarLogosEnSegundoPlanoAsync falló (+{(DateTime.Now - t0).TotalMilliseconds:0}ms): {ex.Message}");
        }
    }

    /// <summary>
    /// Reconstruye la lista aplanada List&lt;BotonTreeviewItem&gt; (un item por botón, con
    /// colores/íconos de su app/opción ya resueltos) a partir de los 3 resultados
    /// deduplicados — así el resto de la clase (GetOpciones, GetArbolMenu, etc.) sigue
    /// funcionando exactamente igual que antes, sin cambios.
    /// </summary>
    private static List<BotonTreeviewItem> Fusionar(TreeviewResult r)
    {
        var apps = r.Aplicaciones.ToDictionary(a => a.COD_APLICACION);
        var opciones = r.Opciones.ToDictionary(o => (o.COD_APLICACION, o.COD_OPCION_APLICACION));

        var result = new List<BotonTreeviewItem>(r.Botones.Count);
        foreach (var b in r.Botones)
        {
            apps.TryGetValue(b.COD_APLICACION, out var app);
            opciones.TryGetValue((b.COD_APLICACION, b.COD_OPCION_APLICACION), out var op);

            result.Add(new BotonTreeviewItem
            {
                COD_APLICACION = b.COD_APLICACION,
                DES_APLICACION = app?.DES_APLICACION,
                COD_TIPO_OPCION_APLICACION = op?.COD_TIPO_OPCION_APLICACION,
                DES_TIPO_OPCION_APLICACION = op?.DES_TIPO_OPCION_APLICACION,
                COD_OPCION_APLICACION_PADRE = b.COD_OPCION_APLICACION_PADRE ?? op?.COD_OPCION_APLICACION_PADRE,
                COD_OPCION_APLICACION = b.COD_OPCION_APLICACION,
                DES_OPCION_APLICACION = op?.DES_OPCION_APLICACION,
                DES_FORMULARIO = op?.DES_FORMULARIO,
                COD_BOTON_OPCION = b.COD_BOTON_OPCION,
                DES_BOTON_OPCION = b.DES_BOTON_OPCION,
                DES_TOOLTIPTEXT = b.DES_TOOLTIPTEXT,
                DES_ALIAS = b.DES_ALIAS,
                FLG_HABILITADO = b.FLG_HABILITADO,
                FLG_HEADER = b.FLG_HEADER,
                FLG_GRILLA = b.FLG_GRILLA,
                FLG_ACCION_PUT = b.FLG_ACCION_PUT,
                FLG_ACCION_POST = b.FLG_ACCION_POST,
                FLG_ACCION_DELETE = b.FLG_ACCION_DELETE,
                FLG_ACCION_GET = b.FLG_ACCION_GET,
                DES_METODO = b.DES_METODO,
                DES_BACKCOLOR_APLICACION = app?.DES_BACKCOLOR_APLICACION ?? "",
                DES_FORECOLOR_APLICACION = app?.DES_FORECOLOR_APLICACION ?? "",
                IMG_ICONO_APLICACION = app?.IMG_ICONO_APLICACION,
                DES_BACKCOLOR_OPCION = op?.DES_BACKCOLOR_OPCION ?? "",
                DES_FORECOLOR_OPCION = op?.DES_FORECOLOR_OPCION ?? "",
                IMG_ICONO_OPCION = op?.IMG_ICONO_OPCION,
                FLG_EXIGE_PASSWORD = op?.FLG_EXIGE_PASSWORD
            });
        }
        return result;
    }

    /// <summary>
    /// Limpia la caché de botones del rol activo (llamar al cerrar sesión o cambiar rol).
    /// </summary>
    public async Task LimpiarCacheAsync()
    {
        if (string.IsNullOrWhiteSpace(_rolActivo)) return;
        try
        {
            await _localStorage.RemoveItemAsync($"botonesTreeview_{_rolActivo}");
            await _localStorage.RemoveItemAsync($"botonesTreeview_{_rolActivo}_ts");
        }
        catch { }
    }

    /// <summary>
    /// Opciones de menú autorizadas para el rol activo — derivadas de los botones
    /// ya cargados en memoria (cada opción que tenga al menos un botón habilitado
    /// con FLG_HABILITADO = true aparece en el menú).
    /// </summary>
    public List<OpcionMenu> GetOpciones()
    {
        // El acceso a la OPCIÓN en sí (si aparece en el menú) viene de
        // ROLES_USUARIOS_APLICACIONES_OPCIONES — toda fila del treeview para el
        // rol activo ya pasó ese filtro en el SP (INNER JOIN con esa tabla vía a).
        // FLG_HABILITADO es del BOTÓN, no de la opción: una opción sin ningún
        // botón configurado aún debe aparecer en el menú (con LEFT JOIN, esa fila
        // llega con FLG_HABILITADO=NULL) — por eso aquí NO se filtra por ese flag.
        return _botones
            .GroupBy(b => new { b.COD_APLICACION, b.COD_OPCION_APLICACION })
            .Select(g => new OpcionMenu
            {
                COD_ROL_USUARIO = _rolActivo ?? "",
                COD_APLICACION = g.Key.COD_APLICACION,
                DES_APLICACION = g.First().DES_APLICACION,
                COD_OPCION_APLICACION = g.Key.COD_OPCION_APLICACION,
                FLG_ACCESO_PERMITIDO = true,
                DES_FORMULARIO = g.First().DES_FORMULARIO,
                DES_OPCION_APLICACION = g.First().DES_OPCION_APLICACION,
                FLG_EXIGE_PASSWORD = g.First().FLG_EXIGE_PASSWORD
            })
            .ToList();
    }

    /// <summary>
    /// Construye el árbol jerárquico completo del menú lateral, agrupando por
    /// COD_OPCION_APLICACION_PADRE. Filtra SMD (no se muestra) y deduplica por
    /// opción (cada opción puede tener varias filas, una por botón).
    /// </summary>
    public List<NodoMenu> GetArbolMenu()
    {
        // Una fila por opción (no por botón) — el resto de columnas son iguales
        // dentro del mismo grupo, así que basta con tomar la primera.
        // SMD SÍ se incluye aquí (para no romper la jerarquía de sus hijos) —
        // es NodoMenuItem quien decide no renderizarlo visualmente y "subir"
        // sus hijos un nivel.
        // Clave compuesta para evitar colisiones entre aplicaciones que reusan
        // los mismos COD_OPCION_APLICACION (ej. SEGURIDAD y ESTANDAR ambos tienen 00, 01, 0100…).
        var opciones = _botones
            .GroupBy(b => new { b.COD_APLICACION, b.COD_OPCION_APLICACION })
            .Select(g => g.First())
            .ToList();

        var porCodigo = opciones.ToDictionary(
            o => $"{o.COD_APLICACION}|{o.COD_OPCION_APLICACION}");

        NodoMenu Construir(BotonTreeviewItem o) => new()
        {
            COD_OPCION_APLICACION      = o.COD_OPCION_APLICACION,
            COD_APLICACION             = o.COD_APLICACION,
            DES_APLICACION             = o.DES_APLICACION,
            DES_OPCION_APLICACION      = o.DES_OPCION_APLICACION,
            DES_FORMULARIO             = o.DES_FORMULARIO,
            COD_TIPO_OPCION_APLICACION = o.COD_TIPO_OPCION_APLICACION,
            DES_BACKCOLOR_APLICACION   = o.DES_BACKCOLOR_APLICACION,
            DES_FORECOLOR_APLICACION   = o.DES_FORECOLOR_APLICACION,
            IMG_ICONO_APLICACION       = o.IMG_ICONO_APLICACION,
            DES_BACKCOLOR_OPCION       = o.DES_BACKCOLOR_OPCION,
            DES_FORECOLOR_OPCION       = o.DES_FORECOLOR_OPCION,
            IMG_ICONO_OPCION           = o.IMG_ICONO_OPCION,
            FLG_EXIGE_PASSWORD         = o.FLG_EXIGE_PASSWORD,
            Hijos = opciones
                .Where(h => h.COD_APLICACION == o.COD_APLICACION
                         && h.COD_OPCION_APLICACION_PADRE == o.COD_OPCION_APLICACION)
                .Select(Construir)
                .ToList()
        };

        // Raíces: sin padre, o con un padre que no existe en el conjunto visible del rol.
        return opciones
            .Where(o => string.IsNullOrWhiteSpace(o.COD_OPCION_APLICACION_PADRE)
                     || !porCodigo.ContainsKey($"{o.COD_APLICACION}|{o.COD_OPCION_APLICACION_PADRE}"))
            .Select(Construir)
            .ToList();
    }

    /// <summary>
    /// Botones autorizados de UNA opción específica, filtrados en memoria de los
    /// datos ya cargados — no hace ninguna llamada nueva a la API.
    /// </summary>
    public List<BotonTreeviewItem> GetBotonesPorOpcion(string codAplicacion, string codOpcionAplicacion) =>
        _botones
            .Where(b => b.COD_APLICACION == codAplicacion
                     && b.COD_OPCION_APLICACION == codOpcionAplicacion
                     && b.FLG_HABILITADO == true
                     && !string.IsNullOrEmpty(b.COD_BOTON_OPCION))   // descarta filas sin botón real (LEFT JOIN sin match)
            .ToList();

    /// <summary>
    /// Resuelve los códigos VIGENTES (COD_APLICACION, COD_OPCION_APLICACION) de una opción a
    /// partir de su ruta/formulario (ej. "Estandar/ClasesOperacion.razor") — clave estable que
    /// no cambia si el usuario mueve la opción a otra carpeta con el botón "Mover" (eso sí
    /// cambia COD_OPCION_APLICACION, vía renumeración jerárquica en el SP de mover).
    /// A diferencia de los parámetros CodAplicacion/CodOpcionAplicacion que quedan fijados una
    /// sola vez al abrir la pestaña (según qué tile de menú se clickeó), esto siempre resuelve
    /// contra los datos de menú ya cargados en memoria — si el usuario cierra sesión y vuelve
    /// a entrar tras mover la opción, refleja la ubicación actual sin tocar código de página.
    /// </summary>
    public (string CodAplicacion, string CodOpcionAplicacion)? BuscarCodigosPorFormulario(string desFormulario)
    {
        var buscado = WorkspaceRegistry.Normalizar(desFormulario);
        var match = _opciones.FirstOrDefault(o => WorkspaceRegistry.Normalizar(o.DES_FORMULARIO ?? "") == buscado);
        return match is null ? null : (match.COD_APLICACION, match.COD_OPCION_APLICACION);
    }

    /// <summary>True si el rol activo tiene el verbo HTTP indicado habilitado para algún
    /// botón de esa opción — usado por las pantallas para mostrar/ocultar botones de acción.</summary>
    public bool PuedeAsync(string codAplicacion, string codOpcionAplicacion, string verbo)
    {
        var botones = GetBotonesPorOpcion(codAplicacion, codOpcionAplicacion);
        return verbo.ToUpperInvariant() switch
        {
            "GET" => botones.Any(b => b.FLG_ACCION_GET == true),
            "POST" => botones.Any(b => b.FLG_ACCION_POST == true),
            "PUT" => botones.Any(b => b.FLG_ACCION_PUT == true),
            "DELETE" => botones.Any(b => b.FLG_ACCION_DELETE == true),
            _ => false
        };
    }

    /// <summary>Limpia todo el estado en memoria (al cerrar sesión).</summary>
    public void Reset()
    {
        _botones = new();
        _roles = new();
        _rolActivo = null;
    }
}
