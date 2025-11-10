-- Script para resetear el IDENTITY de la tabla CAT_USUARIO
-- Esto hará que los próximos IDs empiecen desde el siguiente número disponible
-- basado en el máximo ID actual en la tabla

-- IMPORTANTE: Ejecuta esto solo si quieres resetear el contador de IDENTITY
-- Esto NO afecta los datos existentes, solo cambia el próximo número que se generará

USE GestionActivosDb;
GO

-- Opción 1: Resetear al siguiente número basado en el máximo ID actual
DECLARE @MaxId INT;
SELECT @MaxId = ISNULL(MAX(IdUsuario), 0) FROM CAT_USUARIO;
DBCC CHECKIDENT ('CAT_USUARIO', RESEED, @MaxId);
GO

-- Opción 2: Si quieres que empiece desde 1 (y no tienes datos importantes)
-- DBCC CHECKIDENT ('CAT_USUARIO', RESEED, 0);
-- GO

-- Verificar el próximo ID que se generará
DBCC CHECKIDENT ('CAT_USUARIO', NORESEED);
GO

