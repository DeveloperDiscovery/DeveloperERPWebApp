# Mantenimiento Industrial Textil (CMMS) — proyección MRP II

Módulo de mantenimiento de máquinas industriales para empresa textil: activos, preventivos (tiempo/contador), incidencias, órdenes de trabajo, repuestos e indicadores. Diseñado para integrarse a un ERP bajo MRP II. **Independiente** del proyecto KonSolutions de la raíz del repositorio.

## Stack
- .NET 8, C#. Capas: `Dominio` → `Aplicacion` → `Infraestructura.Access` → `UI`.
- Datos: Access (.accdb) vía OleDb/Microsoft.ACE.OLEDB. Diseñar para migrar luego a SQL Server (solo cambia la capa de infraestructura).
- Pruebas: xUnit en `tests/`.

## Comandos
- `dotnet build MantenimientoTextil.sln`
- `dotnet test MantenimientoTextil.sln`
- `dotnet format MantenimientoTextil.sln`
- Crear la base local: `pwsh db/crear-accdb.ps1` (Windows con Access/ACE instalado)

## Reglas
1. **Nunca editar el .accdb directamente**: es un artefacto generado. Todo cambio de esquema va en `db/schema/*.sql` (DDL compatible con Access y portable a SQL Server).
2. Sin lógica de negocio en la UI ni en repositorios: las reglas viven en `Dominio`.
3. SQL siempre parametrizado; nunca concatenar entradas.
4. Nombres del dominio en español (`OrdenTrabajo`, `PlanMantenimiento`).
5. Cada cambio referencia un ID de requerimiento (`RF-MNT-###`) en el commit y en la prueba.
6. Ambigüedades: registrar `PREGUNTA-###` en `docs/01-requerimientos/preguntas.md`; no inventar respuestas.
7. Activos y repuestos llevan código maestro y unidad de medida compatibles con el ERP (integración MRP II).
8. Definición de terminado: código + prueba + documentación + ID de requerimiento.

## Glosario
OT: orden de trabajo · MTBF: tiempo medio entre fallas · MTTR: tiempo medio de reparación · OEE: eficiencia global del equipo · Criticidad A/B/C · Paro: interrupción de producción por avería o mantenimiento.

## Ciclo de trabajo
Explorar → planificar → pruebas primero → implementar → revisar (`revisor-csharp`) → documentar → commit.
Abrir Claude Code **dentro de esta carpeta** para que cargue `.claude/`.
