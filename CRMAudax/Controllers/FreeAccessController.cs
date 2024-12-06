using Microsoft.AspNetCore.Mvc;
using CRMAudax.Models;
using System.Diagnostics.Metrics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using CoreFtp;
using CRMAudax.Tools;
using System.IO;


namespace CRMAudax.Controllers
{
    public class FreeAccessController : Controller
    {
        public IActionResult CompartilhaPasta(string Hash)
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.Arquivos = ConfirmShareFolder(Hash);        
            return View(mymodel);
        }

        [HttpGet]
        [Route("~/ConfirmShareFile/{Hash}")]
        public async Task<IActionResult> ConfirmShareFile(string Hash)
        {
            using (var context = new MyDbContext())
            {
                var FileShare = (from t in context.CompartilhaArquivos
                                 where t.Hash == Hash
                                 select t).FirstOrDefault();

                if (FileShare != null)
                {
                    var d = (from j in context.ArquivosPastas
                             where j.Id == FileShare.ArquivoId
                             select j).FirstOrDefault();

                    IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                   .AddJsonFile("appsettings.json")
                   .Build();

                    using (var context2 = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        if (d != null)
                        {
                            string arquivo = d.pathArquivo + "/" + d.nomeArquivo;
                            var tempFile = new FileInfo(arquivo);
                            await context2.LoginAsync();
                            using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                            {
                                byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                                var content = new System.IO.MemoryStream(buff);
                                var contentType = d.tipoArquivo;
                                var fileName = d.nomeArquivo;
                                return File(content, contentType, fileName);
                            }
                        }
                    }
                    return BadRequest();
                }

                return Ok();
            }
        }

        [HttpGet]
        [Route("~/ConfirmShareFolder/{Hash}")]
        public IEnumerable<CRMAudax.Models.TableArquivosPastas> ConfirmShareFolder(string Hash)
        {
            using (var context = new MyDbContext())
            {
                var FolderShare = (from t in context.CompartilhaArquivos
                                   where t.Hash == Hash
                                   select t).FirstOrDefault();

                if (FolderShare != null)
                {
                    var pasta = (from t in context.ArquivosPastas

                                 where t.PastaId == FolderShare.PastaId
                                 select new TableArquivosPastas
                                 {
                                     Id = t.Id,
                                     nomeArquivo = t.nomeArquivo,
                                     pathArquivo = t.pathArquivo,
                                     tipoArquivo = t.tipoArquivo,
                                     dataEnvio = t.dataEnvio,

                                 }).ToArray();
                    return pasta;
                }
                else
                {
                    return null;
                }
            }
        }

        [HttpGet]
        [Route("~/DownloadFileShared/{Id}")]
        public async Task<IActionResult> DownloadFileShared(long Id)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            using (var context = new MyDbContext())
            {
                var d = (from j in context.ArquivosPastas
                         where j.Id.Equals(Id)
                         select j).FirstOrDefault();

                using (var context2 = new FtpClient(new FtpClientConfiguration
                {
                    Host = configuration.GetSection("FtpCredentials")["Host"],
                    Username = configuration.GetSection("FtpCredentials")["Username"],
                    Password = configuration.GetSection("FtpCredentials")["Password"],
                    Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                }))
                {
                    if (d != null)
                    {
                        string arquivo = d.pathArquivo + "/" + d.nomeArquivo;
                        var tempFile = new FileInfo(arquivo);
                        await context2.LoginAsync();
                        using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                        {
                            byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                            var content = new System.IO.MemoryStream(buff);
                            var contentType = d.tipoArquivo;
                            var fileName = d.nomeArquivo;
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return BadRequest();
            }
        }


    }
}
