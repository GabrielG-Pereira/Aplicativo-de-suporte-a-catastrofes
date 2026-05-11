INSERT INTO ponto_coleta (nome, endereco, localizacao, contato, temporario)
VALUES (
    'Igreja Matriz de Taubaté', 
    'Praça Dom Epaminondas, s/n - Centro, Taubaté - SP', 
    ST_SetSRID(ST_MakePoint(-45.5558, -23.0264), 4326), 
    '(12) 99678-1231', 
    false
);
