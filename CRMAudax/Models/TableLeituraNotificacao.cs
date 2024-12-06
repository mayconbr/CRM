using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableLeituraNotificacao
    {
        [Key]
        public long Id { get; set; }

        [DataMember]
        public long UsuarioId { get; set; }
        public virtual TableUsuario Usuario { get; set; }

        [DataMember]
        public long NotificacaoId { get; set; }
        public virtual TableNotificacao Notificacao { get; set; }
    }
}
