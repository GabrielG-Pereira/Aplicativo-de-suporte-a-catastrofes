INSERT INTO evento (nome, descricao, localizacao, status)
VALUES (
    'Alagamento Central',
    'Inundação severa na via principal após transbordamento de bueiros.',
    ST_GeomFromText('POINT(-46.633308 -23.550520)', 4326),
    'Ativo'
);
