CREATE TABLE especialidade_voluntario(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    categoria UUID NOT NULL,
    nome VARCHAR(100) NOT NULL,
    
    CONSTRAINT fk_categoria FOREIGN KEY (categoria)
        REFERENCES categoria_especialidade(id) ON DELETE CASCADE
);
