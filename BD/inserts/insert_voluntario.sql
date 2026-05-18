WITH find_user AS (
    SELECT id FROM usuario WHERE email = 'joao@email.com' LIMIT 1
),
ins_voluntario AS (
    INSERT INTO voluntario (id_usuario, localizacao, disponivel, especialidade)
    SELECT 
        id, 
        ST_SetSRID(ST_Point(-45.5555, -23.0333), 4326), 
        true,
        (SELECT id FROM especialidade_voluntario WHERE nome = 'Médico' LIMIT 1)
    FROM find_user
    RETURNING id
)
INSERT INTO voluntario_evento (id_voluntario, id_evento, status, data_aceito)
SELECT 
    id, 
    (SELECT id FROM evento WHERE status = 'Ativo' LIMIT 1),
    'A caminho',
    NOW()
FROM ins_voluntario;
