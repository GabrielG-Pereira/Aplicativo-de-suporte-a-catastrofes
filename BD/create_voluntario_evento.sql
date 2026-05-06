CREATE TABLE ponto_coleta(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_evento UUID NOT NULL,
    id_voluntario UUID NOT NULL,
    status status_voluntario_evento NOT NULL,
    data_aceito TIMESTAMP NOT NULL DEFAULT 'A caminho',
    data_checkin TIMESTAMP,
    data_checkout TIMESTAMP,

    CONSTRAINT fk_evento FOREIGIN KEY (id_evento)
        REFERENCES evento(id) ON DELETE CASCADE,
    CONSTRAINT fk_voluntario FOREIGIN KEY (id_voluntario)
        REFERENCES voluntario(id) ON DELETE CASCADE  
        
    CREATE INDEX idx_voluntario_evento_evento ON voluntario_evento(id_evento);
    CREATE INDEX idx_voluntario_evento_voluntario ON voluntario_evento(id_voluntario);
);
