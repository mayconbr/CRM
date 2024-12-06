using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{

	[Serializable]
	public class AuxProtestQuod
	{
		[DataMember]
		public DateTime? Data { get; set; }
		[DataMember]
		public decimal? ValorTotal { get; set; }
        [DataMember]
        public long Contagem { get; set; }
    }
}
