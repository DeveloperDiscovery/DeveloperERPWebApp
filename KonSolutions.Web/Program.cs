using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using MudBlazor.Services;
using KONSolutions.Web;
using KONSolutions.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
// Fuera de #app para que el zoom (transform:scale en #app) no desalinee los popovers
// de MudBlazor (combos, menús contextuales, tooltips) — ver comentario en index.html.
builder.RootComponents.Add<MudBlazor.MudPopoverProvider>("#mud-popover-host");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7001/";

builder.Services.AddBlazoredLocalStorage();
// El "no cerrar al hacer clic afuera / Escape" se aplica por cada DialogOptions
// individual (BackdropClick = false, CloseOnEscapeKey = false) en cada llamada a
// DialogService.ShowAsync — MudBlazor 7.15 no expone un default global para esto.
builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<JwtAuthStateProvider>());

// Servicios transversales
builder.Services.AddScoped<ErrorLogService>();
builder.Services.AddScoped<ColorClipboardService>();
builder.Services.AddScoped<SeguridadContextService>();
builder.Services.AddScoped<AccessLogService>();
builder.Services.AddScoped<AplicacionesOpcionesBotonesService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<MenuEstadoService>();
builder.Services.AddScoped<ShortcutService>();
builder.Services.AddScoped<TransicionMenuService>();
builder.Services.AddScoped<FavoritosCacheService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<FotoUsuarioService>();
builder.Services.AddScoped<PasswordPolicyService>();
builder.Services.AddScoped<AsignacionService>();
builder.Services.AddScoped<ConfigService>();
builder.Services.AddScoped<EtiquetasService>();
builder.Services.AddScoped<ConfiguracionGeneralService>();
builder.Services.AddScoped<ConfiguracionUsuarioService>();

// Botones Estándar
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Seguridad.BotonEstandar,
    KONSolutions.Shared.Models.Seguridad.BotonEstandarDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Seguridad.BotonesEstandar, "BOTONES_ESTANDAR"));

// Tipos de Motivos (maestro)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Estandar.TipoMotivo,
    KONSolutions.Shared.Models.Estandar.TipoMotivoDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Estandar.TiposMotivos, "TIPOS_MOTIVOS"));

// Diseños (catálogo INGENIERIA.DISENOS)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Ingenieria.Disenos,
    KONSolutions.Shared.Models.Ingenieria.DisenosDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Ingenieria.Disenos, "DISENOS"));

builder.Services.AddScoped<MotivoService>();
builder.Services.AddScoped<AsignacionRolService>();
builder.Services.AddScoped<BiometriaService>();
builder.Services.AddScoped<FavoritoService>();
builder.Services.AddScoped<WorkspaceTabService>();
builder.Services.AddScoped<ControlCambiosClientService>();
builder.Services.AddScoped<NotificacionesFlujoService>();
builder.Services.AddScoped<NotificacionesAprobacionesService>();
builder.Services.AddScoped<LimitesService>();
builder.Services.AddScoped<DashboardBdService>();

// Servicios CRUD de catálogos de seguridad (genéricos)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Seguridad.Perfil,
    KONSolutions.Shared.Models.Seguridad.PerfilDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Seguridad.Perfiles, "PERFILES"));

builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Seguridad.Rol,
    KONSolutions.Shared.Models.Seguridad.RolDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Seguridad.Roles, "ROLES"));

builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Seguridad.ComplejidadPwd,
    KONSolutions.Shared.Models.Seguridad.ComplejidadPwdDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Seguridad.ComplejidadPassword, "COMPLEJIDAD"));

builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Seguridad.Aplicacion,
    KONSolutions.Shared.Models.Seguridad.AplicacionDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Seguridad.Aplicaciones, "APLICACIONES"));

builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Seguridad.TipoOpcion,
    KONSolutions.Shared.Models.Seguridad.TipoOpcionDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Seguridad.TiposOpciones, "TIPOS_OPCIONES"));

// Catálogos Estandar: Tipos de Estados (maestro) y Estados (detalle)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Estandar.TipoEstado,
    KONSolutions.Shared.Models.Estandar.TipoEstadoDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Estandar.TiposEstados, "TIPOS_ESTADOS"));

builder.Services.AddScoped<EstadoService>();

// Tablas x Tipo de Estado (asocia cada tabla del sistema a un COD_TIPO_ESTADO)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Estandar.TablaTipoEstado,
    KONSolutions.Shared.Models.Estandar.TablaTipoEstadoDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Estandar.TablasTiposEstado, "TABLAS_TIPOS_ESTADO"));

// Tipos de Cargo Administrativo (maestro)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Administracion.TipoCargoAdministrativo,
    KONSolutions.Shared.Models.Administracion.TipoCargoAdministrativoDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Administracion.TiposCargoAdministrativo, "TIPOS_CARGO_ADMINISTRATIVO"));

// Cargos (maestro)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Administracion.Cargo,
    KONSolutions.Shared.Models.Administracion.CargoDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Administracion.Cargos, "CARGOS"));

// Años (maestro simple, NUM_ANO identity)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Estandar.Ano,
    KONSolutions.Shared.Models.Estandar.AnoDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Estandar.Anos, "ANOS"));

// Meses (maestro simple, NUM_MES identity)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Estandar.Mes,
    KONSolutions.Shared.Models.Estandar.MesDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Estandar.Meses, "MESES"));

// Áreas (maestro jerárquico)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Administracion.Area,
    KONSolutions.Shared.Models.Administracion.AreaDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Administracion.Areas, "AREAS"));

// Tipos de Centro de Costo (maestro)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Contabilidad.TipoCentroCosto,
    KONSolutions.Shared.Models.Contabilidad.TipoCentroCostoDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Contabilidad.TiposCentroCosto, "TIPOS_CENTRO_COSTO"));

// Centros de Costo (maestro jerárquico)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Contabilidad.CentroCosto,
    KONSolutions.Shared.Models.Contabilidad.CentroCostoDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Contabilidad.CentrosCosto, "CENTROS_COSTO"));

builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Estandar.EstadoEstandar,
    KONSolutions.Shared.Models.Estandar.EstadoEstandarDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Estandar.EstadosEstandar, "ESTADOS_ESTANDAR"));

// Tipos de Notificación (maestro)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Entorno.TipoNotificacion,
    KONSolutions.Shared.Models.Entorno.TipoNotificacionDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Entorno.TiposNotificacion, "TIPOS_NOTIFICACION"));

// Banco de Colores (maestro)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Entorno.BancoColor,
    KONSolutions.Shared.Models.Entorno.BancoColorDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Entorno.BancoColores, "BANCO_COLORES"));

// Tipos de Menú de Opciones (maestro)
builder.Services.AddScoped(sp => new CatalogoService<
    KONSolutions.Shared.Models.Estandar.TipoMenuOpciones,
    KONSolutions.Shared.Models.Estandar.TipoMenuOpcionesDto>(
    sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AccessLogService>(),
    sp.GetRequiredService<ErrorLogService>(), ApiRoutes.Estandar.TiposMenuOpciones, "TIPOS_MENU_OPCIONES"));

// Handler que inyecta el Bearer token
builder.Services.AddScoped<JwtMessageHandler>();
builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<JwtMessageHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
});

var host = builder.Build();

// Aplicar culture guardado en localStorage ANTES de que Blazor renderice nada.
// Esto garantiza que CultureInfo.CurrentCulture sea correcto en todos los componentes.
try
{
    var ls = host.Services.GetRequiredService<Blazored.LocalStorage.ILocalStorageService>();
    var storedCulture = await ls.GetItemAsStringAsync("app-culture");
    if (!string.IsNullOrWhiteSpace(storedCulture))
    {
        // DefaultThreadCurrentCulture solo aplica a hilos NUEVOS — se fija también
        // explícitamente en el hilo actual para que tome efecto de inmediato, sin
        // depender de que ningún otro código ya haya fijado CurrentCulture antes.
        // Los separadores numéricos quedan forzados a estilo US (ver CultureHelper).
        KONSolutions.Web.Services.CultureHelper.Aplicar(storedCulture);
    }
}
catch { }

await host.RunAsync();
