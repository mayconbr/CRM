using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableComite
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public DateTime? dataComite { get; set; }
        [DataMember]
        public long? limite { get; set; }
        [DataMember]
        public string tipoTitulos { get; set; }
        [DataMember]
        public long faturamentoFiscal { get; set; }
        [DataMember]
        public long faturamentoReal { get; set; }
        [DataMember]
        public long valorTotalNegativado { get; set; }
        [DataMember]
        public long valorTotalProtestos { get; set; }
        [DataMember]
        public string observacoes { get; set; }
        [DataMember]
        public Boolean contratoSocial { get; set; }
        [DataMember]
        public Boolean comprovanteEndereco { get; set; }
        [DataMember]
        public Boolean inscricaoEstadual { get; set; }
        [DataMember]
        public Boolean rgCpf { get; set; }
        [DataMember]
        public Boolean impostoRenda { get; set; }
        [DataMember]
        public Boolean cartaoCnpj { get; set; }
        [DataMember]
        public Boolean faturamento { get; set; }
        [DataMember]
        public Boolean balanco { get; set; }
        [DataMember]
        public Boolean tempoFundacao { get; set; }
        [DataMember]
        public Boolean mudancaAtividade { get; set; }
        [DataMember]
        public Boolean mudancaSocios { get; set; }
        [DataMember]
        public Boolean limiteAcima { get; set; }
        [DataMember]
        public Boolean concentracaoSacados { get; set; }
        [DataMember]
        public Boolean antecipacaoImoveis { get; set; }
        [DataMember]
        public Boolean entregaMercadoria { get; set; }
        [DataMember]
        public Boolean confirmaNota { get; set; }
        [DataMember]
        public Boolean consultaAcimaMedia { get; set; }
        [DataMember]
        public Boolean consultaCnpj { get; set; }
        [DataMember]
        public Boolean pendenciaFinanceira { get; set; }
        [DataMember]
        public Boolean aumentoProtesto { get; set; }
        [DataMember]
        public Boolean protestoConsumo { get; set; }
        [DataMember]
        public Boolean perfilProtesto { get; set; }
        [DataMember]
        public Boolean ccf { get; set; }
        [DataMember]
        public Boolean riscoPagamento { get; set; }
        [DataMember]
        public Boolean endividamentoEmergencial { get; set; }
        [DataMember]
        public Boolean aumentoContratoCredito { get; set; }
        [DataMember]
        public Boolean picoConsulta { get; set; }
        [DataMember]
        public Boolean acoesJudiciais { get; set; }
        [DataMember]
        public Boolean recuperacaoJudicial { get; set; }
        [DataMember]
        public Boolean execucaoFiscal { get; set; }
        [DataMember]
        public Boolean acaoPorBanco { get; set; }
        [DataMember]
        public Boolean recisaoServico { get; set; }
        [DataMember]
        public Boolean apontamentoRestritivo { get; set; }
        [DataMember]
        public Boolean maisempresas { get; set; }
        [DataMember]
        public Boolean empresaMesmoRamo { get; set; }
        [DataMember]
        public Boolean socioProtesto { get; set; }
        [DataMember]
        public Boolean socioCheque { get; set; }
        [DataMember]
        public Boolean socioAcao { get; set; }
        [DataMember]
        public Boolean socioApontamentoCPF { get; set; }
        [DataMember]
        public Boolean socioOstentador { get; set; }
        [DataMember]
        public Boolean socioHerdeiro { get; set; }
        [DataMember]
        public Boolean socioGarantiaAdd { get; set; }
    }
}
