﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


public class Requerimento_DB
{

    //INSERT
    public static int Insert(Requerimento requerimento)
    {
        int retorno = 0;
        DateTime date = DateTime.Now;
        try
        {
            IDbConnection conexao;
            IDbCommand objCommand;
            string sql = " ";

            if (requerimento.CodigoGrupo != 0) {
               sql = "INSERT INTO req_requerimento(REQ_CODIGO, PRO_MATRICULA, GRU_CODIGO, REQ_ASSUNTO, REQ_DT_REQUISICAO, REQ_STATUS, REQ_CATEGORIA, REQ_USUARIO, REQ_DT_MODIFICADO) VALUES (?req_codigo, ?pro_matricula, ?gru_codigo, ?req_assunto, ?req_dt_requisicao, ?req_status, ?req_categoria, ?req_usuario, ?data)";
            } else {
               sql = "INSERT INTO req_requerimento(REQ_CODIGO, PRO_MATRICULA, REQ_ASSUNTO, REQ_DT_REQUISICAO, REQ_STATUS, REQ_CATEGORIA, REQ_USUARIO, REQ_DT_MODIFICADO) VALUES (?req_codigo, ?pro_matricula, ?req_assunto, ?req_dt_requisicao, ?req_status, ?req_categoria, ?req_usuario, ?data)";
            }
            
            conexao = Mapped.Connection();
            objCommand = Mapped.Command(sql, conexao);
            objCommand.Parameters.Add(Mapped.Parameter("?req_codigo", requerimento.CodigoReq));
            objCommand.Parameters.Add(Mapped.Parameter("?pro_matricula", requerimento.MatriculaPro));
            objCommand.Parameters.Add(Mapped.Parameter("?gru_codigo", requerimento.CodigoGrupo));
            objCommand.Parameters.Add(Mapped.Parameter("?req_assunto", requerimento.Assunto));
            objCommand.Parameters.Add(Mapped.Parameter("?req_dt_requisicao", requerimento.DataReq));
            objCommand.Parameters.Add(Mapped.Parameter("?req_status", requerimento.Status));
            objCommand.Parameters.Add(Mapped.Parameter("?req_categoria", requerimento.Categoria));
            objCommand.Parameters.Add(Mapped.Parameter("?req_usuario", requerimento.Usuario));
            objCommand.Parameters.Add(Mapped.Parameter("?data", date));
            
            objCommand.ExecuteNonQuery();
            conexao.Close();
            objCommand.Dispose();
            conexao.Dispose();
        }
        catch (Exception e)
        {
            retorno = -2;
        }
        return retorno;
    }

    //UPDATE
    public static int Update(int cod, int status)
    {
        int retorno = 0;
        DateTime date = DateTime.Now;
        try
        {
            IDbConnection conexao;
            IDbCommand objCommand;
            string sql = "UPDATE req_requerimento SET req_status = ?status WHERE req_codigo = ?codigo ";
            conexao = Mapped.Connection();
            objCommand = Mapped.Command(sql, conexao);
            objCommand.Parameters.Add(Mapped.Parameter("?codigo", cod));
            objCommand.Parameters.Add(Mapped.Parameter("?status", status));
            objCommand.ExecuteNonQuery();
            conexao.Close();
            objCommand.Dispose();
            conexao.Dispose();
        }
        catch (Exception e)
        {
            string erro = e.Message;
            retorno = -2;
        }
        return retorno;
    }

    //UPDATE PARA ALTERAÇÃO DE NOTAS (COM COD GRUPO)
    public static int Update(int cod, int status, int codGrupo)
    {
        int retorno = 0;
        DateTime date = DateTime.Now;
        try
        {
            IDbConnection conexao;
            IDbCommand objCommand;
            string sql = "UPDATE req_requerimento SET req_status = ?status WHERE req_codigo = ?codigo and gru_codigo = ?gruCodigo ";
            conexao = Mapped.Connection();
            objCommand = Mapped.Command(sql, conexao);
            objCommand.Parameters.Add(Mapped.Parameter("?codigo", cod));
            objCommand.Parameters.Add(Mapped.Parameter("?status", status));
            objCommand.Parameters.Add(Mapped.Parameter("?gruCodigo", codGrupo));
            objCommand.ExecuteNonQuery();
            conexao.Close();
            objCommand.Dispose();
            conexao.Dispose();
        }
        catch (Exception e)
        {
            string erro = e.Message;
            retorno = -2;
        }
        return retorno;
    }

    //UPDATE TIME
    public static int UpdateTime(int cod)
    {
        int retorno = 0;
        DateTime date = DateTime.Now;
        try
        {
            IDbConnection conexao;
            IDbCommand objCommand;
            string sql = "UPDATE req_requerimento SET req_dt_modificado = ?data WHERE req_codigo = ?codigo ";
            conexao = Mapped.Connection();
            objCommand = Mapped.Command(sql, conexao);
            objCommand.Parameters.Add(Mapped.Parameter("?codigo", cod));
            objCommand.Parameters.Add(Mapped.Parameter("?data", date));
            objCommand.ExecuteNonQuery();
            conexao.Close();
            objCommand.Dispose();
            conexao.Dispose();
        }
        catch (Exception e)
        {
            string erro = e.Message;
            retorno = -2;
        }
        return retorno;
    }

    //SELECT
    public static Requerimento Select(int codigo){
        try{
            Requerimento objRequerimento = null;
            IDbConnection objConnection;
            IDbCommand objCommnad;
            IDataReader objDataReader;
            objConnection = Mapped.Connection();
            objCommnad = Mapped.Command("SELECT * FROM req_requerimento WHERE req_codigo = ?codigo", objConnection);
            objCommnad.Parameters.Add(Mapped.Parameter("?codigo", codigo));
            objDataReader = objCommnad.ExecuteReader();
            while (objDataReader.Read()){
                
                var CodigoReq = Convert.ToInt32(objDataReader["req_codigo"]);
                var Assunto = objDataReader["req_assunto"].ToString();
                var DataReq = Convert.ToDateTime(objDataReader["req_dt_requisicao"]);
                var MatriculaPro = objDataReader["pro_matricula"].ToString();
                int CodigoGrup = new int();
                if(!(objDataReader["gru_codigo"] is DBNull)){
                   CodigoGrup = Convert.ToInt32(objDataReader["gru_codigo"]);
                }
                var Status = Convert.ToInt32(objDataReader["req_status"]);
                var Categoria = objDataReader["req_categoria"].ToString();
                var Usuario = objDataReader["req_usuario"].ToString();
                objRequerimento = new Requerimento(CodigoReq, MatriculaPro, CodigoGrup, Assunto, DataReq, Status, Categoria, Usuario);
            }
            objDataReader.Close();
            objConnection.Close();
            objConnection.Dispose();
            objCommnad.Dispose();
            objDataReader.Dispose();
            return objRequerimento;
        }
        catch (Exception e){
            string erro = e.Message;
            return null;
        }
    }


    public static Requerimento SelectLast()
    {
        try
        {
            Requerimento objRequerimento = null;
            IDbConnection objConnection;
            IDbCommand objCommnad;
            IDataReader objDataReader;
            objConnection = Mapped.Connection();
            objCommnad = Mapped.Command("SELECT * FROM req_requerimento ORDER by req_codigo DESC LIMIT 1", objConnection);            
            objDataReader = objCommnad.ExecuteReader();
            while (objDataReader.Read())
            {

                var CodigoReq = Convert.ToInt32(objDataReader["req_codigo"]);
                var Assunto = objDataReader["req_assunto"].ToString();
                var DataReq = Convert.ToDateTime(objDataReader["req_dt_requisicao"]);
                var MatriculaPro = objDataReader["pro_matricula"].ToString();
                var Status = Convert.ToInt32(objDataReader["req_status"]);
                var Categoria = objDataReader["req_categoria"].ToString();
                var Usuario = objDataReader["req_usuario"].ToString();
                objRequerimento = new Requerimento(CodigoReq, MatriculaPro, Assunto, DataReq, Status, Categoria, Usuario);
            }
            objDataReader.Close();
            objConnection.Close();
            objConnection.Dispose();
            objCommnad.Dispose();
            objDataReader.Dispose();
            return objRequerimento;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    //SELECT ALL
    public static DataSet SelectAll()
    {
        DataSet ds = new DataSet();
        IDbConnection objConnection;
        IDbCommand objCommand;
        IDataAdapter objDataAdapter;
        objConnection = Mapped.Connection();
        objCommand = Mapped.Command("SELECT * FROM req_requerimento ORDER BY req_dt_modificado", objConnection);
        objDataAdapter = Mapped.Adapter(objCommand);
        objDataAdapter.Fill(ds);
        objConnection.Close();
        objCommand.Dispose();
        objConnection.Dispose();
        return ds;
    }

    //SELECT STATUS
    public static DataSet SelectS(int codigo)
    {
        DataSet ds = new DataSet();
        IDbConnection objConnection;
        IDbCommand objCommand;
        IDataAdapter objDataAdapter;
        objConnection = Mapped.Connection();
        objCommand = Mapped.Command("SELECT * FROM req_requerimento WHERE req_status=?codigo ORDER BY req_dt_modificado DESC", objConnection);
        objCommand.Parameters.Add(Mapped.Parameter("?codigo", codigo));
        objDataAdapter = Mapped.Adapter(objCommand);
        objDataAdapter.Fill(ds);
        objConnection.Close();
        objCommand.Dispose();
        objConnection.Dispose();
        return ds;
    }
    //SELECT COM CÓDIGO DE STATUS E CÓDIGO DE MATRÍCULA PARA RECEBER SÓ SOLICITAÇÕES DE UM PROFESSOR ESPECÍFICO
    public static DataSet SelectS(int codigo, string matricula)
    {
        DataSet ds = new DataSet();
        IDbConnection objConnection;
        IDbCommand objCommand;
        IDataAdapter objDataAdapter;
        objConnection = Mapped.Connection();
        objCommand = Mapped.Command("SELECT * FROM req_requerimento WHERE req_status=?codigo and pro_matricula=?matricula ORDER BY req_dt_modificado DESC", objConnection);
        objCommand.Parameters.Add(Mapped.Parameter("?codigo", codigo));
        objCommand.Parameters.Add(Mapped.Parameter("?matricula", matricula));
        objDataAdapter = Mapped.Adapter(objCommand);
        objDataAdapter.Fill(ds);
        objConnection.Close();
        objCommand.Dispose();
        objConnection.Dispose();
        return ds;
    }

}