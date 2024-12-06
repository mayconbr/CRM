using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableResposta
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long PerguntaId { get; set; }
        public virtual TablePergunta Pergunta { get; set; }                            //chave estrangeira do Id da Pergunta
        [DataMember]
        public string Resposta { get; set; }      
    }
}
