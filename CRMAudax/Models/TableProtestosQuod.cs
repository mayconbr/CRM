using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableProtestosQuod
    {
        [Key]
        public long Id { get; set; }

        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }
        [DataMember]
        public string? NomeCartorio { get; set; }
        [DataMember]
        public string? Endereco { get; set; }
        [DataMember]
        public string? Cidade { get; set; }
        [DataMember]
        public DateTime? Data { get; set; }
        [DataMember]
        public decimal? Valor { get; set; }
        [DataMember]
        public string? CPFCNPJ { get; set; }
    }
}
