using Microsoft.AspNetCore.Mvc;
using CRMAudax.Models;
using System.Diagnostics.Metrics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;

namespace CRMAudax.Controllers
{
    [Authorize]
    public class VisitaController : Controller
    {
        public IActionResult Visita()
        {
            return View();
        }

        public IActionResult ExibeVisita(long Id)
        {
            ViewData["visita"] = ListarVisitaId(Id);
            return View();
        }

        public IActionResult ExportacaoVisita(long Id)
        {
            ViewData["visita"] = ListarVisitaId(Id);
            return View();
        }

        [HttpPost]
        [Route("~/CadastrarVisita")]
        public IActionResult CadastrarVisita([FromBody] TableRelatorioVisita request)
        {
            using (var context = new MyDbContext())
            {
                    var r = context.RelatoriosVisita.Add(new TableRelatorioVisita
                    {
                        ClienteId = request.ClienteId,

                        dataVisita = request.dataVisita, 
                        nomeEntrevistado = request.nomeEntrevistado,
                        cargo = request.cargo,
                        nomeIndicacao = request.nomeIndicacao,                        

                        edificacao = request.edificacao,
                        estoque = request.estoque,
                        equipamento = request.equipamento,
                        producao = request.producao,
                        funcionarios = request.funcionarios,
                        organizacaoProducao = request.organizacaoProducao,
                        materiaPrima = request.materiaPrima,
                        impressaoMidia = request.impressaoMidia,
                        apresentacao = request.apresentacao,
                        franqueza = request.franqueza,
                        conhecimentoNegocio = request.conhecimentoNegocio,

                        carater = request.carater,
                        abertura = request.abertura,
                        conhecimentoConcorrencia = request.conhecimentoConcorrencia,
                        tempoCargo = request.tempoCargo,

                        negocioFamiliar = request.negocioFamiliar,                        

                        sazonalidade = request.sazonalidade,
                        parqueIndustrial = request.parqueIndustrial,
                        certificacaoQUalidade = request.certificacaoQUalidade,
                        amplaConcorrencia = request.amplaConcorrencia,
                        regimeTributario = request.regimeTributario,
                        margemLiquida = request.margemLiquida,
                        alteracaoContratual = request. alteracaoContratual,
                        fundacao = request.fundacao,
                        porcentagemCheque = request.porcentagemCheque,
                        porcentagemDuplicata =  request.porcentagemDuplicata,
                        porcentagemConsumidorFisica =  request.porcentagemConsumidorFisica,
                        porcentagemConsumidorJuridica = request.porcentagemConsumidorJuridica,
                        prazoMedioFornecedores = request.prazoMedioFornecedores,
                        prazoMedioClientes = request.prazoMedioClientes,
                        formaEntregaProduto = request.formaEntregaProduto,
                        ticketMedio = request.ticketMedio,
                        Parecer = request.Parecer

                    }).Entity;

                    context.SaveChanges();
                
                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ListarVisita/{Id}")]
        public IEnumerable<CRMAudax.Models.TableRelatorioVisita> ListarVisita(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.RelatoriosVisita

                           where t.ClienteId == Id
                           select new TableRelatorioVisita
                           {
                               Id = t.Id,
                               dataVisita = t.dataVisita,
                               nomeEntrevistado = t.nomeEntrevistado

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarVisitaId/{Id}")]
        public IEnumerable<CRMAudax.Models.TableRelatorioVisita> ListarVisitaId(long Id)
        {
            using (var context = new MyDbContext())
            {
                return (from t in context.RelatoriosVisita

                        where t.dataVisita != null
                        where t.Id.Equals(Id)

                        select new TableRelatorioVisita
                        {
                            Id = t.Id,
                            ClienteId = t.ClienteId,
                            Cliente = t.Cliente,

                            dataVisita = t.dataVisita,
                            nomeEntrevistado = t.nomeEntrevistado,
                            cargo = t.cargo,
                            nomeIndicacao = t.nomeIndicacao,

                            edificacao = t.edificacao,
                            estoque = t.estoque,
                            equipamento = t.equipamento,
                            producao = t.producao,
                            funcionarios = t.funcionarios,
                            organizacaoProducao = t.organizacaoProducao,
                            materiaPrima = t.materiaPrima,
                            impressaoMidia = t.impressaoMidia,
                            apresentacao = t.apresentacao,
                            franqueza = t.franqueza,
                            conhecimentoNegocio = t.conhecimentoNegocio,

                            carater = t.carater,
                            abertura = t.abertura,
                            conhecimentoConcorrencia = t.conhecimentoConcorrencia,
                            tempoCargo = t.tempoCargo,

                            negocioFamiliar = t.negocioFamiliar,

                            sazonalidade = t.sazonalidade,
                            parqueIndustrial = t.parqueIndustrial,
                            certificacaoQUalidade = t.certificacaoQUalidade,
                            amplaConcorrencia = t.amplaConcorrencia,
                            regimeTributario = t.regimeTributario,
                            margemLiquida = t.margemLiquida,
                            alteracaoContratual = t.alteracaoContratual,
                            fundacao = t.fundacao,
                            porcentagemCheque = t.porcentagemCheque,
                            porcentagemDuplicata = t.porcentagemDuplicata,
                            porcentagemConsumidorFisica = t.porcentagemConsumidorFisica,
                            porcentagemConsumidorJuridica = t.porcentagemConsumidorJuridica,
                            prazoMedioFornecedores = t.prazoMedioFornecedores,
                            prazoMedioClientes = t.prazoMedioClientes,
                            formaEntregaProduto = t.formaEntregaProduto,
                            ticketMedio = t.ticketMedio,
                            Parecer = t.Parecer,

                        }).ToArray();
            }
        }
    }
}
