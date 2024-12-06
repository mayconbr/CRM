using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableRespostaCedente
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long PerguntaId { get; set; }
        public virtual TablePergunta Pergunta { get; set; }                            //chave estrangeira do Id da Pergunta
        [DataMember]
        public long? RespostaId { get; set; }                                         //chave estrangeira do Id da Resposta
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do cliente
        [DataMember]
        public string? RespostaAberta { get; set; }                          
        [DataMember]
        public DateTime DataResposta { get; set; }                         
    }
}
