CREATE TABLE voluntario_evento(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_evento UUID NOT NULL,
    id_voluntario UUID NOT NULL,
    status status_voluntario_evento DEFAULT 'A caminho' NOT NULL,
    data_aceito TIMESTAMP NOT NULL,
    data_checkin TIMESTAMP,
    data_checkout TIMESTAMP,

    CONSTRAINT fk_evento FOREIGN KEY (id_evento)
        REFERENCES evento(id) ON DELETE CASCADE,
    CONSTRAINT fk_voluntario FOREIGN KEY (id_voluntario)
        REFERENCES voluntario(id) ON DELETE CASCADE  
);
