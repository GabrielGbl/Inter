﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_Login_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void enviar_Click(object sender, EventArgs e)
    {

        lblMsgErro.Text = "";

        string user = txtLogin.Text.ToString();
        string senha = txtSenha.Text.ToString();

        if (!String.IsNullOrEmpty(user) && !String.IsNullOrEmpty(senha))
        {

            switch (Funcoes_DB.ValidarLogin(user, senha))
            {
                case 0:
                    Session["login"] = user;
                    Session["curso"] = "";
                    Session["semestre"] = "";
                    Session["disciplina"] = "";
                    Session["mae"] = "";
                    Response.Redirect("~/Paginas/Usuario/escolherDisciplina.aspx");
                    break;
                case 1:
                    Session["login"] = user;
                    Response.Redirect("~/Paginas/Administrador/solicitacoes.aspx");
                    break;
                case -2:
                    lblMsgErro.Text = "E-mail ou Senha incorretos.";
                    break;
            }

        }
        else
        {
            lblMsgErro.Text = "Preencha os campos.";
        }
    }


}