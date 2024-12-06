using Microsoft.AspNetCore.Mvc;
using CRMAudax.Models;
using System.Diagnostics.Metrics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;


namespace CRMAudax.Controllers
{
    [Authorize]

    public class ComiteController : Controller
    {
        public IActionResult Comite(long Id)
        {
            return View();
        }

        public IActionResult ExibeComite(long Id)
        {
            ViewData["comite"] = ListarComiteId(Id);
            return View();
        }

        //Comite
        [HttpPost]
        [Route("~/CadastrarComite")]
        public IActionResult InsertComite([FromBody] TableComite request)
        {
            using (var context = new MyDbContext())
            {
                    var comite = (from t in context.Comites
                                  where t.Id.Equals(request.Id)
                                  select t).ToArray().FirstOrDefault();
                    if (comite == null)
                    {
                        var r = context.Comites.Add(new TableComite
                        {
                            ClienteId = request.ClienteId,

                            contratoSocial = request.contratoSocial,
                            comprovanteEndereco = request.comprovanteEndereco,
                            inscricaoEstadual = request.inscricaoEstadual,
                            rgCpf = request.rgCpf,
                            impostoRenda = request.impostoRenda,
                            cartaoCnpj = request.cartaoCnpj,
                            faturamento = request.faturamento,
                            balanco = request.balanco,
                            tempoFundacao = request.tempoFundacao,
                            mudancaAtividade = request.mudancaAtividade,
                            mudancaSocios = request.mudancaSocios,
                            limiteAcima = request.limiteAcima,
                            concentracaoSacados = request.concentracaoSacados,
                            antecipacaoImoveis = request.antecipacaoImoveis,
                            entregaMercadoria = request.entregaMercadoria,
                            confirmaNota = request.confirmaNota,
                            consultaAcimaMedia = request.consultaAcimaMedia,
                            consultaCnpj = request.consultaCnpj,
                            pendenciaFinanceira = request.pendenciaFinanceira,
                            aumentoProtesto = request.aumentoProtesto,
                            protestoConsumo = request.protestoConsumo,
                            perfilProtesto = request.perfilProtesto,
                            ccf = request.ccf,
                            riscoPagamento = request.riscoPagamento,
                            endividamentoEmergencial = request.endividamentoEmergencial,
                            aumentoContratoCredito = request.aumentoContratoCredito,
                            picoConsulta = request.picoConsulta,
                            acoesJudiciais = request.acoesJudiciais,
                            recuperacaoJudicial = request.recuperacaoJudicial,
                            execucaoFiscal = request.execucaoFiscal,
                            acaoPorBanco = request.acaoPorBanco,
                            recisaoServico = request.recisaoServico,
                            apontamentoRestritivo = request.apontamentoRestritivo,
                            maisempresas = request.maisempresas,
                            empresaMesmoRamo = request.empresaMesmoRamo,
                            socioProtesto = request.socioProtesto,
                            socioCheque = request.socioCheque,
                            socioAcao = request.socioAcao,
                            socioApontamentoCPF = request.socioApontamentoCPF,
                            socioOstentador = request.socioOstentador,
                            socioHerdeiro = request.socioHerdeiro,
                            socioGarantiaAdd = request.socioGarantiaAdd,

                            tipoTitulos = request.tipoTitulos,
                            faturamentoFiscal = request.faturamentoFiscal,
                            faturamentoReal = request.faturamentoReal,
                            valorTotalNegativado = request.valorTotalNegativado,
                            valorTotalProtestos = request.valorTotalProtestos,
                            observacoes = request.observacoes,

                            limite = request.limite,
                            dataComite = DateTime.UtcNow

                        }).Entity;

                        context.SaveChanges();
                    }
                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ListarComite/{Id}")]
        public IEnumerable<CRMAudax.Models.TableComite> ListarComite(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.Comites

                           where t.ClienteId == Id
                           select new TableComite
                           {
                               Id = t.Id,
                               dataComite = t.dataComite,
                               limite = t.limite

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarComiteId/{Id}")]
        public IEnumerable<CRMAudax.Models.TableComite> ListarComiteId(long Id)
        {
            using (var context = new MyDbContext())
            {
                return (from t in context.Comites

                        where t.dataComite != null
                        where t.Id.Equals(Id)

                        select new TableComite
                        {
                            Id = t.Id,
                            dataComite = t.dataComite,
                            limite = t.limite,
                            contratoSocial = t.contratoSocial,
                            comprovanteEndereco = t.comprovanteEndereco,
                            inscricaoEstadual = t.inscricaoEstadual,
                            rgCpf = t.rgCpf,
                            impostoRenda = t.impostoRenda,
                            cartaoCnpj = t.cartaoCnpj,
                            faturamento = t.faturamento,
                            balanco = t.balanco,
                            tempoFundacao = t.tempoFundacao,
                            mudancaAtividade = t.mudancaAtividade,
                            mudancaSocios = t.mudancaSocios,
                            limiteAcima = t.limiteAcima,
                            concentracaoSacados = t.concentracaoSacados,
                            antecipacaoImoveis = t.antecipacaoImoveis,
                            entregaMercadoria = t.entregaMercadoria,
                            confirmaNota = t.confirmaNota,
                            consultaAcimaMedia = t.consultaAcimaMedia,
                            consultaCnpj = t.consultaCnpj,
                            pendenciaFinanceira = t.pendenciaFinanceira,
                            aumentoProtesto = t.aumentoProtesto,
                            protestoConsumo = t.protestoConsumo,
                            perfilProtesto = t.perfilProtesto,
                            ccf = t.ccf,
                            riscoPagamento = t.riscoPagamento,
                            endividamentoEmergencial = t.endividamentoEmergencial,
                            aumentoContratoCredito = t.aumentoContratoCredito,
                            picoConsulta = t.picoConsulta,
                            acoesJudiciais = t.acoesJudiciais,
                            recuperacaoJudicial = t.recuperacaoJudicial,
                            execucaoFiscal = t.execucaoFiscal,
                            acaoPorBanco = t.acaoPorBanco,
                            recisaoServico = t.recisaoServico,
                            apontamentoRestritivo = t.apontamentoRestritivo,
                            maisempresas = t.maisempresas,
                            empresaMesmoRamo = t.empresaMesmoRamo,
                            socioProtesto = t.socioProtesto,
                            socioCheque = t.socioCheque,
                            socioAcao = t.socioAcao,
                            socioApontamentoCPF = t.socioApontamentoCPF,
                            socioOstentador = t.socioOstentador,
                            socioHerdeiro = t.socioHerdeiro,
                            socioGarantiaAdd = t.socioGarantiaAdd,

                            tipoTitulos = t.tipoTitulos,
                            faturamentoFiscal = t.faturamentoFiscal,
                            faturamentoReal = t.faturamentoReal,
                            valorTotalNegativado = t.valorTotalNegativado,
                            valorTotalProtestos = t.valorTotalProtestos,
                            observacoes = t.observacoes,

                        }).ToArray();
            }
        }

    }
}
