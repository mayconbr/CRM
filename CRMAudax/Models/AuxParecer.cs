using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{

	[Serializable]
	public class AuxParecer
	{
		[DataMember]
		public string Parecer { get; set; }
		[DataMember]
		public DateTime? DataRegistro { get; set; }
        [DataMember]
        public string Tipo { get; set; }
    }
}
