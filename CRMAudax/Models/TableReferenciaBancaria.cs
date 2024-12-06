using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableReferenciaBancaria
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string instituicaoFinanceira { get; set; }
        [DataMember]
        public string agencia { get; set; }
        [DataMember]
        public string conta { get; set; }
        [DataMember]
        public string nomeContato { get; set; }
        [DataMember]
        public string telefone { get; set; }
        [DataMember]
        public string informacoesDesabonadoras { get; set; }
    }
}
