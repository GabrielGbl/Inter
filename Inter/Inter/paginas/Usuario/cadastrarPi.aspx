﻿<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Usuario/MasterPageMenuPadrao.master" AutoEventWireup="true" Inherits="paginas_Usuario_cadastrarPi" CodeBehind="cadastrarPi.aspx.cs" %>


<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudoCentral" runat="Server">
    <script type="text/javascript">
        function pegarCriterio() {
            var dadosCrit = "";

            $('#sortable4 > li').each(function () {
                dadosCrit += "|" + $(this).html();
            });

            $('#hidden').val(dadosCrit);
        }


    </script>

    <script type="text/javascript">

        $(document).ready(function () {

            // ALTERAR COR DO ÍCONE NO MENU LATERAL 
            $('#cphConteudo_icone5').addClass('corIcone');



        });
    </script>

    <script>
        $(document).ready(function () {
            $("#finalizarCriarPi").click(function () {
                //ALUNOS DE UM GRUPO
                var dadosUl = "";
                $("#sortable6 > li").each(function () {
                    dadosUl += "|" + $(this).html();
                });

            });
        });
    </script>

    <script>
        //CADASTRAR NOVO CRITÉRIO   
        function ul() {
            var nome = $("#ContentPlaceHolder1_txtNomeCriterio").val();
            $("#sortable4").append("<li class=\"ui-state-default\">" + nome + "</li>");
        }
        // function finalizarCadastroPI() {
        //     $("#sortable6 > li").each(function () {
        //             alert($(this).html());
        //     });      
        //}        

    </script>

    <!--SORTABLE-->
    <script>
        $(function () {
            $("#sortable3, #sortable4").sortable({
                connectWith: ".connectedSortable",
            }).disableSelection();
        });

        $(function () {
            $("#sortable5, #sortable6").sortable({
                connectWith: ".connectedSortable",
            }).disableSelection();
        });
    </script>



    <!-- CADASTRAR PI (P5) -->

    <div id="p1" class="first">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Cadastrar PI</h3>
            </div>
            <div class="panel-body-usuarios">
                <asp:HiddenField ID="hidden" runat="server" ClientIDMode="Static" />

                <table id="tabelaCadastrarPi" class="table">

                    <tr>
                        <td>
                            <asp:Label ID="lblCodigoPi" CssClass="label" runat="server" Text="Código PI: "></asp:Label>
                        </td>

                        <td>
                            <asp:Label ID="lblCodigoPiAut" runat="server" Text="01"></asp:Label>
                        </td>

                        <td>
                            <asp:Label ID="lblCurso" CssClass="label" runat="server" Text="Curso: "></asp:Label>
                        </td>

                        <td>
                            <asp:Label ID="lblCursoAut" runat="server" Text=""></asp:Label>
                        </td>

                        <td>
                            <asp:Label ID="lblSemestre" CssClass="label" runat="server" Text="Semestre: "></asp:Label>
                        </td>

                        <td>
                            <asp:Label ID="lblSemestreAut" runat="server" Text=""></asp:Label>

                        </td>


                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="lblAno" CssClass="label" runat="server" Text="Ano: "></asp:Label>
                        </td>

                        <td>
                            <asp:Label ID="lblAnoAut" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSemestreAno" CssClass="label" runat="server" Text="Semestre Ano: "></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSemestreAnoAut" runat="server" Text=""></asp:Label>
                        </td>
                        <td colspan="2"></td>
                    </tr>

                </table>


                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblDiscipEnvolvidas" CssClass="label" runat="server" Text="Disciplinas envolvidas: "></asp:Label>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td>
                            <div style="text-align: left; margin-left: 20px; border: 1px solid #DDD; padding: 10px;">
                                Banco de Dados<br />
                                Engenharia de Software 3<br />
                                Interação Humano Computador<br />
                                Programação em Scripts<br />
                            </div>
                        </td>
                    </tr>

                </table>
                <br />




                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblDatas" CssClass="label" runat="server" Text="Data de Eventos: "></asp:Label>
                        </td>
                        <td>
                            <%--<asp:Button ID="Button2" CssClass="btn btn-default" runat="server" data-toggle="modal" data-target="#myModal1" Text="Button" />--%>
                            <button type="button" class="btn btn-default" id="btnAdicionarDatas" data-toggle="modal" data-target="#myModal1" title="Adicionar evento ao PI">
                                <span class="glyphicon glyphicon-plus"></span>&nbsp Datas</button>
                        </td>
                    </tr>
                </table>

                <div id="containerDatas">
                </div>


                <table class="tableBotoes">
                    <tr>
                        <td class="colunaBotoes"></td>
                        <td class="colunaBotoes"></td>
                        <td class="colunaBotoes">
                            <asp:Button ID="btnContinuarEtapa2" OnClientClick="Mostra('p10'); return false;" ClientIDMode="Static"
                                CssClass="btn btn-default" runat="server" Text="Continuar" title="Ir para adicionar critérios" />
                        </td>
                    </tr>
                </table>
                <br />
                <p style="text-align: right; font-weight: bold; margin-top: 5px;">Passo 1 de 4</p>
            </div>
        </div>

    </div>


    <!-- Adicionar critérios (p10) -->

    <div id="p10" class="first">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Adicionar Critérios</h3>
            </div>
            <div class="panel-body-usuarios">

                <table style="width: 60%; margin-left: -10px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblCriterioGeral" CssClass="label" runat="server" Text="Critérios Gerais"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblCriterioPi" CssClass="label" runat="server" Text="Critérios PI"></asp:Label></td>
                        <td></td>
                    </tr>

                    <tr>
                        <td>
                            <div style="width: 200px; height: 230px; overflow-y: auto;">
                                <ul id="sortable3" class="connectedSortable">
                                    <asp:Literal runat="server" ID="lblCriGerais"></asp:Literal>
                                </ul>
                            </div>
                        </td>

                        <td>
                            <div style="width: 200px; height: 230px; overflow-y: auto;">
                                <ul id="sortable4" class="connectedSortable">
                                </ul>
                            </div>
                        </td>

                        <td></td>
                    </tr>

                    <tr>
                        <td colspan="3">
                            <br />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <button type="button" class="btn btn-default" id="btnVoltarEtapa2" onclick="Mostra('p1');" title="Voltar ao cadastro de PI">
                                <span class="glyphicon glyphicon-arrow-left"></span>&nbsp Voltar</button></td>
                        <td>
                            <button type="button" class="btn btn-default" id="" data-toggle="modal" data-target="#myModalCadastrarCri" title="Ir para cadastro de critérios">
                                <span class="glyphicon glyphicon-plus"></span>&nbsp Cadastrar Critérios
                            </button>
                        </td>
                        <td>
                            <asp:Button ID="btnContinuarEtapa3" runat="server" Text="Continuar" CssClass="btn btn-default"
                                ToolTip="Ir para adicionar peso aos critérios"
                                OnClientClick="pegarCriterio();" OnClick="btnContinuarEtapa3_Click" />
                        </td>
                    </tr>
                </table>

                <p style="text-align: right; font-weight: bold; margin-top: 5px;">Passo 2 de 4</p>
            </div>
        </div>
    </div>

    <!-- Adicionar peso aos critérios (p12) -->


    <div id="p12" class="first">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Adicionar Peso aos Critérios</h3>
            </div>
            <div class="panel-body-usuarios">
                <table style="width: 30%; margin-left: 5%;">
                    <tr>
                        <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <button type="button" class="btn btn-default" id="" onclick="Mostra('p10');" title="Voltar para Adicionar Critérios">
                                <span class="glyphicon glyphicon-arrow-left"></span>&nbsp Voltar</button></td>
                        <td>
                            <%--<asp:Button ID="ContinuarEtapa4" OnClientClick="openModal(); return false;" OnClick="ContinuarEtapa4_Click" CssClass="btn btn-default" runat="server" Text="Continuar" title="" />--%>
                            <button type="button" class="btn btn-default" id="ContinuarEtapa4" onclick="Mostra('p13');" title="Ir para Criar Grupos">Continuar</button>
                        </td>
                    </tr>
                </table>


                <p style="text-align: right; font-weight: bold;">Passo 3 de 4</p>
            </div>
        </div>
    </div>


    <!-- Criar Grupos (p13) -->

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>


            <div id="p13" class="first">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Criar Grupos</h3>
                    </div>
                    <div class="panel-body-usuarios">

                        <table style="width: 70%; margin-left: -10px">
                            <tr>
                                <td>
                                    <asp:Label ID="lblNomeGrupo" CssClass="label" runat="server" Text="Nome do Grupo: "></asp:Label></td>
                                <td colspan="2" style="text-align: start;">
                                    <asp:TextBox ID="txtNomeGrupo" CssClass="text" Width="95%" runat="server"></asp:TextBox></td>

                            </tr>

                            <tr>
                                <td colspan="3">
                                    <br />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblAlunoDisciplina" CssClass="label" runat="server" Text="Alunos da Disciplina"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblAlunoGrupo" CssClass="label" runat="server" Text="Alunos do Grupo"></asp:Label></td>
                                <td></td>
                            </tr>


                            <tr>

                                <td>                                    

                                    <div style="width: 230px; height: 230px; overflow-y: auto;">
                                        <asp:ListBox ID="listaAlunoGeral" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="listaAlunoGeral_SelectedIndexChanged" ClientIDMode="Static">
                                            <asp:ListItem>Bruno</asp:ListItem>
                                            <asp:ListItem>Mariazinha</asp:ListItem>
                                        </asp:ListBox>

                                    </div>
                                </td>

                                <td>
                                    <div style="width: 230px; height: 230px; overflow-y: auto;">
                                        <asp:ListBox ID="listaAlunosGrupo" runat="server" OnSelectedIndexChanged="listaAlunosGrupo_SelectedIndexChanged" AutoPostBack="true"
                                            ClientIDMode="Static"></asp:ListBox>
                                    </div>

                                </td>
                                <td></td>

                            </tr>

                            <tr>
                                <td colspan="3">
                                    <br />
                                </td>
                            </tr>

                            <tr>
                                <td>

                                    <button type="button" class="btn btn-default" id="btnVoltarEtapa3" onclick="Mostra('p12');" title="Voltar para adicionar peso aos critérios ">
                                        <span class="glyphicon glyphicon-arrow-left"></span>&nbsp Voltar</button></td>
                                <td>
                                    <button type="button" class="btn btn-default" id="btnAdicionarGrupo" title="Criar outro grupo">
                                        <span class="glyphicon glyphicon-plus"></span>&nbsp Adicionar Grupo</button></td>
                                <td>
                                    <button type="button" class="btn btn-default" id="finalizarCriarPi" onclick="finalizarCadastroPI();" data-toggle="modal" data-target="#myModalPiCadastrado" title="Finalizar criação de PI">
                                        <span class="glyphicon glyphicon-ok-circle"></span>&nbsp Finalizar</button></td>
                            </tr>

                        </table>

                        <p style="text-align: right; font-weight: bold; margin-top: 5px;">Passo 4 de 4</p>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <!-- Modal Cadastrar Datas de  Eventos -->    
    <div class="modal fade" data-backdrop="static" id="myModal1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true" style="font-size: 35px;">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel1">Cadastrar Datas de Eventos</h4>
                </div>
                <div class="modal-body">
                    <table style="width: 95%;">
                        <tr>
                            <td>
                                <asp:Label ID="lblDescricaoData" CssClass="label" runat="server" Text="Descrição da Data: "></asp:Label>
                            </td>

                            <td>
                                <asp:TextBox ID="txtDescricaoData" CssClass="textData" runat="server" ClientIDMode="Static"></asp:TextBox>

                            </td>
                            <td>
                                <asp:Label ID="lblDescDataMsgErro" runat="server" ClientIDMode="Static"></asp:Label></td>
                        </tr>

                        <tr>
                            <td colspan="3">
                                <br />
                            </td>
                        </tr>

                        <tr style="text-align: left;">
                            <td>
                                <asp:Label ID="lblData" runat="server" CssClass="label" Text="Data: "></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtData" CssClass="textData" TextMode="Date" runat="server" ClientIDMode="Static"></asp:TextBox>

                            </td>
                            <td>
                                <asp:Label ID="lblDataMsgErro" runat="server" ClientIDMode="Static"></asp:Label></td>
                        </tr>
                    </table>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-default" id="btnCancelarData" data-dismiss="modal" title="Cancelar Inserção de Datas">
                        <span class="glyphicon glyphicon-remove"></span>&nbsp Cancelar</button>

                    <button type="button" class="btn btn-default" id="btnConfirmarData" title="Confirmar Inserção">
                        <span class="glyphicon glyphicon-ok"></span>&nbsp Confirmar</button>


                </div>
            </div>
        </div>
    </div>

    <!-- Modal Cadastrar Critérios -->
    <div class="modal fade" data-backdrop="static" id="myModalCadastrarCri" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel3">Cadastrar Critérios</h4>
                </div>
                <div class="modal-body">
                    <table style="width: 70%; margin-left: 5%;">
                        <tr style="text-align: left;">
                            <td>
                                <asp:Label ID="lblNomeCriterio" runat="server" CssClass="label" Text="Nome Critério: "></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtNomeCriterio" CssClass="textCriterio" runat="server"></asp:TextBox></td>

                        </tr>

                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>

                        <tr style="text-align: left;">
                            <td>
                                <asp:Label ID="lblDescricaoCriterio" runat="server" CssClass="label" Text="Descrição do Critério: "></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtDescricaoCriterio" CssClass="textCriterio" runat="server"></asp:TextBox></td>

                        </tr>


                    </table>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-default" id="" data-dismiss="modal" title="Cancelar Inserção">
                        <span class="glyphicon glyphicon-remove"></span>&nbsp Cancelar</button>

                    <button type="button" class="btn btn-default" id="AdicionarCriterios" title="Adicionar mais Critérios">
                        <span class="glyphicon glyphicon-plus"></span>&nbsp Critérios
                    </button>

                    <button type="button" class="btn btn-default" id="btnInserirCriterio" data-dismiss="modal" onclick="ul();" title="Confirmar Inserção">
                        <span class="glyphicon glyphicon-ok"></span>&nbsp Confirmar</button>


                </div>
            </div>
        </div>
    </div>


    <!-- MODAL ADICIONAR PESO 1 -->

    <div class="modal fade" data-backdrop="static" id="myModalPesoUm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div class="modal-body">
                    <h3 style="font-weight: bolder; text-align: center; color: #1f1f1f">
                        <span style="color: #960d10;" class="glyphicon glyphicon-exclamation-sign"></span>&nbsp Deseja atribuir peso "1" aos pesos de critérios não preenchidos?</h3>
                </div>

                <div class="modal-footer">

                    <button type="button" class="btn btn-default" id="" data-dismiss="modal" onclick="Mostra('p13');" title="O sistema atribuirá peso 1 aos campos vazios">Sim</button>

                    <button type="button" class="btn btn-default" id="" data-dismiss="modal">Não</button>

                </div>
            </div>
        </div>
    </div>



    <!-- MODAL PI CADASTRADO -->

    <div class="modal fade" data-backdrop="static" id="myModalPiCadastrado" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <!--   <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button> -->

                </div>
                <div class="modal-body">
                    <h1 style="font-size: 30px; font-weight: bolder; text-align: center; color: #1f1f1f">
                        <span style="color: #09a01c;" class="glyphicon glyphicon-ok-sign"></span>&nbsp PI Cadastrado com Sucesso!</h1>
                </div>

                <div class="modal-footer">

                    <button type="button" class="btn btn-default" id="btnVoltarHome2" data-dismiss="modal" title="Voltar para a Home do sistema">Voltar para Home</button>

                    <button type="button" class="btn btn-default" id="btnVoltarAvaliar" data-dismiss="modal" title="Ir para a avaliação dos grupos do PI">Avaliar grupos</button>

                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {

            $('#btnVoltarHome2').click(function () {
                window.location = "home.aspx";

            });

            $('#btnVoltarAvaliar').click(function () {
                window.location = "avaliarGrupo.aspx";

            });
        });
    </script>

    <div id="boxCampoVazio" title="Preencha todos os campos!" style="display: none;">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span>&nbsp Todos os campos devem ser preenchidos </p>
    </div>


    <!-- dialogs -->
    <div id="boxDesejaExcluir" title="Excluir Evento!" style="display: none;">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span>&nbsp Tem certeza que deseja excluir o evento? </p>
    </div>
</asp:Content>

