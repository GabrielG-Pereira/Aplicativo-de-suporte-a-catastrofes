CREATE TABLE voluntario(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_usuario UUID NOT NULL,
    especialidade UUID NOT NULL,
    localicao GEOMETRY (POINT, 4326) NOT NULL,
    disponivel BOOLEAN NOT NULL

    CONSTRAINT fk
);
