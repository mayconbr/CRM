using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableComprovanteEnderecoCedente
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string pathComprovanteEndereco { get; set; }
        [DataMember]
        public string nomeComprovanteRenda { get; set; }
        [DataMember]
        public string tipoComprovanteRenda { get; set; }
        [DataMember]
        public DateTime? dataEnvio { get; set; }

    }
}
