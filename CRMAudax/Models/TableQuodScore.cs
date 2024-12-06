using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableQuodScore
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public string CPFCNPJ { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string Score { get; set; }
        [DataMember]
        public DateTime DataScore { get; set; }
    }
}
