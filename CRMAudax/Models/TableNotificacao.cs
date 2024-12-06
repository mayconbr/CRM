using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableNotificacao
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }
        [DataMember]
        public string Informacao { get; set; }
        [DataMember]
        public bool? Score { get; set; }
        [DataMember]
        public bool? Status { get; set; }
        [DataMember]
        public DateTime DataNotificacao { get; set; }
    }
}
