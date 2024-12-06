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
    public class ImovelController : Controller
    {
        public IActionResult NovoImovel()
        {
            return View();
        }

        public IActionResult ExibeImovel(long Id)
        {
            ViewData["imovel"] = ListarImovelId(Id);
            return View();
        }

        [HttpPost]
        [Route("~/CadastrarImovel")]
        public IActionResult InsertAtividade([FromBody] TableRelacaoBensImoveis request)
        {
            using (var context = new MyDbContext())
            {
                var atividade = (from t in context.RelacoesBensImoveis
                              where t.Id.Equals(request.Id)
                              select t).ToArray().FirstOrDefault();
                if (atividade == null)
                {
                    var r = context.RelacoesBensImoveis.Add(new TableRelacaoBensImoveis
                    {
                        ClienteId = request.ClienteId,
                        localizacao = request.localizacao,
                        matricula = request.matricula,
                        cartorio = request.cartorio,
                        livro = request.livro,
                        situacao = request.situacao,                    
                        valor = request.valor
                    }).Entity;

                    context.SaveChanges();
                }
                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ListarImovel/{Id}")]
        public IEnumerable<CRMAudax.Models.TableRelacaoBensImoveis> ListarImovel(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.RelacoesBensImoveis

                           where t.ClienteId == Id
                           select new TableRelacaoBensImoveis
                           {
                               Id = t.Id,
                               localizacao = t.localizacao,
                               matricula = t.matricula,
                               cartorio = t.cartorio,
                               livro = t.livro,
                               situacao = t.situacao,
                               valor = t.valor

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarImovelId/{Id}")]
        public IEnumerable<CRMAudax.Models.TableRelacaoBensImoveis> ListarImovelId(long Id)
        {
            using (var context = new MyDbContext())
            {
                return (from t in context.RelacoesBensImoveis

                        where t.Id.Equals(Id)
                        select new TableRelacaoBensImoveis
                        {
                            Id = t.Id,
                            localizacao = t.localizacao,
                            matricula = t.matricula,
                            cartorio = t.cartorio,
                            livro = t.livro,
                            situacao = t.situacao,
                            valor = t.valor

                        }).ToArray();
            }
        }
    }
}
