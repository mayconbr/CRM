using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableCustosFixos
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id
        [DataMember]
        public long valorAluguel { get; set; }
        [DataMember]
        public long valorAguaEnergia { get; set; }
        [DataMember]
        public long folhaPagamento { get; set; }
        [DataMember]
        public long demaisCustos { get; set; }
    }
}
