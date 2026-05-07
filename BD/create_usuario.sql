CREATE TABLE usuario(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(150) NOT NULL,
    email VARCHAR(150) NOT NULL,
    senha VARCHAR(255) NOT NULL,
    contato VARCHAR(50) NOT NULL,
    tipo tipo_usuario NOT NULL
);
