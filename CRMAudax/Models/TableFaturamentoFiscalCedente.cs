using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableFaturamentoFiscalCedente
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do usuário
        [DataMember]
        public string pathFaturamentoFiscalCedente { get; set; }
        [DataMember]
        public string nomeFaturamentoFiscalCedente { get; set; }
        [DataMember]
        public string tipoFaturamentoFiscalCedente { get; set; }
        [DataMember]
        public DateTime? dataEnvio { get; set; }
    }
}
