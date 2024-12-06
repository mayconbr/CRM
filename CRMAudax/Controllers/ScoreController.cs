using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;
using System.Diagnostics;
using CRMAudax.Models;
using Microsoft.AspNetCore.Authorization;

namespace CRMAudax.Controllers
{
    //[Authorize]
    public class ScoreController : Controller
    {
        public IActionResult ExibeSCR(long Id)
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.scr = ListarendividamentoSCRDetalhe(Id);
            return View(mymodel);
        }

        public IActionResult ExibeProtestosD(long Id)
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.protestos = ListarProtestosDecisaoDetalhe(Id);
            return View(mymodel);
        }

        public IActionResult ExibeProtestosD2(long Id)
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.protestos = ListarProtestosDecisaoDetalhe(Id);
            return View(mymodel);
        }

        public IActionResult ExibeProtestosQ(long Id)
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.protestos = ListarProtestosQuodDetalhe(Id);
            return View(mymodel);
        }

        public IActionResult ExibePendenciasD(long Id)
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.protestos = ListarPendenciasDecisaoDetalhe(Id);
            return View(mymodel);
        }

        public IActionResult ExibePendenciasQ(long Id)
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.protestos = ListarPendenciasQuodDetalhe(Id);
            return View(mymodel);
        }

        [HttpGet]
        [Route("~/ListarQuodScore/{Id}")]
        public IEnumerable<CRMAudax.Models.TableQuodScore> ListarQuodScore(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.QuodScores
                           where t.ClienteId.Equals(Id)
                           select new TableQuodScore
                           {
                               Id = t.Id,
                               ClienteId = Id,
                               Score = t.Score,
                               DataScore = t.DataScore,
                               CPFCNPJ = t.CPFCNPJ,
                               //Cliente = t.Cliente

                           }).ToArray().Distinct()
                           .GroupBy(n => new { n.Score, n.DataScore.Date })
                           .Select(g => g.First())
                           .ToArray();

                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarDecisaoScore/{Id}")]
        public IEnumerable<CRMAudax.Models.TableDecisaoScore> ListarDecisaoScore(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.DecisaoScores
                           where t.ClienteId.Equals(Id)

                           select new TableDecisaoScore
                           {
                               Id = t.Id,
                               ClienteId = Id,
                               Score = t.Score,
                               DataScore = t.DataScore,
                               CPFCNPJ = t.CPFCNPJ,
                               ClasseRisco = t.ClasseRisco,
                               Esclarecimento = t.Esclarecimento,
                               ProbabilidadeInadimplencia = t.ProbabilidadeInadimplencia,
                               //Cliente = t.Cliente

                           }).ToArray();

                return aux;
            }
        }


        [HttpGet]
        [Route("~/ListarUltimoQuodScore/{Id}")]
        public IEnumerable<CRMAudax.Models.TableQuodScore> ListarUltimoQuodScore(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.QuodScores
                           where t.ClienteId.Equals(Id)
                           orderby t.Id descending

                           select new TableQuodScore
                           {
                               Id = t.Id,
                               ClienteId = Id,
                               Score = t.Score,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarUltimoDecisaoScore/{Id}")]
        public IEnumerable<CRMAudax.Models.TableDecisaoScore> ListarUltimoDecisaoScore(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.DecisaoScores
                           where t.ClienteId.Equals(Id)
                           orderby t.Id descending

                           select new TableDecisaoScore
                           {
                               Id = t.Id,
                               ClienteId = Id,
                               Score = t.Score,
                           }).ToArray();

                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarSinteseDecisao/{Id}")]
        public IEnumerable<CRMAudax.Models.TableDecisaoScore> ListarSinteseDecisao(long Id)
        {
            using (var context = new MyDbContext())
            {
                var ultimaSintese = (from u in context.DecisaoScores
                                     where u.ClienteId.Equals(Id)
                                     orderby u.Id descending

                                     select new TableDecisaoScore
                                     {
                                         ClasseRisco = u.ClasseRisco,
                                         Esclarecimento = u.Esclarecimento,
                                         ProbabilidadeInadimplencia = u.ProbabilidadeInadimplencia,

                                     }).ToArray();

                return ultimaSintese;
            }
        }

        [HttpGet]
        [Route("~/ListarQuodScoreRaiting/{Id}")]
        public string ListarQuodScoreRaiting(long Id)
        {
            using (var context = new MyDbContext())
            {

                var aux = (from t in context.QuodScores
                           where t.ClienteId.Equals(Id)
                           orderby t.Id descending
                           select t.Score).ToArray().FirstOrDefault();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarDecisaoScoreRaiting/{Id}")]
        public string ListarDecisaoScoreRaiting(long Id)
        {
            using (var context = new MyDbContext())
            {

                var aux = (from t in context.DecisaoScores
                           where t.ClienteId.Equals(Id)
                           orderby t.Id descending
                           select t.Score).ToArray().FirstOrDefault();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarProtestosDecisao/{Id}")]
        public IEnumerable<CRMAudax.Models.AuxProtestDec> ListarProtestosDecisao(long Id)
        {
            using (var context = new MyDbContext())
            {
                var resultado = context.protestosDecisao
                .Where(t => t.ClienteId.Equals(Id))
                .GroupBy(t => new { t.Data, t.Valor }) // Agrupamento por dois campos usando um objeto anônimo
                .Select(g => new AuxProtestDec
                {
                    Data = g.Key.Data, // Acesso ao campo Data do objeto anônimo
                    ValorTotal = g.Sum(x => x.Valor),
                    Contagem = g.Count()
                }).ToArray();

                resultado = resultado
                .GroupBy(t => t.Data) // Agrupamento por dois campos usando um objeto anônimo
                .Select(g => new AuxProtestDec
                {
                    Data = g.Key, // Acesso ao campo Data do objeto anônimo
                    ValorTotal = g.Sum(x => x.ValorTotal),
                    Contagem = g.Count()
                }).ToArray();

                return resultado;
            }
        }

        [HttpGet]
        [Route("~/ListarProtestosQuod/{Id}")]
        public IEnumerable<CRMAudax.Models.AuxProtestQuod> ListarProtestosQuod(long Id)
        {
            using (var context = new MyDbContext())
            {
                var resultado = context.protestosQuod
                .Where(t => t.ClienteId.Equals(Id))
                .GroupBy(t => new { t.Data, t.Valor }) // Agrupamento por dois campos usando um objeto anônimo
                .Select(g => new AuxProtestQuod
                {
                    Data = g.Key.Data, // Acesso ao campo Data do objeto anônimo
                    ValorTotal = g.Sum(x => x.Valor),
                    Contagem = g.Count()
                }).ToArray();

                resultado = resultado
                .GroupBy(t => t.Data) // Agrupamento por dois campos usando um objeto anônimo
                .Select(g => new AuxProtestQuod
                {
                    Data = g.Key, // Acesso ao campo Data do objeto anônimo
                    ValorTotal = g.Sum(x => x.ValorTotal),
                    Contagem = g.Count()
                }).ToArray();

                return resultado;
            }
        }


        [HttpGet]
        [Route("~/ListarPendenciaQuod/{Id}")]
        public IEnumerable<CRMAudax.Models.AuxProtestQuod> ListarPendenciaQuod(long Id)
        {
            using (var context = new MyDbContext())
            {
                var resultado = context.pendenciasQuod
                .Where(t => t.ClienteId.Equals(Id))
                .GroupBy(t => new { t.DateInclued, t.Amount }) // Agrupamento por dois campos usando um objeto anônimo
                .Select(g => new AuxProtestQuod
                {
                    Data = g.Key.DateInclued, // Acesso ao campo Data do objeto anônimo
                    ValorTotal = g.Sum(x => x.Amount),
                    Contagem = g.Count()
                }).ToArray();

                resultado = resultado
                .GroupBy(t => t.Data) // Agrupamento por dois campos usando um objeto anônimo
                .Select(g => new AuxProtestQuod
                {
                    Data = g.Key, // Acesso ao campo Data do objeto anônimo
                    ValorTotal = g.Sum(x => x.ValorTotal),
                    Contagem = g.Count()                    
                }).ToArray();

                return resultado;
            }
        }

        [HttpGet]
        [Route("~/ListarPendenciaDecisao/{Id}")]
        public IEnumerable<CRMAudax.Models.AuxProtestQuod> ListarPendenciaDecisao(long Id)
        {
            using (var context = new MyDbContext())
            {
                var resultado = context.pendenciasDecisao
                .Where(t => t.ClienteId.Equals(Id))
                .GroupBy(t => new { t.Data, t.Valor }) // Agrupamento por dois campos usando um objeto anônimo
                .Select(g => new AuxProtestQuod
                {
                    Data = g.Key.Data, // Acesso ao campo Data do objeto anônimo
                    ValorTotal = g.Sum(x => x.Valor),
                    Contagem = g.Count()
                }).ToArray();

                resultado = resultado
                .GroupBy(t => t.Data) // Agrupamento por dois campos usando um objeto anônimo
                .Select(g => new AuxProtestQuod
                {
                    Data = g.Key, // Acesso ao campo Data do objeto anônimo
                    ValorTotal = g.Sum(x => x.ValorTotal),
                    Contagem = g.Count()
                }).ToArray();

                return resultado;
            }
        }

        [HttpGet]
        [Route("~/ListarendividamentoSCR/{Id}")]
        public IEnumerable<CRMAudax.Models.TableEndividamentoSCR> ListarendividamentoSCR(long Id)
        {
            using (var context = new MyDbContext())
            {
                var endiividamento = (from u in context.endividamentoSCR
                                      where u.ClienteId.Equals(Id)
                                      orderby u.Id ascending

                                     select new TableEndividamentoSCR
                                     {
                                         codigoDoCliente = u.codigoDoCliente,
                                         dataBaseConsultada = u.dataBaseConsultada,
                                         dataInicioRelacionamento = u.dataInicioRelacionamento,
                                         carteiraVencerAte30diasVencidosAte14dias = u.carteiraVencerAte30diasVencidosAte14dias,
                                         carteiraVencer31a60dias = u.carteiraVencer31a60dias,
                                         carteiraVencer61a90dias = u.carteiraVencer61a90dias,
                                         carteiraVencer91a180dias = u.carteiraVencer91a180dias,
                                         carteiraVencer181a360dias = u.carteiraVencer181a360dias,
                                         carteiraVencerPrazoIndeterminado = u.carteiraVencerPrazoIndeterminado,
                                         responsabilidadeTotal = u.responsabilidadeTotal,
                                         creditosaLiberar = u.creditosaLiberar,
                                         limitesdeCredito = u.limitesdeCredito,
                                         riscoTotal = u.riscoTotal,
                                         qtdeOperacoesDiscordancia = u.qtdeOperacoesDiscordancia,
                                         vlrOperacoesDiscordancia = u.vlrOperacoesDiscordancia,
                                         qtdeOperacoesSobJudice = u.qtdeOperacoesSobJudice,
                                         vlrOperacoesSobJudice = u.vlrOperacoesSobJudice,

                                         DataConsulta = u.DataConsulta,
                                         carteiraVencido = u.carteiraVencido,
                                         carteiraVencer = u.carteiraVencer

                                     }).ToArray();

                return endiividamento;
            }
        }

        [HttpGet]
        [Route("~/ListarendividamentoSCRDetalhe/{Id}")]
        public IEnumerable<CRMAudax.Models.TableEndividamentoSCR> ListarendividamentoSCRDetalhe(long Id)
        {
            using (var context = new MyDbContext())
            {
                var endiividamento = (from u in context.endividamentoSCR
                                      where u.ClienteId.Equals(Id)
                                      orderby u.Id descending

                                      select new TableEndividamentoSCR
                                      {
                                          codigoDoCliente = u.codigoDoCliente,
                                          dataBaseConsultada = u.dataBaseConsultada,
                                          dataInicioRelacionamento = u.dataInicioRelacionamento,
                                          carteiraVencerAte30diasVencidosAte14dias = u.carteiraVencerAte30diasVencidosAte14dias,
                                          carteiraVencer31a60dias = u.carteiraVencer31a60dias,
                                          carteiraVencer61a90dias = u.carteiraVencer61a90dias,
                                          carteiraVencer91a180dias = u.carteiraVencer91a180dias,
                                          carteiraVencer181a360dias = u.carteiraVencer181a360dias,
                                          carteiraVencerPrazoIndeterminado = u.carteiraVencerPrazoIndeterminado,
                                          responsabilidadeTotal = u.responsabilidadeTotal,
                                          creditosaLiberar = u.creditosaLiberar,
                                          limitesdeCredito = u.limitesdeCredito,
                                          riscoTotal = u.riscoTotal,
                                          qtdeOperacoesDiscordancia = u.qtdeOperacoesDiscordancia,
                                          vlrOperacoesDiscordancia = u.vlrOperacoesDiscordancia,
                                          qtdeOperacoesSobJudice = u.qtdeOperacoesSobJudice,
                                          vlrOperacoesSobJudice = u.vlrOperacoesSobJudice,

                                          DataConsulta = u.DataConsulta,
                                          carteiraVencido = u.carteiraVencido,
                                          carteiraVencer = u.carteiraVencer

                                      }).ToArray();

                return endiividamento;
            }
        }

        [HttpGet]
        [Route("~/ListarProtestosDecisaoDetalhe/{Id}")]
        public IEnumerable<CRMAudax.Models.TableProtestosDecisao> ListarProtestosDecisaoDetalhe(long Id)
        {
            using (var context = new MyDbContext())
            {
                var protesto = context.protestosDecisao
                                     .Where(u => u.ClienteId == Id)
                                     .AsEnumerable()  // Traz os dados para a memória
                                     .GroupBy(u => new { u.Cidade, u.Valor,})  
                                     .Select(g => g.OrderBy(u => u.Data).First())
                                     .OrderBy(u => u.Data)
                                     .Select(u => new TableProtestosDecisao
                                     {
                                         Id = u.Id,
                                         Cartorio = u.Cartorio,
                                         Cidade = u.Cidade,
                                         Valor = u.Valor,
                                         Uf = u.Uf,
                                         Moeda = u.Moeda,
                                         Data = u.Data,
                                         ClienteId = u.ClienteId
                                     })
                                     .ToList();

                return protesto;
            }
        }





        //[HttpGet]
        //[Route("~/ListarProtestosDecisaoDetalhe/{Id}")]
        //public IEnumerable<CRMAudax.Models.TableProtestosDecisao> ListarProtestosDecisaoDetalhe(long Id)
        //{
        //    using (var context = new MyDbContext())
        //    {
        //        var protesto = (from u in context.protestosDecisao
        //                              where u.ClienteId.Equals(Id)
        //                              orderby u.Data ascending
        //                              select new TableProtestosDecisao
        //                              {
        //                                  Cartorio = u.Cartorio,
        //                                  Cidade = u.Cidade,
        //                                  Valor = u.Valor,
        //                                  Uf = u.Uf,
        //                                  Moeda = u.Moeda,
        //                                  Data = u.Data,
        //                                  ClienteId = u.ClienteId

        //                              }).Distinct().ToList();

        //        return protesto;
        //    }
        //}


        [HttpGet]
        [Route("~/ListarProtestosQuodDetalhe/{Id}")]
        public IEnumerable<CRMAudax.Models.TableProtestosQuod> ListarProtestosQuodDetalhe(long Id)
        {
            using (var context = new MyDbContext())
            {
                var protesto = (from u in context.protestosQuod
                                where u.ClienteId.Equals(Id)
                                select new TableProtestosQuod
                                {
                                    Valor = u.Valor,
                                    NomeCartorio = u.NomeCartorio,
                                    Endereco = u.Endereco,
                                    Cidade = u.Cidade,
                                    Data = u.Data,
                                    Cliente = u.Cliente,
                                    CPFCNPJ = u.CPFCNPJ,

                                }).Distinct().ToList();

                return protesto;
            }
        }

        [HttpGet]
        [Route("~/ListarPendenciasDecisaoDetalhe/{Id}")]
        public IEnumerable<CRMAudax.Models.TablePendenciasDecisao> ListarPendenciasDecisaoDetalhe(long Id)
        {
            using (var context = new MyDbContext())
            {
                var pendencia = (from u in context.pendenciasDecisao
                                where u.ClienteId.Equals(Id)
                                select new TablePendenciasDecisao
                                {
                                    Valor = u.Valor,
                                    Banco = u.Banco,
                                    Modalidade = u.Modalidade,
                                    Motivo = u.Motivo,
                                    Agencia = u.Agencia,
                                    Avalista = u.Avalista,
                                    Cidade = u.Cidade,
                                    Origem = u.Origem,
                                    Data = u.Data,
                                    Contagem = u.Contagem,
                                    Contrato = u.Contrato

                                }).Distinct().ToList();

                return pendencia;
            }
        }

        [HttpGet]
        [Route("~/ListarPendenciasQuodDetalhe/{Id}")]
        public IEnumerable<CRMAudax.Models.TablePendenciasQuod> ListarPendenciasQuodDetalhe(long Id)
        {
            using (var context = new MyDbContext())
            {
                var pendencia = (from u in context.pendenciasQuod
                                 where u.ClienteId.Equals(Id)
                                 select new TablePendenciasQuod
                                 {
                                    Amount = u.Amount,
                                    ApontamentoStatus = u.ApontamentoStatus,
                                    City = u.City,
                                    CNPJ = u.CNPJ,
                                    Comarca = u.Comarca,
                                    CompanyName = u.CompanyName,
                                    ContractNumber = u.ContractNumber,
                                    DateInclued = u.DateInclued,
                                    DateOcurred = u.DateOcurred,
                                    Forum = u.Forum,
                                    JusticeType = u.JusticeType,
                                    Name = u.Name,
                                    Nature = u.Nature,
                                    ParticipantType = u.ParticipantType,
                                    PendenciesControlCred = u.PendenciesControlCred,
                                    Vara = u.Vara,
                                    State = u.State,
                                    ProcessType = u.ProcessType,
                                    ProcessNumber = u.ProcessNumber,
                                    ProcessAuthor = u.ProcessAuthor

                                 }).Distinct().ToList();

                return pendencia;
            }
        }


        [HttpGet]
        [Route("~/ListarUltimoSCR/{Id}")]
        public IEnumerable<CRMAudax.Models.TableEndividamentoSCR> ListarUltimoSCR(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.endividamentoSCR
                           where t.ClienteId.Equals(Id)
                           orderby t.Id descending

                           select new TableEndividamentoSCR
                           {
                               Id = t.Id,
                               ClienteId = Id,
                               carteiraVencido = t.carteiraVencido,
                               carteiraVencer = t.carteiraVencer

                           }).ToArray();
                return aux;
            }
        }

    }
}
