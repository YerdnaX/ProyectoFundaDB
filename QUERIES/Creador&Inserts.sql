    
GO
CREATE DATABASE domus_hogar;
GO
USE domus_hogar;
GO

/* ==========================================================
   1) Tablas base
   ========================================================== */

CREATE TABLE miembros (
  id_miembro INT IDENTITY(1,1) PRIMARY KEY,
  nombre     NVARCHAR(80)  NOT NULL,
  apellido   NVARCHAR(80)  NOT NULL,
  email      NVARCHAR(120) NULL,
  rol        NVARCHAR(10)  NOT NULL DEFAULT N'LECTOR',
  fecha_alta DATE          NOT NULL
);

CREATE TABLE AsignacionesBotones (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdMiembro INT,
    NumeroBoton INT,
    RutaImagen NVARCHAR(255),
    NombreImagen NVARCHAR(100),
    FOREIGN KEY (IdMiembro) REFERENCES miembros(id_miembro)
);

CREATE TABLE areas (
  id_area INT IDENTITY(1,1) PRIMARY KEY,
  nombre  NVARCHAR(60)  NOT NULL,
  detalle NVARCHAR(200) NULL
);

CREATE TABLE categorias_finanzas (
  id_categoria INT IDENTITY(1,1) PRIMARY KEY,
  nombre       NVARCHAR(80) NOT NULL,
  tipo         NVARCHAR(7)  NOT NULL -- 'INGRESO' | 'EGRESO'
);

CREATE TABLE proveedores (
  id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
  nombre       NVARCHAR(120) NOT NULL,
  tipo         NVARCHAR(10)  NOT NULL DEFAULT N'OTRO'
);

CREATE TABLE listas (
  id_lista     INT IDENTITY(1,1) PRIMARY KEY,
  nombre       NVARCHAR(80) NOT NULL,
  tipo         NVARCHAR(10) NOT NULL DEFAULT N'TAREAS', -- 'TAREAS'|'COMPRAS'|'DESEOS'
  id_area      INT          NULL,
  creada_por   INT          NULL,
  fecha_creada DATE         NOT NULL,
  FOREIGN KEY (id_area)   REFERENCES areas(id_area),
  FOREIGN KEY (creada_por)REFERENCES miembros(id_miembro)
);

CREATE TABLE tareas (
  id_tarea       INT IDENTITY(1,1) PRIMARY KEY,
  id_lista       INT           NOT NULL,
  titulo         NVARCHAR(120) NOT NULL,
  descripcion    NVARCHAR(300) NULL,
  prioridad      NVARCHAR(5)   NOT NULL DEFAULT N'MEDIA',   -- BAJA|MEDIA|ALTA
  estado         NVARCHAR(12)  NOT NULL DEFAULT N'PENDIENTE', -- PENDIENTE|EN_PROCESO|HECHA
  fecha_creacion DATE          NOT NULL,
  fecha_limite   DATE          NULL,
  repeticion     NVARCHAR(8)   NOT NULL DEFAULT N'NINGUNA', -- NINGUNA|DIARIA|SEMANAL|MENSUAL
  id_area        INT           NULL,
  FOREIGN KEY (id_lista) REFERENCES listas(id_lista),
  FOREIGN KEY (id_area)  REFERENCES areas(id_area)
);

CREATE TABLE tareas_asignaciones (
  id_tarea   INT NOT NULL,
  id_miembro INT NOT NULL,
  PRIMARY KEY (id_tarea, id_miembro),
  FOREIGN KEY (id_tarea)   REFERENCES tareas(id_tarea),
  FOREIGN KEY (id_miembro) REFERENCES miembros(id_miembro)
);

CREATE TABLE eventos (
  id_evento   INT IDENTITY(1,1) PRIMARY KEY,
  tipo        NVARCHAR(14)  NOT NULL,              -- CITA_MEDICA|CUMPLE|ANIVERSARIO|UNIVERSIDAD|OTRO
  titulo      NVARCHAR(140) NOT NULL,
  fecha_hora  DATETIME2(0)  NOT NULL,
  lugar       NVARCHAR(160) NULL,
  notas       NVARCHAR(300) NULL,
  id_miembro  INT          NULL,
  FOREIGN KEY (id_miembro) REFERENCES miembros(id_miembro)
);

CREATE TABLE facturas (
  id_factura    INT IDENTITY(1,1) PRIMARY KEY,
  id_proveedor  INT NOT NULL,
  monto         DECIMAL(12,2) NOT NULL,
  categoria_id  INT NOT NULL,
  fecha_emision DATE NOT NULL,
  fecha_venc    DATE NOT NULL,
  estado        NVARCHAR(9) NOT NULL DEFAULT N'PENDIENTE', -- PENDIENTE|PAGADA|VENCIDA
  FOREIGN KEY (id_proveedor) REFERENCES proveedores(id_proveedor),
  FOREIGN KEY (categoria_id) REFERENCES categorias_finanzas(id_categoria)
);

CREATE TABLE presupuestos (
  id_presupuesto  INT IDENTITY(1,1) PRIMARY KEY,
  anio            INT NOT NULL,
  mes             NVARCHAR(20) NOT NULL,
  id_categoria    INT NOT NULL,
  monto_planeado  DECIMAL(12,2) NOT NULL,
  monto_ejecutado DECIMAL(12,2) NOT NULL,
  UNIQUE (anio, mes, id_categoria),
  FOREIGN KEY (id_categoria) REFERENCES categorias_finanzas(id_categoria)
);

CREATE TABLE salarios (
  id_salario   INT IDENTITY(1,1) PRIMARY KEY,
  id_miembro   INT NOT NULL,
  monto        DECIMAL(12,2) NOT NULL,
  periodicidad NVARCHAR(10)  NOT NULL,  -- SEMANAL|QUINCENAL|MENSUAL
  deducciones  DECIMAL(12,2) NOT NULL,
  fecha_inicio DATE NOT NULL,
  FOREIGN KEY (id_miembro) REFERENCES miembros(id_miembro)
);

CREATE TABLE movimientos (
  id_mov       INT IDENTITY(1,1) PRIMARY KEY,
  fecha        DATE NOT NULL,
  tipo         NVARCHAR(7) NOT NULL, -- INGRESO|EGRESO
  id_categoria INT NOT NULL,
  monto        DECIMAL(12,2) NOT NULL,
  referencia   NVARCHAR(160) NULL,
  FOREIGN KEY (id_categoria) REFERENCES categorias_finanzas(id_categoria)
);

CREATE TABLE cultivos (
  id_cultivo INT IDENTITY(1,1) PRIMARY KEY,
  nombre     NVARCHAR(100) NOT NULL,
  variedad   NVARCHAR(100) NULL
);

CREATE TABLE siembras (
  id_siembra          INT IDENTITY(1,1) PRIMARY KEY,
  id_cultivo          INT NOT NULL,
  fecha_siembra       DATE NOT NULL,
  fecha_estim_cosecha DATE NULL,
  sector              NVARCHAR(80) NULL,
  notas               NVARCHAR(300) NULL,
  FOREIGN KEY (id_cultivo) REFERENCES cultivos(id_cultivo)
);

CREATE TABLE tratamientos (
  id_tratamiento INT IDENTITY(1,1) PRIMARY KEY,
  id_siembra     INT NOT NULL,
  fecha          DATE NOT NULL,
  producto       NVARCHAR(120) NOT NULL,
  dosis          NVARCHAR(80)  NULL,
  notas          NVARCHAR(300) NULL,
  FOREIGN KEY (id_siembra) REFERENCES siembras(id_siembra)
);

CREATE TABLE inventario_jardin (
  id_item  INT IDENTITY(1,1) PRIMARY KEY,
  nombre   NVARCHAR(120) NOT NULL,
  tipo     NVARCHAR(12)  NOT NULL,  -- FERTILIZANTE|SUSTRATO|HERRAMIENTA|SEMILLA|OTRO
  cantidad DECIMAL(10,2) NOT NULL,
  unidad   NVARCHAR(20)  NOT NULL
);

CREATE TABLE mascotas (
  id_mascota INT IDENTITY(1,1) PRIMARY KEY,
  nombre     NVARCHAR(80) NOT NULL,
  especie    NVARCHAR(5)  NOT NULL, -- PERRO|GATO|AVE|PEZ|OTRO
  raza       NVARCHAR(80) NULL,
  fecha_nac  DATE         NULL,
  peso_kg    DECIMAL(5,2) NULL
);

CREATE TABLE vet_visitas (
  id_visita  INT IDENTITY(1,1) PRIMARY KEY,
  id_mascota INT NOT NULL,
  fecha      DATE NOT NULL,
  motivo     NVARCHAR(160) NOT NULL,
  costo      DECIMAL(10,2) NOT NULL,
  notas      NVARCHAR(300) NULL,
  FOREIGN KEY (id_mascota) REFERENCES mascotas(id_mascota)
);

CREATE TABLE mascotas_meds (
  id_med      INT IDENTITY(1,1) PRIMARY KEY,
  id_mascota  INT NOT NULL,
  nombre_med  NVARCHAR(120) NOT NULL,
  dosis       NVARCHAR(80)  NULL,
  frecuencia  NVARCHAR(80)  NULL,
  fecha_ini   DATE          NULL,
  fecha_fin   DATE          NULL,
  FOREIGN KEY (id_mascota) REFERENCES mascotas(id_mascota)
);

CREATE TABLE mascotas_salud (
  id_registro INT IDENTITY(1,1) PRIMARY KEY,
  id_mascota  INT NOT NULL,
  fecha       DATE NOT NULL,
  evento      NVARCHAR(160) NOT NULL,
  notas       NVARCHAR(300) NULL,
  FOREIGN KEY (id_mascota) REFERENCES mascotas(id_mascota)
);

CREATE TABLE vehiculos (
  id_vehiculo INT IDENTITY(1,1) PRIMARY KEY,
  placa       NVARCHAR(12) NOT NULL,
  marca       NVARCHAR(60) NOT NULL,
  modelo      NVARCHAR(60) NOT NULL,
  anio        INT     NOT NULL,
  poliza      NVARCHAR(80) NULL,
  dekra_fecha DATE         NULL,
);

CREATE TABLE vehiculos_mantenimientos (
  id_mant     INT IDENTITY(1,1) PRIMARY KEY,
  id_vehiculo INT NOT NULL,
  tipo        NVARCHAR(10)  NOT NULL, -- PREVENTIVO|CORRECTIVO
  concepto    NVARCHAR(140) NOT NULL,
  fecha       DATE          NOT NULL,
  kilometraje INT           NULL,
  costo       DECIMAL(12,2) NOT NULL,
  taller      NVARCHAR(120) NULL,
  notas       NVARCHAR(300) NULL,
  FOREIGN KEY (id_vehiculo) REFERENCES vehiculos(id_vehiculo)
);