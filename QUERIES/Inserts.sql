/* ==========================================================
   INSERTS
   ========================================================== */
-- miembros
INSERT INTO miembros (nombre, apellido, email, rol, fecha_alta) VALUES
('Joseph','Monge','joseph@example.com','ADMI','2025-01-10'),
('Wilberth','Mora','wilberth@example.com','EDITOR','2025-02-01'),
('Gaudy','Chinchilla','gaudy@example.com','LECTOR','2025-02-15'),
('Maria','Soto','maria@example.com','EDITOR','2025-03-01'),
('Luis','Arias','luis@example.com','LECTOR','2025-03-10');

-- areas
INSERT INTO areas (nombre, detalle) VALUES
('Casa','Areas internas del hogar'),
('Jardi','Zona verde y huerta'),
('Taller','Herramientas y mantenimiento'),
('Cocina','Alimentos y menaje'),
('Bodega','Almacenaje general');

-- categorias_finanzas
INSERT INTO categorias_finanzas (nombre,tipo) VALUES
('Servicios Publicos','EGRESO'),
('Alimentos','EGRESO'),
('Mantenimiento Hogar','EGRESO'),
('Salud','EGRESO'),
('Salario','INGRESO');

-- proveedores
INSERT INTO proveedores (nombre,tipo) VALUES
('ICE','SERVICIO'),
('Super La Favorita','COMERCIO'),
('Ferreteria El Tornillo','COMERCIO'),
('Clinica Familiar Cartago','SALUD'),
('Universidad CUC','EDUCACIO');

-- listas (usa miembros y areas)
INSERT INTO listas (nombre,tipo,id_area,creada_por,fecha_creada) VALUES
('Pendientes generales','TAREAS',1,1,'2025-04-01'),
('Compras Super','COMPRAS',4,2,'2025-04-02'),
('Compras Ferreteria','COMPRAS',3,1,'2025-04-02'),
('Wishlist Hogar','DESEOS',1,1,'2025-04-03'),
('Tareas Jardin Diarias','TAREAS',2,2,'2025-04-03');

-- tareas (usa listas y areas)
INSERT INTO tareas (id_lista,titulo,descripcion,prioridad,estado,fecha_creacion,fecha_limite,repeticion,id_area) VALUES
(1,'Organizar sala','Ordenar estantes y cables','MEDIA','PENDIENTE','2025-10-16','2025-10-20','NINGUNA',1),
(5,'Regar macetas','Riego suave por la manana','ALTA','PENDIENTE','2025-10-16','2025-10-17','DIARIA',2),
(2,'Leche deslactosada','2 litros','MEDIA','PENDIENTE','2025-10-16',NULL,'NINGUNA',4),
(3,'Silico','Tubo para sellar gotera','MEDIA','PENDIENTE','2025-10-16',NULL,'NINGUNA',3),
(4,'Lampara de pie','Modelo minimalista','BAJA','PENDIENTE','2025-10-16',NULL,'NINGUNA',1);

-- tareas_asignaciones (usa tareas y miembros)
INSERT INTO tareas_asignaciones (id_tarea,id_miembro) VALUES
(1,1),(1,4),(2,2),(3,3),(4,1);

-- eventos (usa miembros)
INSERT INTO eventos (tipo,titulo,fecha_hora,lugar,notas,id_miembro) VALUES
('CITA_MEDICA','Dentista Joseph','2025-10-22T09:00:00','Clinica Odontologica Cartago','Llevar radiografias',1),
('CUMPLE','Cumple de Maria','2025-11-12T00:00:00',NULL,'Comprar regalo',4),
('UNIVERSIDAD','Entrega proyecto','2025-10-30T18:00:00','CUC','Imprimir laminas',2),
('ANIVERSARIO','Aniversario de boda','2026-01-05T00:00:00',NULL,'Reservar cena',1),
('OTRO','Pago impuestos municipalidad','2025-12-15T08:00:00','Municipalidad','Revisar monto',1);

-- facturas (usa proveedores y categorias_finanzas)
INSERT INTO facturas (id_proveedor,monto,categoria_id,fecha_emision,fecha_venc,estado) VALUES
(1,24500.00,1,'2025-10-01','2025-10-20','PENDIENTE'),
(2,68500.50,2,'2025-10-12','2025-10-12','PAGADA'),
(3,15200.00,3,'2025-10-05','2025-10-25','PENDIENTE'),
(4,35000.00,4,'2025-09-28','2025-10-15','VENCIDA'),
(1,26000.00,1,'2025-09-01','2025-09-20','PAGADA');

-- presupuestos (usa categorias_finanzas)
INSERT INTO presupuestos (anio,mes,id_categoria,monto_planeado,monto_ejecutado) VALUES
(2025,'OCTUBRE',1,30000,24500.00),
(2025,'OCTUBRE',2,120000,68500.50),
(2025,'OCTUBRE',3,40000,15200.00),
(2025,'OCTUBRE',4,60000,35000.00),
(2025,'OCTUBRE',5,950000,950000.00);

-- salarios (usa miembros)
INSERT INTO salarios (id_miembro,monto,periodicidad,deducciones,fecha_inicio) VALUES
(1,950000.00,'MENSUAL', 75000.00,'2025-01-01'),
(2,600000.00,'QUINCENAL',30000.00,'2025-02-01'),
(3,450000.00,'MENSUAL', 20000.00,'2025-03-01'),
(4,520000.00,'MENSUAL', 25000.00,'2025-03-01'),
(5,300000.00,'SEMANAL', 10000.00,'2025-04-01');

-- movimientos (usa categorias_finanzas)
INSERT INTO movimientos (fecha,tipo,id_categoria,monto,referencia) VALUES
('2025-10-01','EGRESO',1,24500.00,'Factura ICE octubre'),
('2025-10-12','EGRESO',2,68500.50,'Compra supermercado'),
('2025-10-05','EGRESO',3,15200.00,'Silicon y herramientas'),
('2025-09-28','EGRESO',4,35000.00,'Consulta medica'),
('2025-10-01','INGRESO',5,950000.00,'Salario Joseph');

-- cultivos
INSERT INTO cultivos (nombre,variedad) VALUES
('Cala Holandesa','Zantedeschia aethiopica'),
('Cala Gigante','Blanca'),
('Tomate','Roma'),
('Lechuga','Romaine'),
('Culantro','Criollo');

-- siembras (usa cultivos)
INSERT INTO siembras (id_cultivo,fecha_siembra,fecha_estim_cosecha,sector,notas) VALUES
(1,'2025-09-10',NULL,'Cama A','Bulbos nuevos'),
(2,'2025-09-10',NULL,'Cama B','Sombra parcial'),
(3,'2025-10-01','2025-12-15','Huerta 1','Riego diario'),
(4,'2025-10-05','2025-11-10','Huerta 2','Evitar sol directo 12-2pm'),
(5,'2025-10-08','2025-11-05','Maceta 3','Suelo suelto');

-- tratamientos (usa siembras)
INSERT INTO tratamientos (id_siembra,fecha,producto,dosis,notas) VALUES
(1,'2025-10-12','Fertilizante 20-20-20','2 g/L','Aplicacion foliar'),
(2,'2025-10-12','Calcio','1 ml/L','Refuerzo'),
(3,'2025-10-13','Fungicida','1 g/L','Preventivo'),
(4,'2025-10-14','Insecticida','1 ml/L','Pulgo'),
(5,'2025-10-15','Fertilizante organico','3 ml/L','Semanal');

-- inventario_jardin (sin FKs)
INSERT INTO inventario_jardin (nombre,tipo,cantidad,unidad) VALUES
('20-20-20','FERTILIZANTE',1.50,'kg'),
('Sustrato universal','SUSTRATO',40,'L'),
('Pala de mano','HERRAMIENTA',1,'uds'),
('Semillas de lechuga','SEMILLA',5,'sobres'),
('Pulverizador 2L','HERRAMIENTA',1,'uds');

-- mascotas
INSERT INTO mascotas (nombre,especie,raza,fecha_nac,peso_kg) VALUES
('Remy','PERRO','Border Collie','2025-06-10',6.50),
('Loga','PERRO','Australian Cattle Dog','2023-03-01',22.40),
('Nemo','PEZ','Goldfish','2025-08-20',0.10),
('Luna','GATO','Criollo','2024-05-05',3.80),
('Coco','AVE','Perico','2022-09-12',0.25);

-- vet_visitas (usa mascotas)
INSERT INTO vet_visitas (id_mascota,fecha,motivo,costo,notas) VALUES
(1,'2025-10-05','Vacuna',25000,'Refuerzo'),
(2,'2025-09-20','Control general',15000,'Todo OK'),
(4,'2025-10-01','Desparasitacio',8000,'Pastillas'),
(3,'2025-10-10','Revision aleta',5000,'Cambio parcial de agua'),
(5,'2025-09-01','Chequeo',7000,'Dieta de semillas');

-- mascotas_meds (usa mascotas)
INSERT INTO mascotas_meds (id_mascota,nombre_med,dosis,frecuencia,fecha_ini,fecha_fin) VALUES
(1,'Antiparasitario','1 tableta','Cada 3 meses','2025-10-01',NULL),
(2,'Vitaminas','5 ml','Diario','2025-10-10','2025-11-10'),
(4,'Antipulgas','1 pipeta','Mensual','2025-10-05',NULL),
(3,'Azul de metileno','2 gotas/L','3 dias','2025-10-12','2025-10-14'),
(5,'Calcio','1 ml','Semanal','2025-09-10',NULL);

-- mascotas_salud (usa mascotas)
INSERT INTO mascotas_salud (id_mascota,fecha,evento,notas) VALUES
(1,'2025-10-05','Vacuna','Refuerzo anual'),
(2,'2025-09-20','Control general','Buen estado'),
(3,'2025-10-10','Revisio','Mejorando'),
(4,'2025-10-01','Desparasitacio','OK'),
(5,'2025-09-01','Chequeo','Plumas brillantes');

-- vehiculos
INSERT INTO vehiculos (placa,marca,modelo,anio,poliza,dekra_fecha) VALUES
('ABC123','Toyota','Corolla',2018,'PZ-001','2025-08-20'),
('XYZ987','Hyundai','Tucso',2020,'PZ-002','2025-07-10'),
('CRL456','Nissa','Navara',2017,'PZ-003','2025-06-01'),
('JMN321','Honda','Civic',2019,'PZ-004','2025-09-05'),
('MNG555','Suzuki','Swift',2021,'PZ-005','2025-10-01');

-- vehiculos_mantenimientos (usa vehiculos)
INSERT INTO vehiculos_mantenimientos (id_vehiculo,tipo,concepto,fecha,kilometraje,costo,taller,notas) VALUES
(1,'PREVENTIVO','Cambio de aceite','2025-09-15',85000,28000.00,'Taller Cartago','Filtro incluido'),
(2,'PREVENTIVO','Rotacion de llantas','2025-08-01',42000,15000.00,'Llantera XYZ','Balanceo'),
(3,'CORRECTIVO','Pastillas de freno','2025-07-20',120000,45000.00,'Mecanica Mora','Eje delantero'),
(4,'PREVENTIVO','Alineamiento','2025-09-10',61000,18000.00,'Taller Central','OK'),
(5,'CORRECTIVO','Bateria','2025-10-05',30000,65000.00,'AutoPartes CR','Nueva garantia');
