using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableMaquinasEquipamentos
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string nomeEquipamento { get; set; }
        [DataMember]
        public string marca { get; set; }
        [DataMember]
        public string ano { get; set; }
        [DataMember]
        public string? valorFinanciado { get; set; }
        [DataMember]
        public string? valorMaquina { get; set; }
        [DataMember]
        public string? valorOnus { get; set; }
    }
}
