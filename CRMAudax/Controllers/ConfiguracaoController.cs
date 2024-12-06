using CRMAudax.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CRMAudax.Controllers
{
    [Authorize]

    public class ConfiguracaoController : Controller
    {

        public IActionResult Configuracao()
        {
            return View();
        }

        public IActionResult ConfiguracaoKanban()
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.Boards = ListarTabelasKanban();
            mymodel.Regioes = ListarRegiao();
            return View(mymodel);
        }

        public IActionResult Kanban()
        {
            string nomeDoCliente = Request.Query["nomeDoCliente"];
           

            dynamic mymodel = new ExpandoObject();

            int? regiaoid = null;

            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Country);

            if (claim != null)
            {
                regiaoid = Convert.ToInt32(claim.Value);
            }

            mymodel.Boards = ListarTabelasKanban();
            mymodel.Clientes = new CedenteController().ListarProponenteColuna(nomeDoCliente, regiaoid);
            return View(mymodel);
        }

        [HttpPost]
        [Route("~/CadastrarKanban")]
        public IActionResult InsertKanban([FromBody] TableColunaKanban request)
        {
            using (var context = new MyDbContext())
            {
                var OrdemMax = (from t in context.ColunasKanban
                                orderby t.ordem descending
                                select t).ToArray().FirstOrDefault();

                if (OrdemMax != null)
                {
                    var r = context.ColunasKanban.Add(new TableColunaKanban
                    {
                        nome = request.nome,
                        ordem = OrdemMax.ordem + 1

                    }).Entity;

                    context.SaveChanges();
                }

                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ListarTabelasKanban")]
        public IEnumerable<CRMAudax.Models.TableColunaKanban> ListarTabelasKanban()
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.ColunasKanban
                           where t.DataDelete == null
                           orderby t.ordem
                           select new TableColunaKanban
                           {
                               Id = t.Id,
                               nome = t.nome,
                               ordem = t.ordem
                           }).ToArray();
                return aux;
            }
        }

        [HttpPut]
        [Route("~/DeleteTabelasKanban/{Id}")]
        public IActionResult DeleteTabelasKanban(long Id)
        {
            using (var context = new MyDbContext())
            {
                var coluna = (from t in context.ColunasKanban
                              where t.Id.Equals(Id)
                              select t).ToArray().FirstOrDefault();
                if (coluna != null)
                {
                    coluna.DataDelete = DateTime.UtcNow;
                    context.SaveChanges();
                }
                return Ok();
            }
        }

        [HttpPut]
        [Route("~/OrderTabelasKanban")]
        public IActionResult OrderTabelasKanban(int[] AuxOrderKanban)
        {
            int order = 1;
            foreach (var item in AuxOrderKanban)
            {
                using (var context = new MyDbContext())
                {
                    var coluna = (from t in context.ColunasKanban
                                  where t.Id.Equals(item)
                                  select t).ToArray().FirstOrDefault();

                    if (coluna != null)
                    {
                        coluna.ordem = order;
                        context.SaveChanges();
                    }
                    order++;
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("~/CadastrarRegiao")]
        public IActionResult CadastrarRegiao([FromBody] TableRegiao request)
        {
            using (var context = new MyDbContext())
            {
                var r = context.Regioes.Add(new TableRegiao
                {
                    Nome = request.Nome,

                }).Entity;

                context.SaveChanges();

                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ListarRegiao")]
        public IEnumerable<CRMAudax.Models.TableRegiao> ListarRegiao()
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.Regioes

                           select new TableRegiao
                           {
                               Id = t.Id,
                               Nome = t.Nome,

                           }).ToArray();
                return aux;
            }
        }


    }
}