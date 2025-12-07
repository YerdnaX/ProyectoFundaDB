    
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
  nombre     VARCHAR(80)  NOT NULL,
  apellido   VARCHAR(80)  NOT NULL,
  email      VARCHAR(120) NULL,
  rol        VARCHAR(10)  NOT NULL DEFAULT 'LECTOR',
  fecha_alta DATE          NOT NULL
);

CREATE TABLE AsignacionesBotones (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdMiembro INT,
    NumeroBoton INT,
    RutaImagen VARCHAR(255),
    NombreImagen VARCHAR(100),
    FOREIGN KEY (IdMiembro) REFERENCES miembros(id_miembro)
);

CREATE TABLE areas (
  id_area INT IDENTITY(1,1) PRIMARY KEY,
  nombre  VARCHAR(60)  NOT NULL,
  detalle VARCHAR(200) NULL
);

CREATE TABLE categorias_finanzas (
  id_categoria INT IDENTITY(1,1) PRIMARY KEY,
  nombre       VARCHAR(80) NOT NULL,
  tipo         VARCHAR(7)  NOT NULL -- 'INGRESO' | 'EGRESO'
);

CREATE TABLE proveedores (
  id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
  nombre       VARCHAR(120) NOT NULL,
  tipo         VARCHAR(10)  NOT NULL DEFAULT 'OTRO'
);

CREATE TABLE listas (
  id_lista     INT IDENTITY(1,1) PRIMARY KEY,
  nombre       VARCHAR(80) NOT NULL,
  tipo         VARCHAR(10) NOT NULL DEFAULT 'TAREAS', -- 'TAREAS'|'COMPRAS'|'DESEOS'
  id_area      INT          NULL,
  creada_por   INT          NULL,
  fecha_creada DATE         NOT NULL,
  FOREIGN KEY (id_area)   REFERENCES areas(id_area),
  FOREIGN KEY (creada_por)REFERENCES miembros(id_miembro)
);

CREATE TABLE tareas (
  id_tarea       INT IDENTITY(1,1) PRIMARY KEY,
  id_lista       INT           NOT NULL,
  titulo         VARCHAR(120) NOT NULL,
  descripcion    VARCHAR(300) NULL,
  prioridad      VARCHAR(5)   NOT NULL DEFAULT 'MEDIA',   -- BAJA|MEDIA|ALTA
  estado         VARCHAR(12)  NOT NULL DEFAULT 'PENDIENTE', -- PENDIENTE|EN_PROCESO|HECHA
  fecha_creacion DATE          NOT NULL,
  fecha_limite   DATE          NULL,
  repeticion     VARCHAR(8)   NOT NULL DEFAULT 'NINGUNA', -- NINGUNA|DIARIA|SEMANAL|MENSUAL
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
  tipo        VARCHAR(14)  NOT NULL,              -- CITA_MEDICA|CUMPLE|ANIVERSARIO|UNIVERSIDAD|OTRO
  titulo      VARCHAR(140) NOT NULL,
  fecha_hora  DATETIME2(0)  NOT NULL,
  lugar       VARCHAR(160) NULL,
  notas       VARCHAR(300) NULL,
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
  estado        VARCHAR(9) NOT NULL DEFAULT 'PENDIENTE', -- PENDIENTE|PAGADA|VENCIDA
  FOREIGN KEY (id_proveedor) REFERENCES proveedores(id_proveedor),
  FOREIGN KEY (categoria_id) REFERENCES categorias_finanzas(id_categoria)
);

CREATE TABLE presupuestos (
  id_presupuesto  INT IDENTITY(1,1) PRIMARY KEY,
  anio            INT NOT NULL,
  mes             VARCHAR(20) NOT NULL,
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
  periodicidad VARCHAR(10)  NOT NULL,  -- SEMANAL|QUINCENAL|MENSUAL
  deducciones  DECIMAL(12,2) NOT NULL,
  fecha_inicio DATE NOT NULL,
  FOREIGN KEY (id_miembro) REFERENCES miembros(id_miembro)
);

CREATE TABLE movimientos (
  id_mov       INT IDENTITY(1,1) PRIMARY KEY,
  fecha        DATE NOT NULL,
  tipo         VARCHAR(7) NOT NULL, -- INGRESO|EGRESO
  id_categoria INT NOT NULL,
  monto        DECIMAL(12,2) NOT NULL,
  referencia   VARCHAR(160) NULL,
  FOREIGN KEY (id_categoria) REFERENCES categorias_finanzas(id_categoria)
);

CREATE TABLE cultivos (
  id_cultivo INT IDENTITY(1,1) PRIMARY KEY,
  nombre     VARCHAR(100) NOT NULL,
  variedad   VARCHAR(100) NULL
);

CREATE TABLE siembras (
  id_siembra          INT IDENTITY(1,1) PRIMARY KEY,
  id_cultivo          INT NOT NULL,
  fecha_siembra       DATE NOT NULL,
  fecha_estim_cosecha DATE NULL,
  sector              VARCHAR(80) NULL,
  notas               VARCHAR(300) NULL,
  FOREIGN KEY (id_cultivo) REFERENCES cultivos(id_cultivo)
);

CREATE TABLE tratamientos (
  id_tratamiento INT IDENTITY(1,1) PRIMARY KEY,
  id_siembra     INT NOT NULL,
  fecha          DATE NOT NULL,
  producto       VARCHAR(120) NOT NULL,
  dosis          VARCHAR(80)  NULL,
  notas          VARCHAR(300) NULL,
  FOREIGN KEY (id_siembra) REFERENCES siembras(id_siembra)
);

CREATE TABLE inventario_jardin (
  id_item  INT IDENTITY(1,1) PRIMARY KEY,
  nombre   VARCHAR(120) NOT NULL,
  tipo     VARCHAR(12)  NOT NULL,  -- FERTILIZANTE|SUSTRATO|HERRAMIENTA|SEMILLA|OTRO
  cantidad DECIMAL(10,2) NOT NULL,
  unidad   VARCHAR(20)  NOT NULL
);

CREATE TABLE mascotas (
  id_mascota INT IDENTITY(1,1) PRIMARY KEY,
  nombre     VARCHAR(80) NOT NULL,
  especie    VARCHAR(5)  NOT NULL, -- PERRO|GATO|AVE|PEZ|OTRO
  raza       VARCHAR(80) NULL,
  fecha_nac  DATE         NULL,
  peso_kg    DECIMAL(5,2) NULL
);

CREATE TABLE vet_visitas (
  id_visita  INT IDENTITY(1,1) PRIMARY KEY,
  id_mascota INT NOT NULL,
  fecha      DATE NOT NULL,
  motivo     VARCHAR(160) NOT NULL,
  costo      DECIMAL(10,2) NOT NULL,
  notas      VARCHAR(300) NULL,
  FOREIGN KEY (id_mascota) REFERENCES mascotas(id_mascota)
);

CREATE TABLE mascotas_meds (
  id_med      INT IDENTITY(1,1) PRIMARY KEY,
  id_mascota  INT NOT NULL,
  nombre_med  VARCHAR(120) NOT NULL,
  dosis       VARCHAR(80)  NULL,
  frecuencia  VARCHAR(80)  NULL,
  fecha_ini   DATE          NULL,
  fecha_fin   DATE          NULL,
  FOREIGN KEY (id_mascota) REFERENCES mascotas(id_mascota)
);

CREATE TABLE mascotas_salud (
  id_registro INT IDENTITY(1,1) PRIMARY KEY,
  id_mascota  INT NOT NULL,
  fecha       DATE NOT NULL,
  evento      VARCHAR(160) NOT NULL,
  notas       VARCHAR(300) NULL,
  FOREIGN KEY (id_mascota) REFERENCES mascotas(id_mascota)
);

CREATE TABLE vehiculos (
  id_vehiculo INT IDENTITY(1,1) PRIMARY KEY,
  placa       VARCHAR(12) NOT NULL,
  marca       VARCHAR(60) NOT NULL,
  modelo      VARCHAR(60) NOT NULL,
  anio        INT     NOT NULL,
  poliza      VARCHAR(80) NULL,
  dekra_fecha DATE         NULL,
);

CREATE TABLE vehiculos_mantenimientos (
  id_mant     INT IDENTITY(1,1) PRIMARY KEY,
  id_vehiculo INT NOT NULL,
  tipo        VARCHAR(10)  NOT NULL, -- PREVENTIVO|CORRECTIVO
  concepto    VARCHAR(140) NOT NULL,
  fecha       DATE          NOT NULL,
  kilometraje INT           NULL,
  costo       DECIMAL(12,2) NOT NULL,
  taller      VARCHAR(120) NULL,
  notas       VARCHAR(300) NULL,
  FOREIGN KEY (id_vehiculo) REFERENCES vehiculos(id_vehiculo)
);