using System.ComponentModel.DataAnnotations;
using KONSolutions.Shared.Common;

namespace KONSolutions.Shared.Models.Estandar;

// ───────────── ESTANDAR.TIPOS_ESTADOS ─────────────
public class TipoEstado
{
    public int?    COD_TIPO_ESTADO { get; set; }
    public string? DES_TIPO_ESTADO { get; set; }
    public byte[]? IMG_PICTURE     { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO             { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

public class TipoEstadoDto
{
    public int?    COD_TIPO_ESTADO { get; set; } // identity, nullable (upsert)
    [Required] public string DES_TIPO_ESTADO { get; set; } = string.Empty;
    public byte[]? IMG_PICTURE { get; set; }
}

// ───────────── ESTANDAR.ESTADOS (hijo de TipoEstado) ─────────────
public class Estado
{
    public int?    COD_TIPO_ESTADO            { get; set; }
    public int?    COD_ESTADO                 { get; set; }
    public string? DES_ESTADO                 { get; set; }
    public string? COD_ESTADO_ALIAS           { get; set; }
    public bool    FLG_DEFAULT                { get; set; }
    public bool    FLG_DISPARADOR_MAIL        { get; set; }
    public bool?   FLG_DISPARADOR_NOTIFICACION{ get; set; }
    public bool    FLG_DISPARADOR_AUTORIZACION{ get; set; }
    public bool?   FLG_DISPARADOR_AUTORIZACION_COMERCIAL{ get; set; }
    public int?    NUM_ORDEN_PRESENTACION     { get; set; }
    public int?    COD_TIPO_MOTIVO            { get; set; }
    public string? DES_BACKCOLOR              { get; set; }
    public string? DES_FORECOLOR              { get; set; }
    public byte[]? IMG_PICTURE                { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO             { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

public class EstadoDto
{
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO      { get; set; } // identity dentro del tipo
    public bool EsNuevo         { get; set; } // true = crear (recalcula COD_ESTADO); false = editar
    [Required] public string DES_ESTADO { get; set; } = string.Empty;
    public string? COD_ESTADO_ALIAS { get; set; }
    public bool FLG_DEFAULT                 { get; set; }
    public bool FLG_DISPARADOR_MAIL         { get; set; }
    public bool FLG_DISPARADOR_NOTIFICACION { get; set; }
    public bool FLG_DISPARADOR_AUTORIZACION { get; set; }
    public bool FLG_DISPARADOR_AUTORIZACION_COMERCIAL { get; set; }
    public int  NUM_ORDEN_PRESENTACION      { get; set; }
    public int? COD_TIPO_MOTIVO             { get; set; }
    public string DES_BACKCOLOR { get; set; } = "#FFFFFF";
    public string DES_FORECOLOR { get; set; } = "#000000";
    public byte[]? IMG_PICTURE  { get; set; }
}

// ───────────── ESTANDAR.ESTADOS_ESTANDAR ─────────────
public class EstadoEstandar
{
    public int?    COD_INDICE    { get; set; }
    public int?    COD_ESTADO    { get; set; }
    public string? DES_ESTADO    { get; set; }
    public string? COD_ESTADO_ALIAS { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_FORECOLOR { get; set; }
    public byte[]? IMG_PICTURE   { get; set; }
}

public class EstadoEstandarDto
{
    public int? COD_INDICE { get; set; }
    public int? COD_ESTADO { get; set; }
    [Required] public string DES_ESTADO { get; set; } = string.Empty;
    public string? COD_ESTADO_ALIAS { get; set; }
    public string DES_BACKCOLOR { get; set; } = "#FFFFFF";
    public string DES_FORECOLOR { get; set; } = "#000000";
    public byte[]? IMG_PICTURE  { get; set; }
}

// ───────────── ESTANDAR.TIPOS_MOTIVOS (maestro) ─────────────
public class TipoMotivo
{
    public int?    COD_TIPO_MOTIVO { get; set; }
    public string? DES_TIPO_MOTIVO { get; set; }
    public byte[]? IMG_PICTURE     { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO             { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

public class TipoMotivoDto
{
    public int?    COD_TIPO_MOTIVO { get; set; }
    [Required] public string DES_TIPO_MOTIVO { get; set; } = string.Empty;
    public byte[]? IMG_PICTURE { get; set; }
}

// ───────────── ESTANDAR.MOTIVOS (detalle, hijo de TipoMotivo) ─────────────
public class Motivo
{
    public int?    COD_TIPO_MOTIVO        { get; set; }
    public int?    COD_MOTIVO             { get; set; }
    public string? DES_MOTIVO             { get; set; }
    public bool    FLG_DEFAULT            { get; set; }
    public bool    FLG_EXPLICAR           { get; set; }
    public bool    FLG_AUTOMATICO         { get; set; }
    public int?    NUM_ORDEN_PRESENTACION { get; set; }
    public byte[]? IMG_PICTURE            { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO             { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

public class MotivoDto
{
    public int? COD_TIPO_MOTIVO { get; set; }
    public int? COD_MOTIVO      { get; set; }
    public bool EsNuevo         { get; set; } // true = crear (recalcula COD_MOTIVO); false = editar
    [Required] public string DES_MOTIVO { get; set; } = string.Empty;
    public bool FLG_DEFAULT    { get; set; }
    public bool FLG_EXPLICAR   { get; set; }
    public bool FLG_AUTOMATICO { get; set; }
    public int  NUM_ORDEN_PRESENTACION { get; set; }
    public byte[]? IMG_PICTURE { get; set; }
}

// ───────────── ESTANDAR.ENTIDADES ─────────────
public class Entidad
{
    public decimal? COD_ENTIDAD { get; set; }
    public int?    COD_TIPO_PERSONA { get; set; }
    public string DES_PATERNO  { get; set; } = "";
    public string DES_MATERNO  { get; set; } = "";
    public string DES_NOMBRE   { get; set; } = "";
    public string DES_NOMBRE2  { get; set; } = "";
    [Required] public string DES_NOMBRE_COMPLETO { get; set; } = "";
    public string DES_COMERCIAL       { get; set; } = "";
    /// <summary>Solo aplica cuando COD_TIPO_PERSONA es Persona Jurídica — el diálogo
    /// deshabilita este campo (y lo limpia) para Persona Natural.</summary>
    public string? DES_PREFIJO_USUARIO_COMERCIAL { get; set; }
    /// <summary>Solo aplica cuando COD_TIPO_PERSONA es Persona Natural — el diálogo
    /// deshabilita este campo (y lo limpia) para Persona Jurídica.
    /// NOTA: el SP no trae una descripción de la dependencia sin ambigüedad (ver aviso en
    /// el modelo del lado API) — el diálogo solo maneja el código.</summary>
    public decimal? COD_ENTIDAD_DEPENDENCIA { get; set; }
    public int?    COD_TIPO_DOCUMENTO_IDENTIDAD { get; set; }
    public string  NUM_DOCUMENTO_IDENTIDAD { get; set; } = "";
    public bool?   FLG_DOMICILIADO { get; set; }
    public string  COD_PAIS                { get; set; } = "";
    public string  NUM_DUNS                { get; set; } = "";
    public string  COD_USUARIO_RELACIONADO { get; set; } = "";
    public byte[]? IMG_LOGO                { get; set; }
    public int?    COD_TIPO_ESTADO         { get; set; }
    public int?    COD_ESTADO      { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO             { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION        { get; set; }
    // Desnormalizados del SP
    public string DES_ESTADO                  { get; set; } = "";
    public string DES_BACKCOLOR               { get; set; } = "";
    public string DES_FORECOLOR               { get; set; } = "";
    public string DES_PAIS                    { get; set; } = "";
    public string DES_TIPO_DOCUMENTO_IDENTIDAD{ get; set; } = "";
    /// <summary>Nombre completo de la entidad de dependencia (COD_ENTIDAD_DEPENDENCIA) —
    /// viene del SHOWALL/SHOWBYID/SEARCH ya corregido (alias con sufijo _DEPENDENCIA,
    /// sin colisionar con las columnas propias de la entidad).</summary>
    public string DES_ENTIDAD_DEPENDENCIA     { get; set; } = "";
}

// ───────────── ESTANDAR.MONEDAS ─────────────
public class Moneda
{
    public string  COD_MONEDA       { get; set; } = "";
    [Required] public string DES_MONEDA { get; set; } = "";
    public int?    COD_MONEDA_SUNAT { get; set; }
    public int?    COD_TIPO_ESTADO  { get; set; }
    public int?    COD_ESTADO       { get; set; }
    public byte[]? IMG_ICONO        { get; set; }
    public string  DES_ESTADO       { get; set; } = "";
    public string  DES_BACKCOLOR    { get; set; } = "";
    public string  DES_FORECOLOR    { get; set; } = "";
}

// ───────────── ESTANDAR.MONEDAS_TIPO_CAMBIO ─────────────
public class MonedaTipoCambio
{
    public string   COD_MONEDA          { get; set; } = "";
    public DateTime? FEC_TIPO_CAMBIO    { get; set; }
    public decimal? FAC_COMERCIAL_COMPRA{ get; set; }
    public decimal? FAC_COMERCIAL_VENTA { get; set; }
    public decimal? FAC_SUNAT_COMPRA    { get; set; }
    public decimal? FAC_SUNAT_VENTA     { get; set; }
    public decimal? FAC_PARALELO_COMPRA { get; set; }
    public decimal? FAC_PARALELO_VENTA  { get; set; }
    public string   DES_MONEDA          { get; set; } = "";
    public DateTime? FEC_REGISTRO       { get; set; }
    public string   COD_USUARIO_REGISTRO{ get; set; } = "";
}

// ───────────── ESTANDAR.TIPOS_ENTIDADES ─────────────
public class TiposEntidades
{
    public string COD_TIPO_ENTIDAD { get; set; } = "";
    [Required] public string DES_TIPO_ENTIDAD { get; set; } = "";
}

// ───────────── ESTANDAR.TIPOS_COMPROBANTES_PAGO ─────────────
public class TiposComprobantesPago
{
    [Required] public string COD_TIPO_COMPROBANTE_PAGO { get; set; } = "";
    [Required] public string DES_TIPO_COMPROBANTE_PAGO { get; set; } = "";
    [Required] public string COD_PREFIJO { get; set; } = "";
    public int?  COD_COMPROBANTE_PAGO_SUNAT       { get; set; }
    public bool? FLG_EMISION                      { get; set; }
    public bool? FLG_POSITIVO                     { get; set; }
    public bool? FLG_ANTICIPO                      { get; set; }
    public bool? FLG_PREFIJO_COMPROBANTE_ORIGEN   { get; set; }
    public bool? FLG_DOCUMENTO_APLICABLE          { get; set; }
    public bool? FLG_CONTROL_INTERNO              { get; set; }
}

// ───────────── ESTANDAR.TIPOS_VIA ─────────────
public class TiposVia
{
    public string COD_TIPO_VIA { get; set; } = "";
    [Required] public string DES_TIPO_VIA { get; set; } = "";
}

// ───────────── ESTANDAR.ETIQUETAS_BUSQUEDA_AREAS ─────────────
// Enlaza una Etiqueta de Búsqueda con el Área responsable y los niveles jerárquicos
// (Gerencia/Sub Gerencia/Jefatura/Subjefatura/Supervisor) a los que aplica. PK: COD_ETIQUETA_BUSQUEDA.
public class EtiquetasBusquedaAreas
{
    [Required] public string COD_ETIQUETA_BUSQUEDA { get; set; } = "";
    public byte? COD_AREA { get; set; }
    public bool? FLG_GERENCIA { get; set; }
    public bool? FLG_SUB_GERENCIA { get; set; }
    public bool? FLG_JEFATURA { get; set; }
    public bool? FLG_SUBJEFATURA { get; set; }
    public bool? FLG_SUPERVISOR { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
}

// ───────────── ESTANDAR.SEXOS ─────────────
public class Sexo
{
    public string COD_SEXO { get; set; } = "";
    [Required] public string DES_SEXO { get; set; } = "";
}

// ───────────── ESTANDAR.TIPOS_PERSONAS ─────────────
public class TipoPersona
{
    public byte COD_TIPO_PERSONA { get; set; }
    [Required] public string DES_TIPO_PERSONA { get; set; } = "";
}

// ───────────── ESTANDAR.UNIDADES_MEDIDA ─────────────
public class UnidadesMedida
{
    [Required] public string  COD_UNIDAD_MEDIDA       { get; set; } = "";
    [Required] public string  DES_UNIDAD_MEDIDA       { get; set; } = "";
    public string?            COD_UNIDAD_MEDIDA_SUNAT { get; set; }
    public string?            COD_SIMBOLO             { get; set; }
    public string?            COD_TIPO_UNIDAD_MEDIDA  { get; set; }
    public byte[]?            IMG_ICONO               { get; set; }
    public string?            DES_TIPO_UNIDAD_MEDIDA  { get; set; }
}

public class UnidadesMedidaInsertDto
{
    [Required] public string  COD_UNIDAD_MEDIDA       { get; set; } = "";
    [Required] public string  DES_UNIDAD_MEDIDA       { get; set; } = "";
    public string?            COD_UNIDAD_MEDIDA_SUNAT { get; set; }
    public string?            COD_SIMBOLO             { get; set; }
    public string?            COD_TIPO_UNIDAD_MEDIDA  { get; set; }
    public byte[]?            IMG_ICONO               { get; set; }
}

// ───────────── ESTANDAR.TIPOS_UNIDAD_MEDIDA ─────────────
public class TiposUnidadMedida
{
    [Required] public string COD_TIPO_UNIDAD_MEDIDA { get; set; } = "";
    [Required] public string DES_TIPO_UNIDAD_MEDIDA { get; set; } = "";
    public byte[]? IMG_ICONO { get; set; }
}

// ───────────── ESTANDAR.TIPOS_DOCUMENTO_IDENTIDAD ─────────────
public class TiposDocumentoIdentidad
{
    public int?    COD_TIPO_DOCUMENTO_IDENTIDAD { get; set; }
    [Required] public string DES_TIPO_DOCUMENTO_IDENTIDAD { get; set; } = "";
    public byte[]? IMG_ICONO { get; set; }
}

// ───────────── ESTANDAR.UBIGEOS ─────────────
public class Ubigeo
{
    public string COD_UBIGEO { get; set; } = "";
    [Required] public string DES_UBIGEO { get; set; } = "";
    public bool? FLG_DEPARTAMENTO { get; set; }
    public bool? FLG_PROVINCIA    { get; set; }
    public bool? FLG_DISTRITO     { get; set; }
}

// ───────────── ESTANDAR.PAISES ─────────────
public class Pais
{
    public string  COD_PAIS          { get; set; } = "";
    public string  DES_PAIS          { get; set; } = "";
    public string? COD_PAIS_3        { get; set; }
    public string? DES_PAIS_INGLES   { get; set; }
    public string? DES_PAIS_NATIVO   { get; set; }
    public string? DES_REGION        { get; set; }
    public string? DES_SUBREGION     { get; set; }
    public string? DES_CAPITAL       { get; set; }
    public string? DES_BANDERA_EMOJI { get; set; }
    public string? URL_BANDERA_PNG   { get; set; }
    public string? URL_BANDERA_SVG   { get; set; }
    public byte[]? IMG_ICONO         { get; set; }
    public byte[]? IMG_ICONO_SVG     { get; set; }
    public long?   NUM_POBLACION     { get; set; }
}

// ───────────── ESTANDAR.PRODUCTOS ─────────────
public class Productos
{
    // Sin [Required]: al crear un Producto nuevo el código lo arma el SP internamente
    // y este campo queda vacío en el formulario — [Required] bloquearía el submit.
    public string  COD_PRODUCTO                  { get; set; } = "";
    [Required] public string DES_PRODUCTO                  { get; set; } = "";
    [Required] public string DES_DETALLADA                 { get; set; } = "";
    public decimal? COD_ENTIDAD_PROPIETARIA                { get; set; }
    public string?  COD_DISENO                             { get; set; }
    public int?     NUM_VERSION                            { get; set; }
    public string?  COD_UNIDAD_MEDIDA                      { get; set; }
    public string?  COD_UNIDAD_MEDIDA_SECUNDARIA            { get; set; }
    public string?  COD_GRUPO_PRODUCTO                     { get; set; }
    public string?  COD_FAMILIA_PRODUCTO                   { get; set; }
    public string?  COD_SUB_FAMILIA_PRODUCTO                { get; set; }
    public string?  COD_PARTIDA_ARANCELARIA                { get; set; }
    public string?  COD_TIPO_VALOR_CALCULO                 { get; set; }
    public string?  COD_TIPO_PRODUCTO                      { get; set; }
    public string?  COD_CUBSO                               { get; set; }
    public int?      COD_TIPO_ESTADO                        { get; set; }
    public int?      COD_ESTADO                             { get; set; }
    public string?  DES_CUBSO                               { get; set; }
    public string?  DES_BACKCOLOR                           { get; set; }
    public string?  DES_ESTADO                              { get; set; }
    public string?  DES_FORECOLOR                           { get; set; }
    public string?  DES_SUB_FAMILIA_PRODUCTO                { get; set; }
    public string?  DES_TIPO_PRODUCTO                       { get; set; }
    public string?  DES_TIPO_VALOR_CALCULO                  { get; set; }
    public string?  DES_UNIDAD_MEDIDA                       { get; set; }
    public string   COD_USUARIO_REGISTRO                    { get; set; } = "";
    public string   COD_ESTACION_REGISTRO                   { get; set; } = "";
    public DateTime? FEC_REGISTRO                           { get; set; }
    public string   COD_USUARIO_ACTUALIZACION               { get; set; } = "";
    public string   COD_ESTACION_ACTUALIZACION              { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION                      { get; set; }
}

// ───────────── ESTANDAR.TIPOS_PRODUCTO ─────────────
public class TiposProducto
{
    [Required] public string COD_TIPO_PRODUCTO  { get; set; } = "";
    [Required] public string DES_TIPO_PRODUCTO  { get; set; } = "";
    public bool? FLG_COMPRADO       { get; set; }
    public bool? FLG_SERVICIO       { get; set; }
    public bool? FLG_GASTOS         { get; set; }
    public bool? FLG_INDUSTRIALIZADO{ get; set; }
    public bool? FLG_GENERICO       { get; set; }
    public bool? FLG_DESARROLLO     { get; set; }
    public int?  COD_TIPO_ESTADO_INICIAL { get; set; }
    public int?  COD_ESTADO_INICIAL { get; set; }
    public string? DES_TIPO_ESTADO  { get; set; }
    public string? DES_BACKCOLOR    { get; set; }
    public string? DES_ESTADO       { get; set; }
    public string? DES_FORECOLOR    { get; set; }
}

// ───────────── ESTANDAR.GRUPOS_PRODUCTO ─────────────
public class GruposProducto
{
    // Sin [Required]: al crear, el código lo arma el SP a partir del ALIAS y queda
    // vacío en el formulario — [Required] bloquearía el submit.
    public string  COD_GRUPO_PRODUCTO    { get; set; } = "";
    public string? COD_GRUPO_PRODUCTO_ALIAS        { get; set; }
    [Required] public string DES_GRUPO_PRODUCTO    { get; set; } = "";
    public bool?   FLG_BIEN                        { get; set; }
    public string? COD_CUBSO                       { get; set; }
    public string  COD_PARTIDA_ARANCELARIA         { get; set; } = "";
    public string? COD_TIPO_VALOR_CALCULO          { get; set; }
    public string? COD_UNIDAD_MEDIDA                { get; set; }
    public string? COD_UNIDAD_MEDIDA_SECUNDARIA     { get; set; }
    public int?    COD_TIPO_ESTADO                 { get; set; }
    public int?    COD_ESTADO                      { get; set; }
    public string  DES_ESTADO                      { get; set; } = "";
    public string  DES_BACKCOLOR                   { get; set; } = "";
    public string  DES_FORECOLOR                   { get; set; } = "";
    public string? DES_CUBSO                       { get; set; }
    public string? DES_CUBSO_ES                    { get; set; }
    public string? DES_TIPO_VALOR_CALCULO          { get; set; }
    public string? DES_UNIDAD_MEDIDA               { get; set; }
    public string  COD_USUARIO_REGISTRO       { get; set; } = "";
    public string  COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO             { get; set; }
    public string  COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string  COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

// ───────────── ESTANDAR.FAMILIAS_PRODUCTO ─────────────
public class FamiliasProducto
{
    [Required] public string COD_GRUPO_PRODUCTO    { get; set; } = "";
    // Sin [Required] en el código de Familia: al crear se arma en el SP y queda vacío.
    public string  COD_FAMILIA_PRODUCTO  { get; set; } = "";
    public string? COD_FAMILIA_PRODUCTO_ALIAS      { get; set; }
    [Required] public string DES_FAMILIA_PRODUCTO  { get; set; } = "";
    public bool?   FLG_BIEN                        { get; set; }
    public string? COD_CUBSO                       { get; set; }
    public string  COD_TIPO_PRODUCTO               { get; set; } = "";
    public string  COD_PARTIDA_ARANCELARIA         { get; set; } = "";
    public string? COD_TIPO_VALOR_CALCULO          { get; set; }
    public string? COD_UNIDAD_MEDIDA                { get; set; }
    public string? COD_UNIDAD_MEDIDA_SECUNDARIA     { get; set; }
    public int?    COD_TIPO_ESTADO                 { get; set; }
    public int?    COD_ESTADO                      { get; set; }
    public string  DES_ESTADO                      { get; set; } = "";
    public string  DES_BACKCOLOR                   { get; set; } = "";
    public string  DES_FORECOLOR                   { get; set; } = "";
    public string  DES_GRUPO_PRODUCTO              { get; set; } = "";
    public string  DES_TIPO_PRODUCTO               { get; set; } = "";
    public string? DES_CUBSO                       { get; set; }
    public string? DES_CUBSO_ES                    { get; set; }
    public string? DES_TIPO_VALOR_CALCULO          { get; set; }
    public string? DES_UNIDAD_MEDIDA               { get; set; }
    public string  COD_USUARIO_REGISTRO       { get; set; } = "";
    public string  COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO             { get; set; }
    public string  COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string  COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

// ───────────── ESTANDAR.SUB_FAMILIAS_PRODUCTO ─────────────
public class SubFamiliasProducto
{
    [Required] public string COD_GRUPO_PRODUCTO      { get; set; } = "";
    [Required] public string COD_FAMILIA_PRODUCTO    { get; set; } = "";
    // Sin [Required] en el código de Sub Familia: al crear se arma en el SP y queda vacío.
    public string  COD_SUB_FAMILIA_PRODUCTO{ get; set; } = "";
    public string? COD_SUB_FAMILIA_PRODUCTO_ALIAS    { get; set; }
    [Required] public string DES_SUB_FAMILIA_PRODUCTO{ get; set; } = "";
    public string? COD_CUBSO                         { get; set; }
    public string  COD_TIPO_PRODUCTO                 { get; set; } = "";
    public string  COD_PARTIDA_ARANCELARIA           { get; set; } = "";
    public string? COD_TIPO_VALOR_CALCULO            { get; set; }
    public string? COD_UNIDAD_MEDIDA                  { get; set; }
    public string? COD_UNIDAD_MEDIDA_SECUNDARIA       { get; set; }
    public int?    COD_TIPO_ESTADO                   { get; set; }
    public int?    COD_ESTADO                        { get; set; }
    public string  DES_ESTADO                        { get; set; } = "";
    public string  DES_BACKCOLOR                     { get; set; } = "";
    public string  DES_FORECOLOR                     { get; set; } = "";
    public string  DES_FAMILIA_PRODUCTO              { get; set; } = "";
    public string  DES_TIPO_PRODUCTO                 { get; set; } = "";
    public string? DES_CUBSO                         { get; set; }
    public string? DES_CUBSO_ES                      { get; set; }
    public string? DES_TIPO_VALOR_CALCULO            { get; set; }
    public string? DES_UNIDAD_MEDIDA                 { get; set; }
    public string  COD_USUARIO_REGISTRO       { get; set; } = "";
    public string  COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO             { get; set; }
    public string  COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string  COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

// ── LoadCombos results ──
public class ProductosLoadCombosResult
{
    public int                CodTipoEstado  { get; set; }
    public List<ComboboxItem> Grupos         { get; set; } = new();
    public List<ComboboxItem> UnidadesMedida { get; set; } = new();
    public List<ComboboxItem> TiposProducto  { get; set; } = new();
    public List<ComboboxItem> Disenos        { get; set; } = new();
    public List<ComboboxItem> Entidades      { get; set; } = new();
    public List<ComboboxItem> Cubso          { get; set; } = new();
    public List<EstadoItem>   Estados        { get; set; } = new();
}

public class GruposProductoLoadCombosResult
{
    public int                CodTipoEstado     { get; set; }
    public List<EstadoItem>   Estados           { get; set; } = new();
    public List<ComboboxItem> UnidadesMedida    { get; set; } = new();
    public List<ComboboxItem> TiposValorCalculo { get; set; } = new();
}

public class FamiliasProductoLoadCombosResult
{
    public int                CodTipoEstado     { get; set; }
    public List<ComboboxItem> TiposProducto     { get; set; } = new();
    public List<EstadoItem>   Estados           { get; set; } = new();
    public List<ComboboxItem> UnidadesMedida    { get; set; } = new();
    public List<ComboboxItem> TiposValorCalculo { get; set; } = new();
}

public class SubFamiliasProductoLoadCombosResult
{
    public int                CodTipoEstado     { get; set; }
    public List<ComboboxItem> TiposProducto     { get; set; } = new();
    public List<EstadoItem>   Estados           { get; set; } = new();
    public List<ComboboxItem> UnidadesMedida    { get; set; } = new();
    public List<ComboboxItem> TiposValorCalculo { get; set; } = new();
}

// ───────────── ESTANDAR.CLASES_OPERACION ─────────────
public class ClaseOperacion
{
    [Required] public string COD_CLASE_OPERACION           { get; set; } = "";
    [Required] public string DES_CLASE_OPERACION           { get; set; } = "";
    public string? COD_TIPO_ENTIDAD_RELACIONADA { get; set; }
    public bool?  FLG_POSITIVO                  { get; set; }
    public bool?  FLG_CAMBIO                    { get; set; }
    public bool?  FLG_FINANZAS                  { get; set; }
    public bool?  FLG_RENDICION                 { get; set; }
    public bool?  FLG_PERMUTA                   { get; set; }
    public bool?  FLG_BANCO                     { get; set; }
    public bool?  FLG_CAJA                      { get; set; }
    public bool?  FLG_REGIMEN_ESPECIAL          { get; set; }
    public int?   COD_TIPO_ESTADO_RELACIONADO   { get; set; }
    public int?   COD_TIPO_MOTIVO_RELACIONADO   { get; set; }
    public int?   COD_TIPO_ESTADO               { get; set; }
    public int?   COD_ESTADO                    { get; set; }

    public string  DES_TIPO_ENTIDAD             { get; set; } = "";
    public string  DES_TIPO_ESTADO              { get; set; } = "";
    public string  DES_TIPO_MOTIVO              { get; set; } = "";
    public string  DES_BACKCOLOR                { get; set; } = "";
    public string  DES_ESTADO                   { get; set; } = "";
    public string  DES_FORECOLOR                { get; set; } = "";
    public string  COD_USUARIO_REGISTRO       { get; set; } = "";
    public string  COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO             { get; set; }
    public string  COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string  COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

// ───────────── ESTANDAR.TIPOS_OPERACION (hijo de ClaseOperacion) ─────────────
public class TipoOperacion
{
    [Required] public string COD_CLASE_OPERACION { get; set; } = "";
    [Required] public string COD_TIPO_OPERACION  { get; set; } = "";
    [Required] public string DES_TIPO_OPERACION  { get; set; } = "";
    public bool? FLG_POSITIVO       { get; set; }
    public bool? FLG_PROVISION      { get; set; }
    public bool? FLG_CANCELACION    { get; set; }
    public bool? FLG_BIEN           { get; set; }
    public bool? FLG_DEVOLUCION     { get; set; }
    public bool? FLG_ANULACION      { get; set; }
    public bool? FLG_ANTICIPO       { get; set; }
    public bool? FLG_CENTRO_COSTO   { get; set; }
    public bool? FLG_PROYECTO       { get; set; }
    public bool? FLG_LOTE_CONTROL   { get; set; }
    public bool? FLG_CAJA           { get; set; }
    public bool? FLG_BANCO          { get; set; }
    public bool? FLG_EXTORNO        { get; set; }
    public bool? FLG_COMERCIO_EXTERIOR { get; set; }
    [Required] public string COD_TIPO_OPERACION_SUNAT { get; set; } = "";
    [Required] public string COD_TIPO_ENTIDAD         { get; set; } = "";
    [Required] public string COD_TIPO_PRODUCTO_DEFECTO { get; set; } = "";
    public int? COD_TIPO_PEDIDO_COMERCIAL { get; set; }
    public int? COD_TIPO_PRECIO           { get; set; }
    public int? COD_CONDICION_PAGO        { get; set; }
    public string COD_CLASE_OPERACION_SINCRONIZADA { get; set; } = "";
    public string COD_TIPO_OPERACION_SINCRONIZADA  { get; set; } = "";
    public int? COD_TIPO_ESTADO_RELACIONADO { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO      { get; set; }

    public string DES_CLASE_OPERACION        { get; set; } = "";
    public string DES_BACKCOLOR              { get; set; } = "";
    public string DES_ESTADO                 { get; set; } = "";
    public string DES_FORECOLOR              { get; set; } = "";
    public string DES_TIPO_ENTIDAD           { get; set; } = "";
    public string DES_TIPO_PEDIDO_COMERCIAL  { get; set; } = "";
    public string DES_TIPO_PRECIO            { get; set; } = "";
    public string DES_TIPO_PRODUCTO          { get; set; } = "";
    public string? DES_CLASE_OPERACION_SINCRONIZADA { get; set; }
    public string? DES_TIPO_OPERACION_SINCRONIZADA  { get; set; }
    public string? DES_TIPO_ESTADO_RELACIONADO       { get; set; }
    public string  COD_USUARIO_REGISTRO       { get; set; } = "";
    public string  COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO             { get; set; }
    public string  COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string  COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

// ───────────── ESTANDAR.CONFIGURACION_GENERAL ─────────────
public class ConfiguracionGeneral
{
    public string   DES_NOMBRE_EMPRESA                { get; set; } = "";
    public string   DES_NOMBRE_APLICACION             { get; set; } = "";
    public byte[]?  IMG_LOGO_EMPRESA                  { get; set; }
    public byte[]?  IMG_LOGO_APLICACION               { get; set; }
    public string   NUM_DOCUMENTO_IDENTIDAD           { get; set; } = "";
    public string   DES_DOMICILIO_FISCAL              { get; set; } = "";
    public string   DES_REFERENCIA_FISCAL             { get; set; } = "";
    public string   COD_PAIS                          { get; set; } = "";
    public string   COD_UBIGEO                        { get; set; } = "";
    public string   COD_TIPO_EMPRESA                  { get; set; } = "";
    public string   COD_TIPO_AUTORIZACION_COMERCIAL   { get; set; } = "";
    public string   DES_COLOR_PRIMARY                 { get; set; } = "";
    public string   DES_BACKCOLOR                     { get; set; } = "";
    public string   DES_FORECOLOR                     { get; set; } = "";
    public string   DES_COLOR_SECONDARY               { get; set; } = "";
    public string   DES_TYPOGRAPHY_TITLE_FONT         { get; set; } = "";
    public int      DES_TYPOGRAPHY_TITLE_SIZE         { get; set; }
    public string   DES_FORECOLOR_TITLE               { get; set; } = "";
    public string   DES_TYPOGRAPHY_SUBTITLE_FONT      { get; set; } = "";
    public int      NUM_TYPOGRAPHY_SUBTITLE_SIZE      { get; set; }
    public string   DES_FORECOLOR_SUBTITLE            { get; set; } = "";
    public string   DES_TYPOGRAPHY_CONTROLS_FONT      { get; set; } = "";
    public int      NUM_TYPOGRAPHY_CONTROLS_SIZE      { get; set; }
    public bool     FLG_ACTIVATE_REFRESH_AUTOMATIC    { get; set; }
    public int      NUM_SECONDS_REFRESH_GRID          { get; set; }
    public bool     FLG_TECLADO_VIRTUAL               { get; set; }
    public string   DES_BACKCOLOR_CONTEXTUALMENU      { get; set; } = "";
    public string   DES_FORECOLOR_CONTEXTUALMENU      { get; set; } = "";
    public string   DES_BACKCOLOR_LOGIN               { get; set; } = "";
    public string   DES_FORECOLOR_LOGIN               { get; set; } = "";
    public string   DES_BACKCOLOR_DIALOG              { get; set; } = "";
    public string   DES_FORECOLOR_DIALOG              { get; set; } = "";
    public string   DES_ICONCOLOR_ACTIONS             { get; set; } = "";
    public bool     FLG_CONTROL_CAMBIOS_ACTIVO        { get; set; }
    public int?     NUM_HIGH_ROW_GRID                 { get; set; }
    public int?     NUM_FONT_ROW_SIZE                 { get; set; }
    public int?     NUM_HIGH_HEAD_GRID                { get; set; }
    public int?     NUM_FONT_HEAD_SIZE                { get; set; }
    public bool     FLG_ACTIVAR_LOCKOUT_SCREEN        { get; set; }
    public int      NUM_MINUTOS_LOCKOUT_SCREEN        { get; set; }
    public int      NUM_SECONDS_OUT_FACIAL_RECOGNITION{ get; set; }
    public string   DES_BACKCOLOR_CONTROLS_REQUIRED   { get; set; } = "";
    public int      NUM_ICONS_GRID_WIDTH_SIZE         { get; set; }
    public int      NUM_ICONS_GRID_HEIGHT_SIZE        { get; set; }
    public int      NUM_IMAGES_GRID_WIDTH_SIZE        { get; set; }
    public int      NUM_IMAGES_GRID_HEIGHT_SIZE       { get; set; }
    public int      NUM_IMAGENES_MEGAS_UPLOAD         { get; set; }
    public int      NUM_ROWS_SHOW_GRID_UNIQUE         { get; set; }
    public int      NUM_ROWS_SHOW_GRID_FATHER         { get; set; }
    public int      NUM_ROWS_SHOW_GRID_CHILDREN       { get; set; }
    public string   DES_BACKCOLOR_ROWS_FOCUS          { get; set; } = "";
    public string   DES_FORECOLOR_ROWS_FOCUS          { get; set; } = "";
    public string   DES_TIPOGRAFIA_ROWS_FOCUS         { get; set; } = "";
    public string   DES_TIPOGRAFIA_GRID_ROWS          { get; set; } = "";
    public string   DES_TIPOGRAFIA_GRID_ROWS_EVEN     { get; set; } = "";
    public string   DES_TIPOGRAFIA_GRID_ROWS_ODD      { get; set; } = "";
    public string   DES_SKIN_COLOR                    { get; set; } = "";
    public string   DES_MODE_TEXT                     { get; set; } = "";
    public string   DES_BACKCOLOR_GRID_HEADER         { get; set; } = "";
    public string   DES_FORECOLOR_GRID_HEADER         { get; set; } = "";
    public string   DES_FORECOLOR_GRID_ROWS           { get; set; } = "";
    public string   DES_BACKCOLOR_GRID_ROWS_EVEN      { get; set; } = "";
    public string   DES_BACKCOLOR_GRID_ROWS_ODD       { get; set; } = "";
    public string   DES_LINECOLOR_GRID_ROWS           { get; set; } = "";
    public string   DES_TYPOGRAPHY_GRID_ROWS          { get; set; } = "";
    public string   DES_BACKCOLOR_ENVIRONMENT         { get; set; } = "";
    public string   DES_FORECOLOR_ENVIRONMENT         { get; set; } = "";
    public string   DES_TYPOGRAPHY_ENVIRONMENT_FONT   { get; set; } = "";
    public bool     FLG_DEGRADE_ENVIRONMENT           { get; set; }
    public bool     FLG_DEGRADE_BACKGROUND_COLOR      { get; set; }
    public bool     FLG_DEGRADE_SECUNDARY_COLOR       { get; set; }
    public string   DES_BUTTON_BACKCOLOR              { get; set; } = "";
    public string   DES_BUTTON_FORECOLOR              { get; set; } = "";
    public string   DES_CULTURE                       { get; set; } = "es-PE";
    public string   COD_MONEDA_OFICIAL                { get; set; } = "";
    public string   COD_MONEDA_EXTRANJERA_REFERENCIA  { get; set; } = "";
    public string   COD_MONEDA_PIVOT_TIPO_CAMBIO      { get; set; } = "";
    public int      NUM_CHARACTERS_MIO_SEARCH         { get; set; } = 3;
    public string   DES_TIPO_EMPRESA                  { get; set; } = "";
    public string   DES_UBIGEO                        { get; set; } = "";
    public string   DES_MONEDA                        { get; set; } = "";
    public string   DES_PAIS                          { get; set; } = "";
    public string?  DES_BANDERA_EMOJI                 { get; set; }
    public string?  DES_CAPITAL                       { get; set; }
    public string?  DES_PAIS_INGLES                   { get; set; }
    public string?  DES_PAIS_NATIVO                   { get; set; }
    public string?  DES_REGION                        { get; set; }
    public string?  DES_SUBREGION                     { get; set; }
    public string   COD_USUARIO_REGISTRO              { get; set; } = "";
    public string   COD_ESTACION_REGISTRO             { get; set; } = "";
    public DateTime? FEC_REGISTRO                     { get; set; }
    public string   COD_USUARIO_ACTUALIZACION         { get; set; } = "";
    public string   COD_ESTACION_ACTUALIZACION        { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION                { get; set; }
}

/// <summary>Espejo de ConfiguracionGeneralPublicDto del API — lo único que la pantalla de
/// Login pide (sin token) para pintarse (logo/colores/tipografía) antes de autenticar.
/// GetAll() completo ahora exige sesión.</summary>
public class ConfiguracionGeneralPublica
{
    public string  DES_NOMBRE_EMPRESA              { get; set; } = "";
    public string  DES_NOMBRE_APLICACION           { get; set; } = "";
    public byte[]? IMG_LOGO_EMPRESA                { get; set; }
    public byte[]? IMG_LOGO_APLICACION             { get; set; }
    public string  DES_COLOR_PRIMARY               { get; set; } = "";
    public string  DES_BACKCOLOR                   { get; set; } = "";
    public string  DES_FORECOLOR                   { get; set; } = "";
    public string  DES_COLOR_SECONDARY             { get; set; } = "";
    public string  DES_TYPOGRAPHY_TITLE_FONT       { get; set; } = "";
    public int     DES_TYPOGRAPHY_TITLE_SIZE       { get; set; }
    public string  DES_FORECOLOR_TITLE             { get; set; } = "";
    public string  DES_TYPOGRAPHY_SUBTITLE_FONT    { get; set; } = "";
    public int     NUM_TYPOGRAPHY_SUBTITLE_SIZE    { get; set; }
    public string  DES_FORECOLOR_SUBTITLE          { get; set; } = "";
    public string  DES_TYPOGRAPHY_CONTROLS_FONT    { get; set; } = "";
    public int     NUM_TYPOGRAPHY_CONTROLS_SIZE    { get; set; }
    public bool    FLG_TECLADO_VIRTUAL             { get; set; }
    public string  DES_BACKCOLOR_LOGIN             { get; set; } = "";
    public string  DES_FORECOLOR_LOGIN             { get; set; } = "";
    public string  DES_BACKCOLOR_DIALOG            { get; set; } = "";
    public string  DES_FORECOLOR_DIALOG            { get; set; } = "";
    public bool    FLG_ACTIVAR_LOCKOUT_SCREEN      { get; set; }
    public int     NUM_MINUTOS_LOCKOUT_SCREEN      { get; set; }
    public string  DES_SKIN_COLOR                  { get; set; } = "";
    public string  DES_MODE_TEXT                   { get; set; } = "";
    public string  DES_BUTTON_BACKCOLOR            { get; set; } = "";
    public string  DES_BUTTON_FORECOLOR            { get; set; } = "";
    public string  DES_CULTURE                     { get; set; } = "es-PE";
}

/// <summary>Preferencias propias de cada usuario: zoom web, minutos de bloqueo de pantalla y fondo del workspace.</summary>
public class ConfiguracionUsuario
{
    public string   COD_USUARIO                { get; set; } = "";
    public int?     TAS_ZOOM_WEB               { get; set; }
    public int?     NUM_MINUTOS_LOCKOUT_SCREEN { get; set; }
    public byte[]?  IMG_FONDO_WORKSPACE        { get; set; }
    public bool?    FLG_NOTIFICACION_ACTIVA    { get; set; }
    public bool?    FLG_USUARIO_SURDO          { get; set; }
    public bool?    FLG_TECLADO_VIRTUAL        { get; set; }
    public int?     NUM_ROWS_PAGE              { get; set; }
    public string?  COD_TIPO_MENU_OPCIONES     { get; set; }
    public string   COD_USUARIO_REGISTRO       { get; set; } = "";
    public string   COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO              { get; set; }
    public string   COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string   COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION         { get; set; }
}

/// <summary>Catálogo ESTANDAR.TIPOS_MENU_OPCIONES — estilos/tipos de menú que un usuario puede preferir.</summary>
public class TipoMenuOpciones
{
    public string  COD_TIPO_MENU_OPCIONES  { get; set; } = "";
    public string? DES_TIPO_MENU_OPCIONES  { get; set; }
    public byte[]? IMG_ICONO               { get; set; }
}

public class TipoMenuOpcionesDto
{
    public string  COD_TIPO_MENU_OPCIONES  { get; set; } = "";
    public string? DES_TIPO_MENU_OPCIONES  { get; set; }
    public byte[]? IMG_ICONO               { get; set; }
}

// ───────────── ESTANDAR.TABLAS_TIPOS_ESTADO ─────────────
/// <summary>Asocia cada tabla del sistema (por su nombre "SCHEMA.TABLA") a un COD_TIPO_ESTADO —
/// define qué grupo de Estados (ESTANDAR.ESTADOS) usa esa tabla en su combo de Estado.</summary>
public class TablaTipoEstado
{
    public string DES_TABLA       { get; set; } = "";
    public int?   COD_TIPO_ESTADO { get; set; }

    // Viene por JOIN a ESTANDAR.TIPOS_ESTADOS (Showall/Showcombox/Search).
    public string? DES_TIPO_ESTADO { get; set; }
}

public class TablaTipoEstadoDto
{
    [Required] public string DES_TABLA { get; set; } = "";
    public int?   COD_TIPO_ESTADO { get; set; }
}

/// <summary>Fila de PROC_ENTORNO_TABLAS_TIPOS_ESTADO_TABLASDISPONIBLES — tablas del sistema
/// que todavía NO están configuradas (para el combo de "Nuevo").</summary>
public class TablaDisponible
{
    public string NombreTabla { get; set; } = "";
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public long? FilasEstimadas { get; set; }
}

// ───────────── ESTANDAR.ANOS — NUM_ANO es IDENTITY (autogenerado por la BD),
// PROC_ESTANDAR_ANOS_INSERT es upsert. Sin SP de _SEARCH. ─────────────
public class Ano
{
    public int NUM_ANO { get; set; }
    public string? DES_ANO { get; set; }
}

/// <summary>NUM_ANO = 0 en un alta nueva (la BD lo asigna vía IDENTITY).</summary>
public class AnoDto
{
    public int NUM_ANO { get; set; }
    [Required] public string DES_ANO { get; set; } = "";
}

// ───────────── ESTANDAR.MESES — NUM_MES es IDENTITY (autogenerado por la BD),
// PROC_ESTANDAR_MESES_INSERT es upsert. Sin SP de _SEARCH. ─────────────
public class Mes
{
    public int NUM_MES { get; set; }
    public string? DES_MES { get; set; }
    public int? NUM_DIAS { get; set; }
    public bool? FLG_AFECTA_BISIESTO { get; set; }
}

/// <summary>NUM_MES = 0 en un alta nueva (la BD lo asigna vía IDENTITY).</summary>
public class MesDto
{
    public int NUM_MES { get; set; }
    [Required] public string DES_MES { get; set; } = "";
    public int? NUM_DIAS { get; set; }
    public bool? FLG_AFECTA_BISIESTO { get; set; }
}
