using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableDecisaoScore
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public string CPFCNPJ { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string? Score { get; set; }
        [DataMember]
        public string? ClasseRisco { get; set; }
        [DataMember]
        public string? Esclarecimento { get; set; }
        [DataMember]
        public string? ProbabilidadeInadimplencia { get; set; }
        [DataMember]
        public DateTime DataScore { get; set; }
        [DataMember]
        public long? QuantidadeProtestos { get; set; }
        [DataMember]
        public string? UltimaOcorrenciaProtestos { get; set; }
        [DataMember]
        public decimal? ValorTotalProtestos { get; set; }
    }
}
