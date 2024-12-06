using Microsoft.AspNetCore.Mvc;
using CRMAudax.Models;
using System.Diagnostics.Metrics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace CRMAudax.Controllers
{
    [Authorize]
    public class ImportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("~/ImportSacado")]
        public async Task<IActionResult> ImportSacado()
        {
            try
            {
                var filePath = Path.GetTempFileName();
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var inputStream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(inputStream);
                            byte[] array = new byte[inputStream.Length];
                            inputStream.Seek(0, SeekOrigin.Begin);
                            inputStream.Read(array, 0, array.Length);

                            string fileaux = Encoding.UTF8.GetString(array);

                            //BuscaBordero(fileaux);
                            BuscaNome(fileaux);
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception Message)
            {
                return BadRequest(Message);
                throw;
            }
            return Ok();
        }

        public void BuscaBordero(string bordero)
        {
            int last = bordero.Length;
            foreach (Match m in Regex.Matches(bordero, "Borderô"))
            {
                string nbordero = bordero.Substring(m.Index + 7, 12).Trim();
                if (nbordero != "")
                {
                    Console.WriteLine(nbordero);
                }
            }
        }

        public void BuscaNome(string file)
        {
                string fileaux = file;
            foreach (Match m in Regex.Matches(file, "TotalEmissãoPagto"))
            { 
                int ini = fileaux.IndexOf("TotalEmissãoPagto") + 18;
                int fim = fileaux.IndexOf("Total do Borderô");
                int fimaux = fim + 17;

                string meio = fileaux.Substring(ini, fim - ini);

                fileaux = fileaux.Substring(fimaux, fileaux.LastIndexOf("Total do Borderô") - fim);

                foreach (Match n in Regex.Matches(meio, "\r\n"))
                {
                    Console.WriteLine(meio.Substring(28, n.Index));
                    meio = meio.Substring(n.Index, meio.LastIndexOf("\r\n"));
                }
            }       
        }
    }
}
