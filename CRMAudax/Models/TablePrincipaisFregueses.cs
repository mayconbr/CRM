using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TablePrincipaisFregueses
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string nomeEmpresa { get; set; }
        [DataMember]
        public string nomeSocio { get; set; }
        [DataMember]
        public string tempoRelacionamento { get; set; }
        [DataMember]
        public string telefone { get; set; }
    }
}
