# ADR-0001: Access como motor de datos inicial
Estado: Propuesto
## Decisión
Usar Access (.accdb) solo como almacén; lógica y UI en C#. Esquema como scripts DDL versionados; el .accdb es artefacto generado.
## Consecuencias
Límite de 2 GB y ~10–20 usuarios concurrentes; sin procedimientos almacenados. Acceso a datos tras interfaces para migrar a SQL Server.
