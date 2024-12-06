using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TablePastas
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long UsuarioId { get; set; }
        public virtual TableUsuario? Usuario { get; set; }
        [DataMember]
        public string NomePasta { get; set; }      
        [DataMember]
        public string? FTP { get; set; }
        [DataMember]
        public DateTime? DataDelete { get; set; }
    }
}
