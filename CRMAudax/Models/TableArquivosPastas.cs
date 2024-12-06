using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableArquivosPastas
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long UsuarioId { get; set; }
        public virtual TableUsuario? Usuario { get; set; }
        [DataMember]
        public long PastaId { get; set; }
        public virtual TablePastas? Pastas { get; set; }
        [DataMember]
        public string pathArquivo { get; set; }
        [DataMember]
        public string nomeArquivo { get; set; }
        [DataMember]
        public string tipoArquivo { get; set; }
        [DataMember]
        public DateTime? dataEnvio { get; set; }
    }
}
