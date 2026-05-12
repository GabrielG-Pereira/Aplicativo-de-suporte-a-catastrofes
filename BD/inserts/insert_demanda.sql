WITH ins_ponto_evento AS(
    INSERT INTO ponto_evento(id_evento, id_ponto, status)
    VALUES(
        (SELECT id FROM evento LIMIT 1),
        (SELECT id FROM ponto_coleta LIMIT 1),
        true    
    )
    RETURNING id
)

INSERT INTO demanda (id_ponto_evento, categoria, descricao, status)
SELECT
    id,
    (SELECT id FROM categoria_demanda WHERE nome='Alimentos'LIMIT 1),
    'Fardos de arroz',
    'Crítico'
FROM ins_ponto_evento;  
    
