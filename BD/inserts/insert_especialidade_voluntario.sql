WITH ins_categoria AS(
    INSERT INTO categoria_especialidade(nome)
    VALUES(
        'Saúde'
    )
    RETURNING id
)

INSERT INTO especialidade_voluntario(categoria, nome)
SELECT
    id,
    'Médico'
FROM ins_categoria;  
      
