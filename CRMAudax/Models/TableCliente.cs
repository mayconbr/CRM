using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRMAudax.Models
{
    [DataContract]
    public class TableCliente
    {
        [Key]                                                                      //chave primaria do Id do Proponente
        public long Id { get; set; }
        [DataMember]
        public long tipoPessoa { get; set; }
        [DataMember]
        public string nome { get; set; }
        [DataMember]
        public string cpfCnpj { get; set; }
        [DataMember]
        public string? email { get; set; }
        [DataMember]
        public string? rua { get; set; }
        [DataMember]
        public string? cep { get; set; }
        [DataMember]
        public string? numero { get; set; }
        [DataMember]
        public string? parecer { get; set; }
        [DataMember]
        public string telefone { get; set; }
        [DataMember]
        public string? complemento { get; set; }
        [DataMember]
        public string? cidade { get; set; }
        [DataMember]
        public string? estado { get; set; }
        [DataMember]
        public string? bairro { get; set; }
        [DataMember]
        public string? site { get; set; }
        [DataMember]
        public string? ramoAtividade { get; set; }

        [DataMember]
        public string? nomeResponsavel { get; set; }
        [DataMember]
        public string? razaoSocialCedente { get; set; }
        [DataMember]
        public DateTime? dataFundacaoCedente { get; set; }
        [DataMember]
        public decimal? faturamento { get; set; }

        [DataMember]
        public string tipo { get; set; }
        [DataMember]
        public string situacao { get; set; }
        [DataMember]
        public bool status { get; set; }
        [DataMember]
        public string? InscricaoEstadual { get; set; }

        [DataMember]
        public DateTime? DataDelete { get; set; }

        //Campos novos
        [DataMember]
        public string? recorreu { get; set; }
        [DataMember]
        public string? pep { get; set; }
        [DataMember]
        public string? socio { get; set; }
        [DataMember]
        public string? referencia { get; set; }

        [DataMember]
        public string? cpfSocio { get; set; }
        [DataMember]
        public string? nomeSocio { get; set; }
        [DataMember]
        public string? enderecoSocio { get; set; }
        [DataMember]
        public string? celularSocio { get; set; }
        [DataMember]
        public string? EmailSocio { get; set; }
        [DataMember]
        public long? TipoPessoaSocio { get; set; }

        //SegundoSocio
        [DataMember]
        public string? pepSegundo { get; set; }
        [DataMember]
        public string? socioSegundo { get; set; }      
        [DataMember]
        public string? cpfSocioSegundo { get; set; }
        [DataMember]
        public string? nomeSocioSegundo { get; set; }
        [DataMember]
        public string? enderecoSocioSegundo { get; set; }
        [DataMember]
        public string? celularSocioSegundo { get; set; }
        [DataMember]
        public string? EmailSegundo { get; set; }
        [DataMember]
        public long? TipoPessoaSocioSegundo { get; set; }

        //Terceiro Socio       
        [DataMember]
        public string? pepTerceiro { get; set; }
        [DataMember]
        public string? socioTerceiro { get; set; }        
        [DataMember]
        public string? cpfSocioTerceiro { get; set; }
        [DataMember]
        public string? nomeSocioTerceiro { get; set; }
        [DataMember]
        public string? enderecoSocioTerceiro { get; set; }
        [DataMember]
        public string? celularSocioTerceiro { get; set; }
        [DataMember]
        public string? EmailTerceiro { get; set; }
        [DataMember]
        public long? TipoPessoaSocioTerceiro { get; set; }


        [DataMember]
        public DateTime DataCadastro { get; set; }

        [DataMember]
        public string? justificativaReprova { get; set; }

        [DataMember]
        public long UsuarioId { get; set; }
        public virtual TableUsuario? Usuario { get; set; }

        [DataMember]
        public long RegiaoId { get; set; }
        public virtual TableRegiao Regiao { get; set; }

        [DataMember]
        public string? SituacaoCNPJ { get; set; }

        [DataMember]
        public string? contaBancaria { get; set; }

        [DataMember]
        public string? AssinaturaSocio { get; set; }
        [DataMember]
        public string? AssinaturaSocioSegundo { get; set; }
        [DataMember]
        public string? AssinaturaSocioTerceiro { get; set; }

    }
}
