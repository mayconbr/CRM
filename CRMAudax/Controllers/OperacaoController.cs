using Microsoft.AspNetCore.Mvc;

namespace CRMAudax.Controllers
{
    public class OperacaoController : Controller
    {

        [HttpGet]
        [Route("~/NewTime")]
        public DateTime NewTime()
        {
            DateTime NewTime = DateTime.UtcNow;
            return(NewTime);
        }
    }
}
