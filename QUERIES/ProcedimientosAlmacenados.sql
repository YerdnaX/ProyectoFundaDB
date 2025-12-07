USE domus_hogar;
GO

-- Asignaciones de botones
-- Guarda asignacion unica de boton para un miembro
CREATE OR ALTER PROCEDURE spGuardarAsignacionBoton
    @NumeroBoton INT,
    @IdMiembro INT,
    @RutaImagen VARCHAR(MAX),
    @NombreImagen VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM AsignacionesBotones WHERE NumeroBoton = @NumeroBoton;
    INSERT INTO AsignacionesBotones (NumeroBoton, IdMiembro, RutaImagen, NombreImagen)
    VALUES (@NumeroBoton, @IdMiembro, @RutaImagen, @NombreImagen);
END
GO

-- Inserts
-- Inserta area en catalogo de areas
CREATE OR ALTER PROCEDURE spInsertarArea
    @nombre VARCHAR(150),
    @detalle VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO areas (nombre, detalle) VALUES (@nombre, @detalle);
END
GO

-- Inserta categoria de finanzas
CREATE OR ALTER PROCEDURE spInsertarCategoriaFinanzas
    @nombre VARCHAR(150),
    @tipo VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO categorias_finanzas (nombre, tipo) VALUES (@nombre, @tipo);
END
GO

-- Inserta proveedor en catalogo
CREATE OR ALTER PROCEDURE spInsertarProveedor
    @nombre VARCHAR(200),
    @tipo VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO proveedores (nombre, tipo) VALUES (@nombre, @tipo);
END
GO

-- Inserta cultivo y variedad
CREATE OR ALTER PROCEDURE spInsertarCultivo
    @nombre VARCHAR(200),
    @variedad VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cultivos (nombre, variedad) VALUES (@nombre, @variedad);
END
GO

-- Registra mascota con datos basicos
CREATE OR ALTER PROCEDURE spInsertarMascota
    @nombre VARCHAR(200),
    @especie VARCHAR(100),
    @raza VARCHAR(200) = NULL,
    @fecha_nac DATE,
    @peso DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO mascotas (nombre, especie, raza, fecha_nac, peso_kg)
    VALUES (@nombre, @especie, @raza, @fecha_nac, @peso);
END
GO

-- Registra vehiculo con datos de poliza
CREATE OR ALTER PROCEDURE spInsertarVehiculo
    @placa VARCHAR(50),
    @marca VARCHAR(100),
    @modelo VARCHAR(100),
    @year INT,
    @poliza VARCHAR(200) = NULL,
    @dekra DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO vehiculos (placa, marca, modelo, anio, poliza, dekra_fecha)
    VALUES (@placa, @marca, @modelo, @year, @poliza, @dekra);
END
GO

-- Crea lista opcionalmente ligada a un area
CREATE OR ALTER PROCEDURE spInsertarLista
    @nombre VARCHAR(200),
    @tipo VARCHAR(100),
    @id_area INT = NULL,
    @creada_por INT = NULL,
    @fecha_creada DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO listas (nombre, tipo, id_area, creada_por, fecha_creada)
    VALUES (@nombre, @tipo, @id_area, @creada_por, @fecha_creada);
END
GO

-- Agrega tarea dentro de una lista
CREATE OR ALTER PROCEDURE spInsertarTarea
    @id_lista INT,
    @titulo VARCHAR(200),
    @descripcion VARCHAR(MAX) = NULL,
    @prioridad VARCHAR(50),
    @estado VARCHAR(50),
    @fecha_creacion DATE,
    @fecha_limite DATE = NULL,
    @repeticion VARCHAR(50),
    @id_area INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tareas (id_lista, titulo, descripcion, prioridad, estado, fecha_creacion, fecha_limite, repeticion, id_area)
    VALUES (@id_lista, @titulo, @descripcion, @prioridad, @estado, @fecha_creacion, @fecha_limite, @repeticion, @id_area);
END
GO

-- Asigna miembro a una tarea
CREATE OR ALTER PROCEDURE spInsertarAsignacionTarea
    @id_tarea INT,
    @id_miembro INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tareas_asignaciones (id_tarea, id_miembro)
    VALUES (@id_tarea, @id_miembro);
END
GO

-- Registra evento de calendario
CREATE OR ALTER PROCEDURE spInsertarEvento
    @tipo VARCHAR(50),
    @titulo VARCHAR(200),
    @fecha DATETIME2,
    @lugar VARCHAR(200) = NULL,
    @notas VARCHAR(MAX) = NULL,
    @id_miembro INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO eventos (tipo, titulo, fecha_hora, lugar, notas, id_miembro)
    VALUES (@tipo, @titulo, @fecha, @lugar, @notas, @id_miembro);
END
GO

-- Registra factura de proveedor
CREATE OR ALTER PROCEDURE spInsertarFactura
    @id_proveedor INT,
    @monto DECIMAL(18,2),
    @id_categoria INT,
    @fecha_emision DATETIME2,
    @fecha_vencimiento DATETIME2,
    @estado VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO facturas (id_proveedor, monto, categoria_id, fecha_emision, fecha_venc, estado)
    VALUES (@id_proveedor, @monto, @id_categoria, @fecha_emision, @fecha_vencimiento, @estado);
END
GO

-- Crea presupuesto mensual por categoria
CREATE OR ALTER PROCEDURE spInsertarPresupuesto
    @anio INT,
    @mes VARCHAR(20),
    @id_categoria INT,
    @monto_planeado DECIMAL(18,2),
    @monto_ejecutado DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO presupuestos (anio, mes, id_categoria, monto_planeado, monto_ejecutado)
    VALUES (@anio, @mes, @id_categoria, @monto_planeado, @monto_ejecutado);
END
GO

-- Registra salario de un miembro
CREATE OR ALTER PROCEDURE spInsertarSalario
    @id_miembro INT,
    @monto DECIMAL(18,2),
    @periodicidad VARCHAR(50),
    @deducciones DECIMAL(18,2),
    @fecha_inicio DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO salarios (id_miembro, monto, periodicidad, deducciones, fecha_inicio)
    VALUES (@id_miembro, @monto, @periodicidad, @deducciones, @fecha_inicio);
END
GO

-- Registra movimiento financiero
CREATE OR ALTER PROCEDURE spInsertarMovimiento
    @fecha DATETIME2,
    @tipo VARCHAR(50),
    @id_categoria INT,
    @monto DECIMAL(18,2),
    @referencia VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO movimientos (fecha, tipo, id_categoria, monto, referencia)
    VALUES (@fecha, @tipo, @id_categoria, @monto, @referencia);
END
GO

-- Registra siembra programada
CREATE OR ALTER PROCEDURE spInsertarSiembra
    @id_cultivo INT,
    @fecha_siembra DATETIME2,
    @fecha_estimada DATETIME2 = NULL,
    @sector VARCHAR(100),
    @notas VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO siembras (id_cultivo, fecha_siembra, fecha_estim_cosecha, sector, notas)
    VALUES (@id_cultivo, @fecha_siembra, @fecha_estimada, @sector, @notas);
END
GO

-- Registra tratamiento aplicado a siembra
CREATE OR ALTER PROCEDURE spInsertarTratamiento
    @id_siembra INT,
    @fecha DATETIME2,
    @producto VARCHAR(200),
    @dosis VARCHAR(100),
    @notas VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tratamientos (id_siembra, fecha, producto, dosis, notas)
    VALUES (@id_siembra, @fecha, @producto, @dosis, @notas);
END
GO

-- Registra item de inventario de jardin
CREATE OR ALTER PROCEDURE spInsertarInventarioJardin
    @nombre VARCHAR(200),
    @tipo VARCHAR(100),
    @cantidad DECIMAL(18,2),
    @unidad VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO inventario_jardin (nombre, tipo, cantidad, unidad)
    VALUES (@nombre, @tipo, @cantidad, @unidad);
END
GO

-- Registra mantenimiento de vehiculo
CREATE OR ALTER PROCEDURE spInsertarMantenimientoVehiculo
    @id_vehiculo INT,
    @tipo VARCHAR(50),
    @concepto VARCHAR(200),
    @fecha DATETIME2,
    @kilometraje INT = NULL,
    @costo DECIMAL(18,2),
    @taller VARCHAR(200) = NULL,
    @notas VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO vehiculos_mantenimientos (id_vehiculo, tipo, concepto, fecha, kilometraje, costo, taller, notas)
    VALUES (@id_vehiculo, @tipo, @concepto, @fecha, @kilometraje, @costo, @taller, @notas);
END
GO

-- Actualizaciones
-- Actualiza datos de un area
CREATE OR ALTER PROCEDURE spActualizarArea
    @id_area INT,
    @nombre VARCHAR(150),
    @detalle VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE areas SET nombre = @nombre, detalle = @detalle
    WHERE id_area = @id_area;
END
GO

-- Actualiza categoria de finanzas
CREATE OR ALTER PROCEDURE spActualizarCategoriaFinanzas
    @id_categoria INT,
    @nombre VARCHAR(150),
    @tipo VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE categorias_finanzas SET nombre = @nombre, tipo = @tipo
    WHERE id_categoria = @id_categoria;
END
GO

-- Actualiza datos de proveedor
CREATE OR ALTER PROCEDURE spActualizarProveedor
    @id_proveedor INT,
    @nombre VARCHAR(200),
    @tipo VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE proveedores SET nombre = @nombre, tipo = @tipo
    WHERE id_proveedor = @id_proveedor;
END
GO

-- Actualiza cultivo existente
CREATE OR ALTER PROCEDURE spActualizarCultivo
    @id_cultivo INT,
    @nombre VARCHAR(200),
    @variedad VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cultivos SET nombre = @nombre, variedad = @variedad
    WHERE id_cultivo = @id_cultivo;
END
GO

-- Actualiza mascota y peso
CREATE OR ALTER PROCEDURE spActualizarMascota
    @id_mascota INT,
    @nombre VARCHAR(200),
    @especie VARCHAR(100),
    @raza VARCHAR(200) = NULL,
    @fecha_nac DATE,
    @peso DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE mascotas
    SET nombre = @nombre,
        especie = @especie,
        raza = @raza,
        fecha_nac = @fecha_nac,
        peso_kg = @peso
    WHERE id_mascota = @id_mascota;
END
GO

-- Actualiza vehiculo y poliza
CREATE OR ALTER PROCEDURE spActualizarVehiculo
    @id_vehiculo INT,
    @placa VARCHAR(50),
    @marca VARCHAR(100),
    @modelo VARCHAR(100),
    @year INT,
    @poliza VARCHAR(200) = NULL,
    @dekra DATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE vehiculos
    SET placa = @placa,
        marca = @marca,
        modelo = @modelo,
        anio = @year,
        poliza = @poliza,
        dekra_fecha = @dekra
    WHERE id_vehiculo = @id_vehiculo;
END
GO

-- Actualiza lista y metadatos
CREATE OR ALTER PROCEDURE spActualizarLista
    @id_lista INT,
    @nombre VARCHAR(200),
    @tipo VARCHAR(100),
    @id_area INT = NULL,
    @creada_por INT = NULL,
    @fecha_creada DATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE listas
    SET nombre = @nombre,
        tipo = @tipo,
        id_area = @id_area,
        creada_por = @creada_por,
        fecha_creada = @fecha_creada
    WHERE id_lista = @id_lista;
END
GO

-- Actualiza tarea con sus fechas y estado
CREATE OR ALTER PROCEDURE spActualizarTarea
    @id_tarea INT,
    @id_lista INT,
    @titulo VARCHAR(200),
    @descripcion VARCHAR(MAX) = NULL,
    @prioridad VARCHAR(50),
    @estado VARCHAR(50),
    @fecha_creacion DATE,
    @fecha_limite DATE = NULL,
    @repeticion VARCHAR(50),
    @id_area INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tareas
    SET id_lista = @id_lista,
        titulo = @titulo,
        descripcion = @descripcion,
        prioridad = @prioridad,
        estado = @estado,
        fecha_creacion = @fecha_creacion,
        fecha_limite = @fecha_limite,
        repeticion = @repeticion,
        id_area = @id_area
    WHERE id_tarea = @id_tarea;
END
GO

-- Actualiza asignacion de miembro para tarea
CREATE OR ALTER PROCEDURE spActualizarAsignacionTarea
    @id_tarea INT,
    @id_miembro INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tareas_asignaciones
    SET id_miembro = @id_miembro
    WHERE id_tarea = @id_tarea;
END
GO

-- Actualiza evento existente
CREATE OR ALTER PROCEDURE spActualizarEvento
    @id_evento INT,
    @tipo VARCHAR(50),
    @titulo VARCHAR(200),
    @fecha DATETIME2,
    @lugar VARCHAR(200) = NULL,
    @notas VARCHAR(MAX) = NULL,
    @id_miembro INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE eventos
    SET tipo = @tipo,
        titulo = @titulo,
        fecha_hora = @fecha,
        lugar = @lugar,
        notas = @notas,
        id_miembro = @id_miembro
    WHERE id_evento = @id_evento;
END
GO

-- Actualiza factura registrada
CREATE OR ALTER PROCEDURE spActualizarFactura
    @id_factura INT,
    @id_proveedor INT,
    @monto DECIMAL(18,2),
    @id_categoria INT,
    @fecha_emision DATETIME2,
    @fecha_vencimiento DATETIME2,
    @estado VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE facturas
    SET id_proveedor = @id_proveedor,
        monto = @monto,
        categoria_id = @id_categoria,
        fecha_emision = @fecha_emision,
        fecha_venc = @fecha_vencimiento,
        estado = @estado
    WHERE id_factura = @id_factura;
END
GO

-- Actualiza presupuesto mensual
CREATE OR ALTER PROCEDURE spActualizarPresupuesto
    @id_presupuesto INT,
    @anio INT,
    @mes VARCHAR(20),
    @id_categoria INT,
    @monto_planeado DECIMAL(18,2),
    @monto_ejecutado DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE presupuestos
    SET anio = @anio,
        mes = @mes,
        id_categoria = @id_categoria,
        monto_planeado = @monto_planeado,
        monto_ejecutado = @monto_ejecutado
    WHERE id_presupuesto = @id_presupuesto;
END
GO

-- Actualiza salario y deducciones
CREATE OR ALTER PROCEDURE spActualizarSalario
    @id_salario INT,
    @id_miembro INT,
    @monto DECIMAL(18,2),
    @periodicidad VARCHAR(50),
    @deducciones DECIMAL(18,2),
    @fecha_inicio DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE salarios
    SET id_miembro = @id_miembro,
        monto = @monto,
        periodicidad = @periodicidad,
        deducciones = @deducciones,
        fecha_inicio = @fecha_inicio
    WHERE id_salario = @id_salario;
END
GO

-- Actualiza movimiento financiero
CREATE OR ALTER PROCEDURE spActualizarMovimiento
    @id_mov INT,
    @fecha DATETIME2,
    @tipo VARCHAR(50),
    @id_categoria INT,
    @monto DECIMAL(18,2),
    @referencia VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE movimientos
    SET fecha = @fecha,
        tipo = @tipo,
        id_categoria = @id_categoria,
        monto = @monto,
        referencia = @referencia
    WHERE id_mov = @id_mov;
END
GO

-- Actualiza siembra y su cosecha estimada
CREATE OR ALTER PROCEDURE spActualizarSiembra
    @id_siembra INT,
    @id_cultivo INT,
    @fecha_siembra DATETIME2,
    @fecha_estim_cosecha DATETIME2 = NULL,
    @sector VARCHAR(100),
    @notas VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE siembras
    SET id_cultivo = @id_cultivo,
        fecha_siembra = @fecha_siembra,
        fecha_estim_cosecha = @fecha_estim_cosecha,
        sector = @sector,
        notas = @notas
    WHERE id_siembra = @id_siembra;
END
GO

-- Actualiza tratamiento aplicado
CREATE OR ALTER PROCEDURE spActualizarTratamiento
    @id_tratamiento INT,
    @id_siembra INT,
    @fecha DATETIME2,
    @producto VARCHAR(200),
    @dosis VARCHAR(100),
    @notas VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tratamientos
    SET id_siembra = @id_siembra,
        fecha = @fecha,
        producto = @producto,
        dosis = @dosis,
        notas = @notas
    WHERE id_tratamiento = @id_tratamiento;
END
GO

-- Actualiza item de inventario de jardin
CREATE OR ALTER PROCEDURE spActualizarInventarioJardin
    @id_item INT,
    @nombre VARCHAR(200),
    @tipo VARCHAR(100),
    @cantidad DECIMAL(18,2),
    @unidad VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE inventario_jardin
    SET nombre = @nombre,
        tipo = @tipo,
        cantidad = @cantidad,
        unidad = @unidad
    WHERE id_item = @id_item;
END
GO

-- Actualiza mantenimiento registrado de un vehiculo
CREATE OR ALTER PROCEDURE spActualizarMantenimientoVehiculo
    @id_mant INT,
    @id_vehiculo INT,
    @tipo VARCHAR(50),
    @concepto VARCHAR(200),
    @fecha DATETIME2,
    @kilometraje INT = NULL,
    @costo DECIMAL(18,2),
    @taller VARCHAR(200) = NULL,
    @notas VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE vehiculos_mantenimientos
    SET id_vehiculo = @id_vehiculo,
        tipo = @tipo,
        concepto = @concepto,
        fecha = @fecha,
        kilometraje = @kilometraje,
        costo = @costo,
        taller = @taller,
        notas = @notas
    WHERE id_mant = @id_mant;
END
GO
