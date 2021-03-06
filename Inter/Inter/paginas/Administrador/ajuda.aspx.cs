﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inter.Funcoes;


    public partial class paginas_Admin_ajuda : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Se sessão estiver nula redireciona para o bloqueio Url
            if (Session["login"] == null)
            {
                Response.Redirect("~/BloqueioUrl");
            }

            // CHAMAR A MASTER PAGE CORRESPONDENTE MASTER ou COORD   
            this.Page.MasterPageFile = Funcoes.chamarMasterPage_Admin(Session["menu"].ToString());
        }
    }
