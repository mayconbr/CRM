using CoreFtp;
using CRMAudax.Models;
using CRMAudax.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CRMAudax.Controllers
{
    public class LoginController : Controller
    {
        //[HttpPost]
        //[Route("~/Autenticacao")]
        //public async Task<IActionResult> Autenticacao([FromBody] AuxSenha request)
        //{
        //    using (var context = new MyDbContext())
        //    {
        //        var tg = (from t in context.Usuarios
        //                  where t.Senha.Equals(request.Senha)
        //                  where t.Email.Equals(request.Email)
        //                  where t.Ativo.Equals(true)
        //                  select new TableUsuario
        //                  {
        //                      Id = t.Id,
        //                      Nome = t.Nome,
        //                      TipoUsuario = t.TipoUsuario,
        //                      Email = t.Email,
        //                      RegiaoId = t.RegiaoId,
        //                  }).ToArray().FirstOrDefault();

        //        var claims = new List<Claim>();

        //        if (tg != null)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Name, tg.Nome));
        //            claims.Add(new Claim(ClaimTypes.Sid, Convert.ToString(tg.Id)));
        //            claims.Add(new Claim(ClaimTypes.Email, tg.Email));
        //            claims.Add(new Claim(ClaimTypes.System, tg.TipoUsuario));
        //            claims.Add(new Claim(ClaimTypes.Country, tg.RegiaoId.ToString()));
        //            var id = new ClaimsIdentity(claims, "Basic");

        //            try
        //            {
        //                await HttpContext.SignInAsync(
        //                CookieAuthenticationDefaults.AuthenticationScheme,
        //                new ClaimsPrincipal(id),
        //                new AuthenticationProperties
        //                {
        //                    IsPersistent = true,
        //                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                BadRequest(ex);
        //                throw;
        //            }
        //        }
        //        return Ok(tg);
        //    }
        //}

        [HttpPost]
        [Route("~/Autenticacao")]
        public async Task<IActionResult> Autenticacao([FromBody] AuxSenha request)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync("https://login.gpaudax.com.br/AuthExterna", request);

                if (!response.IsSuccessStatusCode)
                {
                    return Unauthorized();
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                    string Name = jsonResponse.user.name.ToString();

                    string token = jsonResponse.token.ToString();

                    string tokenIntern = AuthTools.GenerateToken(Name);

                    var claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, Convert.ToString(jsonResponse.user.name)));
                    claims.Add(new Claim(ClaimTypes.Sid, Convert.ToString(jsonResponse.user.id)));
                    claims.Add(new Claim(ClaimTypes.Email, Convert.ToString(jsonResponse.user.email)));
                    claims.Add(new Claim(ClaimTypes.System, Convert.ToString(jsonResponse.user.type.id)));
                    claims.Add(new Claim(ClaimTypes.Country, Convert.ToString(jsonResponse.user.subCategory.id)));


                    var id = new ClaimsIdentity(claims, "Basic");

                    try
                    {
                        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(id),
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                        });
                    }
                    catch (Exception ex)
                    {
                        BadRequest(ex);
                        return Unauthorized();
                    }

                    string name = jsonResponse.user.name;
                    LogLogin(name);

                    return Ok();
                }
            }
            catch (HttpRequestException ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("~/Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
            }
            catch (Exception ex)
            {
                BadRequest(ex);
                throw;
            }
            return Ok();
        }

        [HttpPut]
        [Route("~/CadastroSenha")]
        public IActionResult CadastroSenha([FromBody] TableUsuario request)
        {
            using (var context = new MyDbContext())
            {
                var usuario = (from t in context.Usuarios
                               where t.Hash.Equals(request.Hash)
                               select t).ToArray().FirstOrDefault();
                if (usuario != null)
                {
                    usuario.Senha = request.Senha;
                    usuario.Ativo = true;
                    context.SaveChanges();
                }
            };
            return Ok();
        }

        [HttpPost]
        [Route("~/LogLogin")]
        public IActionResult LogLogin(string Name)
        {
            using (var context = new MyDbContext())
            {
                var log = context.LogsLogin.Add(new TableLogLogin
                {
                    DateLogin = DateTime.Now,
                    NomeUser = Name,
                });
                context.SaveChanges();
            }
            return Ok();
        }

    }
}

