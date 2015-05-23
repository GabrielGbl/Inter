﻿CREATE DATABASE INTER;
USE INTER;

CREATE TABLE PER_PERFIL(
  PER_MATRICULA VARCHAR(20) PRIMARY KEY,
  PER_DESCRICAO INT NOT NULL,
  PER_LOGIN VARCHAR(50),
  PER_SENHA VARCHAR(256)
);

select sha1('123');
insert into per_perfil values
('adm_master', 1, 'adm_master', '40bd001563085fc35165329ea1ff5c5ecbdbbeef'),
('PRO106007',2,NULL,NULL);

CREATE TABLE SAN_SEMESTRE_ANO(
  SAN_CODIGO INT PRIMARY KEY AUTO_INCREMENT,
  SAN_ANO INT NOT NULL,
  SAN_SEMESTRE INT NOT NULL,
  SAN_ATIVO BOOL NOT NULL
);

CREATE TABLE PRI_PROJETO_INTER(
  PRI_CODIGO INT PRIMARY KEY AUTO_INCREMENT,
  SAN_CODIGO INT NOT NULL,
  FOREIGN KEY(SAN_CODIGO) REFERENCES SAN_SEMESTRE_ANO(SAN_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE API_ATRIBUICAO_PI(
  PRI_CODIGO INT NOT NULL,
  ADI_CODIGO INT NOT NULL,
  PRIMARY KEY(ADI_CODIGO, PRI_CODIGO),
  FOREIGN KEY(PRI_CODIGO) REFERENCES PRI_PROJETO_INTER(PRI_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE EVE_EVENTOS(
  EVE_CODIGO INT PRIMARY KEY AUTO_INCREMENT,
  PRI_CODIGO INT NOT NULL,
  EVE_DATA DATE NOT NULL,
  EVE_TIPO VARCHAR(100),
  FOREIGN KEY(PRI_CODIGO) REFERENCES PRI_PROJETO_INTER(PRI_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE GRU_GRUPO(
  GRU_CODIGO INT PRIMARY KEY AUTO_INCREMENT,
  PRI_CODIGO INT NOT NULL,
  GRU_NOME_PROJETO VARCHAR(150),
  GRU_MEDIA DOUBLE,
  GRU_FINALIZADO BOOL,
  FOREIGN KEY(PRI_CODIGO) REFERENCES PRI_PROJETO_INTER(PRI_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE REQ_REQUERIMENTO(

  REQ_CODIGO INT PRIMARY KEY AUTO_INCREMENT,

  PRO_MATRICULA VARCHAR(20) NOT NULL,

  GRU_CODIGO INT,

  REQ_ASSUNTO TEXT,

  REQ_DT_REQUISICAO DATETIME NOT NULL,

  REQ_STATUS INT NOT NULL,

  REQ_CATEGORIA varchar(100) NOT NULL,

  FOREIGN KEY(GRU_CODIGO) REFERENCES GRU_GRUPO(GRU_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION

);

CREATE TABLE MSG_MENSAGEM(

  MSG_CODIGO INT PRIMARY KEY AUTO_INCREMENT,

  REQ_CODIGO INT NOT NULL,

  MATRICULA VARCHAR(20),

  MSG_DT_ENVIO DATETIME NOT NULL,

  MSG_CONTEUDO MEDIUMTEXT NOT NULL,

  FOREIGN KEY(REQ_CODIGO) REFERENCES REQ_REQUERIMENTO(REQ_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION

);

CREATE TABLE GAL_GRUPO_ALUNO(
  ALU_MATRICULA VARCHAR(20) NOT NULL,
  GRU_CODIGO INT NOT NULL,
  PRIMARY KEY(ALU_MATRICULA, GRU_CODIGO),
  FOREIGN KEY(GRU_CODIGO) REFERENCES GRU_GRUPO(GRU_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE CGE_CRITERIOS_GERAIS(
  CGE_CODIGO INT PRIMARY KEY AUTO_INCREMENT,
  CGE_NOME VARCHAR(50),
  CGE_DESCRICAO VARCHAR(200),
  CGE_ATIVO BOOL
);

CREATE TABLE CPI_CRITERIO_PI(
  CPI_CODIGO INT PRIMARY KEY AUTO_INCREMENT,
  CGE_CODIGO INT NOT NULL,
  PRI_CODIGO INT NOT NULL,
  ADI_CODIGO INT NOT NULL,
  CPI_PESO INT NOT NULL,
  FOREIGN KEY(CGE_CODIGO) REFERENCES CGE_CRITERIOS_GERAIS(CGE_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION,
  FOREIGN KEY(PRI_CODIGO) REFERENCES API_ATRIBUICAO_PI(PRI_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION,
  FOREIGN KEY(ADI_CODIGO) REFERENCES API_ATRIBUICAO_PI(ADI_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE HIS_HISTORICO_ALUNO_DISCIPLINA(
  HIS_CODIGO INT PRIMARY KEY AUTO_INCREMENT,
  ALU_MATRICULA VARCHAR(20) NOT NULL,
  CPI_CODIGO INT NOT NULL,
  HIS_NOTA DOUBLE,
  FOREIGN KEY(ALU_MATRICULA) REFERENCES GAL_GRUPO_ALUNO(ALU_MATRICULA) ON UPDATE CASCADE ON DELETE NO ACTION,
  FOREIGN KEY(CPI_CODIGO) REFERENCES CPI_CRITERIO_PI(CPI_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE MDD_MEDIA_DISCIPLINA(
  MDD_CODIGO INT PRIMARY KEY AUTO_INCREMENT,
  PRI_CODIGO INT NOT NULL,
  ADI_CODIGO INT NOT NULL,
  GRU_CODIGO INT NOT NULL,
  MED_MEDIA DOUBLE NOT NULL,
  FOREIGN KEY(PRI_CODIGO) REFERENCES API_ATRIBUICAO_PI(PRI_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION,
  FOREIGN KEY(ADI_CODIGO) REFERENCES API_ATRIBUICAO_PI(ADI_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION,
  FOREIGN KEY(GRU_CODIGO) REFERENCES GRU_GRUPO(GRU_CODIGO) ON UPDATE CASCADE ON DELETE NO ACTION
  );