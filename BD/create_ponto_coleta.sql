CREATE TABLE ponto_coleta(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(150) NOT NULL,
    endereco VARCHAR(150) NOT NULL,    
    localizacao GEOMETRY(POINT, 4326) NOT NULL,
    contato VARCHAR(50) NOT NULL,
    temporario BOOLEAN NOT NULL
);
