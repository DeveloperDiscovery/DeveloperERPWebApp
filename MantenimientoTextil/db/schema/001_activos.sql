-- RF-MNT-001, RF-MNT-002. Tipos portables Access/SQL Server. Códigos maestros alineados al ERP.
CREATE TABLE Planta (
  PlantaId LONG NOT NULL PRIMARY KEY,
  Nombre TEXT(100) NOT NULL
);
CREATE TABLE Area (
  AreaId LONG NOT NULL PRIMARY KEY,
  PlantaId LONG NOT NULL,
  Nombre TEXT(100) NOT NULL,
  CONSTRAINT FK_Area_Planta FOREIGN KEY (PlantaId) REFERENCES Planta (PlantaId)
);
CREATE TABLE Activo (
  ActivoId LONG NOT NULL PRIMARY KEY,
  CodigoMaestro TEXT(30) NOT NULL,
  Nombre TEXT(150) NOT NULL,
  AreaId LONG NOT NULL,
  ActivoPadreId LONG NULL,
  Criticidad TEXT(1) NOT NULL,
  Activo YESNO NOT NULL,
  CONSTRAINT FK_Activo_Area FOREIGN KEY (AreaId) REFERENCES Area (AreaId),
  CONSTRAINT FK_Activo_Padre FOREIGN KEY (ActivoPadreId) REFERENCES Activo (ActivoId)
);
CREATE UNIQUE INDEX UX_Activo_Codigo ON Activo (CodigoMaestro);
CREATE TABLE Contador (
  ContadorId LONG NOT NULL PRIMARY KEY,
  ActivoId LONG NOT NULL,
  Tipo TEXT(20) NOT NULL,          -- Horas | Metros | Ciclos
  UnidadMedida TEXT(10) NOT NULL,
  CONSTRAINT FK_Contador_Activo FOREIGN KEY (ActivoId) REFERENCES Activo (ActivoId)
);
CREATE TABLE LecturaContador (
  LecturaId LONG NOT NULL PRIMARY KEY,
  ContadorId LONG NOT NULL,
  Fecha DATETIME NOT NULL,
  Valor DOUBLE NOT NULL,
  CONSTRAINT FK_Lectura_Contador FOREIGN KEY (ContadorId) REFERENCES Contador (ContadorId)
);
