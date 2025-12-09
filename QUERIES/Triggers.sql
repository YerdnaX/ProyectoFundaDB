USE domus_hogar;
GO

/* Tablas auxiliares para auditoria y resumenes */
IF OBJECT_ID('log_areas', 'U') IS NULL
BEGIN
    CREATE TABLE log_areas (
        id_log INT IDENTITY(1,1) PRIMARY KEY,
        accion VARCHAR(10) NOT NULL,
        id_area INT NULL,
        nombre VARCHAR(80) NULL,
        detalle VARCHAR(200) NULL,
        fecha_registro DATETIME2 NOT NULL DEFAULT SYSDATETIME()
    );
END
GO

IF OBJECT_ID('movimientos_resume', 'U') IS NULL
BEGIN
    CREATE TABLE movimientos_resumen (
        categoria_id INT NOT NULL,
        tipo VARCHAR(10) NOT NULL,
        total_monto DECIMAL(18,2) NOT NULL,
        cantidad INT NOT NULL,
        PRIMARY KEY (categoria_id, tipo)
    );
END
GO

IF OBJECT_ID('vehiculos_mant_resume', 'U') IS NULL
BEGIN
    CREATE TABLE vehiculos_mant_resumen (
        id_vehiculo INT PRIMARY KEY,
        total_mantenimientos INT NOT NULL,
        ultima_fecha DATE NULL
    );
END
GO

/* 1. Auditoria de areas */
/* Cada vez que se inserta o elimina un area se guarda un registro en log_areas */
CREATE OR ALTER TRIGGER trg_areas_audit
ON areas
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO log_areas (accion, id_area, nombre, detalle)
    SELECT 'INSERT', i.id_area, i.nombre, i.detalle
    FROM inserted i;

    INSERT INTO log_areas (accion, id_area, nombre, detalle)
    SELECT 'DELETE', d.id_area, d.nombre, d.detalle
    FROM deleted d;
END
GO

/* 2. Validar fechas futuras en eventos */
/* Evita que se registren o actualicen eventos importantes con fechas en el pasado. */
CREATE OR ALTER TRIGGER trg_eventos_fecha_futura
ON eventos
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.fecha_hora < SYSDATETIME()
          AND i.tipo IN ('CITA_MEDICA', 'CUMPLE', 'ANIVERSARIO', 'UNIVERSIDAD')
    )
    BEGIN
        RAISERROR('La fecha del evento debe ser futura.',16,1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

/* 3. Ajustar estado de facturas vencidas */
/* Marca automáticamente como VENCIDA cualquier factura cuya fecha de vencimiento ya haya pasado y no esté pagada. */
CREATE OR ALTER TRIGGER trg_facturas_estado
ON facturas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE f
    SET estado = 'VENCIDA'
    FROM facturas f
    JOIN inserted i ON f.id_factura = i.id_factura
    WHERE f.estado <> 'PAGADA'
      AND f.fecha_venc < CONVERT(date, SYSDATETIME());
END
GO

/* 4. Evitar presupuestos duplicados por (anio, mes, id_categoria) */
/* Impide que se creen presupuestos duplicados para la misma categoría, mes y año. */
CREATE OR ALTER TRIGGER trg_presupuestos_unicos
ON presupuestos
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN presupuestos p
          ON p.anio = i.anio
         AND p.mes = i.mes
         AND p.id_categoria = i.id_categoria
         AND p.id_presupuesto <> i.id_presupuesto
    )
    BEGIN
        RAISERROR('Ya existe un presupuesto para ese año, mes y categoría.',16,1);
        ROLLBACK TRANSACTION;
    END
END
GO

/* 5. Mantener resumen por categoria/tipo en movimientos */
/* Actualiza automáticamente el resumen de totales y cantidades de movimientos por categoría y tipo. */
CREATE OR ALTER TRIGGER trg_movimientos_resumen
ON movimientos
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH cambios AS (
        SELECT id_categoria AS categoria_id, tipo FROM inserted
        UNION
        SELECT id_categoria AS categoria_id, tipo FROM deleted
    )
    MERGE movimientos_resumen AS tgt
    USING (
        SELECT m.id_categoria AS categoria_id, m.tipo, SUM(m.monto) AS total_monto, COUNT(*) AS cantidad
        FROM movimientos m
        JOIN cambios c ON c.categoria_id = m.id_categoria AND c.tipo = m.tipo
        GROUP BY m.id_categoria, m.tipo
    ) AS src
    ON tgt.categoria_id = src.categoria_id AND tgt.tipo = src.tipo
    WHEN MATCHED THEN
        UPDATE SET tgt.total_monto = src.total_monto, tgt.cantidad = src.cantidad
    WHEN NOT MATCHED THEN
        INSERT (categoria_id, tipo, total_monto, cantidad)
        VALUES (src.categoria_id, src.tipo, src.total_monto, src.cantidad)
    WHEN NOT MATCHED BY SOURCE AND EXISTS (SELECT 1 FROM cambios c WHERE c.categoria_id = tgt.categoria_id AND c.tipo = tgt.tipo) THEN
        DELETE;
END
GO

/* 6. Validar fechas en tareas */
/* Evita que una tarea tenga una fecha límite anterior a su fecha de creación. */
CREATE OR ALTER TRIGGER trg_tareas_fechas
ON tareas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.fecha_limite IS NOT NULL
          AND i.fecha_limite < i.fecha_creacion
    )
    BEGIN
        RAISERROR('La fecha límite no puede ser menor que la fecha de creación.',16,1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

/* 7. Evitar tareas duplicadas */
/* Impide crear/actualizar tareas con el mismo titulo en la misma lista. */
CREATE OR ALTER TRIGGER trg_tareas_unicas
ON tareas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN tareas t
          ON t.id_lista = i.id_lista
         AND t.titulo = i.titulo
         AND t.id_tarea <> i.id_tarea
    )
    OR EXISTS (
        SELECT 1
        FROM inserted a
        JOIN inserted b
          ON a.id_lista = b.id_lista
         AND a.titulo = b.titulo
         AND a.id_tarea <> b.id_tarea
    )
    BEGIN
        RAISERROR('Ya existe una tarea con ese titulo en la lista.',16,1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO
/* 8. Resumen de mantenimientos por vehculo */
/* Mantiene actualizado el total de mantenimientos y la fecha del último mantenimiento por vehículo. */
CREATE OR ALTER TRIGGER trg_mant_vehiculo_resumen
ON vehiculos_mantenimientos
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH cambios AS (
        SELECT id_vehiculo FROM inserted
        UNION
        SELECT id_vehiculo FROM deleted
    )
    MERGE vehiculos_mant_resumen AS tgt
    USING (
        SELECT vm.id_vehiculo,
               COUNT(*) AS total_mantenimientos,
               MAX(vm.fecha) AS ultima_fecha
        FROM vehiculos_mantenimientos vm
        JOIN cambios c ON c.id_vehiculo = vm.id_vehiculo
        GROUP BY vm.id_vehiculo
    ) AS src
    ON tgt.id_vehiculo = src.id_vehiculo
    WHEN MATCHED THEN
        UPDATE SET tgt.total_mantenimientos = src.total_mantenimientos,
                   tgt.ultima_fecha = src.ultima_fecha
    WHEN NOT MATCHED THEN
        INSERT (id_vehiculo, total_mantenimientos, ultima_fecha)
        VALUES (src.id_vehiculo, src.total_mantenimientos, src.ultima_fecha)
    WHEN NOT MATCHED BY SOURCE AND EXISTS (SELECT 1 FROM cambios c WHERE c.id_vehiculo = tgt.id_vehiculo) THEN
        DELETE;
END
GO

/* 9. Validar fechas en medicamentos de mascotas */
/* Evita que los medicamentos de mascotas tengan fecha de finalización anterior a la de inicio. */
CREATE OR ALTER TRIGGER trg_mascotas_meds_fechas
ON mascotas_meds
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.fecha_fin IS NOT NULL
          AND i.fecha_fin < i.fecha_ini
    )
    BEGIN
        RAISERROR('La fecha fin del medicamento no puede ser menor que la fecha inicio.',16,1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

/* 10. Validar fechas en siembra */
/* Impide registrar una fecha estimada de cosecha anterior a la fecha de siembra. */
CREATE OR ALTER TRIGGER trg_siembras_fechas
ON siembras
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.fecha_estim_cosecha IS NOT NULL
          AND i.fecha_estim_cosecha < i.fecha_siembra
    )
    BEGIN
        RAISERROR('La fecha estimada de cosecha no puede ser menor que la fecha de siembra.',16,1);
        ROLLBACK TRANSACTION;
    END
END
GO


