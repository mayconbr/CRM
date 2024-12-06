using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableContratoSocialCedente
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string pathContratoSocialCedente { get; set; }
        [DataMember]
        public string nomeContratoSocialCedente { get; set; }
        [DataMember]
        public string tipoContratoSocialCedente { get; set; }
        [DataMember]
        public DateTime? dataEnvio { get; set; }
    }
}
