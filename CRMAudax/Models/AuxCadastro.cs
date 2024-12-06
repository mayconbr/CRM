using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using static CRMAudax.Models.AuxQuod;

namespace CRMAudax.Models
{

	[Serializable]
	public class AuxCadastro
	{ 
		[DataMember]
		public string Nome { get; set; }
		[DataMember]
		public string? Telefone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Cep { get; set; }
        [DataMember]
        public string Rua { get; set; }
        [DataMember]
        public string Numero { get; set; }
        [DataMember]
        public string Complemento { get; set; }
        [DataMember]
        public string Cidade { get; set; }
        [DataMember]
        public string Estado { get; set; }
        [DataMember]
        public long QuodScore { get; set; }
        [DataMember]
        public string CPF { get; set; }
        [DataMember]
        public long IdSacado { get; set; }
        [DataMember]
        public List<Apontamento> QuodPendencias { get; set; }
        [DataMember]
        public List<Protest> QuodProtestos { get; set; }
    }
}
