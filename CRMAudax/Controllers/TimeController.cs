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
    public class TimeController : Controller
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
