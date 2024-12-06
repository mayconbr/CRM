using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableAutomoveis
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }
        [DataMember]
        public string? marca { get; set; }
        [DataMember]
        public string? modelo { get; set; }
        [DataMember]
        public string? ano { get; set; }
        [DataMember]
        public string? placa { get; set; }
        [DataMember]
        public string? valorFipe { get; set; }
        [DataMember]
        public string? valorOnus { get; set; }
    }
}
