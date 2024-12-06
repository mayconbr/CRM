using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableNFE
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }
        [DataMember]
        public string? numero { get; set; }
        [DataMember]
        public string? status { get; set; }

        [DataMember]
        public DateTime? DataNota { get; set; }
    }
}
