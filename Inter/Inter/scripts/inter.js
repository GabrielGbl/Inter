﻿//Variavel Global - Usada para acessar todas variaveis
var global = "";
var globalCor = "";

//Function inicia após caregar a página
$(document).ready(function () {
    Atualiza();
    Dia();

    //A cada 30 segundos chama a function Atualiza

    $("#conteudo").slideUp(500, function () {
        idDiv = "p1";            

        global = idDiv;
        $("#" + idDiv).show();
        $("#c" + idDiv).css("visibility", "visible");
        $("#conteudo").slideDown(500);

    });
});

//Function que exibe o conteudo das Div's conforme o parametro utilizado nela
function Mostra(idDiv) {

    $("#conteudo").slideUp(500, function () {
        $("#" + global).hide();
        $("#c" + global).css("visibility", "hidden");
        global = idDiv;
        $("#" + idDiv).show();
        $("#conteudo").slideDown(500);

    });

}

//Function que pega a hora do servidor e retorna com ela trabalhada

function myFunction() {
    var dt = new Date();
    var hora = dt.getHours();
    var minuto = dt.getMinutes();
    var currentTime;

    if (minuto < 10) {
        minuto = "0" + dt.getMinutes();
    }

    if (hora < 10) {
        hora = "0" + dt.getHours();
    }

    currentTime = hora + ":" + minuto;
    return currentTime;

}


//Function que pega a data do servidor e retorna com ela trabalhada
function Dia() {
    Hoje = new Date();
    Data = Hoje.getDate();
    Dia = Hoje.getDay();
    Mes = Hoje.getMonth();
    Ano = Hoje.getFullYear();

    if (Data < 10) {
        Data = "0" + Data;
    }

    NomeMes = new Array(12)
    NomeMes[0] = "Janeiro"
    NomeMes[1] = "Fevereiro"
    NomeMes[2] = "Março"
    NomeMes[3] = "Abril"
    NomeMes[4] = "Maio"
    NomeMes[5] = "Junho"
    NomeMes[6] = "Julho"
    NomeMes[7] = "Agosto"
    NomeMes[8] = "Setembro"
    NomeMes[9] = "Outubro"
    NomeMes[10] = "Novembro"
    NomeMes[11] = "Dezembro"

    //Imprime a varivel currentDay na tag dia
    var currentDay = Data + " de " + NomeMes[Mes] + " " + Ano;
    document.getElementById("dia").innerHTML = currentDay;
}

//Imprime a varivel myFunction na tag hora
function Atualiza() {
    document.getElementById("hora").innerHTML = myFunction();
    setTimeout('Atualiza()', 1000);
}

//Function que confere login e senha
$('#enviar').click(function () {
    var nome = $('#login').val();
    var idade = $('#senha').val();

    if ($.trim(nome) == 'administrador') {
        $('#choose').modal({});
    } else {
        if ($.trim(nome) == 'professor') {
            window.location = "../Usuario/user.aspx";
        } else {
            $('#incorreto').modal({});
        }
    }
});

var cont = 1; //para nao chamar a tela Home
var iconeClicado = ""; //para voltar nessa pagina




$(document).ready(function () {
    $('#icone1').addClass('corIcone');
    globalCor = "#icone1";

    //Function alterar a cor do ícone do menu
    $('#icone1').click(function () {
        $(globalCor).removeClass('corIcone');
        $('#icone1').addClass('corIcone');
        globalCor = "#icone1";        
    });

    $('#icone2').click(function () {
        $(globalCor).removeClass('corIcone');
        $('#icone2').addClass('corIcone');
        globalCor = "#icone2";
        iconeClicado = "p2";
    });

    $('#icone3').click(function () {
        $(globalCor).removeClass('corIcone');
        $('#icone3').addClass('corIcone');
        globalCor = "#icone3";
        iconeClicado = "p3";
    });

    $('#icone4').click(function () {
        $(globalCor).removeClass('corIcone');
        $('#icone4').addClass('corIcone');
        globalCor = "#icone4";
        iconeClicado = "p4";
    });

    $('#icone5').click(function () {
        $(globalCor).removeClass('corIcone');
        $('#icone5').addClass('corIcone');
        globalCor = "#icone5";
        iconeClicado = "p5";
    });

    $('#icone6').click(function () {
        $(globalCor).removeClass('corIcone');
        $('#icone6').addClass('corIcone');
        globalCor = "#icone6";
        iconeClicado = "p6";
    });

    $('#icone7').click(function () {
        $(globalCor).removeClass('corIcone');
        $('#icone7').addClass('corIcone');
        globalCor = "#icone7";        
    });

    $('#icone8').click(function () {
        $(globalCor).removeClass('corIcone');
        $('#icone8').addClass('corIcone');
        globalCor = "#icone8";
        iconeClicado = "p8";
    });

    $('#icone9').click(function () {
        $(globalCor).removeClass('corIcone');
        $('#icone9').addClass('corIcone');
        globalCor = "#icone9";
        iconeClicado = "p9";
    });

    $('#icone10').click(function () {
        $(globalCor).removeClass('corIcone');
        $('#icone10').addClass('corIcone');
        globalCor = "#icone10";
        iconeClicado = "p15";
    });

    

    //sortable mover com duplo clique
    $("ul#sortable1 li").dblclick(function () {
        $(this).appendTo("ul#sortable2");        
    });

    $('.restaurar').click(function () {
        $(function () {
            $("#boxRestauraSistema").dialog({
                width: 400,
                height: 200,
                modal: true,
                resizable: false,
                draggable: false,
                buttons: {
                    "Confirmar": function () {


                        $(this).dialog("close");

                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }


            });
        });

    });



});


