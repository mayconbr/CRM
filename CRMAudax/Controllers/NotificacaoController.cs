using Microsoft.AspNetCore.Mvc;
using CRMAudax.Models;
using System.Diagnostics.Metrics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.EntityFrameworkCore;
using CRMAudax.Tools;
using Microsoft.Win32;


namespace CRMAudax.Controllers
{
    [Authorize]

    public class NotificacaoController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public NotificacaoController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IActionResult Monitoramento()
        {

            dynamic mymodel = new ExpandoObject();

            mymodel.StatusRotinas = VisualizarRotina();
            return View(mymodel);
        }

        public IActionResult Notificacao()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Notificacoes = ListarNotificacoes();
            return View(mymodel);
        }

        [HttpGet]
        [Route("~/RotinaDeConsultas")]
        public IActionResult RotinaDeConsultas()
        {
            EmailTask.SendFormatedMail("INICIO DE ROTINA",
                    "TI - GPAUDAX", "mbruzolato@gmail.com",
                    "HORARIO DE INICIO DO SERVIDOR:" + DateTime.Now
                    );

            using (var context = new MyDbContext())
            {

                var cnpjs = (from t in context.Clientes
                             where t.DataDelete == null
                             where t.status == true
                             where t.cpfCnpj.Length == 18
                             select new TableCliente
                             {
                                 Id = t.Id,
                                 cpfCnpj = t.cpfCnpj,

                             }).ToArray();

                var cpfs = (from t in context.Clientes
                            where t.DataDelete == null
                            where t.status == true
                            where t.cpfCnpj.Length == 14
                            select new TableCliente
                            {
                                Id = t.Id,
                                cpfCnpj = t.cpfCnpj,

                            }).ToArray();


                var InicioRotina = context.StatusRotinas.Add(new TableStatusRotina
                {
                    DataInicio = DateTime.Now,
                    QtdCpf = cpfs.Count(),
                    QtdCnpj = cnpjs.Count(),

                    QuodProtestosPJuridica = 0,
                    QuodProtestosPFisica = 0,
                    QuodPendenciasPJuiridica = 0,
                    QuodPendenciasPFisica = 0,
                    QuodScorePJuridica = 0,
                    QuodScorePFisica = 0,

                    DecisaoProtestosPJuridica = 0,
                    DecisaoProtestosPFisica = 0,
                    DecisaoPendenciasPJuridica = 0,
                    DecisaoPendenciasPFisica = 0,
                    DecisaoScorePJuridica = 0,
                    DecisaoScorePFisica = 0,

                    ConsultaCNPJ = 0

                }).Entity;

                context.SaveChanges();

                var StatusRotina = (from t in context.StatusRotinas
                                    orderby t.Id descending
                                    select t).FirstOrDefault();

                var ConsultaCNPJ = 0;

                foreach (var cnpj in cnpjs)
                {
                    APIController apiController = new APIController(_clientFactory);

                    //chamada da função
                    apiController.SituacaoCNPJ(cnpj.cpfCnpj, cnpj.Id);

                    if (StatusRotina != null)
                    {
                        //add na tabela
                        StatusRotina.ConsultaCNPJ = ConsultaCNPJ++;
                        context.SaveChanges();
                    }
                }

                if (DateTime.UtcNow.Day == 2)
                {

                    EmailTask.SendFormatedMail("INICIO DE ROTINA DE SCORE",
                        "TI - GPAUDAX", "mbruzolato@gmail.com",
                        "HORARIO DE INICIO DO SERVIDOR:" + DateTime.UtcNow
                        );

                    var QuodProtestosPFisica = 0;
                    var QuodPendenciasPFisica = 0;
                    var QuodScorePFisica = 0;
                    var DecisaoProtestosPFisica = 0;
                    var DecisaoPendenciasPFisica = 0;
                    var DecisaoScorePFisica = 0;

                    foreach (var cpf in cpfs)
                    {
                        var CpfReplace = cpf.cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");

                        APIController apiController = new APIController(_clientFactory);

                        if (StatusRotina != null)
                        {
                            apiController.GetQuodScore(CpfReplace, cpf.Id);

                            //add na tabela
                            StatusRotina.QuodScorePFisica = QuodScorePFisica++;
                            context.SaveChanges();

                            apiController.GetDecisaoScore(CpfReplace, cpf.Id);

                            //add na tabela
                            StatusRotina.DecisaoScorePFisica = DecisaoScorePFisica++;
                            context.SaveChanges();

                            apiController.GetDecisaoPendenciasPF(CpfReplace, cpf.Id);

                            //add na tabela
                            StatusRotina.DecisaoPendenciasPFisica = DecisaoPendenciasPFisica++;
                            context.SaveChanges();

                            apiController.GetPendenciasQuodPF(CpfReplace, cpf.Id);

                            //add na tabela
                            StatusRotina.QuodPendenciasPFisica = QuodPendenciasPFisica++;
                            context.SaveChanges();
                        }
                    }

                    var QuodProtestosPJuridica = 0;
                    var QuodPendenciasPJuiridica = 0;
                    var QuodScorePJuridica = 0; ;
                    var DecisaoProtestosPJuridica = 0;
                    var DecisaoPendenciasPJuridica = 0;
                    var DecisaoScorePJuridica = 0;

                    foreach (var cnpj in cnpjs)
                    {
                        var CnpjReplace = cnpj.cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");

                        APIController apiController = new APIController(_clientFactory);

                        apiController.GetQuodScorePJ(CnpjReplace, cnpj.Id);

                        //add na tabela
                        StatusRotina.QuodScorePJuridica = QuodScorePJuridica++;
                        context.SaveChanges();

                        apiController.GetDecisaoScorePJ(CnpjReplace, cnpj.Id);

                        //add na tabela
                        StatusRotina.DecisaoScorePJuridica = DecisaoScorePJuridica++;
                        context.SaveChanges();

                        apiController.GetDecisaoProtestosPJ(CnpjReplace, cnpj.Id);

                        //add na tabela
                        StatusRotina.DecisaoProtestosPJuridica = DecisaoProtestosPJuridica++;
                        context.SaveChanges();

                        apiController.GetDecisaoPendenciasPJ(CnpjReplace, cnpj.Id);

                        //add na tabela
                        StatusRotina.DecisaoPendenciasPJuridica = DecisaoPendenciasPJuridica++;
                        context.SaveChanges();

                        apiController.GetQuodPendenciasPJ(CnpjReplace, cnpj.Id);

                        //add na tabela
                        StatusRotina.DecisaoPendenciasPJuridica = DecisaoPendenciasPJuridica++;
                        context.SaveChanges();

                    }

                    var notas = (from t in context.NFEs
                                 where t.status != "Desativado"
                                 where t.DataNota >= DateTime.UtcNow.AddDays(-30)
                                 select t).ToArray();

                    foreach (var nota in notas)
                    {
                        APIController apiController = new APIController(_clientFactory);
                        apiController.GetNFEs(nota.numero);
                    }

                    var clientes = (from t in context.Clientes
                                    where t.DataDelete == null
                                    where t.status == true
                                    select new TableCliente
                                    {
                                        Id = t.Id,
                                        cpfCnpj = t.cpfCnpj,

                                    }).ToArray();

                    foreach (var cliente in clientes)
                    {
                        var clientesReplace = cliente.cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                        if (cliente.tipo != "Sacado")
                        {
                            //APIController apiController = new APIController(_clientFactory);
                            //apiController.GetSCR(clientesReplace, cliente.Id);
                        }
                    }
                }

                if (StatusRotina != null)
                {
                    //add na tabela
                    StatusRotina.DataFinal = DateTime.Now;
                    context.SaveChanges();
                }

                EmailTask.SendFormatedMail("FINAL DE ROTINA",
                    "TI - GPAUDAX", "mbruzolato@gmail.com",
                    "HORARIO DE INICIO DO SERVIDOR:" + DateTime.Now
                    );

                return Ok("Rotina executada com sucesso, base de dados atualizada em todas as APIs");
            }
        }

        [HttpGet] 
        [Route("~/ListarNotificacoes")]
        public IEnumerable<CRMAudax.Models.TableNotificacao> ListarNotificacoes()
        {
            int RegiaoLog = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Country).Value);
            int TypeUserLog = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.System).Value);
            int UsuarioId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            using (var context = new MyDbContext())
            {
                TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

                if (TypeUserLog == 1 || TypeUserLog == 2)
                {
                    var notificacao = (from t in context.Notificacoes
                                       orderby t.Id descending
                                       where !(from ln in context.LeituraNotificacaos
                                               where ln.UsuarioId == UsuarioId
                                               select ln.NotificacaoId).Contains(t.Id)
                                       select new TableNotificacao
                                       {
                                           Id = t.Id,
                                           Informacao = t.Informacao,
                                           DataNotificacao = TimeZoneInfo.ConvertTimeFromUtc(t.DataNotificacao, brasiliaTimeZone),
                                           Cliente = t.Cliente,
                                           ClienteId = t.ClienteId,
                                           Score = t.Score,
                                           Status = t.Status,
                                       }).ToArray().Distinct()
                                       .GroupBy(n => new { n.ClienteId, n.DataNotificacao.Date })
                                       .Select(g => g.First())
                                       .ToArray();

                    return notificacao;
                }
                else
                {
                    var notificacao = (from t in context.Notificacoes
                                       join c in context.Clientes on t.ClienteId equals c.Id
                                       orderby t.Id descending
                                       where c.RegiaoId == RegiaoLog
                                       where !(from ln in context.LeituraNotificacaos
                                               where ln.UsuarioId == UsuarioId
                                               select ln.NotificacaoId).Contains(t.Id)
                                       select new TableNotificacao
                                       {
                                           Id = t.Id,
                                           Informacao = t.Informacao,
                                           DataNotificacao = TimeZoneInfo.ConvertTimeFromUtc(t.DataNotificacao, brasiliaTimeZone),
                                           ClienteId = t.ClienteId,
                                           Cliente = t.Cliente,
                                           Score = t.Score,
                                           Status = t.Status,
                                       }).ToArray().Distinct()
                                       .GroupBy(n => new { n.ClienteId, n.DataNotificacao.Date })
                                       .Select(g => g.First())
                                       .ToArray();

                    return notificacao;
                }
            }
        }

        [HttpPost]
        [Route("~/VisualizarNotificacao/{Id}")]
        public IActionResult VisualizarNotificacoes(long Id)
        {
            int idusuarioClains = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            using (var context = new MyDbContext())
            {
                var r = context.LeituraNotificacaos.Add(new TableLeituraNotificacao
                {
                    UsuarioId = idusuarioClains,
                    NotificacaoId = Id
                });
                context.SaveChanges();
                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ConfereNotificacao")]
        public bool ConfereNotificacao()
        {
            int RegiaoLog = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Country).Value);
            int TypeUserLog = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.System).Value);
            int UsuarioId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            using (var context = new MyDbContext())
            {
                if (TypeUserLog == 1 || TypeUserLog == 2)
                {

                    return (from t in context.Notificacoes
                            orderby t.Id descending
                            where !(from ln in context.LeituraNotificacaos
                                    where ln.UsuarioId == UsuarioId
                                    select ln.NotificacaoId).Contains(t.Id)
                            select t).Any();
                }
                else
                {
                    return (from t in context.Notificacoes
                            join c in context.Clientes on t.ClienteId equals c.Id
                            orderby t.Id descending
                            where c.RegiaoId == RegiaoLog
                            where !(from ln in context.LeituraNotificacaos
                                    where ln.UsuarioId == UsuarioId
                                    select ln.NotificacaoId).Contains(t.Id)
                            select t).Any();

                }
            }
        }

        [HttpGet]
        [Route("~/VisualizarRotina")]
        public CRMAudax.Models.TableStatusRotina VisualizarRotina()
        {
            using (var context = new MyDbContext())
            {
                var rotina = (from t in context.StatusRotinas
                              orderby t.Id descending
                              select t).FirstOrDefault();

                return rotina;

            }
        }
    }
}
