-- Script manual para cambiar CuentaContableEtiqueta de nvarchar a bit

-- Paso 1: Eliminar la columna antigua (esto perderá los datos existentes)
ALTER TABLE [MOV_ACTIVO] DROP COLUMN [CuentaContableEtiqueta];

-- Paso 2: Agregar la columna nueva como bit con valor por defecto false
ALTER TABLE [MOV_ACTIVO] ADD [CuentaContableEtiqueta] bit NOT NULL DEFAULT 0;
