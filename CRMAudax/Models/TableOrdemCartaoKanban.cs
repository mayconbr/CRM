using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableOrdemCartaoKanban
    {
        [Key]                                                                      //chave primaria do Id do Cedente
        public long Id { get; set; }
        [DataMember]
        public long ordemCartao { get; set; }
		[DataMember]
		public long ColunaId { get; set; }
		public virtual TableColunaKanban Coluna { get; set; }                            //chave estrangeira do Id da Coluna
		[DataMember]
		public long ClienteId { get; set; }
		public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id


	}
}
