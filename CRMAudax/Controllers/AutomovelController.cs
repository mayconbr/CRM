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

    public class AutomovelController : Controller
    {
        public IActionResult NovoAutomovel()
        {
            return View();
        }

        public IActionResult ExibeAutomovel(long Id)
        {
            ViewData["automovel"] = ListarAutomovelId(Id);
            return View();
        }

        [HttpPost]
        [Route("~/CadastrarAutomovel")]
        public IActionResult InsertAutomovel([FromBody] TableAutomoveis request)
        {
            using (var context = new MyDbContext())
            {
                var automovel = (from t in context.Automoveis
                              where t.Id.Equals(request.Id)
                              select t).ToArray().FirstOrDefault();
                if (automovel == null)
                {
                    var r = context.Automoveis.Add(new TableAutomoveis
                    {
                        ClienteId = request.ClienteId,
                        marca = request.marca,
                        modelo = request.modelo,
                        ano = request.ano,
                        placa = request.placa,
                        valorFipe = request.valorFipe,
                        valorOnus = request.valorOnus
                    }).Entity;

                    context.SaveChanges();
                }
                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ListarAutomovel/{Id}")]
        public IEnumerable<CRMAudax.Models.TableAutomoveis> ListarAutomovel(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.Automoveis

                           where t.ClienteId == Id
                           select new TableAutomoveis
                           {
                               Id = t.Id,
                               marca = t.marca,
                               modelo = t.modelo,
                               ano = t.ano,
                               placa = t.placa,
                               valorFipe = t.valorFipe,
                               valorOnus = t.valorOnus

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarAutomovelId/{Id}")]
        public IEnumerable<CRMAudax.Models.TableAutomoveis> ListarAutomovelId(long Id)
        {
            using (var context = new MyDbContext())
            {
                return (from t in context.Automoveis

                        where t.Id.Equals(Id)
                        select new TableAutomoveis
                        {
                            Id = t.Id,
                            marca = t.marca,
                            modelo = t.modelo,
                            ano = t.ano,
                            placa = t.placa,
                            valorFipe = t.valorFipe,
                            valorOnus = t.valorOnus

                        }).ToArray();
            }
        }

    }
}
