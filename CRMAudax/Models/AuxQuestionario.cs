using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class PerguntaComRespostas
    {
        public long PerguntaId { get; set; }
        public long? Tipopergunta { get; set; }
        public long QuestionarioId { get; set; }
        public string Pergunta { get; set; }
        public List<Resposta> Respostas { get; set; }
    }

    public class Resposta
    {
        public long RespostaId { get; set; }
        public string RespostaTexto { get; set; }
    }
}
