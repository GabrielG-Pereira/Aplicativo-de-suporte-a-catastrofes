INSERT INTO voluntario (id_usuario, especialidade, localizacao, disponivel)
VALUES (
    '6109da31-01ca-4723-af76-d54ec72e4c89', 
    'id-da-especialidade-existente-uuid', 
    ST_SetSRID(ST_MakePoint(-45.5583, -23.0264), 4326), 
    true
);
