using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CRMAudax.Models;

namespace CRMAudax;

public partial class MyDbContext : DbContext
{
    public DbSet<TableAtividadesCedente> Atividades { get; set; }
    public DbSet<TableAutomoveis> Automoveis { get; set; }
    public DbSet<TableCliente> Clientes { get; set; }
    public DbSet<TableCpfRgCnh> CnpjCpfs { get; set; }
    public DbSet<TableComite> Comites { get; set; }
    public DbSet<TableComprovanteEnderecoCedente> ComprovantesEnderecos { get; set; }
    public DbSet<TableContratoSocialCedente> ContratosSociais { get; set; }
    public DbSet<TableCustosFixos> CustosFixos { get; set; }
    public DbSet<TableEndividamentoCedente> EndividamentoCedentes { get; set; }
    public DbSet<TableEstoque> Estoques { get; set; }
    public DbSet<TableFaturamentoCedente> Faturamentos { get; set; }
    public DbSet<TableImpostoRendaCedente> ImpostosRenda { get; set; }
    public DbSet<TableMaquinasEquipamentos> MaquinasEquipamentos { get; set; }
    public DbSet<TableOperacaoCedente> Operacoes { get; set; }
    public DbSet<TablePrincipaisFregueses> PrincipaisFregueses { get; set; }
    public DbSet<TablePrincipalFornecedor> PrincipaisFornecedores { get; set; }
    public DbSet<TableReferenciaBancaria> ReferenciaBancarias { get; set; }
    public DbSet<TableReferenciaComercial> ReferenciaComercials { get; set; }
    public DbSet<TableRelacaoBensImoveis> RelacoesBensImoveis { get; set; }
    public DbSet<TableRelatorioVisita> RelatoriosVisita { get; set; }
    public DbSet<TableUsuario> Usuarios { get; set; }
    public DbSet<TableCartaoCNPJCedente> CartoesCNPJ { get; set; }
    public DbSet<TableFaturamentoFiscalCedente> FaturamentosFiscais { get; set; }
    public DbSet<TableNFE> NFEs { get; set; }
    public DbSet<TableRegiao> Regioes { get; set; }
    public DbSet<TableQuestionario> Questionarios { get; set; }
    public DbSet<TablePergunta> Perguntas { get; set; }
    public DbSet<TableResposta> Respostas { get; set; }
    public DbSet<TableRespostaCedente> RespostasCedentes { get; set; }
    public DbSet<TableColunaKanban> ColunasKanban { get; set; }
    public DbSet<TableOrdemColunaKanban> OrdemColuna { get; set; }
    public DbSet<TableOrdemClienteKanban> OrdemCliente { get; set; }
    public DbSet<TableOrdemCartaoKanban> OrdemCartao { get; set; }
    public DbSet<TableQuodScore> QuodScores { get; set; }
    public DbSet<TableDecisaoScore> DecisaoScores { get; set; }
    public DbSet<TableProtestosDecisao> protestosDecisao { get; set; }
    public DbSet<TableProtestosQuod> protestosQuod { get; set; }
    public DbSet<TablePendenciasDecisao> pendenciasDecisao { get; set; }
    public DbSet<TablePendenciasQuod>  pendenciasQuod { get; set; }
    public DbSet<TableEndividamentoSCR> endividamentoSCR { get; set; }
    public DbSet<TableNotificacao> Notificacoes { get; set; }
    public DbSet<TableLeituraNotificacao> LeituraNotificacaos { get; set; }
    public DbSet<TableStatusRotina> StatusRotinas { get; set; }
    public DbSet<TablePastas> Pastas {  get; set; }
    public DbSet<TableArquivosPastas> ArquivosPastas { get; set; }
    public DbSet<TableCompartilhaArquivo> CompartilhaArquivos { get; set; }
    public DbSet<TableArquivoDAP> ArquivosDAP { get; set; }
    public DbSet<TableDeclaracaoCredito> DeclaracoesCredito { get; set; }
    public DbSet<TableLogLogin> LogsLogin {  get; set; }
    public DbSet<TableLogErrorRotina> LogsErroRotina { get; set; }

    #region DB_FACTORY
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            string conn = configuration.GetSection("DatabaseData")["MySQL"];
            optionsBuilder.UseMySql(conn, Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.17-mysql"));
            //optionsBuilder.UseMySql("server=localhost;user id=root;database=db_fiado_garantido", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.17-mysql"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        //modelBuilder.Entity<TableUsuario>().HasIndex(p => p.Email).IsUnique();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    #endregion

}
