using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TablePergunta
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public long QuestionarioId { get; set; }
        public virtual TableQuestionario Questionario { get; set; }                            //chave estrangeira do Id do Questionario
        [DataMember]
        public string Pergunta { get; set; }
        [DataMember]
        public long? Tipopergunta { get; set; }
    }
}
