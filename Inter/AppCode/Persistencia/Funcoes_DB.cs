﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Inter.Funcoes;
using Interdisciplinar;
public class Funcoes_DB
{


    public static int ValidarAdmMaster(string login, string senha)
    {
        int verificacao = 0;
        IDbConnection objconexao;
        IDbCommand objCommand;
        IDataReader objDataReader;

        string sql = "Select per_login, per_senha, per_descricao from per_perfil where per_login = ?login and per_senha=sha1(?senha) and per_descricao=1;";

        objconexao = Mapped.Connection();
        objCommand = Mapped.Command(sql, objconexao);

        //string criptografada = Funcoes.Criptografar(senha, "SHA1");
        objCommand.Parameters.Add(Mapped.Parameter("?LOGIN", login));
        objCommand.Parameters.Add(Mapped.Parameter("?SENHA", senha));
        objDataReader = objCommand.ExecuteReader();

        while (objDataReader.Read())
        {

            //if (objDataReader["per_descricao"].ToString() == "") se é nulo ele nem entra no while então isso era desnecessário
            //{
            //    verificacao = -2;
            //}

            if (Convert.ToInt32(objDataReader["per_descricao"]) == 1)
            {
                verificacao = 1;
            }


        }

        objDataReader.Close();
        objconexao.Close();
        objconexao.Dispose();
        objCommand.Dispose();
        objDataReader.Dispose();

        return verificacao; //retorna o valor do resultado da verificação feita acima
    }

    public static int ValidaSenha(string senha)
    {
        int verificacao = 0;
        IDbConnection objconexao;
        IDbCommand objCommand;
        IDataReader objDataReader;

        string sql = "Select per_senha, per_descricao from per_perfil where per_senha=sha1(?senha) and per_descricao=1;";

        objconexao = Mapped.Connection();
        objCommand = Mapped.Command(sql, objconexao);

        //string criptografada = Funcoes.Criptografar(senha, "SHA1");
        objCommand.Parameters.Add(Mapped.Parameter("?SENHA", senha));
        objDataReader = objCommand.ExecuteReader();

        while (objDataReader.Read())
        {

            //if (objDataReader["per_descricao"].ToString() == "") se é nulo ele nem entra no while então isso era desnecessário
            //{
            //    verificacao = -2;
            //}

            if (Convert.ToInt32(objDataReader["per_descricao"]) == 1)
            {
                verificacao = 1;
            }


        }

        objDataReader.Close();
        objconexao.Close();
        objconexao.Dispose();
        objCommand.Dispose();
        objDataReader.Dispose();

        return verificacao; //retorna o valor do resultado da verificação feita acima
    }



    public static int ValidarAdmCoord(Professor prof)
    {
        string matricula = prof.Matricula; //pega matrícula do objeto professor obtido do método Professor.Validar
        int verificacao = 0;
        //string verificaLogin = "";
        //bool adm = false;
        //int proadmcodigo = 0;
        //int promatricula = 0;
        IDbConnection objconexao;
        IDbCommand objCommand;
        IDataReader objDataReader;
        string sql = "Select per_matricula, per_descricao from per_perfil where per_matricula = ?per_matricula;";
        objconexao = Mapped.Connection();
        objCommand = Mapped.Command(sql, objconexao);

        //string criptografada = Funcoes.Criptografar(senha, "SHA1");
        objCommand.Parameters.Add(Mapped.Parameter("?per_matricula", matricula));

        objDataReader = objCommand.ExecuteReader();

        while (objDataReader.Read())
        {

            if (Convert.ToInt32(objDataReader["per_descricao"]) == 2)
            {
                verificacao = 2;
            }
        }

        objDataReader.Close();
        objconexao.Close();
        objconexao.Dispose();
        objCommand.Dispose();
        objDataReader.Dispose();

        return verificacao; //retorna o valor do resultado da verificação feita acima
    }

    public static DataSet SelectDisciplina(int codProf)
    {
        DataSet ds = new DataSet();
        IDbConnection objConnection;
        IDbCommand objCommand;
        IDataAdapter objDataAdapter;
        objConnection = Mapped.Connection();
        objCommand = Mapped.Command("select tr.trm_nome, dg.dge_sigla, ad.adi_mae, c.cur_sigla from trm_turma tr inner join cur_curso c using(cur_codigo) inner join adi_atribuicao_disciplina ad using(trm_codigo) inner join dge_disciplinas_gerais dg using(dge_codigo) where ad.pro_matricula=?codProf;", objConnection);
        objCommand.Parameters.Add(Mapped.Parameter("?codProf", codProf));
        objDataAdapter = Mapped.Adapter(objCommand);
        objDataAdapter.Fill(ds);
        objConnection.Close();
        objCommand.Dispose();
        objConnection.Dispose();
        return ds;
    }

    public static DataSet SelectAllPIs()
    {
        DataSet ds = new DataSet();
        IDbConnection objConnection;
        IDbCommand objCommand;
        IDataAdapter objDataAdapter;
        objConnection = Mapped.Connection();
        objCommand = Mapped.Command("SELECT G.GRU_CODIGO, P.PRI_CODIGO, G.GRU_NOME_PROJETO, P.CUR_NOME, P.PRI_SEMESTRE, CONCAT(S.SAN_ANO, '-', S.SAN_SEMESTRE) AS SAN, G.GRU_FINALIZADO FROM SAN_SEMESTRE_ANO S INNER JOIN PRI_PROJETO_INTER P ON (S.SAN_CODIGO = P.SAN_CODIGO)" + 
        "INNER JOIN GRU_GRUPO G ON (P.PRI_CODIGO = G.PRI_CODIGO);", objConnection);
        objDataAdapter = Mapped.Adapter(objCommand);
        objDataAdapter.Fill(ds);
        objConnection.Close();
        objCommand.Dispose();
        objConnection.Dispose();
        /*if (ds.Tables[0].Rows.Count == 0)
        {
            ds = null;
        }*/
        return ds;
    }

    /*public static DataSet SelectFiltroPI(string pesquisa)
    {

        DataSet ds = new DataSet();
        IDbConnection objConnection;
        IDbCommand objCommand;
        IDataAdapter objDataAdapter;
        objConnection = Mapped.Connection();
        objCommand = Mapped.Command("select gru.gru_codigo, pri1.pri_codigo, gru.GRU_NOME_PROJETO, pri1.cur_nome, pri1.pri_semestre, concat(s.san_ano, '-', s.san_semestre) as SAN, gru.GRU_FINALIZADO from gru_grupo gru" + 
        " inner join pri_projeto_inter pri1 on pri1.pri_codigo = gru.pri_codigo"+
        " inner join san_semestre_ano s on s.san_codigo = pri1.san_codigo"+
        " inner join pri_projeto_inter pri2 on pri2.san_codigo = s.san_codigo"+
        " inner join eve_eventos eve on pri2.pri_codigo = eve.pri_codigo"+
        " inner join pri_projeto_inter pri3 on pri3.pri_codigo = eve.pri_codigo"+
        " inner join api_atribuicao_pi api on pri3.pri_codigo = api.pri_codigo"+
        " inner join cpi_criterio_pi cpi on api.adi_codigo = cpi.adi_codigo and api.pri_codigo = cpi.cpi_codigo"+
        " inner join cge_criterios_gerais cge on cpi.cge_codigo = cge.cge_codigo"+
        " and concat(gru.gru_nome_projeto, ' ', s.san_ano, ' ', eve.eve_data,' ', eve.eve_tipo, ' ', cge.cge_descricao, ' ', cge.cge_nome)like '%" + pesquisa + "%';", objConnection);
        //objCommand.Parameters.Add(Mapped.Parameter("?pesquisa", pesquisa));
        objDataAdapter = Mapped.Adapter(objCommand);
        objDataAdapter.Fill(ds);
        objConnection.Close();
        objCommand.Dispose();
        objConnection.Dispose();
        return ds;
    }*/

    public static DataSet SelectFiltroPI(string curso, string semestre_ano, string status, string pesquisa)
    {
        string filtro = "";

        if (curso != "")
        {
                filtro = filtro + "and pri3.cur_nome like '%" + curso + "%'";
        }

        if (semestre_ano != "")
        {
                filtro = filtro + "and pri3.pri_semestre = '" + semestre_ano + "'";
        }

        if (status != "")
        {
                filtro = filtro + "and gru.GRU_FINALIZADO = '" + status + "'";
        }

        if (pesquisa != "")
        {
                filtro = filtro + "and concat(gru.gru_nome_projeto, ' ', s.san_ano, ' ', eve.eve_data,' ', eve.eve_tipo, ' ', cge.cge_descricao, ' ', cge.cge_nome) like '%" + pesquisa + "%'";
        }

        DataSet ds = new DataSet();
        IDbConnection objConnection;
        IDbCommand objCommand;
        IDataAdapter objDataAdapter;
        objConnection = Mapped.Connection();
        string sql = "select gru.gru_codigo, pri1.pri_codigo, gru.GRU_NOME_PROJETO, pri1.cur_nome, pri1.pri_semestre, concat(s.san_ano, '-', s.san_semestre) as SAN, gru.GRU_FINALIZADO from gru_grupo gru" + 
        " inner join pri_projeto_inter pri1 on pri1.pri_codigo = gru.pri_codigo"+
        " inner join san_semestre_ano s on s.san_codigo = pri1.san_codigo"+
        " inner join pri_projeto_inter pri2 on pri2.san_codigo = s.san_codigo"+
        " inner join eve_eventos eve on pri2.pri_codigo = eve.pri_codigo"+
        " inner join pri_projeto_inter pri3 on pri3.pri_codigo = eve.pri_codigo"+
        " inner join api_atribuicao_pi api on pri3.pri_codigo = api.pri_codigo"+
        " inner join cpi_criterio_pi cpi on api.adi_codigo = cpi.adi_codigo and api.pri_codigo = cpi.pri_codigo"+
        " inner join cge_criterios_gerais cge on cpi.cge_codigo = cge.cge_codigo " + filtro + " group by gru.gru_codigo;";
        objCommand = Mapped.Command(sql, objConnection);
        objDataAdapter = Mapped.Adapter(objCommand);
        objDataAdapter.Fill(ds);
        objConnection.Close();
        objCommand.Dispose();
        objConnection.Dispose();

        return ds;
    }

    public static int DropDatabase()
    {
        int retorno = 0;
        try
        {
            IDbConnection conexao;
            IDbCommand objComando;
            string sql = "DROP DATABASE INTER; CREATE DATABASE INTER;";
            conexao = Mapped.Connection();
            objComando = Mapped.Command(sql, conexao);
            objComando.ExecuteNonQuery();
            conexao.Close();
            objComando.Dispose();
            conexao.Dispose();
        }
        catch (Exception e)
        {
            retorno = -2;
        }
        return retorno;
    }

}