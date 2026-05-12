WITH ins_voluntario_evento AS(
    INSERT INTO voluntario_evento(id_evento, id_voluntario, status, data_checkin, data)
)

INSERT INTO voluntario (id_usuario, especialidade, localizacao, disponivel)
VALUES (
    '18212433-23cd-47e1-82ba-f725d3e230b2', 
    '660c44db-d581-45be-b38f-4afadac23e2b', 
    ST_SetSRID(ST_MakePoint(-45.5594, -23.0211), 4326), 
    TRUE
);
