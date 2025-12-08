USE domus_hogar;
GO

CREATE PROCEDURE Gasto_Mensual
AS
BEGIN
    SET NOCOUNT ON;
    WITH gastos AS (
        SELECT YEAR(fecha) AS anio,
               MONTH(fecha) AS mes,
               SUM(CASE WHEN tipo = 'EGRESO' THEN monto ELSE 0 END) AS egreso
        FROM movimientos
        GROUP BY YEAR(fecha), MONTH(fecha)
    ),
    gastos_prev AS (
        SELECT *,
               LAG(egreso) OVER (ORDER BY anio, mes) AS egresoPrevio,
               AVG(egreso) OVER (ORDER BY anio, mes ROWS BETWEEN 2 PRECEDING AND CURRENT ROW) AS egresoMa3
        FROM gastos
    )
    SELECT anio,
           mes,
           egreso,
           egresoPrevio,
           CASE WHEN egresoPrevio IS NULL OR egresoPrevio = 0 THEN NULL ELSE (egreso - egresoPrevio) / egresoPrevio END AS egresoMoM,
           egresoMa3 AS egresoPronostico
    FROM gastos_prev
    ORDER BY anio, mes;
END;
GO