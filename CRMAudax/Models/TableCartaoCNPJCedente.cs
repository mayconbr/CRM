using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableCartaoCNPJCedente
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string pathCartaoCNPJ { get; set; }
        [DataMember]
        public string nomeCartaoCNPJ { get; set; }
        [DataMember]
        public string tipoCartaoCNPJ { get; set; }
        [DataMember]
        public DateTime? dataEnvio { get; set; }

    }
}
