CREATE TABLE ponto_evento(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_evento UUID NOT NULL,
    id_ponto UUID NOT NULL,
    status BOOLEAN,

    CONSTRAINT fk_evento FOREIGN KEY (id_evento)
        REFERENCES evento(id) ON DELETE CASCADE,

    CONSTRAINT fk_ponto FOREIGN KEY (id_ponto)
        REFERENCES ponto_coleta(id) ON DELETE CASCADE
);
