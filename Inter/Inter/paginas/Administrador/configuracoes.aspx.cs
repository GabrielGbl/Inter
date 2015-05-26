﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inter.Funcoes;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.IO;
using System.Data;
using Interdisciplinar;


public partial class paginas_Admin_configuracoes : System.Web.UI.Page
{
    internal DataTable dt = null;
    protected void Page_PreInit(object sender, EventArgs e)
    {
        // Se sessão estiver nula redireciona para o bloqueio Url
        if (Session["login"] == null)
        {
            Response.Redirect("~/Paginas/Login/bloqueioUrl.aspx");
        }

        // CHAMAR A MASTER PAGE CORRESPONDENTE MASTER ou COORD   
        this.Page.MasterPageFile = Funcoes.chamarMasterPage_Admin(Session["coord"].ToString());
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        Backup(gdvBkp);

    }

    public DataTable Backup(GridView gv_arquivos)
    {
        string caminho = (Request.PhysicalApplicationPath + "Backup\\");
        DirectoryInfo pasta = new DirectoryInfo(caminho);
        //DirectoryInfo[] subPastas = pasta.GetDirectories();
        //FileInfo[] arquivos = pasta.GetFiles();
        string[] arquivos = Directory.GetFiles(caminho, "*");

        DataTable dt = new DataTable();

        DataColumn mDataColumn;
        mDataColumn = new DataColumn();
        mDataColumn.DataType = typeof(string);
        mDataColumn.ColumnName = "Nome";
        dt.Columns.Add(mDataColumn);


        int i, j, min;
        string varAux;
        for (i = 0; i < arquivos.Length - 1; i++)
        {
            min = i;
            for (j = i + 1; j < arquivos.Length; j++)
            {
                // Utilizando o CompareTo para comparar as string do vetor
                // resultado -1 significa que arquivos[j] < arquivos[min]
                if (arquivos[j].CompareTo(arquivos[min]) != -1)
                {
                    min = j;
                }
            }
            varAux = arquivos[min];
            arquivos[min] = arquivos[i];
            arquivos[i] = varAux;
        }

        /*foreach (FileInfo file in arquivos)
        {
            DataRow dr = dt.NewRow();
            dr["Nome"] = file.Name;
            dt.Rows.Add(dr);
        }*/

        foreach (string file in arquivos)
        {
            DataRow dr = dt.NewRow();
            dr["Nome"] = file.Replace(caminho, "").Replace(".sql", "");

            dt.Rows.Add(dr);

        }

        gv_arquivos.DataSource = dt.DefaultView;
        gv_arquivos.DataBind();

        return dt;
    }

    protected void btnCriarBackup_Click(object sender, EventArgs e)
    {

        string user = "root";
        string password = "123";
        string database = "inter";
        string server = "localhost";
        string nome_arquivo = "bkp_" + database + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".sql";
        string directory = (Request.PhysicalApplicationPath + "Backup");
        string caminhoDump = ("C:\\Program Files\\MySQL\\MySQL Server 5.6\\bin\\mysqldump.exe");



        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            if (!Directory.Exists(directory))
            {

            }
        }

        if (!Directory.Exists(caminhoDump))
        {

        }


        Process.Start(caminhoDump, ("-u " + user + " -p" + password + " -x -e -B " + database + " > -r " + directory + "\\" + nome_arquivo));
        System.Threading.Thread.Sleep(500);

        Backup(gdvBkp);
        UpdatePanelBkp.Update();

        /*string constring = ("server=" + server + ";user=" + user + ";database=" + database + ";password=" + password);
        string file = (directory + "\\" + nome_arquivo);
        using (MySqlConnection conn = new MySqlConnection(constring))
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlBackup mb = new MySqlBackup(cmd))
                {
                    cmd.Connection = conn;
                    conn.Open();
                    mb.ExportToFile(file);
                    conn.Close();
                }
            }
        }*/

    }

    protected void gdvBkp_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gdvBkp_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void gdvBkp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvBkp.PageIndex = e.NewPageIndex;
        Backup(gdvBkp);
    }
}