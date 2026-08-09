using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Administracion;

/// <summary>Catálogo ADMINISTRACION.TIPOS_CARGO_ADMINISTRATIVO — tipos de cargo administrativo.</summary>
public class TipoCargoAdministrativo
{
    public int?    COD_TIPO_CARGO { get; set; }
    public string  DES_TIPO_CARGO { get; set; } = "";
    public bool?   FLG_JEFATURA   { get; set; }
}

/// <summary>COD_TIPO_CARGO = null/0 en un alta nueva (la BD lo asigna vía IDENTITY).</summary>
public class TipoCargoAdministrativoDto
{
    public int?    COD_TIPO_CARGO { get; set; }
    [Required] public string  DES_TIPO_CARGO { get; set; } = "";
    public bool?   FLG_JEFATURA   { get; set; }
}

/// <summary>Catálogo ADMINISTRACION.CARGOS.</summary>
public class Cargo
{
    public int?    COD_CARGO              { get; set; }
    public string  DES_CARGO              { get; set; } = "";
    public int?    COD_AREA               { get; set; }
    public int?    COD_TIPO_CARGO         { get; set; }
    public int?    NUM_ORDEN_PRESENTACION { get; set; }
    public int?    COD_TIPO_ESTADO        { get; set; }
    public int?    COD_ESTADO             { get; set; }
    public string? DES_AREA               { get; set; }
    public string? DES_TIPO_CARGO         { get; set; }
    public string? DES_ESTADO             { get; set; }
    public string? DES_BACKCOLOR          { get; set; }
    public string? DES_FORECOLOR          { get; set; }
    public string  COD_USUARIO_REGISTRO       { get; set; } = "";
    public string  COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO             { get; set; }
    public string  COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string  COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

/// <summary>COD_CARGO = null/0 en un alta nueva (la BD lo asigna vía IDENTITY).</summary>
public class CargoDto
{
    public int?   COD_CARGO              { get; set; }
    [Required] public string DES_CARGO              { get; set; } = "";
    public int?   COD_AREA               { get; set; }
    public int?   COD_TIPO_CARGO         { get; set; }
    public int?   NUM_ORDEN_PRESENTACION { get; set; }
    public int?   COD_TIPO_ESTADO        { get; set; }
    public int?   COD_ESTADO             { get; set; }
}

/// <summary>Resultado de PROC_ADMINISTRACION_AREAS_LOADCOMBOS: sucursales, centros de costo
/// y estados (ya filtrados por el COD_TIPO_ESTADO de ADMINISTRACION.AREAS) en un solo viaje.</summary>
public class AreasLoadCombosResult
{
    public int CodTipoEstado { get; set; }
    public List<KONSolutions.Shared.Common.ComboboxItem> Sucursales   { get; set; } = new();
    public List<KONSolutions.Shared.Common.ComboboxItem> CentrosCosto { get; set; } = new();
    public List<KONSolutions.Shared.Common.EstadoItem>   Estados      { get; set; } = new();
}

/// <summary>Catálogo jerárquico ADMINISTRACION.AREAS (auto-referenciada por COD_AREA_PADRE).</summary>
public class Area
{
    public int?    COD_AREA               { get; set; }
    public string  DES_AREA               { get; set; } = "";
    public int?    COD_AREA_PADRE         { get; set; }
    public string  COD_SUCURSAL           { get; set; } = "";
    public string  COD_CENTRO_COSTO       { get; set; } = "";
    public bool?   FLG_GERENCIA           { get; set; }
    public bool?   FLG_SUB_GERENCIA       { get; set; }
    public bool?   FLG_JEFATURA           { get; set; }
    public bool?   FLG_SUBJEFATURA        { get; set; }
    public bool?   FLG_SUPERVISOR         { get; set; }
    public int?    NUM_ORDEN_PRESENTACION { get; set; }
    public int?    COD_TIPO_ESTADO        { get; set; }
    public int?    COD_ESTADO             { get; set; }
    public string? DES_CENTRO_COSTO       { get; set; }
    public string? DES_SUCURSAL           { get; set; }
    public string? DES_ESTADO             { get; set; }
    public string? DES_BACKCOLOR          { get; set; }
    public string? DES_FORECOLOR          { get; set; }
    public string  COD_USUARIO_REGISTRO       { get; set; } = "";
    public string  COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO             { get; set; }
    public string  COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string  COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION        { get; set; }

    /// <summary>Construido en el cliente al armar el árbol — no viene de la API.</summary>
    public List<Area> Hijos { get; set; } = new();
}

/// <summary>COD_AREA = null/0 en un alta nueva (la BD lo asigna vía IDENTITY).</summary>
public class AreaDto
{
    public int?   COD_AREA               { get; set; }
    [Required] public string DES_AREA               { get; set; } = "";
    public int?   COD_AREA_PADRE         { get; set; }
    public string COD_SUCURSAL           { get; set; } = "";
    public string COD_CENTRO_COSTO       { get; set; } = "";
    public bool?  FLG_GERENCIA           { get; set; }
    public bool?  FLG_SUB_GERENCIA       { get; set; }
    public bool?  FLG_JEFATURA           { get; set; }
    public bool?  FLG_SUBJEFATURA        { get; set; }
    public bool?  FLG_SUPERVISOR         { get; set; }
    public int?   NUM_ORDEN_PRESENTACION { get; set; }
    public int?   COD_TIPO_ESTADO        { get; set; }
    public int?   COD_ESTADO             { get; set; }
}

/// <summary>Tabla puente ADMINISTRACION.AREAS_CARGOS_ENTIDADES: entidades (personas)
/// asignadas a un cargo dentro de un área. PK compuesta (COD_AREA, COD_CARGO, COD_ENTIDAD).</summary>
public class AreaCargoEntidad
{
    public int?     COD_AREA  { get; set; }
    public int?     COD_CARGO { get; set; }
    public decimal? COD_ENTIDAD { get; set; }
    public string   COD_USUARIO_REGISTRO  { get; set; } = "";
    public string   COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }
}

public class AreaCargoEntidadDto
{
    [Required] public int?     COD_AREA  { get; set; }
    [Required] public int?     COD_CARGO { get; set; }
    [Required] public decimal? COD_ENTIDAD { get; set; }
}

/// <summary>Fila plana del árbol Área &gt; Cargo &gt; Entidad asignada
/// (PROC_ADMINISTRACION_AREAS_CARGOS_ENTIDADES_TREEVIEW). COD_TIPO: 'A' = Área,
/// 'C' = Cargo, 'P' = Entidad/Persona asignada.</summary>
public class AreaCargoEntidadNodo
{
    public int      COD_NODO       { get; set; }
    public string   DES_NODO       { get; set; } = "";
    public int?     COD_NODO_PADRE { get; set; }
    public int?     COD_AREA       { get; set; }
    public int?     COD_CARGO      { get; set; }
    public decimal? COD_ENTIDAD    { get; set; }
    public string   COD_TIPO       { get; set; } = "";

    /// <summary>Construido en el cliente al armar el árbol — no viene de la API.</summary>
    public List<AreaCargoEntidadNodo> Hijos { get; set; } = new();
}

/// <summary>Fila de PROC_ESTANDAR_ENTIDADES_ENTIDADES_DISPONIBLES — personas activas aún
/// no asignadas a ningún área/cargo, candidatas para el diálogo "Asignar".</summary>
public class EntidadDisponible
{
    public decimal COD_ENTIDAD { get; set; }
    public string  DES_NOMBRE_COMPLETO { get; set; } = "";
    public string? DES_TIPO_DOCUMENTO_IDENTIDAD { get; set; }
    public string? NUM_DOCUMENTO_IDENTIDAD { get; set; }
}
