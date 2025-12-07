USE domus_hogar;
GO

/* Tablas auxiliares para auditoria y resúmenes */
IF OBJECT_ID('dbo.log_miembros', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.log_miembros (
        id_log INT IDENTITY(1,1) PRIMARY KEY,
        accion NVARCHAR(10) NOT NULL,
        id_miembro INT NULL,
        nombre NVARCHAR(80) NULL,
        apellido NVARCHAR(80) NULL,
        rol NVARCHAR(50) NULL,
        fecha_registro DATETIME2 NOT NULL DEFAULT SYSDATETIME()
    );
END
GO

IF OBJECT_ID('dbo.movimientos_resumen', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.movimientos_resumen (
        categoria_id INT NOT NULL,
        tipo NVARCHAR(10) NOT NULL,
        total_monto DECIMAL(18,2) NOT NULL,
        cantidad INT NOT NULL,
        PRIMARY KEY (categoria_id, tipo)
    );
END
GO

IF OBJECT_ID('dbo.vehiculos_mant_resumen', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.vehiculos_mant_resumen (
        id_vehiculo INT PRIMARY KEY,
        total_mantenimientos INT NOT NULL,
        ultima_fecha DATE NULL
    );
END
GO

/* 1. Auditoría de miembros */
CREATE OR ALTER TRIGGER trg_miembros_audit
ON dbo.miembros
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.log_miembros (accion, id_miembro, nombre, apellido, rol)
    SELECT 'INSERT', i.id_miembro, i.nombre, i.apellido, i.rol
    FROM inserted i;

    INSERT INTO dbo.log_miembros (accion, id_miembro, nombre, apellido, rol)
    SELECT 'DELETE', d.id_miembro, d.nombre, d.apellido, d.rol
    FROM deleted d;
END
GO

/* 2. Validar fechas futuras en eventos */
CREATE OR ALTER TRIGGER trg_eventos_fecha_futura
ON dbo.eventos
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.fecha_hora < SYSDATETIME()
          AND i.tipo IN (N'CITA_MEDICA', N'CUMPLE', N'ANIVERSARIO', N'UNIVERSIDAD')
    )
    BEGIN
        RAISERROR('La fecha del evento debe ser futura.',16,1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

/* 3. Ajustar estado de facturas vencidas */
CREATE OR ALTER TRIGGER trg_facturas_estado
ON dbo.facturas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE f
    SET estado = N'VENCIDA'
    FROM dbo.facturas f
    JOIN inserted i ON f.id_factura = i.id_factura
    WHERE f.estado <> N'PAGADA'
      AND f.fecha_venc < CONVERT(date, SYSDATETIME());
END
GO

/* 4. Evitar presupuestos duplicados por (anio, mes, id_categoria) */
CREATE OR ALTER TRIGGER trg_presupuestos_unicos
ON dbo.presupuestos
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN dbo.presupuestos p
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

/* 5. Mantener resumen por categoría/tipo en movimientos */
CREATE OR ALTER TRIGGER trg_movimientos_resumen
ON dbo.movimientos
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH cambios AS (
        SELECT id_categoria AS categoria_id, tipo FROM inserted
        UNION
        SELECT id_categoria AS categoria_id, tipo FROM deleted
    )
    MERGE dbo.movimientos_resumen AS tgt
    USING (
        SELECT m.id_categoria AS categoria_id, m.tipo, SUM(m.monto) AS total_monto, COUNT(*) AS cantidad
        FROM dbo.movimientos m
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
CREATE OR ALTER TRIGGER trg_tareas_fechas
ON dbo.tareas
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

/* 7. Evitar asignaciones duplicadas de tareas */
CREATE OR ALTER TRIGGER trg_tareas_asignaciones_unicas
ON dbo.tareas_asignaciones
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN dbo.tareas_asignaciones ta
          ON ta.id_tarea = i.id_tarea AND ta.id_miembro = i.id_miembro
    )
    BEGIN
        RAISERROR('La tarea ya está asignada a ese miembro.',16,1);
        RETURN;
    END

    INSERT INTO dbo.tareas_asignaciones (id_tarea, id_miembro)
    SELECT id_tarea, id_miembro FROM inserted;
END
GO

/* 8. Resumen de mantenimientos por vehículo */
CREATE OR ALTER TRIGGER trg_mant_vehiculo_resumen
ON dbo.vehiculos_mantenimientos
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH cambios AS (
        SELECT id_vehiculo FROM inserted
        UNION
        SELECT id_vehiculo FROM deleted
    )
    MERGE dbo.vehiculos_mant_resumen AS tgt
    USING (
        SELECT vm.id_vehiculo,
               COUNT(*) AS total_mantenimientos,
               MAX(vm.fecha) AS ultima_fecha
        FROM dbo.vehiculos_mantenimientos vm
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
CREATE OR ALTER TRIGGER trg_mascotas_meds_fechas
ON dbo.mascotas_meds
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

/* 10. Validar fechas en siembras */
CREATE OR ALTER TRIGGER trg_siembras_fechas
ON dbo.siembras
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
