using CoreFtp;
using CRMAudax.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using CoreFtp;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using static System.Net.WebRequestMethods;
using CoreFtp.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CRMAudax.Tools;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using System.Security.Cryptography;


namespace CRMAudax.Controllers
{
    [Authorize]
    public class PastasController : Controller
    {
        public ActionResult TodasPastas()
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.Pastas = ListarPastas();
            return View(mymodel);

        }

        public ActionResult ExibePasta(long Id)
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.Pastas = ListarPastasId(Id);
            mymodel.Arquivos = ListarFilePasta(Id);

            return View(mymodel);

        }

        [HttpPost]
        [Route("~/CriarPastas")]
        public async Task CriarPastas([FromBody] TablePastas request)
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            long PastaId;

            using (var context = new MyDbContext())
            {
                var r = context.Pastas.Add(new TablePastas
                {
                    NomePasta = request.NomePasta,
                    UsuarioId = UserId,
                }).Entity;

                context.SaveChanges();
                PastaId = r.Id;

                r.FTP = "www/uploads/CRM/Pastas/User/" + UserId + "/" + r.Id.ToString();
                context.SaveChanges();
            }

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            using (var ftpClient = new FtpClient(new FtpClientConfiguration
            {
                Host = configuration.GetSection("FtpCredentials")["Host"],
                Username = configuration.GetSection("FtpCredentials")["Username"],
                Password = configuration.GetSection("FtpCredentials")["Password"],
                Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
            }))
            {
                string rootPath = "www/uploads/CRM/Pastas/User/" + UserId + "/" + PastaId.ToString();

                await ftpClient.LoginAsync();
                await ftpClient.CreateDirectoryAsync(rootPath);

            }
        }

        [HttpGet]
        [Route("~/ListarPastas")]
        public IEnumerable<CRMAudax.Models.TablePastas> ListarPastas()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            using (var context = new MyDbContext())
            {
                var pastas = (from t in context.Pastas
                              where t.DataDelete == null
                              where t.UsuarioId == UserId
                              select new TablePastas
                              {
                                  Id = t.Id,
                                  NomePasta = t.NomePasta,
                                  FTP = t.FTP,

                              }).ToArray();

                return pastas;
            }
        }

        [HttpPut]
        [Route("~/DeletePasta/{Id}")]
        public async Task DeletePasta(long Id)
        {
            using (var context = new MyDbContext())
            {
                var pasta = (from t in context.Pastas
                             where t.Id.Equals(Id)
                             select t).ToArray().FirstOrDefault();
                if (pasta != null)
                {
                    pasta.DataDelete = DateTime.UtcNow;
                    context.SaveChanges();

                    IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                    using (var ftpClient = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        await ftpClient.LoginAsync();

                        await ftpClient.ChangeWorkingDirectoryAsync("www/uploads/CRM/Pastas/User/" + pasta.UsuarioId + "/" + pasta.Id);
                        var ListFile = await ftpClient.ListFilesAsync();

                        foreach (var item in ListFile)
                        {
                            await ftpClient.DeleteFileAsync(item.Name);
                        }

                        await ftpClient.ChangeWorkingDirectoryAsync("..");

                        var ListDirectory = await ftpClient.ListDirectoriesAsync();

                        foreach (var item in ListDirectory)
                        {
                            if (item.Name == Id.ToString())
                            {
                                await ftpClient.DeleteDirectoryAsync(item.Name);
                            }
                        }
                    }
                }
            }
        }


        [HttpGet]
        [Route("~/ListarPastasId")]
        public IEnumerable<CRMAudax.Models.TablePastas> ListarPastasId(long Id)
        {
            using (var context = new MyDbContext())
            {
                var pastas = (from t in context.Pastas
                              where t.DataDelete == null
                              where t.Id == Id
                              select new TablePastas
                              {
                                  Id = t.Id,
                                  NomePasta = t.NomePasta,
                                  FTP = t.FTP,

                              }).ToArray();

                return pastas;
            }
        }


        [HttpPost]
        [Route("~/UploadFilePasta/{IdPasta}")]
        public async System.Threading.Tasks.Task<IActionResult> UploadFilePasta(long IdPasta)
        {
            using (var context = new MyDbContext())
            {
                var pastas = (from t in context.Pastas
                              where t.DataDelete == null
                              where t.Id == IdPasta
                              select t).FirstOrDefault();

                IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();

                var filePath = Path.GetTempFileName();
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var context2 = new FtpClient(new FtpClientConfiguration
                        {
                            Host = configuration.GetSection("FtpCredentials")["Host"],
                            Username = configuration.GetSection("FtpCredentials")["Username"],
                            Password = configuration.GetSection("FtpCredentials")["Password"],
                            Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                        }))
                        {
                            string rootPath = "www/uploads/CRM/Pastas/User/" + pastas.UsuarioId + "/" + IdPasta;
                            try
                            {
                                await context2.LoginAsync();
                                await context2.ChangeWorkingDirectoryAsync(rootPath);

                                using (var writeStream = await context2.OpenFileWriteStreamAsync(formFile.FileName))
                                {
                                    await formFile.CopyToAsync(writeStream);
                                }


                                var c = context.ArquivosPastas.Add(new TableArquivosPastas
                                {
                                    PastaId = IdPasta,
                                    UsuarioId = pastas.UsuarioId,
                                    pathArquivo = rootPath,
                                    tipoArquivo = formFile.ContentType,
                                    nomeArquivo = formFile.FileName,
                                    dataEnvio = DateTime.UtcNow
                                }).Entity;
                                context.SaveChanges();


                                return Ok();
                            }
                            catch (Exception ex)
                            {
                                return BadRequest(ex.Message);
                                throw;
                            }
                        }
                    }
                }
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("~/ListarFilePasta/{Id}")]
        public IEnumerable<CRMAudax.Models.TableArquivosPastas> ListarFilePasta(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.ArquivosPastas

                           where t.PastaId == Id
                           select new TableArquivosPastas
                           {
                               Id = t.Id,
                               nomeArquivo = t.nomeArquivo,
                               pathArquivo = t.pathArquivo,
                               tipoArquivo = t.tipoArquivo,
                               dataEnvio = t.dataEnvio,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/DownloadFilePasta/{Id}")]
        public async Task<IActionResult> DownloadFilePasta(long Id)
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

        [HttpPost]
        [Route("~/ShareFile/{Id}")]
        public IActionResult ShareFile(long Id)
        {
            string hashString = string.Empty;
            using (var context = new MyDbContext())
            {                
                var arquivo = (from t in context.ArquivosPastas
                               where t.Id == Id
                               select t).FirstOrDefault();
               
                var pastas = (from t in context.Pastas
                              where t.DataDelete == null
                              where t.Id == arquivo.PastaId
                              select t).FirstOrDefault();               

                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] bytes = new byte[64];
                    rng.GetBytes(bytes);

                    using (var sha1 = SHA1.Create())
                    {
                        byte[] hash = sha1.ComputeHash(bytes);

                        foreach (byte x in hash)
                        {
                            hashString += String.Format("{0:x2}", x);
                        }
                    }
                }

                string path = arquivo.pathArquivo;

                var sharefile = context.CompartilhaArquivos.Add(new TableCompartilhaArquivo
                {
                    PastaId = pastas.Id,
                    Data = DateTime.UtcNow,
                    UsuarioId = pastas.UsuarioId,
                    Path = path,
                    Hash = hashString,
                    TipoPasta = "File",
                     ArquivoId = Id

                }).Entity;
                context.SaveChanges();
            }
            return Ok(hashString);
        }

        [HttpPost]
        [Route("~/ShareFolder/{Id}")]
        public IActionResult ShareFolder(long Id)
        {
            string hashString = string.Empty;
            using (var context = new MyDbContext())
            {

                var pastas = (from t in context.Pastas
                              where t.DataDelete == null
                              where t.Id == Id
                              select t).FirstOrDefault();

                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] bytes = new byte[64];
                    rng.GetBytes(bytes);

                    using (var sha1 = SHA1.Create())
                    {
                        byte[] hash = sha1.ComputeHash(bytes);

                        foreach (byte x in hash)
                        {
                            hashString += String.Format("{0:x2}", x);
                        }
                    }
                }

                var sharefile = context.CompartilhaArquivos.Add(new TableCompartilhaArquivo
                {
                    PastaId = pastas.Id,
                    Data = DateTime.UtcNow,
                    UsuarioId = pastas.UsuarioId,
                    Path = pastas.FTP,
                    Hash = hashString,
                    TipoPasta = "Pasta",
                    ArquivoId = null

                }).Entity;
                context.SaveChanges();
            }
            return Ok(hashString);
        }

    }
}
