CREATE TABLE evento(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(150) NOT NULL,
    descricao TEXT,
    localizacao GEOMETRY(POINT, 4326) NOT NULL,
    data_inicio TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    status status_evento DEFAULT NOT NULL 'Ativo' 
);
