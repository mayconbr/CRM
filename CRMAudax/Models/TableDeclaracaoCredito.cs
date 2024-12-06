using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableDeclaracaoCredito
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string pathDeclaracaoC { get; set; }
        [DataMember]
        public string nomeDeclaracaoC { get; set; }
        [DataMember]
        public string tipoDeclaracaoC { get; set; }
        [DataMember]
        public DateTime? dataEnvio { get; set; }

    }
}
