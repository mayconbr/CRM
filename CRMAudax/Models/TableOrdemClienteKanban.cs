using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableOrdemClienteKanban
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public long ColunaId { get; set; }
        public virtual TableColunaKanban Coluna { get; set; }                            //chave estrangeira do Id da Coluna
        [DataMember]
        public long ordemCard { get; set; }

    }
}
