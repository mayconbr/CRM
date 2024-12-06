using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableFaturamentoCedente
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string pathFaturamentoCedente { get; set; }
        [DataMember]
        public string nomeFaturamentoCedente { get; set; }
        [DataMember]
        public string tipoFaturamentoCedente { get; set; }
        [DataMember]
        public DateTime? dataEnvio { get; set; }
    }
}
