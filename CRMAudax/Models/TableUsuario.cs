using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableUsuario
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string? Senha { get; set; }
        [DataMember]
        public string Hash { get; set; }
        [DataMember]
        public string TipoUsuario { get; set; }
        [DataMember]
        public bool Ativo { get; set; }
        [DataMember]
        public DateTime? DataDelete { get; set; }
        [DataMember]
        public long? RegiaoId { get; set; }
        public virtual TableRegiao Regiao { get; set; }                          //chave estrangeira do Id da regiao
    }
}
