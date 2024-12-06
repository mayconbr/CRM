using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableCpfRgCnh
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string pathCpfRgCnhCedente { get; set; }
        [DataMember]
        public string nomeCpfRgCnhCedente { get; set; }
        [DataMember]
        public string tipoCpfRgCnhfCedente { get; set; }
        [DataMember]
        public DateTime? dataEnvio { get; set; }
    }
}
