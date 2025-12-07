/* ==========================================================
   INSERTS
   ========================================================== */
-- miembros
INSERT INTO miembros (nombre, apellido, email, rol, fecha_alta) VALUES
(N'Joseph',N'Monge',N'joseph@example.com',N'ADMIN','2025-01-10'),
(N'Wilberth',N'Mora',N'wilberth@example.com',N'EDITOR','2025-02-01'),
(N'Gaudy',N'Chinchilla',N'gaudy@example.com',N'LECTOR','2025-02-15'),
(N'Mar�a',N'Soto',N'maria@example.com',N'EDITOR','2025-03-01'),
(N'Luis',N'Arias',N'luis@example.com',N'LECTOR','2025-03-10');

-- areas
INSERT INTO areas (nombre, detalle) VALUES
(N'Casa',N'�reas internas del hogar'),
(N'Jard�n',N'Zona verde y huerta'),
(N'Taller',N'Herramientas y mantenimiento'),
(N'Cocina',N'Alimentos y menaje'),
(N'Bodega',N'Almacenaje general');

-- categorias_finanzas
INSERT INTO categorias_finanzas (nombre,tipo) VALUES
(N'Servicios P�blicos',N'EGRESO'),
(N'Alimentos',N'EGRESO'),
(N'Mantenimiento Hogar',N'EGRESO'),
(N'Salud',N'EGRESO'),
(N'Salario',N'INGRESO');

-- proveedores
INSERT INTO proveedores (nombre,tipo) VALUES
(N'ICE',N'SERVICIO'),
(N'Super La Favorita',N'COMERCIO'),
(N'Ferreter�a El Tornillo',N'COMERCIO'),
(N'Cl�nica Familiar Cartago',N'SALUD'),
(N'Universidad CUC',N'EDUCACION');

-- listas (usa miembros y areas)
INSERT INTO listas (nombre,tipo,id_area,creada_por,fecha_creada) VALUES
(N'Pendientes generales',N'TAREAS',1,1,'2025-04-01'),
(N'Compras Super',N'COMPRAS',4,2,'2025-04-02'),
(N'Compras Ferreter�a',N'COMPRAS',3,1,'2025-04-02'),
(N'Wishlist Hogar',N'DESEOS',1,1,'2025-04-03'),
(N'Tareas Jard�n Diarias',N'TAREAS',2,2,'2025-04-03');

-- tareas (usa listas y areas)
INSERT INTO tareas (id_lista,titulo,descripcion,prioridad,estado,fecha_creacion,fecha_limite,repeticion,id_area) VALUES
(1,N'Organizar sala',N'Ordenar estantes y cables',N'MEDIA',N'PENDIENTE','2025-10-16','2025-10-20',N'NINGUNA',1),
(5,N'Regar macetas',N'Riego suave por la ma�ana',N'ALTA',N'PENDIENTE','2025-10-16','2025-10-17',N'DIARIA',2),
(2,N'Leche deslactosada',N'2 litros',N'MEDIA',N'PENDIENTE','2025-10-16',NULL,N'NINGUNA',4),
(3,N'Silic�n',N'Tubo para sellar gotera',N'MEDIA',N'PENDIENTE','2025-10-16',NULL,N'NINGUNA',3),
(4,N'L�mpara de pie',N'Modelo minimalista',N'BAJA',N'PENDIENTE','2025-10-16',NULL,N'NINGUNA',1);

-- tareas_asignaciones (usa tareas y miembros)
INSERT INTO tareas_asignaciones (id_tarea,id_miembro) VALUES
(1,1),(1,4),(2,2),(3,3),(4,1);

-- eventos (usa miembros)
INSERT INTO eventos (tipo,titulo,fecha_hora,lugar,notas,id_miembro) VALUES
(N'CITA_MEDICA',N'Dentista Joseph','2025-10-22T09:00:00',N'Cl�nica Odontol�gica Cartago',N'Llevar radiograf�as',1),
(N'CUMPLE',N'Cumple de Mar�a','2025-11-12T00:00:00',NULL,N'Comprar regalo',4),
(N'UNIVERSIDAD',N'Entrega proyecto','2025-10-30T18:00:00',N'CUC',N'Imprimir l�minas',2),
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
('2025-10-05',N'EGRESO',3,15200.00,N'Silic�n y herramientas'),
('2025-09-28',N'EGRESO',4,35000.00,N'Consulta m�dica'),
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
(1,'2025-10-12',N'Fertilizante 20-20-20',N'2 g/L',N'Aplicaci�n foliar'),
(2,'2025-10-12',N'Calcio',N'1 ml/L',N'Refuerzo'),
(3,'2025-10-13',N'Fungicida',N'1 g/L',N'Preventivo'),
(4,'2025-10-14',N'Insecticida',N'1 ml/L',N'Pulg�n'),
(5,'2025-10-15',N'Fertilizante org�nico',N'3 ml/L',N'Semanal');

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
(4,'2025-10-01',N'Desparasitaci�n',8000,N'Pastillas'),
(3,'2025-10-10',N'Revisi�n aleta',5000,N'Cambio parcial de agua'),
(5,'2025-09-01',N'Chequeo',7000,N'Dieta de semillas');

-- mascotas_meds (usa mascotas)
INSERT INTO mascotas_meds (id_mascota,nombre_med,dosis,frecuencia,fecha_ini,fecha_fin) VALUES
(1,N'Antiparasitario',N'1 tableta',N'Cada 3 meses','2025-10-01',NULL),
(2,N'Vitaminas',N'5 ml',N'Diario','2025-10-10','2025-11-10'),
(4,N'Antipulgas',N'1 pipeta',N'Mensual','2025-10-05',NULL),
(3,N'Azul de metileno',N'2 gotas/L',N'3 d�as','2025-10-12','2025-10-14'),
(5,N'Calcio',N'1 ml',N'Semanal','2025-09-10',NULL);

-- mascotas_salud (usa mascotas)
INSERT INTO mascotas_salud (id_mascota,fecha,evento,notas) VALUES
(1,'2025-10-05',N'Vacuna',N'Refuerzo anual'),
(2,'2025-09-20',N'Control general',N'Buen estado'),
(3,'2025-10-10',N'Revisi�n',N'Mejorando'),
(4,'2025-10-01',N'Desparasitaci�n',N'OK'),
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
(2,N'PREVENTIVO',N'Rotaci�n de llantas','2025-08-01',42000,15000.00,N'Llantera XYZ',N'Balanceo'),
(3,N'CORRECTIVO',N'Pastillas de freno','2025-07-20',120000,45000.00,N'Mec�nica Mora',N'Eje delantero'),
(4,N'PREVENTIVO',N'Alineamiento','2025-09-10',61000,18000.00,N'Taller Central',N'OK'),
(5,N'CORRECTIVO',N'Bater�a','2025-10-05',30000,65000.00,N'AutoPartes CR',N'Nueva garant�a');
