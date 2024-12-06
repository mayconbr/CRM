using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableEndividamentoCedente
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string nomeInstituicao { get; set; }
        [DataMember]
        public DateTime dataContratacao { get; set; }
        [DataMember]
        public string prazoContratado { get; set; }
        [DataMember]
        public long valorParcela { get; set; }
        [DataMember]
        public long valorQuitacao { get; set; }
        [DataMember]
        public string situacaoContrato { get; set; }
    }
}
