using CRMAudax.Models;
//using CRMAudax.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CoreFtp;
using CoreFtp.Enum;

namespace CRMAudax.Controllers
{
    [Authorize]
    public class FtpController : Controller
	{
        [HttpPost]
        [Route("~/FTPUpload")]
        public async System.Threading.Tasks.Task<IActionResult> FTPUpload()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            var filePath = Path.GetTempFileName();
            foreach (var formFile in Request.Form.Files)
            {
                if (formFile.Length > 0)
                {
                    using (var context = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        string rootPath = "www/uploads/CRM";
                        try
                        {
                            await context.LoginAsync();
                            await context.ChangeWorkingDirectoryAsync(rootPath);
                        }
                        catch (Exception ex)
                        {
                            BadRequest(ex.Message);
                            throw;
                        }

                        using (var writeStream = await context.OpenFileWriteStreamAsync(formFile.FileName))
                        {
                            await formFile.CopyToAsync(writeStream);
                            return Ok();
                        }
                    }
                }
            }
            return BadRequest();
        }
    }
}

