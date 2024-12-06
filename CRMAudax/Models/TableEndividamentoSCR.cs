using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableEndividamentoSCR
    {
        [Key]
        public long Id { get; set; }

        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }
        [DataMember]
        public string? codigoDoCliente { get; set; }
        [DataMember]
        public string? dataBaseConsultada { get; set; }
        [DataMember]
        public string? dataInicioRelacionamento { get; set; }
        [DataMember]
        public string? carteiraVencerAte30diasVencidosAte14dias { get; set; }
        [DataMember]
        public string? carteiraVencer31a60dias { get; set; }
        [DataMember]
        public string? carteiraVencer61a90dias { get; set; }
        [DataMember]
        public string? carteiraVencer91a180dias { get; set; }
        [DataMember]
        public string? carteiraVencer181a360dias { get; set; }
        [DataMember]
        public string? carteiraVencerPrazoIndeterminado { get; set; }
        [DataMember]
        public string? responsabilidadeTotal { get; set; }
        [DataMember]
        public string? creditosaLiberar { get; set; }
        [DataMember]
        public string? limitesdeCredito { get; set; }
        [DataMember]
        public string? riscoTotal { get; set; }

        [DataMember]
        public string? qtdeOperacoesDiscordancia { get; set; }
        [DataMember]
        public string? vlrOperacoesDiscordancia { get; set; }
        [DataMember]
        public string? qtdeOperacoesSobJudice { get; set; }
        [DataMember]
        public string? vlrOperacoesSobJudice { get; set; }            

        [DataMember]
        public DateTime? DataConsulta { get; set; }

        [DataMember]
        public string? carteiraVencido { get; set; }
        [DataMember]
        public string? carteiraVencer { get; set; }
    }
}
