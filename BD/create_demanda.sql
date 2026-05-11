CREATE TABLE demanda(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_ponto_evento UUID NOT NULL,
    categoria UUID NOT NULL,
    descricao TEXT,
    status status_demanda NOT NULL,

    CONSTRAINT fk_ponto_evento FOREIGN KEY (id_ponto_evento)
        REFERENCES ponto_evento(id) ON DELETE CASCADE,

    CONSTRAINT fk_categoria FOREIGN KEY (categoria)
        REFERENCES categoria_demanda(id) ON DELETE CASCADE
);
