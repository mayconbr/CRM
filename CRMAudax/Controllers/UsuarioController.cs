using Microsoft.AspNetCore.Mvc;
using CRMAudax.Models;
using CRMAudax.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Dynamic;

namespace CRMAudax.Controllers
{
    [Authorize]

    public class UsuarioController : Controller
    {
        public IActionResult TodosUsuarios()
        {
            return View(ListarUsuario());
        }

        public IActionResult NovoUsuario()
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.Regioes = new ConfiguracaoController().ListarRegiao();
            return View(mymodel);
        }

        public IActionResult ExibeUsuario(long Id)
        {

            dynamic mymodel = new ExpandoObject();

            mymodel.Regioes = new ConfiguracaoController().ListarRegiao();
            mymodel.Usuarios  = ListarUsuarioId(Id);

            return View(mymodel);

        }

        public IActionResult GerenciaRegiao() 
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.Regioes = new ConfiguracaoController().ListarRegiao();

            return View(mymodel); 
        }

        [HttpPost]
        [Route("~/Cadastrar")]
        public IActionResult CadastrarUsuario([FromBody] TableUsuario request)
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    string hashString = string.Empty;

                    using (var sha1 = SHA1.Create())
                    {
                        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(request.Email));

                        foreach (byte x in hash)
                        {
                            hashString += String.Format("{0:x2}", x);
                        }
                    }

                    context.Usuarios.Add(new TableUsuario
                    {
                        Nome = request.Nome,
                        Email = request.Email,
                        Senha = request.Senha,
                        TipoUsuario = request.TipoUsuario,
                        Ativo = false,
                        Hash = hashString,
                        RegiaoId = request.RegiaoId 
                    });

                    context.SaveChanges();

                    var link = HttpContext.Request.Host.Host + ':' + HttpContext.Request.Host.Port + "/Home/RecuperarSenha?pass=" + hashString;

                    EmailTask.SendFormatedMail("Novo Usuário - Sistema de Gestão de relacionamento com o cliente",
                    request.Nome, request.Email,
                    "Foi solicitado o cadastro de uma nova senha de acesso. Para continuar com a solicitação, Use o link a seguir:" +
                    "<br/>" +
                    "Copie o link abaixo e cole no seu navegador:" +
                    "<br/>" + link
                    );

                    return Ok();
                }

                catch (Exception)
                {
                    return BadRequest();
                    throw;
                }
            }
        }

        [HttpGet]
        [Route("~/ListarUsuario")]
        public IEnumerable<CRMAudax.Models.TableUsuario> ListarUsuario()
        {
            using (var context = new MyDbContext())
            {
                return (from t in context.Usuarios

                        where t.DataDelete == null

                        select new TableUsuario
                        {
                            Id = t.Id,
                            Nome = t.Nome,
                            Senha = t.Senha,
                            Email = t.Email,
                            Hash = t.Hash,
                            Ativo = t.Ativo,
                            TipoUsuario = t.TipoUsuario,
                            DataDelete = t.DataDelete,
                            RegiaoId = t.RegiaoId,

                        }).ToArray();
            }
        }

        [HttpGet]
        [Route("~/ListarUsuarioId/{Id}")]
        public IEnumerable<CRMAudax.Models.TableUsuario> ListarUsuarioId(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.Usuarios

                           where t.DataDelete == null
                           where t.Id.Equals(Id)

                           select new TableUsuario
                           {
                               Id = t.Id,
                               Nome = t.Nome,
                               Senha = t.Senha,
                               Email = t.Email,
                               Hash = t.Hash,
                               Ativo = t.Ativo,
                               TipoUsuario = t.TipoUsuario,
                               DataDelete = t.DataDelete,
                               RegiaoId = t.RegiaoId,
                               Regiao = t.Regiao
                           }).ToArray();
                return aux;
            }
        }

        [HttpPut]
        [Route("~/AtualizaUsuario")]
        public IActionResult AtualizaUsuario([FromBody] TableUsuario request)
        {
            using (var context = new MyDbContext())
            {
                var usuario = (from t in context.Usuarios
                               where t.Id.Equals(request.Id)
                               select t).ToArray().FirstOrDefault();
                if (usuario != null)
                {
                    usuario.Nome = request.Nome;
                    usuario.Email = request.Email;
                    usuario.TipoUsuario = request.TipoUsuario;
                    usuario.Ativo = request.Ativo;
                    usuario.RegiaoId = request.RegiaoId;
                    context.SaveChanges();
                }
            };
            return Ok();
        }

        [HttpGet]
        [Route("~/ListarOperador")]
        public IEnumerable<CRMAudax.Models.TableUsuario> ListarOperador()
        {
            using (var context = new MyDbContext())
            {
                return (from t in context.Usuarios
                        where t.TipoUsuario == "3"
                        where t.DataDelete == null

                        select new TableUsuario
                        {
                            Id = t.Id,
                            Nome = t.Nome,
                            Senha = t.Senha,
                            Email = t.Email,
                            Hash = t.Hash,
                            Ativo = t.Ativo,
                            TipoUsuario = t.TipoUsuario,
                            DataDelete = t.DataDelete,
                            RegiaoId = t.RegiaoId,

                        }).ToArray();
            }
        }

    }
}
