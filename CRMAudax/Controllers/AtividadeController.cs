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

    public class AtividadeController : Controller
    {
        public IActionResult NovaAtividade()
        {
            return View();
        }

        public IActionResult ExibeAtividade(long Id)
        {
            ViewData["atividade"] = ListarAtividadeId(Id);
            return View();
        }

        //Atividade
        [HttpPost]
        [Route("~/CadastrarAtividade")]
        public IActionResult InsertAtividade([FromBody] TableAtividadesCedente request)
        {
            using (var context = new MyDbContext())
            {
                var atividade = (from t in context.Atividades
                              where t.Id.Equals(request.Id)
                              select t).ToArray().FirstOrDefault();
                if (atividade == null)
                {
                    var r = context.Atividades.Add(new TableAtividadesCedente
                    {
                        ClienteId = request.ClienteId,
                        atividade = request.atividade,
                        dataAtividade = request.dataAtividade,
                        descricao = request.descricao
                    }).Entity;

                    context.SaveChanges();
                }
                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ListarAtividade/{Id}")]
        public IEnumerable<CRMAudax.Models.TableAtividadesCedente> ListarAtividade(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.Atividades

                           where t.ClienteId == Id
                           select new TableAtividadesCedente
                           {
                               Id = t.Id,
                               atividade = t.atividade,
                               dataAtividade = t.dataAtividade,
                               descricao = t.descricao

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarAtividadeId/{Id}")]
        public IEnumerable<CRMAudax.Models.TableAtividadesCedente> ListarAtividadeId(long Id)
        {
            using (var context = new MyDbContext())
            {
                return (from t in context.Atividades

                        where t.Id.Equals(Id)

                        select new TableAtividadesCedente
                        {
                            Id = t.Id,
                            atividade = t.atividade,
                            dataAtividade = t.dataAtividade,
                            descricao = t.descricao

                        }).ToArray();
            }
        }
    }
}
