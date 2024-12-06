using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class AuxCliente
    {
        [Key]                                                                      //chave primaria do Id do Proponente
        public long Id { get; set; }
        [DataMember]
        public long tipoPessoa { get; set; }
        [DataMember]
        public string nome { get; set; }
        [DataMember]
        public string cpfCnpj { get; set; }
        [DataMember]
        public string? email { get; set; }
        [DataMember]
        public string? rua { get; set; }
        [DataMember]
        public string? cep { get; set; }
        [DataMember]
        public string? numero { get; set; }
        [DataMember]
        public string? parecer { get; set; }
        [DataMember]
        public string telefone { get; set; }
        [DataMember]
        public string? complemento { get; set; }
        [DataMember]
        public string? cidade { get; set; }
        [DataMember]
        public string? estado { get; set; }
        [DataMember]
        public string? site { get; set; }
        [DataMember]
        public string? ramoAtividade { get; set; }

        [DataMember]
        public string? nomeResponsavel { get; set; }
        [DataMember]
        public string? razaoSocialCedente { get; set; }
        [DataMember]
        public DateTime? dataFundacaoCedente { get; set; }
        [DataMember]
        public decimal? faturamento { get; set; }

        [DataMember]
        public string tipo { get; set; }
        [DataMember]
        public string situacao { get; set; }
        [DataMember]
        public bool status { get; set; }

        [DataMember]
        public DateTime? DataDelete { get; set; }

        [DataMember]
        public long? ordemCartao { get; set; }
        [DataMember]
        public long ColunaId { get; set; }
        public virtual TableColunaKanban Coluna { get; set; }                            //chave estrangeira do Id da Coluna
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                               //chave estrangeira do Id
        public object Usuario { get; internal set; }

        public object Regiao { get; internal set; }

        public bool TemDados { get; internal set; }
        public bool TemContrato { get; internal set; }
        public bool TemEndereco { get; internal set; }
        public bool TemCartaoCnpj { get; internal set; }
        public bool TemFaturamento { get; internal set; }
        public bool TemFiscal { get; internal set; }
        public bool TemImposto { get; internal set; }
        public bool TemDocumento { get; internal set; }
        public int TotalTrueVariables { get; internal set; }
        public bool TemDAP { get; internal set; }
        public bool TemDeclaracao { get; internal set; }
    }
}
