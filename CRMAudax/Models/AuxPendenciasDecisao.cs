using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class AuxPendenciasDecisao
    {
        [Key]
        public long Id { get; set; }

        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }
        [DataMember]
        public string? Agencia { get; set; }
        [DataMember]
        public string? Banco { get; set; }
        [DataMember]
        public string? Cidade { get; set; }
        [DataMember]
        public DateTime? Data { get; set; }
        [DataMember]
        public string? Motivo { get; set; }
        [DataMember]
        public string? Origem { get; set; }
        [DataMember]
        public string? Avalista { get; set; }
        [DataMember]
        public string? Contrato { get; set; }
        [DataMember]
        public string? Modalidade { get; set; }
        [DataMember]
        public decimal? Valor { get; set; }
        [DataMember]
        public string? Abreviacao { get; set; }
        [DataMember]
        public long? Contagem { get; set; }
        

    }
}
