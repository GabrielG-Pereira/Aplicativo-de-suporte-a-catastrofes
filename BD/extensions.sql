CREATE EXTENSION IF NOT EXISTS postgis;

CREATE TYPE status_evento AS ENUM (
'Ativo', 
'Encerrado', 
'Alerta Crítico'
);

CREATE TYPE status_voluntario_evento AS ENUM(
    'A caminho',
    'No local',
    'Finalizado',
    'Cancelado'
);

CREATE TYPE tipo_usuario AS ENUM(
    'Gestor',
    'Voluntario'
);
