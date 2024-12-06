using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableArquivoDAP
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string pathArquivoDAP { get; set; }
        [DataMember]
        public string nomeArquivoDAP { get; set; }
        [DataMember]
        public string tipoArquivoDAP { get; set; }
        [DataMember]
        public DateTime? dataEnvio { get; set; }

    }
}
