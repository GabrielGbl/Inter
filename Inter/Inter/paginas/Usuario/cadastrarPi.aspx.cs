﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inter.Funcoes;
using AppCode.Persistencia;
using System.Web.Services;
using System.Web.Script.Serialization;
using Interdisciplinar;
//using System.Runtime.Serialization.Json;


public partial class paginas_Usuario_cadastrarPi : System.Web.UI.Page
{

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //VERIFICAR SESSAO LOGIN
        if (Session["Professor"] == null)
        {
            Response.Redirect("~/Paginas/Login/bloqueioUrl.aspx");
        }
        // CHAMAR A MASTER PAGE - OBS: MASTERPAGEFILE É O CAMINHO DO ARQUIVO MASTERPAGE QUE VOCÊ DESEJA CHAMAR        
        this.Page.MasterPageFile = Funcoes.chamarMasterPage(Session["mae"].ToString());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //BLOQUEIO URL SE NÃO TIVER ESCOLHIDO ALGUMA DISCIPLINA 
        if (Session["disciplina"] == "")
        {
            Response.Redirect("escolherDisciplina.aspx");
        }

        //BLOQUEIO SE NÃO FOR DISCIPLINA-MÃE

        if (Session["mae"] == "FILHA")
        {
            Response.Redirect("home.aspx");
        }


        //SE NÃO FOR POSTBACK VAI CARREGAR OS MÉTODOS ABAIXO DESCRITOS
        if (!IsPostBack)
        {
            CarregaCriGerais();
            CarregaAlunosCadastrarPi();
            CarregarDisciplinasEnvolvidas();
            updPanelGrupos.Update();
            PegarAnoeSemestreAno();
            PegarUltimoCodPI();
            lblCursoAut.Text = Session["curso"].ToString();
            lblSemestreAut.Text = Session["semestre"].ToString();
            index = 1;
            btnConfirmarEdicao.Style.Add("opacity", "0.4");
            btnExcluirGrupo.Style.Add("opacity", "0.4");
            btnCancelarEdicao.Style.Add("opacity", "0.4");
            btnConfirmarEdicao.Style.Add("pointer-events", "none");
            btnCancelarEdicao.Style.Add("pointer-events", "none");
            btnExcluirGrupo.Style.Add("pointer-events", "none");

        }

        PanelCriterios.Controls.Clear();
        updPanelPeso.Update();
        CriarCriterio();
        updPanelPeso.Update();

    }

    // ******************  ETAPA 1 - CADASTRO PI, CADASTRO DE DATAS ******************
    // *******************************************************************************
    //public static int tamanhoVetorCodigoDisciplina;
    public static List<int> listCodDisciplinas = new List<int>();
    private void CarregarDisciplinasEnvolvidas(){

        
        Calendario cal = new Calendario();
        DataSet ds = (DataSet)Session["DataSetPIsbyCalendario"];
        if (Session["DataSetPIsbyCalendario"] == null)
        {
            cal = Calendario.SelectbyAtual();
            ds = Professor.SelectAllPIsbyCalendario(cal.Codigo, cal.AnoSemestreAtual);
            Session["DataSetPIsbyCalendario"] = ds;
        }
            
        int qtd = ds.Tables[0].Rows.Count;

        Table tableDisciplina = new Table();
        tableDisciplina.CssClass = "gridView";
        PainelDisciplinas.Controls.Add(tableDisciplina);
        int row = ds.Tables[0].Rows.Count;

        TableHeaderCell th = new TableHeaderCell();
        TableHeaderRow thr = new TableHeaderRow();

        //ADICIONANDO CABEÇALHO  DISCIPLINAS / MÃE-FILHAS
        th = new TableHeaderCell();
        th.Text = "Código";        
        thr.Cells.Add(th);
        tableDisciplina.Rows.Add(thr);

        th = new TableHeaderCell();
        th.Text = "Disciplinas";
        thr.Cells.Add(th);
        tableDisciplina.Rows.Add(thr);

        th = new TableHeaderCell();
        th.Text = "Mãe/Filha";
        thr.Cells.Add(th);
        tableDisciplina.Rows.Add(thr);

        Label lblDisciplinas = new Label();
        Label lblCodigoDisciplina = new Label();
        string[] vetorReturnFunction = new string[3];

        for (int i = 0; i < row; i++)
        {
            TableRow rows = new TableRow();
            for (int j = 0; j < 3; j++)
            {
                TableCell cell = new TableCell();
                vetorReturnFunction = Funcoes.tratarDadosProfessor(ds.Tables[0].Rows[i]["disciplina"].ToString());
                if ((vetorReturnFunction[0] == Session["Curso"].ToString()) && (vetorReturnFunction[1] == Session["Semestre"].ToString()))
                {
                    if (j == 1)
                    {
                        lblDisciplinas = new Label();
                        lblDisciplinas.Text = vetorReturnFunction[2].ToString();
                        cell.Controls.Add(lblDisciplinas);
                    }
                    else if(j == 2)
                    {
                        lblDisciplinas = new Label();
                        if (ds.Tables[0].Rows[i]["tipo"].ToString() == "MAE")
                        {
                            //ícone da estrelinha
                            lblDisciplinas.Text = "<span class='glyphicon glyphicon-star'></span>";
                        }
                        else
                        {
                            //ícone de tracinho
                            lblDisciplinas.Text = "<span class='glyphicon glyphicon-minus'></span>";
                        }
                        cell.Controls.Add(lblDisciplinas);
                    }
                    else
                    {
                        lblCodigoDisciplina = new Label();
                        lblCodigoDisciplina.Text = ds.Tables[0].Rows[i]["atr_codigo"].ToString();
                        lblCodigoDisciplina.ID = "codDisciplina" + i;
                        cell.Controls.Add(lblCodigoDisciplina);
                        listCodDisciplinas.Add(Convert.ToInt32(lblCodigoDisciplina.Text));
                        //tamanhoVetorCodigoDisciplina++;
                    }
                    rows.Cells.Add(cell);
                }
            }
            tableDisciplina.Rows.Add(rows);
        }  
    }

    private void PegarUltimoCodPI()
    {
        // PEGAR ULTIMO CODIGO DE PI E ACRESCENTAR 1
        int cod = Projeto_Inter_DB.SelectUltimoCod();
        int codMais = cod + 1;
        if (codMais < 0) {
            codMais = 1;
        }
        lblCodigoPiAut.Text = codMais.ToString();
    }

    private void PegarAnoeSemestreAno()
    {
        // PEGAR ANO E SEMESTRE DO ANO DO BANCO DE DADOS
        Semestre_Ano objSemAno = new Semestre_Ano();
        objSemAno = Semestre_Ano_DB.Select();
        lblSemestreAnoAut.Text = objSemAno.San_semestre.ToString();
        lblAnoAut.Text = objSemAno.San_ano.ToString();
    }

    public static string[] desc;
    public static string[] dat;

    [System.Web.Services.WebMethod]
    public static string GetEventos(string dadosEventos)
    {

        string[] eventos = dadosEventos.Split('|'); //divide quando achar o pipe('|') 

        List<string> descricao = new List<string>(); //cria uma List, porque não tem um tamanho definido
        List<string> data = new List<string>();


        for (int i = 0; i < eventos.Length - 1; i++)
        {
            if (i % 2 == 0) //se for par
            {
                descricao.Add(eventos[i]);
            }
            else //se for impar
            {
                data.Add(eventos[i]);
            }

        }

        desc = descricao.ToArray(); //toArray converte a List em Array
        dat = data.ToArray();


        return dadosEventos;

    }



    // ****************** ETAPA 2 - CADASTRO DE CRITÉRIOS ******************
    //**********************************************************************

    //EVENTO QUE MOVE OS CRITÉRIOS GERAIS PARA A LISTBOX DE CRITÉRIOS ESCOLHIDOS PARA O PI
    protected void listaCritGeral_SelectedIndexChanged(object sender, EventArgs e)
    {
        listaCritPi.Items.Add(listaCritGeral.SelectedItem);
        listaCritGeral.Items.RemoveAt(listaCritGeral.SelectedIndex);
        listaCritGeral.ClearSelection();
        listaCritPi.ClearSelection();
        CarregaTip();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa2", "etapa2();", true);
    }

    //EVENTO QUE MOVE OS CRITÉRIOS ESCOLHIDOS PARA O PI PARA A LISTBOX DE CRITÉRIOS GERAIS
    protected void listaCritPi_SelectedIndexChanged(object sender, EventArgs e)
    {
        listaCritGeral.Items.Add(listaCritPi.SelectedItem);
        listaCritPi.Items.RemoveAt(listaCritPi.SelectedIndex);
        listaCritGeral.ClearSelection();
        listaCritPi.ClearSelection();
        CarregaTip();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa2", "etapa2();", true);
    }

    private void CarregaTip()
    {
        string[] vetTip = liCritTip.ToArray();
        string[] vetCod = liCritCod.ToArray();
        foreach (ListItem li in listaCritGeral.Items)
        {
            for (int j = 0; j < vetCod.Length; j++)
            {
                if (li.Value == vetCod[j])
                {
                    li.Attributes.Add("title", vetTip[j]);
                }
            }

        }

        foreach (ListItem li in listaCritPi.Items)
        {
            for (int j = 0; j < vetCod.Length; j++)
            {
                if (li.Value == vetCod[j])
                {
                    li.Attributes.Add("title", vetTip[j]);
                }
            }

        }
    }

    public static List<string> liCritTip = new List<string>(); //PARA ARMAZENAR O QUE VAI APARECER NO TIP
    public static List<string> liCritCod = new List<string>(); //PARA ARMAZENAR O CÓDIGO E COMPARAR COM O VALUE PARA JOGAR NO LISTITEM CERTO O TIP
    
    public static int UltCodCrit = 0;

    //MÉTODO PARA CARREGAR OS CRITÉRIOS GERAIS DO BANCO NO COMPONENTE LISTBOX
    private void CarregaCriGerais()
    {
        //DATASET VAI RECEBER TODOS OS CRITÉRIOS DO BANCO DE DADOS PELO SELECTALL
        DataSet ds = Criterios_Gerais_DB.SelectAll();
        int qtd = ds.Tables[0].Rows.Count;
        UltCodCrit = Criterios_Gerais_DB.SelectUltimoCod();
        //SE HOUVER CRITÉRIOS 
        if (qtd > 0)
        {
            //VAI RODAR TODAS AS LINHAS DO DATASET E JOGAR OS DADOS NA LISTBOX 
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListItem li = new ListItem();
                li.Value = dr["cge_codigo"].ToString();
                li.Text = dr["cge_nome"].ToString();
                li.Attributes.Add("title", dr["cge_descricao"].ToString());
                liCritTip.Add(dr["cge_descricao"].ToString());
                liCritCod.Add(dr["cge_codigo"].ToString());
                //ADICIONANDO CÓDIGO E NOME DO CRITÉRIO AOS CRITÉRIOS ENCONTRADOS NO DATASET
                listaCritGeral.Items.Add(li);

            }

        }
    }

    //EVENTO DO BOTÃO CRIAR NOVO CRITERIO: CRIA UM NOVO CRITÉRIO E MOVE PARA O LISTBOX CRITÉRIOS DO PI
    protected void btnCriarNovoCriterio_Click(object sender, EventArgs e)
    {
        txtNomeCriterio.Style.Clear();
        txtDescricaoCriterio.Style.Clear();

        if (!String.IsNullOrEmpty(txtNomeCriterio.Text.Trim()) && !String.IsNullOrEmpty(txtDescricaoCriterio.Text.Trim()))
        {
            //ADICIONA OS NOVOS CRITÉRIOS NAS LISTAS
            ListItem li = new ListItem();
            int ultimoCod = UltCodCrit;
            li.Value = (ultimoCod + 1).ToString();
            li.Text = txtNomeCriterio.Text;
            li.Attributes.Add("title", txtDescricaoCriterio.Text);
            liCritTip.Add(txtDescricaoCriterio.Text);
            liCritCod.Add(li.Value);
            //ADICIONANDO CÓDIGO E NOME DO CRITÉRIO AOS CRITÉRIOS ENCONTRADOS NO DATASET
            listaCritPi.Items.Add(li);
            updPanelCriterio.Update();
            UltCodCrit += 1;
            CarregaTip();
            Criterios_Gerais cge = new Criterios_Gerais();
            cge.Cge_codigo = Convert.ToInt32(li.Value);
            cge.Cge_nome = li.Text;
            cge.Cge_descricao = txtDescricaoCriterio.Text;
            if (Criterios_Gerais_DB.Insert(cge) != -2)
            {
                lblMsgCriterio.Text = "<span class='glyphicon glyphicon-ok-circle'></span> &nbsp Cadastrado com sucesso.";
                lblMsgCriterio.Style.Add("color", "green");
                txtNomeCriterio.Text = "";
                txtDescricaoCriterio.Text = "";
            }
            else
            {
                lblMsgCriterio.Text = "<span class='glyphicon glyphicon-remove-circle'></span> &nbsp Falha ao cadastrar critério, tente novamente.";
                lblMsgCriterio.Style.Add("color", "red");
            }
            
        }
        else if (String.IsNullOrEmpty(txtNomeCriterio.Text.Trim()) && String.IsNullOrEmpty(txtDescricaoCriterio.Text.Trim()))
        {
            lblMsgCriterio.Text = "<span class='glyphicon glyphicon-remove-circle'></span>&nbsp Campo obrigatório.";
            lblMsgCriterio.Style.Add("color", "red");

            txtNomeCriterio.Style.Add("border", "solid 1px red");
            txtDescricaoCriterio.Style.Add("border", "solid 1px red");
        }
        else if (String.IsNullOrEmpty(txtNomeCriterio.Text.Trim()))
        {
            lblMsgCriterio.Text = "<span class='glyphicon glyphicon-remove-circle'></span>&nbsp Campo obrigatório.";
            lblMsgCriterio.Style.Add("color", "red");

            txtNomeCriterio.Style.Add("border", "solid 1px red");
        }
        else
        {
            lblMsgCriterio.Text = "<span class='glyphicon glyphicon-remove-circle'></span>&nbsp Campo obrigatório.";
            lblMsgCriterio.Style.Add("color", "red");

            txtDescricaoCriterio.Style.Add("border", "solid 1px red");
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa2", "etapa2();", true);
    }

    //EVENTO DO BOTÃO CONTINUAR(ETAPA 3 CRITÉRIOS(PESOS)) : CRIA OS CRITÉRIOS, ATUALIZA O PAINEL E REDIRECIONA PARA PRÓXIMA ETAPA
    protected void btnContinuarEtapa3_Click(object sender, EventArgs e)
    {
        CarregaTip();
        if (listaCritPi.Items.Count >= 1)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa3", "etapa3();", true);
            lblMsgErroAdicionarCriterio.Visible = false;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa2", "etapa2();", true);
            lblMsgErroAdicionarCriterio.Visible = true;
        }
    }

    protected void btnCancelarCriterio_Click(object sender, EventArgs e)
    {
        txtNomeCriterio.Text = "";
        txtDescricaoCriterio.Text = "";
        txtNomeCriterio.Style.Clear();
        txtDescricaoCriterio.Style.Clear();
        lblMsgCriterio.Text = "";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "FechaModalCriacaoCriterio", "FechaModalCriacaoCriterio();", true);
    }

    // ******************* ETAPA 3 - ADICIONAR PESO AOS CRITÉRIOS ********************
    // *******************************************************************************

    //MÉTODO PARA CRIAR OS COMPONENTES LABELS E TEXTBOX PARA COLOCAR OS PESOS NOS CRITÉRIOS
    public void CriarCriterio()
    {
        //QUANTIDADE DE CRITÉRIOS SELECIONADOS PARA O PI
        int tamanho = (Int32)listaCritPi.Items.Count;

        //CRIA VETORES DE COMPONENTES
        Label[] lblCriterios = new Label[tamanho];
        TextBox[] txtCriterios = new TextBox[tamanho];
        Label[] lblLinha = new Label[tamanho];


        for (int i = 0; i < tamanho; i++)
        {
            //CRIANDO ATRIBUTOS PARA OS COMPONENTES
            lblCriterios[i] = new Label();
            lblCriterios[i].ID = "lblCriterio" + (i);
            lblCriterios[i].CssClass = "label";
            lblCriterios[i].Text = listaCritPi.Items[i].ToString() + ": ";

            txtCriterios[i] = new TextBox();
            txtCriterios[i].ID = "txtCriterio" + (i);
            txtCriterios[i].CssClass = "text";
            txtCriterios[i].Attributes["type"] = "Number";
            txtCriterios[i].Attributes["min"] = "1";
            txtCriterios[i].Attributes["max"] = "10";
            txtCriterios[i].ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtCriterios[i].Attributes["onkeyup"] = "funcaoImpedirValor(this.id);";

            lblLinha[i] = new Label();
            lblLinha[i].ID = "lblL" + (i);
            lblLinha[i].Text = String.Format("<br/><br/>");

            //ADICIONANDO OS COMPONENTES PARA O PAINEL 
            PanelCriterios.Controls.Add(lblCriterios[i]);
            PanelCriterios.Controls.Add(txtCriterios[i]);
            PanelCriterios.Controls.Add(lblLinha[i]);


        }

    }


    //VERIFICAR SE OS TEXTBOXS DOS PESOS ESTÃO EM BRANCO OU INVÁLIDOS
    protected int verificarPesoVazio()
    {
        int peso = 0;
        int ret = 0;
        foreach (Control txt in PanelCriterios.Controls)
        {
            if (txt is TextBox)
            {
                TextBox txtCri = (TextBox)txt;
                txtCri.Style.Clear();
                if (String.IsNullOrEmpty(txtCri.Text))
                {
                    return 1;
                }

                peso = Convert.ToInt32(txtCri.Text);
                if ((peso < 1) || (peso > 10))
                {
                    txtCri.Style.Add("border", "1px solid red");
                    ret = 2;
                    lblMsgPesosCriterios.Visible = true;
                }

            }
        }
        return ret;

    }

    //VERIFICAR SE OS TEXTBOXS DOS PESOS ESTÃO EM BRANCO E ACRESCENTA 1 AO PESO
    protected void PreencherPesoVazio()
    {

        foreach (Control txt in PanelCriterios.Controls)
        {
            if (txt is TextBox)
            {
                TextBox txtCri = (TextBox)txt;

                if (String.IsNullOrEmpty(txtCri.Text))
                {
                    txtCri.Text = "1";
                }

            }
        }

    }

    protected void ContinuarEtapa4_Click(object sender, EventArgs e)
    {
        if (verificarPesoVazio() == 1) // QUANDO TEXTBOX ESTÁ VAZIO
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa3", "etapa3();", true);
            //CHAMA A MODAL PESO VAZIO
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostraModalPesoUm", "MostraModalPesoUm();", true);
        }
        else if (verificarPesoVazio() == 2) // VALOR DE PESO INVÁLIDO
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa3", "etapa3();", true);
        }
        else // SECESSO 
        {
            lblMsgPesosCriterios.Visible = false;
            CarregaTipAluno();
            updPanelGrupos.Update();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa4", "Modaletapa4('p13');", true);

        }
    }

    //ADICIONA PESO 1 AS TEXTBOXS VAZIAS  
    protected void btnAdicionarPesoUm_Click(object sender, EventArgs e)
    {
        PreencherPesoVazio();
        updPanelPeso.Update();
        CarregaTip();
        lblMsgPesosCriterios.Visible = false;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "fechaModalPeso1", "fechaModalPeso1();", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa3", "etapa3();", true);
    }


    // ******************** ETAPA 4 - CRIAR GRUPO ******************
    // *************************************************************
    public static List<string> liNomeAlunoTip = new List<string>(); //PARA ARMAZENAR O QUE VAI APARECER NO TIP
    public static List<string> liMatriculaAluno = new List<string>(); //PARA ARMAZENAR O CÓDIGO E COMPARAR COM O VALUE PARA JOGAR NO LISTITEM CERTO O TIP

    private void CarregaTipAluno()
    {
        string[] vetTip = liNomeAlunoTip.ToArray();
        string[] vetCod = liMatriculaAluno.ToArray();
        foreach (ListItem li in listaAlunoGeral.Items)
        {
            for (int j = 0; j < vetCod.Length; j++)
            {
                if (li.Value == vetCod[j])
                {
                    li.Attributes.Add("title", vetTip[j]);
                }
            }

        }

        foreach (ListItem li in listaAlunosGrupo.Items)
        {
            for (int j = 0; j < vetCod.Length; j++)
            {
                if (li.Value == vetCod[j])
                {
                    li.Attributes.Add("title", vetTip[j]);
                }
            }

        }
    }

    //MÉTODO PARA CARREGAR OS ALUNOS GERAIS DO BANCO NO COMPONENTE LISTBOX
    private void CarregaAlunosCadastrarPi()
    {
        //DATASET VAI RECEBER TODOS OS CRITÉRIOS DO BANCO DE DADOS PELO SELECTALL
        

        DataSet dsAluno = new DataSet();
        int codAtr = (int)Session["codAtr"];
        dsAluno = Matricula.ListaPresenca(codAtr);      
        int qtd = dsAluno.Tables[0].Rows.Count;

        //se houver alunos 
        if (qtd > 0)
        {
            //vai rodar todas as linhas do dataset e jogar os dados na listbox 
            foreach (DataRow dr in dsAluno.Tables[0].Rows)
            {
                ListItem li = new ListItem();
                li.Value = dr["Matricula"].ToString();
                li.Text = Funcoes.SplitNomes(dr["Nome"].ToString());
                li.Attributes.Add("title", dr["Nome"].ToString());
                liNomeAlunoTip.Add(dr["Nome"].ToString());
                liMatriculaAluno.Add(dr["Matricula"].ToString());
                //adicionando código e nome do critério aos critérios encontrados no dataset
                listaAlunoGeral.Items.Add(li);

            }
            //gdvdisciplinasenvolvidas.DataSource = dsAluno;
            //gdvdisciplinasenvolvidas.DataBind();
        }
    }

    //EVENTO QUE MOVE OS ALUNOS DA LISTA GERAL PARA A LISTA ESPECÍFICA DE ALUNOS DAQUELE GRUPO 
    protected void listaAlunoGeral_SelectedIndexChanged(object sender, EventArgs e)
    {
        listaAlunosGrupo.Items.Add(listaAlunoGeral.SelectedItem);
        listaAlunoGeral.Items.RemoveAt(listaAlunoGeral.SelectedIndex);
        listaAlunoGeral.ClearSelection();
        listaAlunosGrupo.ClearSelection();
        CarregaTipAluno();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa4", "etapa4();", true);
    }


    //EVENTO QUE MOVE OS ALUNOS DA LISTA ESPECÍFICA PARA A LISTA GERAL DE ALUNOS  
    protected void listaAlunosGrupo_SelectedIndexChanged(object sender, EventArgs e)
    {
        listaAlunoGeral.Items.Add(listaAlunosGrupo.SelectedItem);
        listaAlunosGrupo.Items.RemoveAt(listaAlunosGrupo.SelectedIndex);
        listaAlunoGeral.ClearSelection();
        listaAlunosGrupo.ClearSelection();
        CarregaTipAluno();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa4", "etapa4();", true);
    }

    //EVENTO DO BOTÃO VOLTAR: REDIRECIONA PARA A ETAPA ANTERIOR
    protected void LkbVoltarEtapa3_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa3", "etapa3();", true);
    }

    //REDIRECIONA PARA A PÁGINA AVALIAR GRUPO
    protected void btnVoltarAvaliar_Click(object sender, EventArgs e)
    {
        Response.Redirect("avaliarGrupo.aspx");
    }

    //REDIRECIONA PARA A PÁGINA HOME
    protected void btnVoltarHome2_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }


    //EVENTO DO BOTÃO CONFIRMAR GRUPO: GUARDA O GRUPO ATUAL(COM OS ALUNOS QUE FORAM ESCOLHIDOS) E SUCESSIVAMENTE CRIA NOVO GRUPO
    public static int index = 1; //É UMA CONTROLADORA DO VIEWSTATE (EX: VIEWSTATE["ALUNOS1"]) OBS: PELO INDEX COMEÇAR NO VALOR "1" NÃO HAVERÁ "GRUPO0"
    protected void btnConfirmarGrupo_Click(object sender, EventArgs e)
    {
        CarregaTipAluno();

        if (!String.IsNullOrEmpty(txtNomeGrupo.Text))
        {
            txtNomeGrupo.Style.Clear();

            string listItem = ""; //GUARDA OS ALUNOS ESCOLHIDOS EM UM GRUPO 
            string listItemValue = ""; // GUARDA OS CÓDIGOS DOS ALUNOS

            foreach (ListItem item in listaAlunosGrupo.Items)
            {
                listItem += item.Text + "|"; //GUARDANDO TODOS OS ALUNOS NA MESMA VARIÁVEL E DETERMINANDO UM CARACTER DE "QUEBRA"("|") PARA SEPARAR OS NOMES
                listItemValue += item.Value + "|"; // GUARDANDO TODOS OS CÓDIGOS DOS ALUNOS
            }

            ViewState["NomeGrupo" + index.ToString()] = txtNomeGrupo.Text;
            ViewState["Alunos" + index.ToString()] = listItem;
            ViewState["CodAlunos" + index.ToString()] = listItemValue;

            //ADICIONANDO AO LISTBOX DE GRUPOS, ADICIONA O NOME DO GRUPO AO LISITEM E NO VALUE RECEBE UM INDEX ÚNICO PARA CADA LISTITEM
            listaGrupos.Items.Add(new ListItem("Grupo: " + txtNomeGrupo.Text, index.ToString()));
            //"Clique para Editar o grupo" -> vamos inserir um texto de ajuda fixo na lateral!
            index++;
            listaAlunosGrupo.Items.Clear();
            txtNomeGrupo.Text = "";

            // ** CASO VENHA A CANCELAR OU EXCLUIR, IRÁ RETORNAR AO ESTADO ANTERIOR
            string listItemGeral = "", listItemValueGeral = "";
            foreach (ListItem item in listaAlunoGeral.Items)
            {
                listItemGeral += item.Text + "|"; //ATRIBUINDO TODOS OS ALUNOS DA LISTA ALUNO GERAL
                listItemValueGeral += item.Value + "|";
            }
            Session["alunosGerais"] = listItemGeral; // ** CASO CANCELAR OU EXCLUIR
            Session["CodAlunosGerais"] = listItemValueGeral;
        }
        else
        {
            txtNomeGrupo.Style.Add("border", "1px solid red");

        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa4", "etapa4();", true);
    }

    //EDIÇÃO: QUANDO CLICAR NO LISTBOX DOS GRUPOS CRIADOS(AO CLICAR EM UM DOS GRUPOS INSERIDOS ELE JÁ VAI PARA O MODO DE EDIÇÃO)
    protected void listaGrupos_SelectedIndexChanged(object sender, EventArgs e)
    {
        listaAlunosGrupo.Items.Clear();
        listaAlunoGeral.Items.Clear();

        string listItemGeral = Session["alunosGerais"].ToString();
        string listItemCodGeral = Session["CodAlunosGerais"].ToString();

        string[] arrayAlunosGerais = listItemGeral.Split('|');
        string[] arrayCodAlunosGerais = listItemCodGeral.Split('|');

        //COLOCANDO DE VOLTA OS VALORES NO LISTA DE ALUNO GERAL
        for (int i = 0; i < arrayAlunosGerais.Length - 1; i++)
        {
            listaAlunoGeral.Items.Add(new ListItem(arrayAlunosGerais[i], arrayCodAlunosGerais[i]));
        }

        int indice = Convert.ToInt32(listaGrupos.SelectedValue); //PEGA O VALUE(INDICE DA VIEW) DA LISTBOX QUE IRÁ EDITAR
        string listItem = ViewState["Alunos" + indice.ToString()].ToString();
        string listItemValue = ViewState["CodAlunos" + indice.ToString()].ToString();
        string[] arrayCodAlunos = listItemValue.Split('|');
        string[] arrayAlunos = listItem.Split('|');

        //COLOCAR TODOS OS ALUNOS NA LISTBOX ALUNOS GRUPO
        for (int i = 0; i < arrayAlunos.Length - 1; i++)
        {
            listaAlunosGrupo.Items.Add(new ListItem(arrayAlunos[i], arrayCodAlunos[i])); /*ESTÁ DANDO ERRO AQUI*/
        }

        txtNomeGrupo.Text = ViewState["NomeGrupo" + indice.ToString()].ToString();

        //HABILITANDO OS BOTÕES E RETIRANDO O CSS DE OPACITY 	
        btnConfirmarGrupo.Enabled = false;
        btnConfirmarEdicao.Enabled = true;
        btnExcluirGrupo.Enabled = true;
        btnCancelarEdicao.Enabled = true;
        listaGrupos.Enabled = false;


        btnConfirmarGrupo.Style.Add("opacity", "0.4");
        btnConfirmarGrupo.Style.Add("pointer-events", "none");

        btnConfirmarEdicao.Style.Clear();
        btnExcluirGrupo.Style.Clear();
        btnCancelarEdicao.Style.Clear();

        CarregaTipAluno();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa4", "etapa4();", true);
    }

    //CONFIRMAR EDIÇÃO
    protected void btnConfirmarEdicao_Click(object sender, EventArgs e)
    {
        CarregaTipAluno();
        int indice = Convert.ToInt32(listaGrupos.SelectedValue); //PEGA O INDICE DO GRUPO QUE IRÁ SER EDITADO 

        string listItem = "";
        string listItemCod = "";

        foreach (ListItem item in listaAlunosGrupo.Items) //GUARDA OS ALUNOS DE UM GRUPO EM UM PI
        {
            listItem += item.Text + "|";
            listItemCod += item.Value + "|";
        }

        //RECOLOCANDO OS VALORES EM SEUS DEVIDOS INDICES 	
        ViewState["Alunos" + indice.ToString()] = listItem;
        ViewState["CodAlunos" + indice.ToString()] = listItemCod;
        ViewState["NomeGrupo" + indice.ToString()] = txtNomeGrupo.Text;
        listaGrupos.SelectedItem.Text = "Grupo: " + txtNomeGrupo.Text;

        listaAlunosGrupo.Items.Clear();
        listaGrupos.SelectedIndex = -1;
        txtNomeGrupo.Text = "";

        // ** CASO VENHA A CANCELAR OU EXCLUIR, IRÁ RETORNAR AO ESTADO ANTERIOR
        string listItemGeral = "";
        string listItemCodGeral = "";
        foreach (ListItem item in listaAlunoGeral.Items)
        {
            listItemGeral += item.Text + "|"; //ATRIBUINDO TODOS OS ALUNOS DA LISTA ALUNO GERAL
            listItemCodGeral += item.Value + "|";
        }
        Session["alunosGerais"] = listItemGeral; // ** CASO CANCELAR OU EXCLUIR
        Session["CodAlunosGerais"] = listItemCodGeral;

        btnConfirmarEdicao.Enabled = false;
        btnExcluirGrupo.Enabled = false;
        btnCancelarEdicao.Enabled = false;
        btnConfirmarGrupo.Enabled = true;
        listaGrupos.Enabled = true;
        btnConfirmarGrupo.Style.Clear();

        btnConfirmarEdicao.Style.Add("opacity", "0.4");
        btnExcluirGrupo.Style.Add("opacity", "0.4");
        btnCancelarEdicao.Style.Add("opacity", "0.4");
        btnConfirmarEdicao.Style.Add("pointer-events", "none");
        btnCancelarEdicao.Style.Add("pointer-events", "none");
        btnExcluirGrupo.Style.Add("pointer-events", "none");

        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa4", "etapa4();", true);
    }

    //EXCLUIR
    protected void btnExcluirGrupo_Click(object sender, EventArgs e)
    {
        int indice = Convert.ToInt32(listaGrupos.SelectedIndex); //INDICE QUE CLICOU
        int indice2 = Convert.ToInt32(listaGrupos.SelectedValue); //INDICE DO VIEWSTATE

        listaAlunoGeral.Items.Clear();

        string listItem = ViewState["Alunos" + indice2.ToString()].ToString(); //TODOS ALUNOS DESSE GRUPO
        string[] arrayAlunos = listItem.Split('|');
        string listItemValue = ViewState["CodAlunos" + indice2.ToString()].ToString();
        string[] arrayCodAlunos = listItemValue.Split('|');

        //RETORNANDO OS ALUNOS QUE NÃO TINHAM UM GRUPO PARA A LISTBOX DE ALUNOS GERAIS
        string listItem2 = Session["alunosGerais"].ToString(); //PARA RETORNAR AO ESTADO ORIGINAL
        string[] arrayAlunosGerais = listItem2.Split('|');
        string listItemCodGeral = Session["CodAlunosGerais"].ToString(); //PARA RETORNAR AO ESTADO ORIGINAL
        string[] arrayCodAlunosGerais = listItemCodGeral.Split('|');

        //COLOCANDO DE VOLTA OS VALORES NO LISTA DE ALUNO GERAL
        for (int i = 0; i < arrayAlunosGerais.Length - 1; i++)
        {
            listaAlunoGeral.Items.Add(new ListItem(arrayAlunosGerais[i], arrayCodAlunosGerais[i]));
        }

        //RETORNANDO OS ALUNOS QUE TINHAM UM GRUPO PARA A LISTBOX DE ALUNOS GERAIS
        for (int i = 0; i < arrayAlunos.Length - 1; i++)
        {
            listaAlunoGeral.Items.Add(new ListItem(arrayAlunos[i], arrayCodAlunos[i]));
        }

        ViewState["Alunos" + indice2.ToString()] = null;
        ViewState["CodAlunos" + indice2.ToString()] = null;

        txtNomeGrupo.Text = null;
        listaGrupos.Items.RemoveAt(indice);
        listaAlunosGrupo.Items.Clear();

        // ** CASO VENHA A CANCELAR, IRÁ RETORNAR AO ESTADO ANTERIOR
        string listItemGeral = "";
        string listItemCodigoGeral = "";
        foreach (ListItem item in listaAlunoGeral.Items)
        {
            listItemGeral += item.Text + "|"; //ATRIBUINDO TODOS OS ALUNOS DA LISTA ALUNO GERAL
            listItemCodigoGeral += item.Value + "|";
        }
        Session["alunosGerais"] = listItemGeral; // ** CASO CANCELAR
        Session["CodAlunosGerais"] = listItemCodigoGeral;

        btnConfirmarEdicao.Enabled = false;
        btnExcluirGrupo.Enabled = false;
        btnCancelarEdicao.Enabled = false;
        btnConfirmarGrupo.Enabled = true;
        listaGrupos.Enabled = true;

        btnConfirmarGrupo.Style.Clear();
        btnConfirmarEdicao.Style.Add("opacity", "0.4");
        btnExcluirGrupo.Style.Add("opacity", "0.4");
        btnCancelarEdicao.Style.Add("opacity", "0.4");
        btnConfirmarEdicao.Style.Add("pointer-events", "none");
        btnCancelarEdicao.Style.Add("pointer-events", "none");
        btnExcluirGrupo.Style.Add("pointer-events", "none");
        CarregaTipAluno();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa4", "etapa4();", true);
    }

    //CANCELAR
    protected void btnCancelarEdicao_Click(object sender, EventArgs e)
    {
        listaAlunosGrupo.Items.Clear();
        listaAlunoGeral.Items.Clear();

        string listItem = Session["alunosGerais"].ToString(); //PARA RETORNAR AO ESTADO ORIGINAL
        string[] arrayAlunosGerais = listItem.Split('|');

        string listItemCod = Session["CodAlunosGerais"].ToString(); //PARA RETORNAR AO ESTADO ORIGINAL
        string[] arrayCodAlunosGerais = listItemCod.Split('|');

        //COLOCANDO DE VOLTA OS VALORES NO LISTA DE ALUNO GERAL
        for (int i = 0; i < arrayAlunosGerais.Length - 1; i++)
        {
            listaAlunoGeral.Items.Add(new ListItem(arrayAlunosGerais[i], arrayCodAlunosGerais[i]));
        }

        txtNomeGrupo.Text = null;
        listaGrupos.SelectedIndex = -1;

        // ** CASO VENHA A CANCELAR, IRÁ RETORNAR AO ESTADO ANTERIOR
        string listItemGeral = "";
        string listItemCodGeral = "";

        foreach (ListItem item in listaAlunoGeral.Items)
        {
            listItemGeral += item.Text + "|"; //ATRIBUINDO TODOS OS ALUNOS DA LISTA ALUNO GERAL
            listItemCodGeral += item.Value + "|";
        }
        Session["alunosGerais"] = listItemGeral; // ** CASO CANCELAR
        Session["CodAlunosGerais"] = listItemCodGeral;


        btnConfirmarEdicao.Enabled = false;
        btnExcluirGrupo.Enabled = false;
        btnCancelarEdicao.Enabled = false;
        btnConfirmarGrupo.Enabled = true;
        listaGrupos.Enabled = true;

        btnConfirmarEdicao.Style.Add("opacity", "0.4");
        btnExcluirGrupo.Style.Add("opacity", "0.4");
        btnCancelarEdicao.Style.Add("opacity", "0.4");
        btnConfirmarEdicao.Style.Add("pointer-events", "none");

        btnCancelarEdicao.Style.Add("pointer-events", "none");
        btnExcluirGrupo.Style.Add("pointer-events", "none");
        btnConfirmarGrupo.Style.Clear();
        CarregaTipAluno();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalEtapa4", "etapa4();", true);
    }

    protected void btnFinalizarCriarPi_Click(object sender, EventArgs e)
    {
        //Inserindo na tabela Projeto_Inter
        Projeto_Inter pi = new Projeto_Inter();
        pi.Pri_codigo = Convert.ToInt32(lblCodigoPiAut.Text);
        Semestre_Ano san = new Semestre_Ano();
        san = Semestre_Ano_DB.Select();
        pi.San_codigo = san;
        Projeto_Inter_DB.Insert(pi);
        //Inserindo na tabela Eventos
        for (int i = 0; i < desc.Length; i++)
        {
            Eventos eve = new Eventos();
            eve.Pri_codigo = pi;
            eve.Eve_tipo = desc[i];
            eve.Eve_data = Convert.ToDateTime(dat[i]);
            Eventos_DB.Insert(eve);
        }
        //Inserindo na tabela Atribuicao_PI
        //int iCodDisciplina = 0;
        int[] codDisciplina = listCodDisciplinas.ToArray();
        //int[] vetCodigoDisciplina = new int[tamanhoVetorCodigoDisciplina];
        for (int i = 0; i < codDisciplina.Length; i++)
        {
            Atribuicao_PI atr = new Atribuicao_PI();
            atr.Adi_codigo = codDisciplina[i];
            atr.Pri_codigo = pi;
            Atribuicao_PI_DB.Insert(atr);
        }

            //foreach (Control lbl in PainelDisciplinas.Controls)
            //{
            //    if (lbl is Label)
            //    {
            //        Label lblAdiCodigo = new Label();
            //        lblAdiCodigo = (Label)lbl;
            //        if (lbl.ID == "cphConteudo_cphConteudoCentral_codDisciplina" + iCodDisciplina)
            //        {
            //            Atribuicao_PI atr = new Atribuicao_PI();
            //            atr.Adi_codigo = Convert.ToInt32(lblAdiCodigo.Text);
            //            atr.Pri_codigo = pi;
            //            Atribuicao_PI_DB.Insert(atr);
            //            vetCodigoDisciplina[iCodDisciplina] = atr.Adi_codigo;
            //            iCodDisciplina++;
            //        }
            //    }
            //}
        //Inserindo na tabela Criterio_PI
        int indiceCrit = 0;
        foreach(ListItem li in listaCritPi.Items){            
            TextBox txtPeso = (TextBox) PanelCriterios.FindControl("txtCriterio"+(indiceCrit));
            for (int i = 0; i < codDisciplina.Length; i++)
            {
                Criterio_PI critPi = new Criterio_PI();
                Criterios_Gerais crit = new Criterios_Gerais();
                Atribuicao_PI atr = new Atribuicao_PI();
                atr.Adi_codigo = codDisciplina[i];
                crit.Cge_codigo = Convert.ToInt32(li.Value);
                critPi.Cge_codigo = crit;
                critPi.Adi_codigo = atr;
                critPi.Pri_codigo = pi;
                critPi.Cpi_peso = Convert.ToInt32(txtPeso.Text);
                Criterio_PI_DB.Insert(critPi);
            }
            indiceCrit++;
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalPiCadastrado", "msgFinalizarCadastroPi();", true);
    }



}