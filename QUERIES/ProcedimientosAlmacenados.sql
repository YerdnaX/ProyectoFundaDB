USE domus_hogar;
GO

-- Asignaciones de botones
CREATE OR ALTER PROCEDURE dbo.spGuardarAsignacionBoton
    @NumeroBoton INT,
    @IdMiembro INT,
    @RutaImagen NVARCHAR(MAX),
    @NombreImagen NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM AsignacionesBotones WHERE NumeroBoton = @NumeroBoton;
    INSERT INTO AsignacionesBotones (NumeroBoton, IdMiembro, RutaImagen, NombreImagen)
    VALUES (@NumeroBoton, @IdMiembro, @RutaImagen, @NombreImagen);
END
GO

-- Inserciones
CREATE OR ALTER PROCEDURE dbo.spInsertarArea
    @nombre NVARCHAR(150),
    @detalle NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO areas (nombre, detalle) VALUES (@nombre, @detalle);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarCategoriaFinanzas
    @nombre NVARCHAR(150),
    @tipo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO categorias_finanzas (nombre, tipo) VALUES (@nombre, @tipo);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarProveedor
    @nombre NVARCHAR(200),
    @tipo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO proveedores (nombre, tipo) VALUES (@nombre, @tipo);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarCultivo
    @nombre NVARCHAR(200),
    @variedad NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cultivos (nombre, variedad) VALUES (@nombre, @variedad);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarMascota
    @nombre NVARCHAR(200),
    @especie NVARCHAR(100),
    @raza NVARCHAR(200) = NULL,
    @fecha_nac DATE,
    @peso DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO mascotas (nombre, especie, raza, fecha_nac, peso_kg)
    VALUES (@nombre, @especie, @raza, @fecha_nac, @peso);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarVehiculo
    @placa NVARCHAR(50),
    @marca NVARCHAR(100),
    @modelo NVARCHAR(100),
    @year INT,
    @poliza NVARCHAR(200) = NULL,
    @dekra DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO vehiculos (placa, marca, modelo, anio, poliza, dekra_fecha)
    VALUES (@placa, @marca, @modelo, @year, @poliza, @dekra);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarLista
    @nombre NVARCHAR(200),
    @tipo NVARCHAR(100),
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

CREATE OR ALTER PROCEDURE dbo.spInsertarTarea
    @id_lista INT,
    @titulo NVARCHAR(200),
    @descripcion NVARCHAR(MAX) = NULL,
    @prioridad NVARCHAR(50),
    @estado NVARCHAR(50),
    @fecha_creacion DATE,
    @fecha_limite DATE = NULL,
    @repeticion NVARCHAR(50),
    @id_area INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tareas (id_lista, titulo, descripcion, prioridad, estado, fecha_creacion, fecha_limite, repeticion, id_area)
    VALUES (@id_lista, @titulo, @descripcion, @prioridad, @estado, @fecha_creacion, @fecha_limite, @repeticion, @id_area);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarAsignacionTarea
    @id_tarea INT,
    @id_miembro INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tareas_asignaciones (id_tarea, id_miembro)
    VALUES (@id_tarea, @id_miembro);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarEvento
    @tipo NVARCHAR(50),
    @titulo NVARCHAR(200),
    @fecha DATETIME2,
    @lugar NVARCHAR(200) = NULL,
    @notas NVARCHAR(MAX) = NULL,
    @id_miembro INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO eventos (tipo, titulo, fecha_hora, lugar, notas, id_miembro)
    VALUES (@tipo, @titulo, @fecha, @lugar, @notas, @id_miembro);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarFactura
    @id_proveedor INT,
    @monto DECIMAL(18,2),
    @id_categoria INT,
    @fecha_emision DATETIME2,
    @fecha_vencimiento DATETIME2,
    @estado NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO facturas (id_proveedor, monto, categoria_id, fecha_emision, fecha_venc, estado)
    VALUES (@id_proveedor, @monto, @id_categoria, @fecha_emision, @fecha_vencimiento, @estado);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarPresupuesto
    @anio INT,
    @mes NVARCHAR(20),
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

CREATE OR ALTER PROCEDURE dbo.spInsertarSalario
    @id_miembro INT,
    @monto DECIMAL(18,2),
    @periodicidad NVARCHAR(50),
    @deducciones DECIMAL(18,2),
    @fecha_inicio DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO salarios (id_miembro, monto, periodicidad, deducciones, fecha_inicio)
    VALUES (@id_miembro, @monto, @periodicidad, @deducciones, @fecha_inicio);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarMovimiento
    @fecha DATETIME2,
    @tipo NVARCHAR(50),
    @id_categoria INT,
    @monto DECIMAL(18,2),
    @referencia NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO movimientos (fecha, tipo, id_categoria, monto, referencia)
    VALUES (@fecha, @tipo, @id_categoria, @monto, @referencia);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarSiembra
    @id_cultivo INT,
    @fecha_siembra DATETIME2,
    @fecha_estimada DATETIME2 = NULL,
    @sector NVARCHAR(100),
    @notas NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO siembras (id_cultivo, fecha_siembra, fecha_estim_cosecha, sector, notas)
    VALUES (@id_cultivo, @fecha_siembra, @fecha_estimada, @sector, @notas);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarTratamiento
    @id_siembra INT,
    @fecha DATETIME2,
    @producto NVARCHAR(200),
    @dosis NVARCHAR(100),
    @notas NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tratamientos (id_siembra, fecha, producto, dosis, notas)
    VALUES (@id_siembra, @fecha, @producto, @dosis, @notas);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarInventarioJardin
    @nombre NVARCHAR(200),
    @tipo NVARCHAR(100),
    @cantidad DECIMAL(18,2),
    @unidad NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO inventario_jardin (nombre, tipo, cantidad, unidad)
    VALUES (@nombre, @tipo, @cantidad, @unidad);
END
GO

CREATE OR ALTER PROCEDURE dbo.spInsertarMantenimientoVehiculo
    @id_vehiculo INT,
    @tipo NVARCHAR(50),
    @concepto NVARCHAR(200),
    @fecha DATETIME2,
    @kilometraje INT = NULL,
    @costo DECIMAL(18,2),
    @taller NVARCHAR(200) = NULL,
    @notas NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO vehiculos_mantenimientos (id_vehiculo, tipo, concepto, fecha, kilometraje, costo, taller, notas)
    VALUES (@id_vehiculo, @tipo, @concepto, @fecha, @kilometraje, @costo, @taller, @notas);
END
GO

-- Actualizaciones
CREATE OR ALTER PROCEDURE dbo.spActualizarArea
    @id_area INT,
    @nombre NVARCHAR(150),
    @detalle NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE areas SET nombre = @nombre, detalle = @detalle
    WHERE id_area = @id_area;
END
GO

CREATE OR ALTER PROCEDURE dbo.spActualizarCategoriaFinanzas
    @id_categoria INT,
    @nombre NVARCHAR(150),
    @tipo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE categorias_finanzas SET nombre = @nombre, tipo = @tipo
    WHERE id_categoria = @id_categoria;
END
GO

CREATE OR ALTER PROCEDURE dbo.spActualizarProveedor
    @id_proveedor INT,
    @nombre NVARCHAR(200),
    @tipo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE proveedores SET nombre = @nombre, tipo = @tipo
    WHERE id_proveedor = @id_proveedor;
END
GO

CREATE OR ALTER PROCEDURE dbo.spActualizarCultivo
    @id_cultivo INT,
    @nombre NVARCHAR(200),
    @variedad NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cultivos SET nombre = @nombre, variedad = @variedad
    WHERE id_cultivo = @id_cultivo;
END
GO

CREATE OR ALTER PROCEDURE dbo.spActualizarMascota
    @id_mascota INT,
    @nombre NVARCHAR(200),
    @especie NVARCHAR(100),
    @raza NVARCHAR(200) = NULL,
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

CREATE OR ALTER PROCEDURE dbo.spActualizarVehiculo
    @id_vehiculo INT,
    @placa NVARCHAR(50),
    @marca NVARCHAR(100),
    @modelo NVARCHAR(100),
    @year INT,
    @poliza NVARCHAR(200) = NULL,
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

CREATE OR ALTER PROCEDURE dbo.spActualizarLista
    @id_lista INT,
    @nombre NVARCHAR(200),
    @tipo NVARCHAR(100),
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

CREATE OR ALTER PROCEDURE dbo.spActualizarTarea
    @id_tarea INT,
    @id_lista INT,
    @titulo NVARCHAR(200),
    @descripcion NVARCHAR(MAX) = NULL,
    @prioridad NVARCHAR(50),
    @estado NVARCHAR(50),
    @fecha_creacion DATE,
    @fecha_limite DATE = NULL,
    @repeticion NVARCHAR(50),
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

CREATE OR ALTER PROCEDURE dbo.spActualizarAsignacionTarea
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

CREATE OR ALTER PROCEDURE dbo.spActualizarEvento
    @id_evento INT,
    @tipo NVARCHAR(50),
    @titulo NVARCHAR(200),
    @fecha DATETIME2,
    @lugar NVARCHAR(200) = NULL,
    @notas NVARCHAR(MAX) = NULL,
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

CREATE OR ALTER PROCEDURE dbo.spActualizarFactura
    @id_factura INT,
    @id_proveedor INT,
    @monto DECIMAL(18,2),
    @id_categoria INT,
    @fecha_emision DATETIME2,
    @fecha_vencimiento DATETIME2,
    @estado NVARCHAR(50)
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

CREATE OR ALTER PROCEDURE dbo.spActualizarPresupuesto
    @id_presupuesto INT,
    @anio INT,
    @mes NVARCHAR(20),
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

CREATE OR ALTER PROCEDURE dbo.spActualizarSalario
    @id_salario INT,
    @id_miembro INT,
    @monto DECIMAL(18,2),
    @periodicidad NVARCHAR(50),
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

CREATE OR ALTER PROCEDURE dbo.spActualizarMovimiento
    @id_mov INT,
    @fecha DATETIME2,
    @tipo NVARCHAR(50),
    @id_categoria INT,
    @monto DECIMAL(18,2),
    @referencia NVARCHAR(MAX) = NULL
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

CREATE OR ALTER PROCEDURE dbo.spActualizarSiembra
    @id_siembra INT,
    @id_cultivo INT,
    @fecha_siembra DATETIME2,
    @fecha_estim_cosecha DATETIME2 = NULL,
    @sector NVARCHAR(100),
    @notas NVARCHAR(MAX) = NULL
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

CREATE OR ALTER PROCEDURE dbo.spActualizarTratamiento
    @id_tratamiento INT,
    @id_siembra INT,
    @fecha DATETIME2,
    @producto NVARCHAR(200),
    @dosis NVARCHAR(100),
    @notas NVARCHAR(MAX) = NULL
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

CREATE OR ALTER PROCEDURE dbo.spActualizarInventarioJardin
    @id_item INT,
    @nombre NVARCHAR(200),
    @tipo NVARCHAR(100),
    @cantidad DECIMAL(18,2),
    @unidad NVARCHAR(50)
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

CREATE OR ALTER PROCEDURE dbo.spActualizarMantenimientoVehiculo
    @id_mant INT,
    @id_vehiculo INT,
    @tipo NVARCHAR(50),
    @concepto NVARCHAR(200),
    @fecha DATETIME2,
    @kilometraje INT = NULL,
    @costo DECIMAL(18,2),
    @taller NVARCHAR(200) = NULL,
    @notas NVARCHAR(MAX) = NULL
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
