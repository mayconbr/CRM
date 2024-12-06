using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableRegiao
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public string Nome { get; set; }      
    }
}
