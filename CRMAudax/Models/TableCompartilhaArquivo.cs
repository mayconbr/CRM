using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableCompartilhaArquivo
    {
        [Key]
        public long Id { get; set; }        
        [DataMember]
        public string Hash { get; set; }
        [DataMember]
        public string TipoPasta { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public DateTime? Data { get; set; }
        [DataMember]
        public long? UsuarioId { get; set; }
        public virtual TableUsuario Usuario { get; set; }
        [DataMember]
        public long PastaId { get; set; }
        public virtual TablePastas? Pastas { get; set; }
        [DataMember]
        public long? ArquivoId { get; set; }
        public virtual TableArquivosPastas Arquivo { get; set; }
    }
}
