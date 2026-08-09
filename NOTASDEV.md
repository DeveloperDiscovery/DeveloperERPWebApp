# Notas de desarrollo — KONSolutions WebApp (Blazor WASM)

## Cuando un cambio "no se aplica" después de recompilar

Blazor WebAssembly guarda los ensamblados (`.dll`) y el runtime en el **Cache Storage**
del navegador (una capa distinta a la caché HTTP normal). Ni el hard-refresh
(Ctrl+Shift+R) ni el checkbox "Disable cache" de DevTools la tocan. Por eso a veces
un cambio real en el código no se refleja aunque el build haya sido correcto.

### Checklist de diagnóstico, en orden

1. **Confirma que el archivo realmente cambió en disco** en la ruta que estás editando
   (a veces se guarda en una carpeta equivocada, o hay dos copias del proyecto).

2. **Confirma qué le llegó al navegador**, antes de sospechar del código:
   - DevTools → **Network** → clic en la primera petición (el documento HTML, normalmente
     aparece con el nombre de la ruta, ej. `login`) → pestaña **Response** → busca el
     texto que cambiaste.
   - Si no aparece ahí, el problema es de build/copiado — no sigas revisando el código C#.

3. **Si el HTML sí trae el cambio pero el navegador se sigue comportando raro** (sobre
   todo con cambios en `Program.cs`, root components, JS interop de arranque):
   - DevTools → **Application** → **Storage** → botón **"Clear site data"**.
   - Esto borra Cache Storage, IndexedDB y todo lo demás — fuerza que el navegador
     re-descargue y ejecute el `.dll` recién compilado.

4. **Solo si los pasos 2 y 3 no resuelven nada**, sospecha del build:
   - Cierra Visual Studio / detén la depuración.
   - Borra `bin` y `obj` del proyecto `KONSolutions.Web`.
   - Rebuild Solution (no solo Build) y F5.

### Regla rápida

> Hard-refresh limpia la caché HTTP. "Clear site data" limpia TODO (incluyendo el
> Cache Storage de Blazor). Si algo no cambia después de un hard-refresh, el siguiente
> paso es "Clear site data" — no asumir que el código está mal.

## CSS/JS estáticos (`app.css`, `appZoom.js`, etc.) con `?v=` en el link/script

Varios archivos en `index.html` se referencian con un query string de versión, ej.:
```html
<link href="css/app.css?v=20260705" rel="stylesheet" />
```
Esto es cache-busting manual: el navegador cachea por URL completa (incluyendo el
query string). **Si editas `app.css` (o cualquier archivo referenciado así) y NO
subes el número de versión, el navegador puede seguir sirviendo la copia vieja
indefinidamente**, sin importar rebuilds ni "Clear site data" del sitio (a veces esa
caché de recursos estáticos individual sobrevive incluso a eso).

Regla: **cada vez que edites uno de estos archivos, sube el `?v=` en su
`<link>`/`<script>` correspondiente en `index.html`** (usa la fecha del día, ej.
`?v=20260706`). Si un cambio de CSS "no aparece" pero sí ves el nuevo `--css-var` con
el valor correcto (confirmado por `getComputedStyle`), sospecha primero de esto antes
de seguir revisando el layout — significa que la variable se está pasando bien pero
la regla CSS que la consume todavía es la versión vieja.

## Zoom propio de la app (`appZoom.js`)

- Usa `transform: scale()` sobre `#app` (no la propiedad CSS `zoom`, que desalinea
  `window.innerHeight`).
- Cualquier elemento con `position:fixed` **dentro** de `#app` queda atrapado por ese
  transform (se vuelve su "containing block" según la spec CSS) y se desalinea al
  hacer zoom. Por eso `MudPopoverProvider` (combos, menús contextuales, tooltips) vive
  en su propio root component fuera de `#app` — ver `#mud-popover-host` en `index.html`
  y `Program.cs`.
- Si en el futuro se agrega otro overlay `position:fixed` de MudBlazor o de una librería
  externa y aparece desalineado con el zoom, la causa casi siempre es la misma: está
  viviendo dentro de `#app`.

## Regla transversal: registros ANULADOS son de solo lectura

Un registro cuyo estado tenga `COD_ESTADO_ALIAS = 'ANU'` (ESTANDAR.ESTADOS) queda
**congelado**: solo se puede consultar. No admite edición, eliminación, ni ninguna acción
que lo modifique (asignar responsable, reasignar asesor, agregar memorias, crear versiones…).

**Aplica a TODOS los módulos**, no solo a los que ya lo implementan.

### Cómo implementarlo en un módulo nuevo

1. El SP de listado debe devolver `COD_ESTADO_ALIAS` (JOIN contra `ESTANDAR.ESTADOS`).
2. El modelo (Shared y Domain) declara `public string? COD_ESTADO_ALIAS { get; set; }`.
3. En la grilla, cortar en `OnAccionGrilla` **antes** de despachar la acción:

```csharp
private bool BloquearPorAnulado(MiModelo item, BotonTreeviewItem b)
{
    if (!EstadoAlias.EsAnulado(item.COD_ESTADO_ALIAS)) return false;
    if (b.FLG_ACCION_GET == true) return false;   // consultar siempre se permite
    Snackbar.Add("El registro está anulado: solo se permite consultarlo.", Severity.Warning);
    return true;
}
```

El helper es `KONSolutions.Shared.Common.EstadoAlias` (ya incluido en `_Imports.razor`).
`EsAnulado` tolera null: si el SP todavía no devuelve el alias, el registro **no** se
bloquea — degradación segura, nunca deja al usuario trabado por un dato faltante.

Módulos donde ya está aplicado: Comercial/SolicitudesComerciales(.Dependencia/.Cliente),
Ingenieria/Disenos y Ingenieria/DisenosVersionPanel.
