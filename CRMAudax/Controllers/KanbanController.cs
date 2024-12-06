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
    public class KanbanController : Controller
    {
        [HttpPut]
        [Route("~/MoveCardColuna/{ClienteId}/{ColunaId}")]
        public IActionResult MoveCardColuna(long ClienteId, long ColunaId)
        {
            using (var context = new MyDbContext())
            {
                var x = (from t in context.OrdemCartao
                         where t.ClienteId.Equals(ClienteId)
                         select t).ToArray().FirstOrDefault();
                if (x != null)
                {
                    x.ColunaId = ColunaId;
                    context.SaveChanges();
                }
            }
            return Ok();
        }


        [HttpPut]
        [Route("~/OrdenaSequenciacartao")]
        public IActionResult OrdenaSequenciacartao(int[] ArrayOrdemcartao)
        {
            int order = 1;
            foreach (var item in ArrayOrdemcartao)
            {
                using (var context = new MyDbContext())
                {
                    var sequencia = (from t in context.OrdemCartao
                                  where t.ClienteId.Equals(item)
                                  select t).ToArray().FirstOrDefault();
                    if (sequencia != null)
                    {
                        sequencia.ordemCartao = order;
                        context.SaveChanges();
                    }
                    order++;
                }
            }
            return Ok();
        }
    }
}
