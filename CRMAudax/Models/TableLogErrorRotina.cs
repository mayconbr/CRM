using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableLogErrorRotina
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public string? Documento { get; set; }
        [DataMember]
        public string? Erro { get; set; }
        [DataMember]
        public string? Consulta { get; set; }
        [DataMember]
        public DateTime? DataConsulta { get; set; }

    }
}
