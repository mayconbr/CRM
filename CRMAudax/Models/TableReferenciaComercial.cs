using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableReferenciaComercial
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string nomeReferenciaComercial { get; set; }
        [DataMember]
        public string ligacaoProponente { get; set; }
        [DataMember]
        public string observacoes { get; set; }
    }
}
