using CRMAudax.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableLogLogin
    {
        [Key]
        public long Id { get; set; }
        [DataMember]
        public string NomeUser { get; set; }
        [DataMember]
        public DateTime? DateLogin { get; set; }
    }
}
