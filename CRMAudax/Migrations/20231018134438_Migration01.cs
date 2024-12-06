using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tipoPessoa = table.Column<long>(type: "bigint", nullable: false),
                    nome = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    cpfCnpj = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    email = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    rua = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    cep = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    numero = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    parecer = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    telefone = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    complemento = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    cidade = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    estado = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    site = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ramoAtividade = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeResponsavel = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    razaoSocialCedente = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataFundacaoCedente = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    faturamento = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    tipo = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    situacao = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataDelete = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    recorreu = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    pep = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    socio = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    referencia = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    cpfSocio = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeSocio = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    enderecoSocio = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    celularSocio = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    DataCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "ColunasKanban",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ordem = table.Column<long>(type: "bigint", nullable: false),
                    DataDelete = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColunasKanban", x => x.Id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Questionarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionarios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Regioes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regioes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Atividades",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    atividade = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataAtividade = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    descricao = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atividades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atividades_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Automoveis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    marca = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    modelo = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ano = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    placa = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    valorFipe = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    valorOnus = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Automoveis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Automoveis_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "CartoesCNPJ",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    pathCartaoCNPJ = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeCartaoCNPJ = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tipoCartaoCNPJ = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartoesCNPJ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartoesCNPJ_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "CnpjCpfs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    pathCpfRgCnhCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeCpfRgCnhCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tipoCpfRgCnhfCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CnpjCpfs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CnpjCpfs_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Comites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    dataComite = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    limite = table.Column<long>(type: "bigint", nullable: true),
                    tipoTitulos = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    faturamentoFiscal = table.Column<long>(type: "bigint", nullable: false),
                    faturamentoReal = table.Column<long>(type: "bigint", nullable: false),
                    valorTotalNegativado = table.Column<long>(type: "bigint", nullable: false),
                    valorTotalProtestos = table.Column<long>(type: "bigint", nullable: false),
                    observacoes = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    contratoSocial = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    comprovanteEndereco = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    inscricaoEstadual = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    rgCpf = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    impostoRenda = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    cartaoCnpj = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    faturamento = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    balanco = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    tempoFundacao = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    mudancaAtividade = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    mudancaSocios = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    limiteAcima = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    concentracaoSacados = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    antecipacaoImoveis = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    entregaMercadoria = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    confirmaNota = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    consultaAcimaMedia = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    consultaCnpj = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    pendenciaFinanceira = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    aumentoProtesto = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    protestoConsumo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    perfilProtesto = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ccf = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    riscoPagamento = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    endividamentoEmergencial = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    aumentoContratoCredito = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    picoConsulta = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    acoesJudiciais = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    recuperacaoJudicial = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    execucaoFiscal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    acaoPorBanco = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    recisaoServico = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    apontamentoRestritivo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    maisempresas = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    empresaMesmoRamo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    socioProtesto = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    socioCheque = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    socioAcao = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    socioApontamentoCPF = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    socioOstentador = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    socioHerdeiro = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    socioGarantiaAdd = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comites_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "ComprovantesEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    pathComprovanteEndereco = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeComprovanteRenda = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tipoComprovanteRenda = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprovantesEnderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprovantesEnderecos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "ContratosSociais",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    pathContratoSocialCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeContratoSocialCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tipoContratoSocialCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratosSociais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratosSociais_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "CustosFixos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    valorAluguel = table.Column<long>(type: "bigint", nullable: false),
                    valorAguaEnergia = table.Column<long>(type: "bigint", nullable: false),
                    folhaPagamento = table.Column<long>(type: "bigint", nullable: false),
                    demaisCustos = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustosFixos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustosFixos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "DecisaoScores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CPFCNPJ = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    Score = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    DataScore = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisaoScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisaoScores_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "EndividamentoCedentes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    nomeInstituicao = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataContratacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    prazoContratado = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    valorParcela = table.Column<long>(type: "bigint", nullable: false),
                    valorQuitacao = table.Column<long>(type: "bigint", nullable: false),
                    situacaoContrato = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndividamentoCedentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndividamentoCedentes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Estoques",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    valorEstoque = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estoques_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Faturamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    pathFaturamentoCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeFaturamentoCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tipoFaturamentoCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faturamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faturamentos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "FaturamentosFiscais",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    pathFaturamentoFiscalCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeFaturamentoFiscalCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tipoFaturamentoFiscalCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaturamentosFiscais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaturamentosFiscais_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "ImpostosRenda",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    pathImpostoRendaCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeImpostoRendaCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tipoImpostoRendaCedente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpostosRenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImpostosRenda_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "MaquinasEquipamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    nomeEquipamento = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    marca = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ano = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    valorFinanciado = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    valorMaquina = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    valorOnus = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaquinasEquipamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaquinasEquipamentos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "NFEs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    numero = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    status = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFEs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NFEs_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Operacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    nBordero = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataOperacao = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operacoes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "PrincipaisFornecedores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    nomeEmpresa = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeSocio = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tempoRelacionamento = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    telefone = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrincipaisFornecedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrincipaisFornecedores_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "PrincipaisFregueses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    nomeEmpresa = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeSocio = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tempoRelacionamento = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    telefone = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrincipaisFregueses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrincipaisFregueses_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "QuodScores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CPFCNPJ = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    Score = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    DataScore = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuodScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuodScores_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "ReferenciaBancarias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    instituicaoFinanceira = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    agencia = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    conta = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeContato = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    telefone = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    informacoesDesabonadoras = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenciaBancarias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenciaBancarias_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "ReferenciaComercials",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    nomeReferenciaComercial = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ligacaoProponente = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    observacoes = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenciaComercials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenciaComercials_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "RelacoesBensImoveis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    localizacao = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    matricula = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    cartorio = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    livro = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    situacao = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    valor = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelacoesBensImoveis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelacoesBensImoveis_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "RelatoriosVisita",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    dataVisita = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    nomeEntrevistado = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    cargo = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeIndicacao = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    edificacao = table.Column<long>(type: "bigint", nullable: true),
                    estoque = table.Column<long>(type: "bigint", nullable: true),
                    equipamento = table.Column<long>(type: "bigint", nullable: false),
                    producao = table.Column<long>(type: "bigint", nullable: true),
                    funcionarios = table.Column<long>(type: "bigint", nullable: true),
                    organizacaoProducao = table.Column<long>(type: "bigint", nullable: false),
                    materiaPrima = table.Column<long>(type: "bigint", nullable: true),
                    impressaoMidia = table.Column<long>(type: "bigint", nullable: true),
                    apresentacao = table.Column<long>(type: "bigint", nullable: false),
                    franqueza = table.Column<long>(type: "bigint", nullable: true),
                    conhecimentoNegocio = table.Column<long>(type: "bigint", nullable: true),
                    carater = table.Column<long>(type: "bigint", nullable: true),
                    abertura = table.Column<long>(type: "bigint", nullable: true),
                    conhecimentoConcorrencia = table.Column<long>(type: "bigint", nullable: true),
                    tempoCargo = table.Column<long>(type: "bigint", nullable: true),
                    negocioFamiliar = table.Column<long>(type: "bigint", nullable: true),
                    sazonalidade = table.Column<long>(type: "bigint", nullable: true),
                    parqueIndustrial = table.Column<long>(type: "bigint", nullable: true),
                    certificacaoQUalidade = table.Column<long>(type: "bigint", nullable: true),
                    amplaConcorrencia = table.Column<long>(type: "bigint", nullable: true),
                    regimeTributario = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    margemLiquida = table.Column<long>(type: "bigint", nullable: false),
                    alteracaoContratual = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    fundacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    porcentagemCheque = table.Column<long>(type: "bigint", nullable: false),
                    porcentagemDuplicata = table.Column<long>(type: "bigint", nullable: false),
                    porcentagemConsumidorFisica = table.Column<long>(type: "bigint", nullable: false),
                    porcentagemConsumidorJuridica = table.Column<long>(type: "bigint", nullable: false),
                    prazoMedioFornecedores = table.Column<long>(type: "bigint", nullable: false),
                    prazoMedioClientes = table.Column<long>(type: "bigint", nullable: false),
                    formaEntregaProduto = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ticketMedio = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatoriosVisita", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatoriosVisita_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "OrdemCartao",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ordemCartao = table.Column<long>(type: "bigint", nullable: false),
                    ColunaId = table.Column<long>(type: "bigint", nullable: false),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdemCartao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdemCartao_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdemCartao_ColunasKanban_ColunaId",
                        column: x => x.ColunaId,
                        principalTable: "ColunasKanban",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "OrdemCliente",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ColunaId = table.Column<long>(type: "bigint", nullable: false),
                    ordemCard = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdemCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdemCliente_ColunasKanban_ColunaId",
                        column: x => x.ColunaId,
                        principalTable: "ColunasKanban",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "OrdemColuna",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ColunaId = table.Column<long>(type: "bigint", nullable: false),
                    ordemColuna = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdemColuna", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdemColuna_ColunasKanban_ColunaId",
                        column: x => x.ColunaId,
                        principalTable: "ColunasKanban",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Perguntas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestionarioId = table.Column<long>(type: "bigint", nullable: false),
                    Pergunta = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Tipopergunta = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perguntas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Perguntas_Questionarios_QuestionarioId",
                        column: x => x.QuestionarioId,
                        principalTable: "Questionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Email = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Senha = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Hash = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    TipoUsuario = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataDelete = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RegiaoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Regioes_RegiaoId",
                        column: x => x.RegiaoId,
                        principalTable: "Regioes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "Respostas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PerguntaId = table.Column<long>(type: "bigint", nullable: false),
                    Resposta = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respostas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Respostas_Perguntas_PerguntaId",
                        column: x => x.PerguntaId,
                        principalTable: "Perguntas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "RespostasCedentes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PerguntaId = table.Column<long>(type: "bigint", nullable: false),
                    RespostaId = table.Column<long>(type: "bigint", nullable: true),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    RespostaAberta = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    DataResposta = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespostasCedentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RespostasCedentes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RespostasCedentes_Perguntas_PerguntaId",
                        column: x => x.PerguntaId,
                        principalTable: "Perguntas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Atividades_ClienteId",
                table: "Atividades",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Automoveis_ClienteId",
                table: "Automoveis",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CartoesCNPJ_ClienteId",
                table: "CartoesCNPJ",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CnpjCpfs_ClienteId",
                table: "CnpjCpfs",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Comites_ClienteId",
                table: "Comites",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprovantesEnderecos_ClienteId",
                table: "ComprovantesEnderecos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratosSociais_ClienteId",
                table: "ContratosSociais",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CustosFixos_ClienteId",
                table: "CustosFixos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisaoScores_ClienteId",
                table: "DecisaoScores",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_EndividamentoCedentes_ClienteId",
                table: "EndividamentoCedentes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Estoques_ClienteId",
                table: "Estoques",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Faturamentos_ClienteId",
                table: "Faturamentos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_FaturamentosFiscais_ClienteId",
                table: "FaturamentosFiscais",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ImpostosRenda_ClienteId",
                table: "ImpostosRenda",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_MaquinasEquipamentos_ClienteId",
                table: "MaquinasEquipamentos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_NFEs_ClienteId",
                table: "NFEs",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Operacoes_ClienteId",
                table: "Operacoes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemCartao_ClienteId",
                table: "OrdemCartao",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemCartao_ColunaId",
                table: "OrdemCartao",
                column: "ColunaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemCliente_ColunaId",
                table: "OrdemCliente",
                column: "ColunaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemColuna_ColunaId",
                table: "OrdemColuna",
                column: "ColunaId");

            migrationBuilder.CreateIndex(
                name: "IX_Perguntas_QuestionarioId",
                table: "Perguntas",
                column: "QuestionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PrincipaisFornecedores_ClienteId",
                table: "PrincipaisFornecedores",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PrincipaisFregueses_ClienteId",
                table: "PrincipaisFregueses",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_QuodScores_ClienteId",
                table: "QuodScores",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenciaBancarias_ClienteId",
                table: "ReferenciaBancarias",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenciaComercials_ClienteId",
                table: "ReferenciaComercials",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_RelacoesBensImoveis_ClienteId",
                table: "RelacoesBensImoveis",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatoriosVisita_ClienteId",
                table: "RelatoriosVisita",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Respostas_PerguntaId",
                table: "Respostas",
                column: "PerguntaId");

            migrationBuilder.CreateIndex(
                name: "IX_RespostasCedentes_ClienteId",
                table: "RespostasCedentes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_RespostasCedentes_PerguntaId",
                table: "RespostasCedentes",
                column: "PerguntaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RegiaoId",
                table: "Usuarios",
                column: "RegiaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atividades");

            migrationBuilder.DropTable(
                name: "Automoveis");

            migrationBuilder.DropTable(
                name: "CartoesCNPJ");

            migrationBuilder.DropTable(
                name: "CnpjCpfs");

            migrationBuilder.DropTable(
                name: "Comites");

            migrationBuilder.DropTable(
                name: "ComprovantesEnderecos");

            migrationBuilder.DropTable(
                name: "ContratosSociais");

            migrationBuilder.DropTable(
                name: "CustosFixos");

            migrationBuilder.DropTable(
                name: "DecisaoScores");

            migrationBuilder.DropTable(
                name: "EndividamentoCedentes");

            migrationBuilder.DropTable(
                name: "Estoques");

            migrationBuilder.DropTable(
                name: "Faturamentos");

            migrationBuilder.DropTable(
                name: "FaturamentosFiscais");

            migrationBuilder.DropTable(
                name: "ImpostosRenda");

            migrationBuilder.DropTable(
                name: "MaquinasEquipamentos");

            migrationBuilder.DropTable(
                name: "NFEs");

            migrationBuilder.DropTable(
                name: "Operacoes");

            migrationBuilder.DropTable(
                name: "OrdemCartao");

            migrationBuilder.DropTable(
                name: "OrdemCliente");

            migrationBuilder.DropTable(
                name: "OrdemColuna");

            migrationBuilder.DropTable(
                name: "PrincipaisFornecedores");

            migrationBuilder.DropTable(
                name: "PrincipaisFregueses");

            migrationBuilder.DropTable(
                name: "QuodScores");

            migrationBuilder.DropTable(
                name: "ReferenciaBancarias");

            migrationBuilder.DropTable(
                name: "ReferenciaComercials");

            migrationBuilder.DropTable(
                name: "RelacoesBensImoveis");

            migrationBuilder.DropTable(
                name: "RelatoriosVisita");

            migrationBuilder.DropTable(
                name: "Respostas");

            migrationBuilder.DropTable(
                name: "RespostasCedentes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "ColunasKanban");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Perguntas");

            migrationBuilder.DropTable(
                name: "Regioes");

            migrationBuilder.DropTable(
                name: "Questionarios");
        }
    }
}
