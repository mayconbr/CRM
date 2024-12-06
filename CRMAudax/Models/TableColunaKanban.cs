using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableColunaKanban
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public string nome { get; set; }
        [DataMember]
        public long ordem { get; set; }
        [DataMember]
        public DateTime? DataDelete { get; set; }

        

    }
}
