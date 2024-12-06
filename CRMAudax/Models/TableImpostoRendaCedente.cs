using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableImpostoRendaCedente
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string pathImpostoRendaCedente { get; set; }
        [DataMember]
        public string nomeImpostoRendaCedente { get; set; }
        [DataMember]
        public string tipoImpostoRendaCedente { get; set; }
        [DataMember]
        public DateTime? dataEnvio { get; set; }
    }
}
