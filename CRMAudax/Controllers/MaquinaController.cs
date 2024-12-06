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

    public class MaquinaController : Controller
    {
        public IActionResult NovaMaquina()
        {
            return View();
        }

        public IActionResult ExibeMaquina(long Id)
        {
            ViewData["maquina"] = ListarMaquinaId(Id);
            return View();
        }

        [HttpPost]
        [Route("~/CadastrarMaquina")]
        public IActionResult InsertMaquina([FromBody] TableMaquinasEquipamentos request)
        {
            using (var context = new MyDbContext())
            {
                var maquina = (from t in context.MaquinasEquipamentos
                              where t.Id.Equals(request.Id)
                              select t).ToArray().FirstOrDefault();
                if (maquina == null)
                {
                    var r = context.MaquinasEquipamentos.Add(new TableMaquinasEquipamentos
                    {
                        ClienteId = request.ClienteId,
                        nomeEquipamento = request.nomeEquipamento,
                        marca = request.marca,
                        ano = request.ano,
                        valorFinanciado = request.valorFinanciado,
                        valorMaquina = request.valorMaquina,
                        valorOnus = request.valorOnus,
                    }).Entity;

                    context.SaveChanges();
                }
                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ListarMaquina/{Id}")]
        public IEnumerable<CRMAudax.Models.TableMaquinasEquipamentos> ListarMaquina(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.MaquinasEquipamentos

                           where t.ClienteId == Id
                           select new TableMaquinasEquipamentos
                           {
                               Id = t.Id,
                               nomeEquipamento = t.nomeEquipamento,
                               marca = t.marca,
                               ano = t.ano,
                               valorFinanciado = t.valorFinanciado,
                               valorMaquina = t.valorMaquina,
                               valorOnus = t.valorOnus,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarMaquinaId/{Id}")]
        public IEnumerable<CRMAudax.Models.TableMaquinasEquipamentos> ListarMaquinaId(long Id)
        {
            using (var context = new MyDbContext())
            {
                return (from t in context.MaquinasEquipamentos

                        where t.Id.Equals(Id)
                        select new TableMaquinasEquipamentos
                        {
                            Id = t.Id,
                            nomeEquipamento = t.nomeEquipamento,
                            marca = t.marca,
                            ano = t.ano,
                            valorFinanciado = t.valorFinanciado,
                            valorMaquina = t.valorMaquina,
                            valorOnus = t.valorOnus,

                        }).ToArray();
            }
        }

    }
}
