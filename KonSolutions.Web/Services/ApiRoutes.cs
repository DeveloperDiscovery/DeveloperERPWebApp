namespace KONSolutions.Web.Services;

/// <summary>
/// Rutas centralizadas de la API.
/// </summary>
public static class ApiRoutes
{
    public const string Prefix = "api/v1";

    public static class Auth
    {
        public const string Login = $"{Prefix}/auth/login";
    }

    public static class Administracion
    {
        public const string TiposCargoAdministrativo = $"{Prefix}/administracion/tipos-cargo-administrativo";
        public const string Cargos = $"{Prefix}/administracion/cargos";
        public const string Areas = $"{Prefix}/administracion/areas";
        public const string AreasLoadcombos = $"{Areas}/loadcombos";
        public const string AreasCargosEntidades = $"{Prefix}/administracion/areas-cargos-entidades";
        public const string AreasCargosEntidadesTreeview = $"{AreasCargosEntidades}/treeview";
        public const string EntidadesDisponibles = $"{Prefix}/administracion/areas-cargos-entidades/entidades-disponibles";
        public const string AsesoresPorEtiqueta = $"{AreasCargosEntidades}/asesores-por-etiqueta";
    }

    public static class Contabilidad
    {
        public const string TiposCentroCosto = $"{Prefix}/contabilidad/tipos-centro-costo";
        public const string CentrosCosto     = $"{Prefix}/contabilidad/centros-costo";
    }

    public static class Entorno
    {
        public const string ControlCambios  = $"{Prefix}/entorno/control-cambios";
        public const string LimitesColumnas = $"{Prefix}/entorno/limites-columnas";
        public const string TiposNotificacion = $"{Prefix}/entorno/tipos-notificacion";
        public const string TiposNotificacionAcciones = $"{Prefix}/entorno/tipos-notificacion-acciones";
        public const string EjecutarSentencias = $"{Prefix}/entorno/ejecutar-sentencias";
        public const string BancoColores = $"{Prefix}/entorno/banco-colores";
        public const string DiccionarioEtiquetas = $"{Prefix}/entorno/diccionario-etiquetas";
        public const string DominiosEmpresa = $"{Prefix}/entorno/dominios-empresa";
        public const string DominiosEmpresaCombobox = $"{DominiosEmpresa}/combobox";
        public const string NotificacionesFlujoPendientes = $"{Prefix}/entorno/notificaciones-flujo/pendientes";
        public const string NotificacionesFlujoShowall    = $"{Prefix}/entorno/notificaciones-flujo/showall";
        public const string NotificacionesAprobaciones = $"{Prefix}/entorno/notificaciones-aprobaciones";
        public const string Servidores = $"{Prefix}/entorno/servidores";
        public const string PathFiles  = $"{Prefix}/entorno/pathfiles";
        // Documentos por carpeta — genérico y reutilizable desde cualquier diálogo (Solicitudes
        // Comerciales, Entidades, etc.). codKeyPathfile = clave de ENTORNO.PATHFILES; identificador
        // = subcarpeta (N° de Solicitud, código de Entidad, etc.).
        public static string DocumentosCarpeta(string codKeyPathfile, string identificador) =>
            $"{Prefix}/entorno/documentos-carpeta/{Uri.EscapeDataString(codKeyPathfile)}/{Uri.EscapeDataString(identificador)}";
        public const string PostIts    = $"{Prefix}/entorno/postits";
        public const string DashboardBd = $"{Prefix}/entorno/dashboard-bd";
    }

    public static class Sistema
    {
        public const string GenerarSpsText = $"{Prefix}/sistema/herramientas/generar-sps-text";
        public const string Tablas         = $"{Prefix}/sistema/herramientas/tablas";
    }

    public static class Seguridad
    {
        public const string Usuarios              = $"{Prefix}/seguridad/usuarios";
        public const string UsuariosCombobox      = $"{Prefix}/seguridad/usuarios/combobox";
        public const string BotonesEstandar       = $"{Prefix}/seguridad/botones-estandar";
        public const string RolesUsuarioUsuarios  = $"{Prefix}/seguridad/roles-usuario-usuarios";
        public const string MisRoles              = $"{Prefix}/seguridad/roles-usuario-usuarios/mis-roles";
        public const string RolesUsuariosAplicacionesOpcionesBotones = $"{Prefix}/seguridad/roles-usuarios-aplicaciones-opciones-botones";
        public const string BotonesTreeview       = $"{Prefix}/seguridad/roles-usuarios-aplicaciones-opciones-botones/treeview";
        public const string BotonesTreeviewLogos  = $"{BotonesTreeview}/logos";
        public const string UsuariosFavoritos     = $"{Prefix}/seguridad/usuarios-favoritos";
        public const string Favoritos             = $"{Prefix}/seguridad/favoritos";
        public const string FavoritosWorkspace    = $"{Prefix}/seguridad/favoritos/workspace";
        public const string FavoritosWorkspaceLogos = $"{Prefix}/seguridad/favoritos/workspace/logos";
        public const string EstadosPorTabla       = $"{Prefix}/seguridad/estados-por-tabla";
        public const string UsuariosSearch        = $"{Usuarios}/search";
        public const string UsuariosPassword      = $"{Usuarios}/password";
        public const string UsuariosPasswordReset = $"{Usuarios}/password-reset";
        public const string UsuariosEstados       = $"{Usuarios}/estados";
        public const string UsuariosResetIntentos = $"{Usuarios}/resetintentos";
        public const string PerfilesCombobox      = $"{Prefix}/seguridad/perfiles-usuario/combobox";
        public const string Perfiles              = $"{Prefix}/seguridad/perfiles-usuario";
        public const string Roles                 = $"{Prefix}/seguridad/roles-usuario";
        // ── Hijos del rol (asignación pura: dos grillas, sin diálogo de alta) ──
        // Cada uno expone además /disponibles/{...}: lo que le falta al rol lo resuelve un SP,
        // no una resta en el cliente.
        public const string RolesAlmacenes            = $"{Prefix}/seguridad/roles-usuario-almacenes";
        public const string RolesAlmacenesDisponibles = $"{RolesAlmacenes}/disponibles";
        public const string RolesAlmacenesAsignar     = $"{RolesAlmacenes}/asignar";
        public const string RolesAlmacenesDesasignar  = $"{RolesAlmacenes}/desasignar";

        public const string RolesAlmacenesTiposMovimiento            = $"{Prefix}/seguridad/roles-usuario-almacenes-tipos-movimiento";
        public const string RolesAlmacenesTiposMovimientoDisponibles = $"{RolesAlmacenesTiposMovimiento}/disponibles";
        public const string RolesAlmacenesTiposMovimientoAsignar     = $"{RolesAlmacenesTiposMovimiento}/asignar";
        public const string RolesAlmacenesTiposMovimientoDesasignar  = $"{RolesAlmacenesTiposMovimiento}/desasignar";

        public const string RolesTiposOperacion            = $"{Prefix}/seguridad/roles-usuario-tipos-operacion";
        public const string RolesTiposOperacionDisponibles = $"{RolesTiposOperacion}/disponibles";
        public const string RolesTiposOperacionAsignar     = $"{RolesTiposOperacion}/asignar";
        public const string RolesTiposOperacionDesasignar  = $"{RolesTiposOperacion}/desasignar";

        public const string RolesAsientosContables            = $"{Prefix}/seguridad/roles-usuario-asientos-contables";
        public const string RolesAsientosContablesDisponibles = $"{RolesAsientosContables}/disponibles";
        public const string RolesAsientosContablesAsignar     = $"{RolesAsientosContables}/asignar";
        public const string RolesAsientosContablesDesasignar  = $"{RolesAsientosContables}/desasignar";

        public const string ComplejidadPassword   = $"{Prefix}/seguridad/complejidad-password";
        public const string Aplicaciones          = $"{Prefix}/seguridad/aplicaciones";
        public const string RolesCombobox               = $"{Roles}/combobox";
        public const string ComplejidadPasswordCombobox = $"{ComplejidadPassword}/combobox";
        public const string AplicacionesCombobox        = $"{Aplicaciones}/combobox";
        public const string AplicacionesOpcionesTreeview = $"{Prefix}/seguridad/aplicaciones-opciones-treeview";
        public const string AplicacionesOpcionesTreeviewLogos = $"{AplicacionesOpcionesTreeview}/logos";
        public const string Opciones       = $"{Prefix}/seguridad/roles-usuarios-aplicaciones-opciones";
        public const string OpcionesPorRol = $"{Prefix}/seguridad/roles-usuarios-aplicaciones-opciones/por-rol";
        public const string Botones        = $"{Prefix}/seguridad/roles-usuarios-aplicaciones-opciones-botones";
        public const string AplicacionesOpcionesBotones = $"{Prefix}/seguridad/aplicaciones-opciones-botones";
        public const string AsignacionRoles    = $"{Prefix}/seguridad/usuario-asignaciones/roles-de-usuario";
        public const string AsignacionOpciones = $"{Prefix}/seguridad/usuario-asignaciones/opciones";
        public const string AsignacionBotones  = $"{Prefix}/seguridad/usuario-asignaciones/botones";
        public const string TiposOpciones        = $"{Prefix}/seguridad/tipos-opciones";
        public const string TiposOpcionesCombobox = $"{Prefix}/seguridad/tipos-opciones/combobox";
        public const string AplicacionesOpciones  = $"{Prefix}/seguridad/aplicaciones-opciones";
        public const string AplicacionesOpcionesMove = $"{AplicacionesOpciones}/move";
        public const string OpcionesTreeviewAsignadas   = $"{Opciones}/treeview-asignadas";
        public const string OpcionesTreeviewDisponibles = $"{Opciones}/treeview-disponibles";
    }

    public static class Estandar
    {
        public const string TiposUso              = $"{Prefix}/estandar/tipos-uso";
        public const string TiposUsoCombobox      = $"{TiposUso}/combobox";
        public const string EntidadesUsuariosComerciales  = $"{Prefix}/estandar/entidades-usuarios-sugeridos/comerciales";
        public const string EntidadesUsuariosDependientes = $"{Prefix}/estandar/entidades-usuarios-sugeridos/dependientes";
        public const string TiposSucursales         = $"{Prefix}/estandar/tipos-sucursales";
        public const string TiposSucursalesCombobox = $"{TiposSucursales}/combobox";
        public const string Sucursales              = $"{Prefix}/estandar/sucursales";
        public const string SucursalesCombobox      = $"{Sucursales}/combobox";
        public const string SucursalesLoadcombos    = $"{Sucursales}/loadcombos";
        public const string EstadosCombobox       = $"{Prefix}/estandar/estados/combobox";
        public const string TiposEstados          = $"{Prefix}/estandar/tipos-estados";
        public const string TiposEstadosCombobox  = $"{TiposEstados}/combobox";
        public const string TablasTiposEstado           = $"{Prefix}/estandar/tablas-tipos-estado";
        public const string TablasTiposEstadoDisponibles = $"{TablasTiposEstado}/tablas-disponibles";
        public const string Estados               = $"{Prefix}/estandar/estados";
        public const string EstadosPorTipo        = $"{Estados}/por-tipo";
        public const string EstadosImportEstandar = $"{Estados}/import-estandar";
        public const string EstadosEstandar       = $"{Prefix}/estandar/estados-estandar";
        public const string TiposMotivosCombobox  = $"{Prefix}/estandar/tipos-motivos/combobox";
        public const string TiposMotivos          = $"{Prefix}/estandar/tipos-motivos";
        public const string Motivos               = $"{Prefix}/estandar/motivos";
        public const string MotivosPorTipo        = $"{Motivos}/por-tipo";
        public const string TiposEntidades        = $"{Prefix}/estandar/tipos-entidades";
        public const string TiposEntidadesCombobox = $"{TiposEntidades}/combobox";
        public const string EntidadesTiposEntidades = $"{Prefix}/estandar/entidades-tipos-entidades";
        public const string EntidadesTiposEntidadesShowselect = $"{EntidadesTiposEntidades}/showselect";
        public const string EntidadesTiposEntidadesShowcomboxid = $"{EntidadesTiposEntidades}/showcomboxid";
        public const string TiposVia              = $"{Prefix}/estandar/tipos-via";
        public const string TiposViaCombobox      = $"{TiposVia}/combobox";
        public const string EtiquetasBusquedaAreas = $"{Prefix}/estandar/etiquetas-busqueda-areas";
        public const string Sexos                 = $"{Prefix}/estandar/sexos";
        public const string TiposPersonas         = $"{Prefix}/estandar/tipos-personas";
        public const string Anos                  = $"{Prefix}/estandar/anos";
        public const string Meses                 = $"{Prefix}/estandar/meses";
        public const string TiposUnidadMedida               = $"{Prefix}/estandar/tipos-unidad-medida";
        public const string TiposUnidadMedidaCombobox       = $"{TiposUnidadMedida}/combobox";
        public const string UnidadesMedida                  = $"{Prefix}/estandar/unidades-medida";
        public const string UnidadesMedidaCombobox          = $"{UnidadesMedida}/combobox";
        public const string UnidadesMedidaDownloadSnippet       = $"{UnidadesMedida}/download-from-snippet";
        public const string UnidadesMedidaEquivalencias              = $"{Prefix}/estandar/unidades-medida-equivalencias";
        public const string UnidadesMedidaEquivalenciasPorTipo       = $"{UnidadesMedidaEquivalencias}/por-tipo";
        public const string UnidadesMedidaEquivalenciasUpsert        = $"{UnidadesMedidaEquivalencias}/upsert";
        public const string UnidadesMedidaEquivalenciasCargarEstandar= $"{UnidadesMedidaEquivalencias}/cargar-estandar";
        public const string TiposDocumentoIdentidad         = $"{Prefix}/estandar/tipos-documento-identidad";
        public const string TiposDocumentoIdentidadCombobox = $"{TiposDocumentoIdentidad}/combobox";
        public const string EntidadesDocumentosIdentidad    = $"{Prefix}/estandar/entidades-documentos-identidad";
        public const string PaisesCombobox                  = $"{Paises}/combobox";
        public const string Ubigeos                    = $"{Prefix}/estandar/ubigeos";
        public const string UbigeosCombobox            = $"{Ubigeos}/combobox";
        public const string UbigeosDownloadFromSnippet = $"{Ubigeos}/download-from-snippet";
        public const string TiposEmpresa               = $"{Prefix}/estandar/tipos-empresa";
        public const string TiposEmpresaCombobox       = $"{TiposEmpresa}/combobox";
        public const string Paises                     = $"{Prefix}/estandar/paises";
        public const string PaisesDownloadFromSnippet  = $"{Paises}/download-from-snippet";
        public const string PaisesDownloadStatus       = $"{Paises}/flag-download-status";
        public const string TiposProducto              = $"{Prefix}/estandar/tipos-producto";
        public const string TiposProductoCombobox      = $"{TiposProducto}/combobox";
        public const string GruposProducto               = $"{Prefix}/estandar/grupos-producto";
        public const string GruposProductoCombobox       = $"{GruposProducto}/combobox";
        public const string GruposProductoLoadcombos     = $"{GruposProducto}/loadcombos";
        public const string FamiliasProducto             = $"{Prefix}/estandar/familias-producto";
        public const string FamiliasProductoCombobox     = $"{FamiliasProducto}/combobox";
        // Encadenados: cada uno pide el código de su padre.
        public static string FamiliasProductoPorGrupo(string grupo) => $"{FamiliasProducto}/combobox/{Uri.EscapeDataString(grupo)}";
        public const string FamiliasProductoLoadcombos   = $"{FamiliasProducto}/loadcombos";
        public const string SubFamiliasProducto          = $"{Prefix}/estandar/sub-familias-producto";
        public const string SubFamiliasProductoLoadcombos = $"{SubFamiliasProducto}/loadcombos";
        public static string SubFamiliasProductoPorFamilia(string grupo, string familia)
            => $"{SubFamiliasProducto}/combobox/{Uri.EscapeDataString(grupo)}/{Uri.EscapeDataString(familia)}";
        public const string Entidades                  = $"{Prefix}/estandar/entidades";
        public const string EntidadesCombobox          = $"{Entidades}/combobox";
        public const string EntidadesParaDepender      = $"{Entidades}/para-depender";
        public const string EntidadesImages            = $"{Entidades}/images";
        public const string Monedas                    = $"{Prefix}/estandar/monedas";
        public const string MonedasCombobox            = $"{Prefix}/estandar/monedas/combobox";
        public const string MonedasTipoCambio          = $"{Prefix}/estandar/monedas-tipo-cambio";
        public const string MonedasTipoCambioDownloadSnippet = $"{MonedasTipoCambio}/download-from-snippet";
        public const string ConfiguracionUsuario           = $"{Prefix}/estandar/configuracion-usuario";
        public const string ConfiguracionUsuarioMia        = $"{ConfiguracionUsuario}/mi-configuracion";
        public const string TiposMenuOpciones              = $"{Prefix}/estandar/tipos-menu-opciones";
        public const string TiposMenuOpcionesCombobox      = $"{TiposMenuOpciones}/combobox";
        public const string ClasesOperacion              = $"{Prefix}/estandar/clases-operacion";
        public const string ClasesOperacionCombobox      = $"{ClasesOperacion}/combobox";
        public const string TiposOperacion               = $"{Prefix}/estandar/tipos-operacion";
        public const string TiposOperacionCombobox       = $"{TiposOperacion}/combobox";
        public const string TiposOperacionSearch         = $"{TiposOperacion}/search";
        public const string TiposComprobantesPago         = $"{Prefix}/estandar/tipos-comprobantes-pago";
        public const string TiposComprobantesPagoCombobox = $"{TiposComprobantesPago}/combobox";

        // Tabla hija de Sucursales: el listado cuelga del COD_SUCURSAL
        // (…/sucursales-tipos-comprobante-pago/{codSucursal}).
        public const string SucursalesTiposComprobantePago       = $"{Prefix}/estandar/sucursales-tipos-comprobante-pago";
        public const string SucursalesTiposComprobantePagoSearch = $"{SucursalesTiposComprobantePago}/search";
        public const string Productos           = $"{Prefix}/estandar/productos";
        public const string ProductosSearch     = $"{Productos}/search";
        public const string ProductosLoadcombos = $"{Productos}/loadcombos";
        public const string ProductosEspecificacionesTecnicas = $"{Prefix}/estandar/productos-especificaciones-tecnicas";
        public static string ProductosActualizarDescripcionDetalle(string codProducto) => $"{Productos}/{Uri.EscapeDataString(codProducto)}/actualizar-descripcion-detalle";
    }

    public static class Comercial
    {
        public const string EntidadesAsesoresComerciales    = $"{Prefix}/comercial/entidades-asesores-comerciales";
        public const string EntidadesTemporadas             = $"{Prefix}/comercial/entidades-temporadas";
        public const string EntidadesTemporadasColecciones  = $"{Prefix}/comercial/entidades-temporadas-colecciones";
        public const string TiposPrecio                  = $"{Prefix}/comercial/tipos-precio";
        public const string TiposPrecioCombobox          = $"{TiposPrecio}/combobox";
        public const string TiposPedidoComercial         = $"{Prefix}/comercial/tipos-pedido-comercial";
        public const string TiposPedidoComercialCombobox = $"{TiposPedidoComercial}/combobox";
        public const string CondicionesPago              = $"{Prefix}/comercial/condiciones-pago";
        public const string CondicionesPagoCombobox      = $"{CondicionesPago}/combobox";
        public const string FormasPago                   = $"{Prefix}/comercial/formas-pago";
        public const string FormasPagoCombobox           = $"{FormasPago}/combobox";
        public const string TiposTransporte              = $"{Prefix}/comercial/tipos-transporte";
        public const string TiposTransporteCombobox      = $"{TiposTransporte}/combobox";
        public const string FasesIncoterm                = $"{Prefix}/comercial/fases-incoterm";
        public const string FasesIncotermCombobox        = $"{FasesIncoterm}/combobox";
        public const string TiposDescuento               = $"{Prefix}/comercial/tipos-descuento";
        public const string TiposDescuentoCombobox       = $"{TiposDescuento}/combobox";
        public const string Incoterms                    = $"{Prefix}/comercial/incoterms";
        public const string IncotermsCombobox            = $"{Incoterms}/combobox";
        public const string IncotermsFases                    = $"{Prefix}/comercial/incoterms-fases";
        public const string IncotermsTiposTransporte          = $"{Prefix}/comercial/incoterms-tipos-transporte";
        public const string TiposPos                  = $"{Prefix}/comercial/tipos-pos";
        public const string TiposPosCombobox          = $"{TiposPos}/combobox";
        public const string TiposImpuesto             = $"{Prefix}/comercial/tipos-impuesto";
        public const string TiposImpuestoCombobox     = $"{TiposImpuesto}/combobox";
        public const string TiposImpuestoValores      = $"{Prefix}/comercial/tipos-impuesto-valores";
        public const string TiposValorCalculo         = $"{Prefix}/comercial/tipos-valor-calculo";
        public const string TiposValorCalculoCombobox = $"{TiposValorCalculo}/combobox";
        public const string TiposOperacionCalculo         = $"{Prefix}/comercial/tipos-operacion-calculo";
        public const string TiposOperacionCalculoCombobox = $"{TiposOperacionCalculo}/combobox";
        public const string TiposOperacionCalculoSearch   = $"{TiposOperacionCalculo}/search";
        public const string TiposOperacionCalculoCopy     = $"{TiposOperacionCalculo}/copy";
        public const string TiposOperacionCalculoAfectados       = $"{Prefix}/comercial/tipos-operacion-calculo-afectados";
        public const string TiposOperacionCalculoAfectadosSearch = $"{TiposOperacionCalculoAfectados}/search";
        public const string TiposOperacionTiposComprobantePago         = $"{Prefix}/comercial/tipos-operacion-tipos-comprobante-pago";
        public const string TiposOperacionTiposComprobantePagoCombobox = $"{TiposOperacionTiposComprobantePago}/combobox";
        public const string TiposMedioContacto        = $"{Prefix}/comercial/tipos-medio-contacto";
        public const string TiposMedioContactoCombobox = $"{TiposMedioContacto}/combobox";
        public const string EntidadesTiposMedioContacto = $"{Prefix}/comercial/entidades-tipos-medio-contacto";
        public const string TiposDireccion            = $"{Prefix}/comercial/tipos-direccion";
        public const string TiposDireccionCombobox    = $"{TiposDireccion}/combobox";
        public const string EntidadesDirecciones      = $"{Prefix}/comercial/entidades-direcciones";
        public const string SolicitudesComerciales         = $"{Prefix}/comercial/solicitudes-comerciales";
        public const string SolicitudesComercialesCombobox = $"{SolicitudesComerciales}/combobox";
        public const string SolicitudesComercialesAsignarResponsable = $"{SolicitudesComerciales}/asignar-responsable";
        public const string SolicitudesComercialesReasignaAsesor = $"{SolicitudesComerciales}/reasigna-asesor";
        public const string SolicitudesComercialesLoadcombos = $"{SolicitudesComerciales}/loadcombos";
        public const string SolicitudesComercialesMemorias = $"{Prefix}/comercial/solicitudes-comerciales-memorias";
        public const string SolicitudesComercialesNotificaciones = $"{Prefix}/comercial/solicitudes-comerciales-notificaciones";
        public const string SolicitudesComercialesCliente       = $"{Prefix}/comercial/solicitudes-comerciales-cliente";
        public const string SolicitudesComercialesClienteSearch = $"{SolicitudesComercialesCliente}/search";
        public const string TiposSolicitudComercial         = $"{Prefix}/comercial/tipos-solicitud-comercial";
        public const string TiposSolicitudComercialCombobox = $"{TiposSolicitudComercial}/combobox";
        public const string TiposSolicitudComercialAreas    = $"{Prefix}/comercial/tipos-solicitud-comercial-areas";
        public const string TiposSolicitudComercialAreasCargos = $"{Prefix}/comercial/tipos-solicitud-comercial-areas-cargos";
    }

    public static class Ingenieria
    {
        public const string TiposEspecificacionTecnica         = $"{Prefix}/ingenieria/tipos-especificacion-tecnica";
        public const string TiposEspecificacionTecnicaCombobox = $"{TiposEspecificacionTecnica}/combobox";
        public const string EspecificacionesTecnicas            = $"{Prefix}/ingenieria/especificaciones-tecnicas";
        public const string EspecificacionesTecnicasCombobox    = $"{EspecificacionesTecnicas}/combobox";
        public const string EspecificacionesTecnicasLoadcombos  = $"{EspecificacionesTecnicas}/loadcombos";
        public const string EspecificacionesItemsTecnicosTreelist = $"{Prefix}/ingenieria/especificaciones-items-tecnicos/treelist";
        public const string EspecificacionesItemsTecnicosUpdate   = $"{Prefix}/ingenieria/especificaciones-items-tecnicos";
        public const string EspecificacionesItemsTecnicosDelete   = $"{Prefix}/ingenieria/especificaciones-items-tecnicos";
        public const string EspecificacionesItemsTecnicosTreelistPorEspecificacion = $"{Prefix}/ingenieria/especificaciones-items-tecnicos/treelist-por-especificacion";
        public const string EspecificacionesItemsTecnicosComboDependientes = $"{Prefix}/ingenieria/especificaciones-items-tecnicos/combo-dependientes";
        // Mismo SP (SP.SCHEMAIngenieria.EspecificacionesTecnicasItems.*) que el árbol de arriba,
        // pero este SHOWBYID sí trae los 6 campos de auditoría — se usa solo para el botón de
        // auditoría del diálogo de edición (el árbol nunca los selecciona).
        public const string EspecificacionesTecnicasItems = $"{Prefix}/ingenieria/especificaciones-tecnicas-items";
        public const string TiposMerma         = $"{Prefix}/ingenieria/tipos-merma";
        public const string TiposMermaCombobox = $"{TiposMerma}/combobox";
        public const string TiposRegistroDatos         = $"{Prefix}/ingenieria/tipos-registro-datos";
        public const string TiposRegistroDatosCombobox = $"{TiposRegistroDatos}/combobox";
        public const string TiposDiseno         = $"{Prefix}/ingenieria/tipos-diseno";
        public const string TiposDisenoCombobox = $"{TiposDiseno}/combobox";
        public const string Disenos         = $"{Prefix}/ingenieria/disenos";
        public const string DisenosCombobox = $"{Disenos}/combobox";
        public const string DisenosLoadcombos = $"{Disenos}/loadcombos";
        public const string DisenosMemorias = $"{Prefix}/ingenieria/disenos-memorias";
        public const string DisenosVersionesMemorias = $"{Prefix}/ingenieria/disenos-versiones-memorias";
        public const string DisenosProducto         = $"{Prefix}/ingenieria/disenos-producto";
        public const string DisenosProductoCombobox = $"{DisenosProducto}/combobox";
        public const string GruposEspecificacionTecnica         = $"{Prefix}/ingenieria/grupos-familias-sub-familias-especificacion-tecnica";
        public const string GruposEspecificacionTecnicaTreelist = $"{GruposEspecificacionTecnica}/treelist";
        public const string GruposEspecificacionTecnicaCopiar   = $"{GruposEspecificacionTecnica}/copiar";
        public const string DisenosVersion          = $"{Prefix}/ingenieria/disenos-version";
        public const string DisenosVersionCombobox  = $"{DisenosVersion}/combobox";
        public const string DisenosVersionSearch    = $"{DisenosVersion}/search";
        public const string DisenosVersionLoadcombos = $"{DisenosVersion}/loadcombos";
        public const string ProduccionRutas         = $"{Prefix}/ingenieria/produccion-rutas";
        public const string ProduccionRutasCombobox = $"{ProduccionRutas}/combobox";
        public const string ClasesDesarrollo         = $"{Prefix}/ingenieria/clases-desarrollo";
        public const string ClasesDesarrolloCombobox = $"{ClasesDesarrollo}/combobox";
        public const string TiposDesarrollo          = $"{Prefix}/ingenieria/tipos-desarrollo";
        public const string TiposDesarrolloCombobox  = $"{TiposDesarrollo}/combobox";
        public const string TiposDesarrolloSearch    = $"{TiposDesarrollo}/search";
        public const string GruposEspecificacionTecnicaGrupos         = $"{Prefix}/ingenieria/grupos-familias-sub-familias-grupos-especificacion-tecnica";
        public const string GruposEspecificacionTecnicaGruposCombobox = $"{GruposEspecificacionTecnicaGrupos}/combobox";
    }

    public static class Controlcalidad
    {
        public const string ClasesDefectosTecnicos         = $"{Prefix}/controlcalidad/clases-defectos-tecnicos";
        public const string ClasesDefectosTecnicosCombobox = $"{ClasesDefectosTecnicos}/combobox";
        public const string TiposDefectosTecnicos          = $"{Prefix}/controlcalidad/tipos-defectos-tecnicos";
        public const string TiposDefectosTecnicosCombobox  = $"{TiposDefectosTecnicos}/combobox";
    }

    public static class Biometria
    {
        public const string Base      = $"{Prefix}/biometria";
        public const string Estados   = $"{Base}/estados";
        public const string Estado    = $"{Base}/estado";
        public const string Registrar = $"{Base}/registrar";
        public const string Verificar = $"{Base}/verificar";
    }

    public static class Externo
    {
        public const string Snippets = $"{Prefix}/externo/snippets";
        public const string Traducir = $"{Snippets}/traducir";
    }
    public static class Sunat
    {
        public const string RegimenesTributarios         = $"{Prefix}/sunat/regimenes-tributarios";
        public const string RegimenesTributariosCombobox = $"{RegimenesTributarios}/combobox";
        public const string RegimenesTributariosSearch   = $"{RegimenesTributarios}/search";

        public const string CubsoSunat                   = $"{Prefix}/sunat/cubso-sunat";
        public const string CubsoSunatCombobox            = $"{CubsoSunat}/combobox";
        public const string CubsoSunatSearch              = $"{CubsoSunat}/search";
        public const string CubsoSunatDownloadFromSnippet = $"{CubsoSunat}/download-from-snippet";
    }

    public static class Developer
    {
        public const string ErrorLog         = $"{Prefix}/developer/error-log";
        public const string AccessLog        = $"{Prefix}/developer/access-log";
        public const string Logs             = $"{Prefix}/developer/logs";
        // Inventario de métodos de un Controller (por reflexión), para el combo "Método de
        // la API" al configurar botones. Lo sirve MetodosApiController: no toca la base.
        public const string MetadataMetodosController = $"{Prefix}/developer/metadata/metodos-controller";
    }

    public static class Logistica
    {
        // ── Requerimientos y Reservas de stock ────────────────────────────────
        // Los lotes alimentan el combo del detalle: el ítem puede no tener lote, así que
        // el combo se deja vaciar.
        // Existencias por almacén y ubicación. La consulta va por POST: son veintitrés
        // filtros y varios de texto libre.
        public const string StocksAlmacenUbicacion             = $"{Prefix}/logistica/stocks-almacen-ubicacion";
        public const string StocksAlmacenUbicacionSearchDetail = $"{StocksAlmacenUbicacion}/search-detail";
        // Los cinco combos independientes en una sola llamada.
        public const string StocksAlmacenUbicacionLoadCombos   = $"{StocksAlmacenUbicacion}/loadcombos";
        // Los lotes de un proveedor.
        public static string LotesControlPorEntidad(decimal cod) => $"{LotesControl}/por-entidad/{cod}";

        public const string LotesControl                = $"{Prefix}/logistica/lotes-control";
        public const string LotesControlCombobox        = $"{LotesControl}/combobox";
        // El detalle del lote se pide por cabecera: el procedimiento lo exige.
        public const string LotesControlDetalle         = $"{Prefix}/logistica/lotes-control-detalle";
        public const string LotesControlDetallePorDoc   = $"{LotesControlDetalle}/por-cabecera";
        public const string TiposLoteControl            = $"{Prefix}/logistica/tipos-lote-control";
        public const string TiposLoteControlCombobox    = $"{TiposLoteControl}/combobox";

        public const string TiposRequerimiento          = $"{Prefix}/logistica/tipos-requerimiento";
        public const string TiposRequerimientoCombobox  = $"{TiposRequerimiento}/combobox";
        public const string TiposReservaStock           = $"{Prefix}/logistica/tipos-reserva-stock";
        public const string TiposReservaStockCombobox   = $"{TiposReservaStock}/combobox";

        public const string RequerimientosStock         = $"{Prefix}/logistica/requerimientos-stock";
        public const string RequerimientosStockCombobox = $"{RequerimientosStock}/combobox";
        // El detalle cuelga de la cabecera: se pide por número de documento.
        public const string RequerimientosStockDetalle       = $"{Prefix}/logistica/requerimientos-stock-detalle";
        public const string RequerimientosStockDetallePorDoc = $"{RequerimientosStockDetalle}/por-cabecera";

        public const string ReservasStock               = $"{Prefix}/logistica/reservas-stock";
        public const string ReservasStockCombobox       = $"{ReservasStock}/combobox";
        public const string ReservasStockDetalle        = $"{Prefix}/logistica/reservas-stock-detalle";
        public const string ReservasStockDetallePorDoc  = $"{ReservasStockDetalle}/por-cabecera";

        public const string Almacenes                = $"{Prefix}/logistica/almacenes";
        public const string AlmacenesCombobox        = $"{Almacenes}/combobox";
        public const string AlmacenesSearch          = $"{Almacenes}/search";

        // Tabla hija: el listado y el combo cuelgan del COD_ALMACEN
        // (…/almacenes-ubicaciones-almacen/{codAlmacen}).
        public const string AlmacenesUbicaciones     = $"{Prefix}/logistica/almacenes-ubicaciones-almacen";
        public const string AlmacenesUbicacionesSearch = $"{AlmacenesUbicaciones}/search";

        public const string ContenedoresAlmacenaje         = $"{Prefix}/logistica/contenedores-almacenaje";
        public const string ContenedoresAlmacenajeCombobox = $"{ContenedoresAlmacenaje}/combobox";
        public const string ContenedoresAlmacenajeSearch   = $"{ContenedoresAlmacenaje}/search";

        // Casilleros: hijos del contenedor. Showall/combobox llevan el COD_CONTENEDOR en la ruta.
        public const string ContenedoresCasilleros         = $"{Prefix}/logistica/contenedores-almacenaje-casilleros";
        public const string ContenedoresCasillerosCombobox = $"{ContenedoresCasilleros}/combobox";
        public const string ContenedoresCasillerosSearch   = $"{ContenedoresCasilleros}/search";
        public const string ContenedoresCasillerosGenerar  = $"{ContenedoresCasilleros}/generar";

        // Sin ruta de search: la tabla no tiene SP _SEARCH y se filtra en el cliente.
        public const string TiposUbicacion           = $"{Prefix}/logistica/tipos-ubicacion";
        public const string TiposUbicacionCombobox   = $"{TiposUbicacion}/combobox";

        public const string TiposAlmacen             = $"{Prefix}/logistica/tipos-almacen";
        public const string TiposAlmacenCombobox     = $"{TiposAlmacen}/combobox";
        public const string TiposAlmacenSearch       = $"{TiposAlmacen}/search";

        public const string ClasesMovimiento         = $"{Prefix}/logistica/clases-movimiento";
        public const string ClasesMovimientoCombobox = $"{ClasesMovimiento}/combobox";
        public const string ClasesMovimientoSearch   = $"{ClasesMovimiento}/search";

        public const string TiposMovimiento          = $"{Prefix}/logistica/tipos-movimiento";
        public const string TiposMovimientoCombobox  = $"{TiposMovimiento}/combobox";
        public const string TiposMovimientoSearch    = $"{TiposMovimiento}/search";

        // Tipos de movimiento habilitados por almacén. Las lecturas llevan almacén y clase
        // en la ruta porque el SP exige las dos: un tipo sólo existe dentro de su clase.
        public const string AlmacenesTiposMovimiento           = $"{Prefix}/logistica/almacenes-tipos-movimiento";
        public const string AlmacenesTiposMovimientoAsignar    = $"{AlmacenesTiposMovimiento}/asignar";
        public const string AlmacenesTiposMovimientoDesasignar = $"{AlmacenesTiposMovimiento}/desasignar";
    }

    public static class Zonificacion
    {
        // Tabla hija de Sucursales: el listado y el combo cuelgan del COD_SUCURSAL.
        public const string Zonificaciones       = $"{Prefix}/zonificacion/zonificacion";
        public const string ZonificacionesSearch = $"{Zonificaciones}/search";

        public const string TiposZonificacion         = $"{Prefix}/zonificacion/tipos-zonificacion";
        public const string TiposZonificacionCombobox = $"{TiposZonificacion}/combobox";
        public const string TiposZonificacionSearch   = $"{TiposZonificacion}/search";
    }

    public static class Produccion
    {
        public const string ClasesProceso          = $"{Prefix}/produccion/clases-proceso";
        public const string ClasesProcesoCombobox  = $"{ClasesProceso}/combobox";
        public const string ClasesProcesoSearch    = $"{ClasesProceso}/search";

        public const string Procesos               = $"{Prefix}/produccion/procesos";
        public const string ProcesosCombobox        = $"{Procesos}/combobox";
        public const string ProcesosSearch          = $"{Procesos}/search";

        public const string SubProcesos             = $"{Prefix}/produccion/sub-procesos";
        public const string SubProcesosCombobox     = $"{SubProcesos}/combobox";
        public const string SubProcesosSearch       = $"{SubProcesos}/search";

        public const string TiposLineaProduccion         = $"{Prefix}/produccion/tipos-linea-produccion";
        public const string TiposLineaProduccionCombobox = $"{TiposLineaProduccion}/combobox";
        public const string TiposLineaProduccionSearch   = $"{TiposLineaProduccion}/search";
    }

    public static class Rrhh
    {
        public const string TiposZona         = $"{Prefix}/rrhh/tipos-zona";
        public const string TiposZonaCombobox = $"{TiposZona}/combobox";
        public const string TiposZonaSearch   = $"{TiposZona}/search";
    }
}
