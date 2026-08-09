using KONSolutions.Web.Pages.Auth;
using KONSolutions.Web.Pages.Shared;
using MudBlazor;

namespace KONSolutions.Web.Services;

/// <summary>
/// Confirmación de acceso para opciones con FLG_EXIGE_PASSWORD = true. Si el usuario
/// tiene reconocimiento facial configurado (FLG_LOGIN_FACE + FLG_REGISTRO_FACIAL),
/// usa ese método en vez de pedir contraseña — misma decisión que ya se toma en el login.
/// </summary>
public static class AccesoOpcionHelper
{
    public static async Task<bool> ConfirmarAccesoAsync(IDialogService dialogService, IAuthService authService, ConfigService configSvc)
    {
        var sesion = await authService.GetSesionAsync();
        var codUsuario = sesion?.COD_USUARIO ?? "";

        if (sesion?.FLG_LOGIN_FACE == true && sesion.FLG_REGISTRO_FACIAL == true)
        {
            var dlgFacial = await dialogService.ShowAsync<VerificacionFacialDialog>(
                "Verificación facial",
                new DialogParameters
                {
                    ["CodUsuario"]      = codUsuario,
                    ["NombreUsuario"]   = sesion.DES_USUARIO ?? codUsuario,
                    ["TimeoutSegundos"] = configSvc.Current.FaceTimeoutSegundos
                },
                new DialogOptions
                {
                    MaxWidth         = MaxWidth.Small,
                    FullWidth        = true,
                    CloseOnEscapeKey = false,
                    BackdropClick    = false
                });
            var resultadoFacial = await dlgFacial.Result;
            return resultadoFacial is not null && !resultadoFacial.Canceled;
        }

        var dlg = await dialogService.ShowAsync<ConfirmarPasswordDialog>(
            "Confirmar contraseña",
            new DialogParameters { ["CodUsuario"] = codUsuario },
            new DialogOptions { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, CloseButton = true });
        var resultado = await dlg.Result;
        return resultado is not null && !resultado.Canceled;
    }
}
