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

    public class NFEController : Controller
    {
        public IActionResult NovaNFE()
        {
            return View();
        }

        [HttpPost]
        [Route("~/CadastrarNFE/{ClienteId}/{numero}")]
        public IActionResult InsertAtividade(long ClienteId, string numero)
        {
            using (var context = new MyDbContext())
            {
                var NFE = (from t in context.NFEs
                           where t.ClienteId == ClienteId
                           where t.numero == numero
                           select t).ToArray().FirstOrDefault();

                if (NFE == null)
                {
                    var r = context.NFEs.Add(new TableNFE
                    {
                        ClienteId = ClienteId,
                        numero = numero,
                        status = "Ativo",
                        DataNota =  DateTime.Now
                    }).Entity;

                    context.SaveChanges();
                }
                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ListarNFE/{Id}")]
        public IEnumerable<CRMAudax.Models.TableNFE> ListarNFE(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.NFEs
                           where t.ClienteId == Id
                           select new TableNFE
                           {
                               Id = t.Id,
                               ClienteId = t.ClienteId,
                               numero = t.numero,
                               status = t.status,
                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/ListarNFEId/{Id}")]
        public IEnumerable<CRMAudax.Models.TableNFE> ListarNFEId(long Id)
        {
            using (var context = new MyDbContext())
            {
                return (from t in context.NFEs

                        where t.Id.Equals(Id)

                        select new TableNFE
                        {
                            Id = t.Id,
                            ClienteId = t.ClienteId,
                            numero = t.numero,
                            status = t.status,

                        }).ToArray();
            }
        }


        [HttpPut]
        [Route("~/UpdateStatusNFE/{status}/{numero}")]
        public IActionResult UpdateStatusNFE(string status, string numero)
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    var tg = (from t in context.NFEs
                              where t.numero.Equals(numero)
                              select t).ToArray();

                    if (tg != null)
                    {
                        foreach (var item in tg)
                        {
                            item.status = status;
                        }
                        context.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    return BadRequest();
                    throw;
                }
                return Ok();
            }
        }



    }
}
