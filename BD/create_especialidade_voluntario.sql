CREATE TABLE especialidade_voluntario(
    categoria UUID NOT NULL,
    nome VARCHAR(100) NOT NULL,
    
    CONSTRAINT fk_categoria_especialidade FOREIGIN 
);
