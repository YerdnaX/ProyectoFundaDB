    
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

/* ==========================================================
   INSERTS (5+ por tabla) respetando FK
   ========================================================== */
-- miembros
INSERT INTO miembros (nombre, apellido, email, rol, fecha_alta) VALUES
(N'Joseph',N'Monge',N'joseph@example.com',N'ADMIN','2025-01-10'),
(N'Wilberth',N'Mora',N'wilberth@example.com',N'EDITOR','2025-02-01'),
(N'Gaudy',N'Chinchilla',N'gaudy@example.com',N'LECTOR','2025-02-15'),
(N'María',N'Soto',N'maria@example.com',N'EDITOR','2025-03-01'),
(N'Luis',N'Arias',N'luis@example.com',N'LECTOR','2025-03-10');

-- areas
INSERT INTO areas (nombre, detalle) VALUES
(N'Casa',N'Áreas internas del hogar'),
(N'Jardín',N'Zona verde y huerta'),
(N'Taller',N'Herramientas y mantenimiento'),
(N'Cocina',N'Alimentos y menaje'),
(N'Bodega',N'Almacenaje general');

-- categorias_finanzas
INSERT INTO categorias_finanzas (nombre,tipo) VALUES
(N'Servicios Públicos',N'EGRESO'),
(N'Alimentos',N'EGRESO'),
(N'Mantenimiento Hogar',N'EGRESO'),
(N'Salud',N'EGRESO'),
(N'Salario',N'INGRESO');

-- proveedores
INSERT INTO proveedores (nombre,tipo) VALUES
(N'ICE',N'SERVICIO'),
(N'Super La Favorita',N'COMERCIO'),
(N'Ferretería El Tornillo',N'COMERCIO'),
(N'Clínica Familiar Cartago',N'SALUD'),
(N'Universidad CUC',N'EDUCACION');

-- listas (usa miembros y areas)
INSERT INTO listas (nombre,tipo,id_area,creada_por,fecha_creada) VALUES
(N'Pendientes generales',N'TAREAS',1,1,'2025-04-01'),
(N'Compras Super',N'COMPRAS',4,2,'2025-04-02'),
(N'Compras Ferretería',N'COMPRAS',3,1,'2025-04-02'),
(N'Wishlist Hogar',N'DESEOS',1,1,'2025-04-03'),
(N'Tareas Jardín Diarias',N'TAREAS',2,2,'2025-04-03');

-- tareas (usa listas y areas)
INSERT INTO tareas (id_lista,titulo,descripcion,prioridad,estado,fecha_creacion,fecha_limite,repeticion,id_area) VALUES
(1,N'Organizar sala',N'Ordenar estantes y cables',N'MEDIA',N'PENDIENTE','2025-10-16','2025-10-20',N'NINGUNA',1),
(5,N'Regar macetas',N'Riego suave por la mañana',N'ALTA',N'PENDIENTE','2025-10-16','2025-10-17',N'DIARIA',2),
(2,N'Leche deslactosada',N'2 litros',N'MEDIA',N'PENDIENTE','2025-10-16',NULL,N'NINGUNA',4),
(3,N'Silicón',N'Tubo para sellar gotera',N'MEDIA',N'PENDIENTE','2025-10-16',NULL,N'NINGUNA',3),
(4,N'Lámpara de pie',N'Modelo minimalista',N'BAJA',N'PENDIENTE','2025-10-16',NULL,N'NINGUNA',1);

-- tareas_asignaciones (usa tareas y miembros)
INSERT INTO tareas_asignaciones (id_tarea,id_miembro) VALUES
(1,1),(1,4),(2,2),(3,3),(4,1);

-- eventos (usa miembros)
INSERT INTO eventos (tipo,titulo,fecha_hora,lugar,notas,id_miembro) VALUES
(N'CITA_MEDICA',N'Dentista Joseph','2025-10-22T09:00:00',N'Clínica Odontológica Cartago',N'Llevar radiografías',1),
(N'CUMPLE',N'Cumple de María','2025-11-12T00:00:00',NULL,N'Comprar regalo',4),
(N'UNIVERSIDAD',N'Entrega proyecto','2025-10-30T18:00:00',N'CUC',N'Imprimir láminas',2),
(N'ANIVERSARIO',N'Aniversario de boda','2026-01-05T00:00:00',NULL,N'Reservar cena',1),
(N'OTRO',N'Pago impuestos municipalidad','2025-12-15T08:00:00',N'Municipalidad',N'Revisar monto',1);

-- facturas (usa proveedores y categorias_finanzas)
INSERT INTO facturas (id_proveedor,monto,categoria_id,fecha_emision,fecha_venc,estado) VALUES
(1,24500.00,1,'2025-10-01','2025-10-20',N'PENDIENTE'),
(2,68500.50,2,'2025-10-12','2025-10-12',N'PAGADA'),
(3,15200.00,3,'2025-10-05','2025-10-25',N'PENDIENTE'),
(4,35000.00,4,'2025-09-28','2025-10-15',N'VENCIDA'),
(1,26000.00,1,'2025-09-01','2025-09-20',N'PAGADA');

-- presupuestos (usa categorias_finanzas)
INSERT INTO presupuestos (anio,mes,id_categoria,monto_planeado,monto_ejecutado) VALUES
(2025,'OCTUBRE',1,30000,24500.00),
(2025,'OCTUBRE',2,120000,68500.50),
(2025,'OCTUBRE',3,40000,15200.00),
(2025,'OCTUBRE',4,60000,35000.00),
(2025,'OCTUBRE',5,950000,950000.00);

-- salarios (usa miembros)
INSERT INTO salarios (id_miembro,monto,periodicidad,deducciones,fecha_inicio) VALUES
(1,950000.00,N'MENSUAL', 75000.00,'2025-01-01'),
(2,600000.00,N'QUINCENAL',30000.00,'2025-02-01'),
(3,450000.00,N'MENSUAL', 20000.00,'2025-03-01'),
(4,520000.00,N'MENSUAL', 25000.00,'2025-03-01'),
(5,300000.00,N'SEMANAL', 10000.00,'2025-04-01');

-- movimientos (usa categorias_finanzas)
INSERT INTO movimientos (fecha,tipo,id_categoria,monto,referencia) VALUES
('2025-10-01',N'EGRESO',1,24500.00,N'Factura ICE octubre'),
('2025-10-12',N'EGRESO',2,68500.50,N'Compra supermercado'),
('2025-10-05',N'EGRESO',3,15200.00,N'Silicón y herramientas'),
('2025-09-28',N'EGRESO',4,35000.00,N'Consulta médica'),
('2025-10-01',N'INGRESO',5,950000.00,N'Salario Joseph');

-- cultivos
INSERT INTO cultivos (nombre,variedad) VALUES
(N'Cala Holandesa',N'Zantedeschia aethiopica'),
(N'Cala Gigante',N'Blanca'),
(N'Tomate',N'Roma'),
(N'Lechuga',N'Romaine'),
(N'Culantro',N'Criollo');

-- siembras (usa cultivos)
INSERT INTO siembras (id_cultivo,fecha_siembra,fecha_estim_cosecha,sector,notas) VALUES
(1,'2025-09-10',NULL,N'Cama A',N'Bulbos nuevos'),
(2,'2025-09-10',NULL,N'Cama B',N'Sombra parcial'),
(3,'2025-10-01','2025-12-15',N'Huerta 1',N'Riego diario'),
(4,'2025-10-05','2025-11-10',N'Huerta 2',N'Evitar sol directo 12-2pm'),
(5,'2025-10-08','2025-11-05',N'Maceta 3',N'Suelo suelto');

-- tratamientos (usa siembras)
INSERT INTO tratamientos (id_siembra,fecha,producto,dosis,notas) VALUES
(1,'2025-10-12',N'Fertilizante 20-20-20',N'2 g/L',N'Aplicación foliar'),
(2,'2025-10-12',N'Calcio',N'1 ml/L',N'Refuerzo'),
(3,'2025-10-13',N'Fungicida',N'1 g/L',N'Preventivo'),
(4,'2025-10-14',N'Insecticida',N'1 ml/L',N'Pulgón'),
(5,'2025-10-15',N'Fertilizante orgánico',N'3 ml/L',N'Semanal');

-- inventario_jardin (sin FKs)
INSERT INTO inventario_jardin (nombre,tipo,cantidad,unidad) VALUES
(N'20-20-20',N'FERTILIZANTE',1.50,N'kg'),
(N'Sustrato universal',N'SUSTRATO',40,N'L'),
(N'Pala de mano',N'HERRAMIENTA',1,N'uds'),
(N'Semillas de lechuga',N'SEMILLA',5,N'sobres'),
(N'Pulverizador 2L',N'HERRAMIENTA',1,N'uds');

-- mascotas
INSERT INTO mascotas (nombre,especie,raza,fecha_nac,peso_kg) VALUES
(N'Remy',N'PERRO',N'Border Collie','2025-06-10',6.50),
(N'Logan',N'PERRO',N'Australian Cattle Dog','2023-03-01',22.40),
(N'Nemo',N'PEZ',N'Goldfish','2025-08-20',0.10),
(N'Luna',N'GATO',N'Criollo','2024-05-05',3.80),
(N'Coco',N'AVE',N'Perico','2022-09-12',0.25);

-- vet_visitas (usa mascotas)
INSERT INTO vet_visitas (id_mascota,fecha,motivo,costo,notas) VALUES
(1,'2025-10-05',N'Vacuna',25000,N'Refuerzo'),
(2,'2025-09-20',N'Control general',15000,N'Todo OK'),
(4,'2025-10-01',N'Desparasitación',8000,N'Pastillas'),
(3,'2025-10-10',N'Revisión aleta',5000,N'Cambio parcial de agua'),
(5,'2025-09-01',N'Chequeo',7000,N'Dieta de semillas');

-- mascotas_meds (usa mascotas)
INSERT INTO mascotas_meds (id_mascota,nombre_med,dosis,frecuencia,fecha_ini,fecha_fin) VALUES
(1,N'Antiparasitario',N'1 tableta',N'Cada 3 meses','2025-10-01',NULL),
(2,N'Vitaminas',N'5 ml',N'Diario','2025-10-10','2025-11-10'),
(4,N'Antipulgas',N'1 pipeta',N'Mensual','2025-10-05',NULL),
(3,N'Azul de metileno',N'2 gotas/L',N'3 días','2025-10-12','2025-10-14'),
(5,N'Calcio',N'1 ml',N'Semanal','2025-09-10',NULL);

-- mascotas_salud (usa mascotas)
INSERT INTO mascotas_salud (id_mascota,fecha,evento,notas) VALUES
(1,'2025-10-05',N'Vacuna',N'Refuerzo anual'),
(2,'2025-09-20',N'Control general',N'Buen estado'),
(3,'2025-10-10',N'Revisión',N'Mejorando'),
(4,'2025-10-01',N'Desparasitación',N'OK'),
(5,'2025-09-01',N'Chequeo',N'Plumas brillantes');

-- vehiculos
INSERT INTO vehiculos (placa,marca,modelo,anio,poliza,dekra_fecha) VALUES
(N'ABC123',N'Toyota',N'Corolla',2018,N'PZ-001','2025-08-20'),
(N'XYZ987',N'Hyundai',N'Tucson',2020,N'PZ-002','2025-07-10'),
(N'CRL456',N'Nissan',N'Navara',2017,N'PZ-003','2025-06-01'),
(N'JMN321',N'Honda',N'Civic',2019,N'PZ-004','2025-09-05'),
(N'MNG555',N'Suzuki',N'Swift',2021,N'PZ-005','2025-10-01');

-- vehiculos_mantenimientos (usa vehiculos)
INSERT INTO vehiculos_mantenimientos (id_vehiculo,tipo,concepto,fecha,kilometraje,costo,taller,notas) VALUES
(1,N'PREVENTIVO',N'Cambio de aceite','2025-09-15',85000,28000.00,N'Taller Cartago',N'Filtro incluido'),
(2,N'PREVENTIVO',N'Rotación de llantas','2025-08-01',42000,15000.00,N'Llantera XYZ',N'Balanceo'),
(3,N'CORRECTIVO',N'Pastillas de freno','2025-07-20',120000,45000.00,N'Mecánica Mora',N'Eje delantero'),
(4,N'PREVENTIVO',N'Alineamiento','2025-09-10',61000,18000.00,N'Taller Central',N'OK'),
(5,N'CORRECTIVO',N'Batería','2025-10-05',30000,65000.00,N'AutoPartes CR',N'Nueva garantía');


/* ==========================================================
   VISTAS
   ========================================================== */

   --VISTA 1
CREATE VIEW TAREASPENDIENTES AS
    SELECT 
        M.nombre as Miembro,
        T.titulo as Tarea,
        L.nombre as Lista,
        T.prioridad,
        T.fecha_limite,
        A.nombre as area
    FROM tareas T
    JOIN tareas_asignaciones TA ON T.id_tarea = TA.id_tarea
    JOIN miembros M ON TA.id_miembro = M.id_miembro
    JOIN listas L ON T.id_lista = L.id_lista
    LEFT JOIN areas A ON T.id_area = A.id_area
    WHERE T.estado = 'PENDIENTE';

     --VISTA 2
CREATE VIEW FACTURASAVENCER AS
    SELECT 
        P.nombre as Proveedor,
        F.monto,
        F.fecha_venc,
        CF.nombre as Categoria,
        DATEDIFF(DAY, GETDATE(), F.fecha_venc) as [Dias para vencer],
        F.estado as [Estado Pago]
    FROM facturas F
    JOIN proveedores P ON F.id_proveedor = P.id_proveedor
    JOIN categorias_finanzas CF ON F.categoria_id = CF.id_categoria
    WHERE F.estado IN ('PENDIENTE', 'VENCIDA');
    

     --VISTA 3
CREATE VIEW EVENTOSDELMES AS
    SELECT 
        E.titulo,
        E.tipo,
        E.fecha_hora,
        E.lugar,
        M.nombre + ' ' + M.apellido as Responsable
    FROM eventos E
    LEFT JOIN miembros M ON E.id_miembro = M.id_miembro
    WHERE MONTH(E.fecha_hora) = MONTH(GETDATE())
        AND YEAR(E.fecha_hora) = YEAR(GETDATE());

     --VISTA 4
CREATE VIEW MANTEVEHICULO AS
    SELECT 
        VM.id_mant,
        V.placa,
        V.marca,
        V.modelo,
        VM.tipo as [Tipo Mantenimiento],
        VM.concepto,
        VM.fecha,
        VM.kilometraje,
        VM.costo,
        VM.taller,
        DATEDIFF(DAY, VM.fecha, GETDATE()) as [Dias desde el mantenimiento]
    FROM vehiculos V
    JOIN vehiculos_mantenimientos VM ON V.id_vehiculo = VM.id_vehiculo;

     --VISTA 5
CREATE VIEW SALUDMASCOTA AS
    SELECT 
        M.nombre as Mascota,
        M.especie,
        M.raza,
        V.fecha as [Ultima vista],
        V.motivo,
        V.costo,
        MS.evento as [Ultimo evento],
        MS.fecha as [fecha evento]
    FROM mascotas M
    LEFT JOIN vet_visitas V ON M.id_mascota = V.id_mascota
    LEFT JOIN mascotas_salud MS ON M.id_mascota = MS.id_mascota;

     --VISTA 6
CREATE VIEW RESUMENFINANCIERO AS
    SELECT 
        CF.nombre as Categoria,
        CF.tipo,
        SUM(M.monto) as [Total Movimientos],
        COUNT(M.id_mov) as [Cantidad de movimientos]
    FROM movimientos M
    JOIN categorias_finanzas CF ON M.id_categoria = CF.id_categoria
    WHERE MONTH(M.fecha) = MONTH(GETDATE()) 
        AND YEAR(M.fecha) = YEAR(GETDATE())
    GROUP BY CF.nombre, CF.tipo;

     --VISTA 7
CREATE VIEW MEDICAMENTOSACTIVOS AS
    SELECT 
        M.nombre as Mascota,
        MM.nombre_med as Medicamento,
        MM.dosis,
        MM.frecuencia,
        MM.fecha_ini,
        MM.fecha_fin,
        DATEDIFF(DAY, GETDATE(), MM.fecha_fin) as [Dias Restantes]
    FROM mascotas_meds MM
    JOIN mascotas M ON MM.id_mascota = M.id_mascota
    WHERE MM.fecha_fin IS NULL 
        OR MM.fecha_fin >= GETDATE();

     --VISTA 8
CREATE VIEW TOTALFACTURASPAGAR AS
    SELECT 
        COUNT(*) as [Cantidad de facturas],
        SUM(F.monto) as [Monto total pendiente]
    FROM facturas F
    WHERE F.estado IN ('PENDIENTE', 'VENCIDA');

     --VISTA 9
CREATE VIEW GASTOVETERINARIOPORMASCOTA AS
    SELECT 
        M.nombre as Mascota,
        M.especie,
        COUNT(V.id_visita) as[Cantidad Visitas],
        SUM(V.costo) as [Total Gastado]
    FROM mascotas M
    LEFT JOIN vet_visitas V ON M.id_mascota = V.id_mascota
    GROUP BY M.id_mascota, M.nombre, M.especie;

    --VISTA 10
CREATE VIEW RESUMENVEHICULOS AS
    SELECT 
        V.placa,
        V.marca,
        V.modelo,
        V.anio,
        V.dekra_fecha,
        DATEDIFF(DAY, GETDATE(), V.dekra_fecha) as [Días para DEKRA],
        COUNT(VM.id_mant) as [Total Mantenimientos],
        MAX(VM.fecha) as [Último Mantenimiento]
    FROM vehiculos V
    LEFT JOIN vehiculos_mantenimientos VM ON V.id_vehiculo = VM.id_vehiculo
    GROUP BY V.id_vehiculo, V.placa, V.marca, V.modelo, V.anio, V.dekra_fecha;
