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
GO

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
GO 

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
GO

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
GO

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
GO

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
GO

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
GO
     --VISTA 8
CREATE VIEW TOTALFACTURASPAGAR AS
    SELECT 
        COUNT(*) as [Cantidad de facturas],
        SUM(F.monto) as [Monto total pendiente]
    FROM facturas F
    WHERE F.estado IN ('PENDIENTE', 'VENCIDA');
GO

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
GO

    --VISTA 10
CREATE VIEW RESUMENVEHICULOS AS
    SELECT 
        V.placa,
        V.marca,
        V.modelo,
        V.anio,
        V.dekra_fecha,
        DATEDIFF(DAY, GETDATE(), V.dekra_fecha) as [Dias para DEKRA],
        COUNT(VM.id_mant) as [Total Mantenimientos],
        MAX(VM.fecha) as [Ultimo Mantenimiento]
    FROM vehiculos V
    LEFT JOIN vehiculos_mantenimientos VM ON V.id_vehiculo = VM.id_vehiculo
    GROUP BY V.id_vehiculo, V.placa, V.marca, V.modelo, V.anio, V.dekra_fecha;
GO