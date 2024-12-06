using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableAtividadesCedente
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }

        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do cedente

        [DataMember]
        public string atividade { get; set; }
        [DataMember]
        public DateTime dataAtividade { get; set; }
        [DataMember]
        public string descricao { get; set; }
    }
}
