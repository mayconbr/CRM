using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TablePendenciasQuod
    {
        [Key]
        public long Id { get; set; }

        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }
        [DataMember]
        public string? PendenciesControlCred { get; set; }
        [DataMember]
        public string? CNPJ { get; set; }
        [DataMember]
        public string? CompanyName { get; set; }
        [DataMember]
        public string? Nature { get; set; }
        [DataMember]
        public decimal? Amount { get; set; }
        [DataMember]
        public string? ContractNumber { get; set; }
        [DataMember]
        public DateTime? DateOcurred { get; set; }
        [DataMember]
        public DateTime? DateInclued { get; set; }
        [DataMember]
        public string? ParticipantType { get; set; }
        [DataMember]
        public string? ApontamentoStatus { get; set; }
        [DataMember]
        public string? City { get; set; }
        [DataMember]
        public string? State { get; set; }
        [DataMember]
        public string? ProcessNumber { get; set; }
        [DataMember]
        public string? Comarca { get; set; }
        [DataMember]
        public string? Forum { get; set; }
        [DataMember]
        public string? Vara { get; set; }
        [DataMember]
        public string? Name { get; set; }
        [DataMember]
        public string? ProcessType { get; set; }
        [DataMember]
        public string? ProcessAuthor { get; set; }
        [DataMember]
        public string? JusticeType { get; set; }
    }
}
