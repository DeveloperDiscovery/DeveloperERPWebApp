using KONSolutions.Shared.Common;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>Una fila de existencias: un producto, en un lote, en una ubicación de un almacén.
///
/// Es el resultado de PROC_LOGISTICA_STOCKS_ALMACEN_UBICACION_SEARCHDETAIL, que ya resuelve por
/// JOIN todas las descripciones. Se separa de la entidad StocksAlmacenUbicacion porque aquélla
/// representa la tabla y ésta el resultado de una consulta: mezclarlas obligaría a cargar de
/// nulos la entidad cada vez que se usa para escribir.</summary>
public class StocksAlmacenUbicacionDetalle
{
    // ── Dónde está ──────────────────────────────────────────────────────────
    public string?  COD_ALMACEN                  { get; set; }
    public string?  DES_ALMACEN                  { get; set; }
    public string?  COD_UBICACION_ALMACEN        { get; set; }
    public string?  DES_UBICACION_ALMACEN        { get; set; }
    /// <summary>El polígono del plano. Es lo que permite marcar la ubicación en el mapa.</summary>
    public string?  DES_POLIGONO                 { get; set; }

    // ── Qué es ──────────────────────────────────────────────────────────────
    public string?  COD_GRUPO_PRODUCTO           { get; set; }
    public string?  DES_GRUPO_PRODUCTO           { get; set; }
    public string?  COD_FAMILIA_PRODUCTO         { get; set; }
    public string?  DES_FAMILIA_PRODUCTO         { get; set; }
    public string?  COD_SUB_FAMILIA_PRODUCTO     { get; set; }
    public string?  DES_SUB_FAMILIA_PRODUCTO     { get; set; }
    public string?  COD_TIPO_PRODUCTO            { get; set; }
    public string?  DES_TIPO_PRODUCTO            { get; set; }
    public string?  COD_PRODUCTO                 { get; set; }
    public string?  DES_PRODUCTO                 { get; set; }

    // ── De qué lote viene ───────────────────────────────────────────────────
    public string?  NUM_LOTE_CONTROL             { get; set; }
    public string?  DES_LOTE_CONTROL             { get; set; }
    public string?  NUM_LOTE_CONTROL_ENTIDAD     { get; set; }
    public decimal? COD_ENTIDAD                  { get; set; }
    public string?  DES_NOMBRE_COMPLETO          { get; set; }

    // ── Cuánto hay ──────────────────────────────────────────────────────────
    // Cada cantidad viene con su par en la unidad secundaria, que puede no existir.
    public decimal? CAN_PRODUCTO                 { get; set; }
    public string?  COD_UNIDAD_MEDIDA            { get; set; }
    public string?  DES_UNIDAD_MEDIDA            { get; set; }
    public decimal? CAN_PRODUCTO_SECUNDARIO      { get; set; }
    public string?  COD_UNIDAD_MEDIDA_SECUNDARIO { get; set; }
    public string?  DES_UNIDAD_MEDIDA_SECUNDARIO { get; set; }

    public decimal? CAN_RESERVADA                { get; set; }
    public decimal? CAN_RESERVADA_SECUNDARIO     { get; set; }
    public decimal? CAN_TRANSITO                 { get; set; }
    public decimal? CAN_TRANSITO_SECUNDARIO      { get; set; }
    public decimal? CAN_INMOVILIZADO             { get; set; }
    public decimal? CAN_INMOVILIZADO_SECUNDARIO  { get; set; }
    public decimal? CAN_DISPONIBLE               { get; set; }
    public decimal? CAN_DISPONIBLE_SECUNDARIO    { get; set; }
}

/// <summary>Los filtros de la consulta. Todos opcionales: el procedimiento ignora los que
/// llegan vacíos o en cero, así que la pantalla puede mandarlos todos siempre.</summary>
public class StocksAlmacenUbicacionSearchDetailDto
{
    public string?  COD_ALMACEN                 { get; set; }
    public string?  COD_UBICACION_ALMACEN       { get; set; }
    public string?  COD_GRUPO_PRODUCTO          { get; set; }
    public string?  COD_FAMILIA_PRODUCTO        { get; set; }
    public string?  COD_SUB_FAMILIA_PRODUCTO    { get; set; }
    public string?  COD_PRODUCTO                { get; set; }
    public string?  COD_TIPO_PRODUCTO           { get; set; }
    public string?  DES_PRODUCTO                { get; set; }
    public decimal? COD_ENTIDAD_PROVEEDOR       { get; set; }
    public string?  NUM_LOTE_CONTROL            { get; set; }
    public string?  NUM_LOTE_CONTROL_ENTIDAD    { get; set; }

    // Rangos: cero significa "sin límite por ese lado", no "igual a cero".
    public decimal? CAN_PRODUCTO_INICIAL        { get; set; }
    public decimal? CAN_PRODUCTO_FINAL          { get; set; }
    public decimal? CAN_RESERVADA_INICIAL       { get; set; }
    public decimal? CAN_RESERVADA_FINAL         { get; set; }
    public decimal? CAN_TRANSITO_INICIAL        { get; set; }
    public decimal? CAN_TRANSITO_FINAL          { get; set; }
    public decimal? CAN_IMOVILIZADO_INICIAL     { get; set; }
    public decimal? CAN_IMOVILIZADO_FINAL       { get; set; }
    public decimal? CAN_DISPONIBLE_INICIAL      { get; set; }
    public decimal? CAN_DISPONIBLE_FINAL        { get; set; }
    public string?  DES_ESPECIFICACIONES_TECNICAS { get; set; }
}

/// <summary>Los combos que no dependen de ningún otro filtro, resueltos en una sola llamada.</summary>
public class StocksAlmacenUbicacionCombos
{
    public List<ComboboxItem> Almacenes     { get; set; } = new();
    public List<ComboboxItem> Grupos        { get; set; } = new();
    public List<ComboboxItem> TiposProducto { get; set; } = new();
    public List<ComboboxItem> Proveedores   { get; set; } = new();
    public List<ComboboxItem> Productos     { get; set; } = new();
}
