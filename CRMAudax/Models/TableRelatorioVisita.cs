using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableRelatorioVisita
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                          //chave estrangeira do Id do usuário        
        [DataMember]
        public DateTime? dataVisita { get; set; }                
        [DataMember]
        public string? nomeEntrevistado { get; set; }
        [DataMember]
        public string? cargo{ get; set; }
        [DataMember]
        public string? nomeIndicacao { get; set; }
       
        [DataMember]
        public long? edificacao { get; set; }
        [DataMember]
        public long? estoque { get; set; }
        [DataMember]
        public long equipamento { get; set; }
        [DataMember]
        public long? producao { get; set; }
        [DataMember]
        public long? funcionarios { get; set; }
        [DataMember]
        public long organizacaoProducao { get; set; }
        [DataMember]
        public long? materiaPrima { get; set; }
        [DataMember]
        public long? impressaoMidia { get; set; }
        [DataMember]
        public long apresentacao { get; set; }
        [DataMember]
        public long? franqueza { get; set; }
        [DataMember]
        public long? conhecimentoNegocio { get; set; }
        [DataMember]
        public long? carater { get; set; }
        [DataMember]
        public long? abertura { get; set; }
        [DataMember]
        public long? conhecimentoConcorrencia { get; set; }
        [DataMember]
        public long? tempoCargo { get; set; }
        [DataMember]
        public long? negocioFamiliar { get; set; }
        [DataMember]
        public long? sazonalidade { get; set; }
        [DataMember]
        public long? parqueIndustrial { get; set; }
        [DataMember]
        public long? certificacaoQUalidade { get; set; }
        [DataMember]
        public long? amplaConcorrencia { get; set; }
        [DataMember]
        public string regimeTributario { get; set; }
        [DataMember]
        public long margemLiquida { get; set; }
        [DataMember]
        public string alteracaoContratual { get; set; }
        [DataMember]
        public DateTime fundacao { get; set; }

        [DataMember]
        public long porcentagemCheque { get; set; }
        [DataMember]
        public long porcentagemDuplicata { get; set; }
        [DataMember]
        public long porcentagemConsumidorFisica { get; set; }
        [DataMember]
        public long porcentagemConsumidorJuridica { get; set; }
        [DataMember]
        public long prazoMedioFornecedores { get; set; }
        [DataMember]
        public long prazoMedioClientes { get; set; }
        [DataMember]
        public string formaEntregaProduto { get; set; }
        [DataMember]
        public long ticketMedio { get; set; }
        [DataMember]
        public string Parecer { get; set; }

    }
}
