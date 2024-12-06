using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{

    [Serializable]
    public class AuxRespostaCoaf
    {

        [Key]
        public long Id { get; set; }
        [DataMember]
        public long PerguntaId { get; set; }
        public virtual TablePergunta Pergunta { get; set; }                            //chave estrangeira do Id da Pergunta
        [DataMember]
        public long? RespostaId { get; set; }
        [DataMember]
        public long ClienteId { get; set; }
        public virtual TableCliente Cliente { get; set; }                            //chave estrangeira do Id do cliente
        [DataMember]
        public string PerguntaCoaf { get; set; }
        [DataMember]
        public string RespostaPergunta { get; set; }
        [DataMember]
        public string RespostaAberta { get; set; }
        [DataMember]
        public DateTime DataResposta { get; set; }
        [DataMember]
        public long? TipoPergunta { get; set; }

    }
}
