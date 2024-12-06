using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableOrdemColunaKanban
    {
        [Key]                                                                            //chave primaria do Id 
        public long Id { get; set; }
        [DataMember]
        public long ColunaId { get; set; }
        public virtual TableColunaKanban Coluna { get; set; }                            //chave estrangeira do Id da Coluna
        [DataMember]
        public long ordemColuna { get; set; }
    }
}
