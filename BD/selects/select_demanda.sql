SELECT
    d.id AS demanda_id,
    c.nome AS categoria_nome,
    pc.nome AS ponto_coleta_nome,
    e.nome AS evento_nome,
    d.descricao,
    d.status

FROM demanda d
JOIN categoria_demanda c ON d.categoria = c.id
JOIN ponto_evento pe ON d.id_ponto_evento = pe.id
JOIN ponto_coleta pc ON pe.id_ponto = pc.id
JOIN evento e ON pe.id_evento = e.id;
