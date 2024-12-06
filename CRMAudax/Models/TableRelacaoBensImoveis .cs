using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableRelacaoBensImoveis
    {
        [Key]                                                                      
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                           
        [DataMember]
        public string localizacao { get; set; }
        [DataMember]
        public string? matricula { get; set; }
        [DataMember]
        public string? cartorio { get; set; }
        [DataMember]
        public string? livro { get; set; }
        [DataMember]
        public string situacao { get; set; }
        [DataMember]
        public string valor { get; set; }
    }
}
