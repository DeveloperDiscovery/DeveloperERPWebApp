-- RF-MNT-010, 011, 020, 021, 030
CREATE TABLE PlanMantenimiento (
  PlanId LONG NOT NULL PRIMARY KEY,
  ActivoId LONG NOT NULL,
  Nombre TEXT(150) NOT NULL,
  Tipo TEXT(10) NOT NULL,          -- Tiempo | Contador
  Intervalo DOUBLE NOT NULL,       -- días (Tiempo) o unidades del contador
  ContadorId LONG NULL,
  DuracionEstimadaHoras DOUBLE NOT NULL,
  Activo YESNO NOT NULL,
  CONSTRAINT FK_Plan_Activo FOREIGN KEY (ActivoId) REFERENCES Activo (ActivoId)
);
CREATE TABLE Solicitud (
  SolicitudId LONG NOT NULL PRIMARY KEY,
  ActivoId LONG NOT NULL,
  FechaReporte DATETIME NOT NULL,
  Descripcion MEMO NOT NULL,
  Prioridad TEXT(10) NOT NULL,
  CONSTRAINT FK_Solicitud_Activo FOREIGN KEY (ActivoId) REFERENCES Activo (ActivoId)
);
CREATE TABLE OrdenTrabajo (
  OrdenId LONG NOT NULL PRIMARY KEY,
  ActivoId LONG NOT NULL,
  Tipo TEXT(12) NOT NULL,          -- Preventivo | Correctivo
  Estado TEXT(15) NOT NULL,
  SolicitudId LONG NULL,
  PlanId LONG NULL,
  FechaProgramada DATETIME NULL,
  FechaInicio DATETIME NULL,
  FechaFin DATETIME NULL,
  CONSTRAINT FK_OT_Activo FOREIGN KEY (ActivoId) REFERENCES Activo (ActivoId)
);
CREATE TABLE Paro (
  ParoId LONG NOT NULL PRIMARY KEY,
  ActivoId LONG NOT NULL,
  OrdenId LONG NULL,
  Inicio DATETIME NOT NULL,
  Fin DATETIME NULL,
  Causa TEXT(100) NULL,
  CONSTRAINT FK_Paro_Activo FOREIGN KEY (ActivoId) REFERENCES Activo (ActivoId)
);
