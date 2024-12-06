using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableStatusRotina
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }

        [DataMember]
        public DateTime? DataInicio { get; set; }

        [DataMember]
        public long? ConsultaCNPJ { get; set; }

        [DataMember]
        public long? QtdCpf { get; set; }

        [DataMember]
        public long? QtdCnpj { get; set; }

        [DataMember]
        public long? QuodScorePJuridica { get; set; }

        [DataMember]
        public long? QuodScorePFisica { get; set; }

        [DataMember]
        public long? DecisaoScorePJuridica { get; set; }

        [DataMember]
        public long? DecisaoScorePFisica { get; set; }

        [DataMember]
        public long? DecisaoProtestosPJuridica { get; set; }

        [DataMember]
        public long? DecisaoProtestosPFisica { get; set; }

        [DataMember]
        public long? QuodProtestosPJuridica { get; set; }

        [DataMember]
        public long? QuodProtestosPFisica { get; set; }

        [DataMember]
        public long? DecisaoPendenciasPJuridica { get; set; }

        [DataMember]
        public long? DecisaoPendenciasPFisica { get; set; }

        [DataMember]
        public long? QuodPendenciasPJuiridica { get; set; }

        [DataMember]
        public long? QuodPendenciasPFisica { get; set; }

        [DataMember]
        public DateTime? DataFinal { get; set; }
    }
}
