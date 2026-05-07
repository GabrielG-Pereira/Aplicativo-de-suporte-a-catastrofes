CREATE TABLE voluntario(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_usuario UUID NOT NULL,
    especialidade UUID NOT NULL,
    localizacao GEOMETRY (POINT, 4326) NOT NULL,
    disponivel BOOLEAN NOT NULL,

    CONSTRAINT fk_usuario FOREIGN KEY (id_usuario)
        REFERENCES usuario(id) ON DELETE CASCADE,
    
    CONSTRAINT fk_especialidade FOREIGN KEY (especialidade)
        REFERENCES especialidade_voluntario(id)
);
