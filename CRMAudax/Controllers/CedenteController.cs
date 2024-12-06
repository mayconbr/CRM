using Microsoft.AspNetCore.Mvc;
using CRMAudax.Models;
using System.Diagnostics.Metrics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using CoreFtp;
using CRMAudax.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using static CRMAudax.Models.AuxUsuario;

namespace CRMAudax.Controllers
{
    [Authorize]
    public class CedenteController : Controller
    {

        public IActionResult NovoSacado()
        {
            return View();
        }

        public IActionResult NovoProponentePF()
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.Regioes = new ConfiguracaoController().ListarRegiao();
            mymodel.Operadores = new UsuarioController().ListarOperador();
            return View(mymodel);
        }

        public IActionResult NovoProponentePJ()
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.Regioes = new ConfiguracaoController().ListarRegiao();
            mymodel.Operadores = new UsuarioController().ListarOperador();
            return View(mymodel);
        }

        public IActionResult TodosCadastros()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Regioes = new ConfiguracaoController().ListarRegiao();
            mymodel.Operadores = new UsuarioController().ListarOperador();
            
            string nomeDoCliente = Request.Query["nomeDoCliente"];
            string tipoDoCliente = Request.Query["tipoDoCliente"];
            string situacaoDoCliente = Request.Query["situacaoDoCliente"];
            bool? statusDoCliente = bool.TryParse(Request.Query["statusDoCliente"], out bool parsedValue) ? (bool?)parsedValue : null;

            string QRegiaoId = Request.Query["RegiaoId"];
            long? RegiaoId = null;

            string QUsuarioId = Request.Query["UsuarioId"];
            long? UsuarioId = null;

            if (QRegiaoId != null && QRegiaoId != "")
            {
                RegiaoId = Convert.ToInt64(QRegiaoId);
            }

            if (QUsuarioId != null && QUsuarioId != "")
            {
                UsuarioId = Convert.ToInt64(QUsuarioId);
            }

            mymodel.Proponente = ListarProponente(nomeDoCliente, tipoDoCliente, situacaoDoCliente, statusDoCliente, RegiaoId, UsuarioId);
            return View(mymodel);
        }

        public IActionResult EditarCedente(long Id)
        {
            return View(ListarProponenteId(Id));
        }

        public ActionResult ExibeCadastros(long Id)
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.Cedentes = ListarProponenteId(Id);
            mymodel.ContratosSociais = ListarFileContratoSocial(Id);
            mymodel.ComprovantesEnderecos = ListarFileComprovanteEndereco(Id);
            mymodel.CartoesCnpj = ListarFileCartaoCNPJ(Id);
            mymodel.Faturamentos = ListarFileFaturamento(Id);
            mymodel.FaturamentosFiscais = ListarFileFaturamentoFiscal(Id);
            mymodel.CpfRgCnhs = ListarFileCpfRgCnh(Id);
            mymodel.ImpostosRenda = ListarFileImpostoRenda(Id);
            mymodel.Comites = new ComiteController().ListarComite(Id);
            mymodel.Atividades = new AtividadeController().ListarAtividade(Id);
            mymodel.Visitas = new VisitaController().ListarVisita(Id);
            mymodel.Imoveis = new ImovelController().ListarImovel(Id);
            mymodel.Maquinas = new MaquinaController().ListarMaquina(Id);
            mymodel.Automoveis = new AutomovelController().ListarAutomovel(Id);
            mymodel.NFEs = new NFEController().ListarNFE(Id);
            mymodel.Questionarios = new QuestionarioController().ListarDataComiteCedente(Id);
            mymodel.QuodScores = new ScoreController().ListarQuodScore(Id);
            mymodel.DecisaoScores = new ScoreController().ListarDecisaoScore(Id);
            mymodel.Sintese = new ScoreController().ListarSinteseDecisao(Id);
            //mymodel.RaitingQuods = new ScoreController().ListarQuodScoreRaiting(Id);
            //mymodel.RaitingDecisaos = new ScoreController().ListarDecisaoScoreRaiting(Id);
            mymodel.DecisaoProtestos = new ScoreController().ListarProtestosDecisao(Id);
            mymodel.UltimosScoresQuod = new ScoreController().ListarUltimoQuodScore(Id);
            mymodel.UltimosSCR = new ScoreController().ListarUltimoSCR(Id);           
            mymodel.UltimosScoresDecisao = new ScoreController().ListarUltimoDecisaoScore(Id);
            mymodel.Limites = new QuestionarioController().ListarLimite(Id);
            mymodel.QuodProtestos = new ScoreController().ListarProtestosQuod(Id);
            mymodel.PendenciasDecisao = new ScoreController().ListarPendenciaDecisao(Id);
            mymodel.PendenciasQuod = new ScoreController().ListarPendenciaQuod(Id);
            mymodel.EndividamentosSCR = new ScoreController().ListarendividamentoSCR(Id);
            mymodel.Parecer = ListarParecer(Id);
            mymodel.ArquivosDap = ListarFileDAP(Id);
            mymodel.DeclaracoesC = ListarFileDeclaracaoC(Id);
            return View(mymodel);
        }

        [HttpPost]
        [Route("~/CadastrarProponente")]
        public IActionResult InsertProponente([FromBody] TableCliente request)
        {            
            using (var context = new MyDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    long idProponente;
                    var cd = (from t in context.Clientes
                              where t.cpfCnpj.Equals(request.cpfCnpj)
                              where t.tipo == "Cedente" || t.tipo == "Proponente"
                              select t).ToArray().FirstOrDefault();
                    if (cd == null)
                    {

                        var r = context.Clientes.Add(new TableCliente
                        {
                            tipoPessoa = request.tipoPessoa,
                            nome = request.nome,
                            cpfCnpj = request.cpfCnpj,
                            email = request.email,
                            telefone = request.telefone,
                            complemento = request.complemento,
                            cidade = request.cidade,
                            estado = request.estado,
                            site = request.site,
                            rua = request.rua,
                            cep = request.cep,
                            numero = request.numero,
                            bairro = request.bairro, 
                            parecer = request.parecer,
                            ramoAtividade = request.ramoAtividade,
                            faturamento = request.faturamento,
                            dataFundacaoCedente = request.dataFundacaoCedente,
                            InscricaoEstadual = request.InscricaoEstadual,
                            razaoSocialCedente = request.razaoSocialCedente,

                            tipo = "Proponente",
                            situacao = "Analise",
                            status = false,

                            recorreu = request.recorreu,
                            pep = request.pep,
                            socio = request.socio,
                            referencia = request.referencia,
                            cpfSocio = request.cpfSocio,
                            nomeSocio = request.nomeSocio,
                            enderecoSocio = request.enderecoSocio,
                            celularSocio = request.celularSocio,
                            EmailSocio = request.EmailSocio,
                            TipoPessoaSocio = request.TipoPessoaSocio,

                            DataCadastro = DateTime.Now,

                            RegiaoId = request.RegiaoId,
                            UsuarioId = request.UsuarioId,

                            contaBancaria = request.contaBancaria,

                            celularSocioSegundo = request.celularSocioSegundo,
                            enderecoSocioSegundo = request.enderecoSocioSegundo,
                            nomeSocioSegundo = request.nomeSocioSegundo,
                            cpfSocioSegundo = request.socioSegundo,
                            pepSegundo = request.pepSegundo,
                            socioSegundo = request.socioSegundo,
                            EmailSegundo = request.EmailSegundo,
                            TipoPessoaSocioSegundo = request.TipoPessoaSocioSegundo,

                            celularSocioTerceiro = request.celularSocioTerceiro,
                            enderecoSocioTerceiro = request.enderecoSocioTerceiro,
                            nomeSocioTerceiro = request.nomeSocioTerceiro,
                            cpfSocioTerceiro = request.cpfSocioTerceiro,
                            pepTerceiro = request.pepTerceiro,
                            socioTerceiro = request.pepTerceiro,
                            EmailTerceiro = request.EmailTerceiro,
                            TipoPessoaSocioTerceiro =   request.TipoPessoaSocioTerceiro,

                            AssinaturaSocio = request.AssinaturaSocio,
                            AssinaturaSocioSegundo = request.AssinaturaSocioSegundo,
                            AssinaturaSocioTerceiro = request.AssinaturaSocioTerceiro

                        }).Entity;

                        context.SaveChanges();

                        idProponente = r.Id;

                        var o = context.OrdemCartao.Add(new TableOrdemCartaoKanban
                        {
                            ClienteId = idProponente,
                            ColunaId = 1,
                            ordemCartao = 0,

                        }).Entity;

                        context.SaveChanges();
                    }
                    else
                    {
                        idProponente = cd.Id;
                        return BadRequest("Cliente já cadastrado");
                    }
                    try
                    {
                        context.SaveChanges();
                        transaction.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                        return BadRequest(ex);
                    }
                }
            }
        }

        [HttpPut]
        [Route("~/AtualizaProponente")]
        public IActionResult AtualizaProponente([FromBody] TableCliente request)
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    var tg = (from t in context.Clientes
                              where t.Id.Equals(request.Id)
                              select t).ToArray().FirstOrDefault();

                    if (tg != null)
                    {
                        tg.recorreu = request.recorreu;
                        tg.nome = request.nome;
                        tg.razaoSocialCedente = request.razaoSocialCedente;
                        tg.cpfCnpj = request.cpfCnpj;
                        tg.ramoAtividade = request.ramoAtividade;
                        tg.telefone = request.telefone;
                        tg.email = request.email;
                        tg.cep = request.cep;
                        tg.rua = request.rua;
                        tg.numero = request.numero;
                        tg.complemento = request.complemento;
                        tg.cidade = request.cidade;
                        tg.estado = request.estado;
                        tg.bairro = request.bairro;

                        tg.nomeResponsavel = request.nomeResponsavel;
                        tg.site = request.site;
                        tg.faturamento = request.faturamento;                       

                        tg.TipoPessoaSocio = request.TipoPessoaSocio;
                        tg.TipoPessoaSocioSegundo = request.TipoPessoaSocioSegundo;
                        tg.TipoPessoaSocioTerceiro = request.TipoPessoaSocioTerceiro;

                        tg.nomeSocio = request.nomeSocio;
                        tg.nomeSocioSegundo = request.nomeSocioSegundo;
                        tg.nomeSocioTerceiro = request.nomeSocioTerceiro;

                        tg.cpfSocio = request.cpfSocio;
                        tg.cpfSocioSegundo = request.cpfSocioSegundo;
                        tg.cpfSocioTerceiro = request.cpfSocioTerceiro;

                        tg.enderecoSocio = request.enderecoSocio;
                        tg.enderecoSocioSegundo = request.enderecoSocioSegundo;
                        tg.enderecoSocioTerceiro = request.enderecoSocioTerceiro;

                        tg.celularSocio = request.celularSocio;
                        tg.celularSocioSegundo = request.celularSocioSegundo;
                        tg.celularSocioTerceiro = request.celularSocioTerceiro;

                        tg.pep = request.pep;
                        tg.pepSegundo = request.pepSegundo;
                        tg.pepTerceiro = request.pepTerceiro;
                       
                        tg.socio = request.socio;
                        tg.socioSegundo = request.socioSegundo;
                        tg.socioTerceiro = request.socioTerceiro;

                        tg.EmailSocio = request.EmailSocio;
                        tg.EmailSegundo = request.EmailSegundo;
                        tg.EmailTerceiro = request.EmailTerceiro;

                        tg.contaBancaria = request.contaBancaria;
                        tg.referencia = request.referencia;
                        tg.parecer = request.parecer;

                        //tg.tipo = request.tipo;
                        //tg.status = request.status;
                        //tg.situacao = request.situacao;

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

        [HttpDelete]
        [Route("~/DeleteProponente/{Id}")]
        public IActionResult DeleteProponente(long Id)
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    var tg = (from t in context.Clientes
                              where t.Id.Equals(Id)
                              select t).ToArray().FirstOrDefault();
                    if (tg != null)
                    {
                        tg.DataDelete = DateTime.UtcNow;
                        context.SaveChanges();
                        return Ok();
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

        [HttpGet]
        [Route("~/ListarProponente")]
        public IEnumerable<CRMAudax.Models.TableCliente> ListarProponente(string nomeDoCliente, string tipoDoCliente, string situacaoDoCliente, bool? statusDoCliente, long? RegiaoIdFiltro, long? UsuarioId)
        {
            int regiaoid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Country).Value);

            if (regiaoid == 1)
            {
                using (var context = new MyDbContext())
                {
                    var aux = (from t in context.Clientes
                               where t.DataDelete == null
                               orderby(t.tipo == "Sacado" ? 1 : 0) ascending, t.Id descending
                               select new TableCliente
                               {
                                   Id = t.Id,
                                   nome = t.nome,
                                   tipoPessoa = t.tipoPessoa,
                                   cpfCnpj = t.cpfCnpj,
                                   email = t.email,
                                   rua = t.rua,
                                   cep = t.cep,
                                   numero = t.numero,
                                   parecer = t.parecer,
                                   telefone = t.telefone,
                                   complemento = t.complemento,
                                   cidade = t.cidade,
                                   estado = t.estado,
                                   site = t.site,
                                   tipo = t.tipo,
                                   status = t.status,
                                   situacao = t.situacao,
                                   ramoAtividade = t.ramoAtividade,
                                   faturamento = t.faturamento,
                                   RegiaoId = t.RegiaoId,
                                   UsuarioId = t.UsuarioId,
                                   SituacaoCNPJ = t.SituacaoCNPJ

                               }).ToList();

                    if (!string.IsNullOrEmpty(nomeDoCliente))
                    {
                        aux = aux.Where(c => c.nome.ToLower().Contains(nomeDoCliente.ToLower())).ToList();
                    }
                    if (!string.IsNullOrEmpty(tipoDoCliente))
                    {
                        aux = aux.Where(c => c.tipo.ToLower().Contains(tipoDoCliente.ToLower())).ToList();
                    }
                    if (!string.IsNullOrEmpty(situacaoDoCliente))
                    {
                        aux = aux.Where(c => c.situacao.ToLower().Contains(situacaoDoCliente.ToLower())).ToList();
                    }
                    if (statusDoCliente != null)
                    {
                        aux = aux.Where(c => c.status == statusDoCliente).ToList();
                    }
                    if (RegiaoIdFiltro != null)
                    {
                        aux = aux.Where(c => c.RegiaoId == RegiaoIdFiltro).ToList();
                    }
                    if (UsuarioId != null)
                    {
                        aux = aux.Where(c => c.UsuarioId == UsuarioId).ToList();
                    }

                    return aux;
                }
            }
            else
            {
                using (var context = new MyDbContext())
                {
                    var aux = (from t in context.Clientes
                               where t.DataDelete == null
                               where t.RegiaoId == regiaoid
                               orderby (t.tipo == "Sacado" ? 1 : 0) ascending, t.Id descending
                               select new TableCliente
                               {
                                   Id = t.Id,
                                   nome = t.nome,
                                   tipoPessoa = t.tipoPessoa,
                                   cpfCnpj = t.cpfCnpj,
                                   email = t.email,
                                   rua = t.rua,
                                   cep = t.cep,
                                   numero = t.numero,
                                   parecer = t.parecer,
                                   telefone = t.telefone,
                                   complemento = t.complemento,
                                   cidade = t.cidade,
                                   estado = t.estado,
                                   site = t.site,
                                   tipo = t.tipo,
                                   status = t.status,
                                   situacao = t.situacao,
                                   ramoAtividade = t.ramoAtividade,
                                   faturamento = t.faturamento,
                                   SituacaoCNPJ = t.SituacaoCNPJ

                               }).ToList();

                    if (!string.IsNullOrEmpty(nomeDoCliente))
                    {
                        aux = aux.Where(c => c.nome.ToLower().Contains(nomeDoCliente.ToLower())).ToList();
                    }
                    if (!string.IsNullOrEmpty(tipoDoCliente))
                    {
                        aux = aux.Where(c => c.tipo.ToLower().Contains(tipoDoCliente.ToLower())).ToList();
                    }
                    if (!string.IsNullOrEmpty(situacaoDoCliente))
                    {
                        aux = aux.Where(c => c.situacao.ToLower().Contains(situacaoDoCliente.ToLower())).ToList();
                    }
                    if (statusDoCliente != null)
                    {
                        aux = aux.Where(c => c.status == statusDoCliente).ToList();
                    }

                    return aux;
                }
            }
            
        }

        [HttpGet]
        [Route("~/ListarProponenteColuna")]
        public IEnumerable<CRMAudax.Models.AuxCliente> ListarProponenteColuna(string nomeDoCliente, int? regiaoId)
        {
            if (regiaoId == null)
            {
                var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Country);

                if (claim != null)
                {
                    regiaoId = Convert.ToInt32(claim.Value);
                }
            }

            if (regiaoId == 1)
            {
                using (var context = new MyDbContext())
                {
                    var aux = (from t in context.Clientes
                               join et in context.OrdemCartao on t.Id equals et.ClienteId
                               orderby et.ordemCartao
                               select new AuxCliente
                               {
                                   Id = t.Id,
                                   nome = t.nome,
                                   tipoPessoa = t.tipoPessoa,
                                   cpfCnpj = t.cpfCnpj,
                                   email = t.email,
                                   rua = t.rua,
                                   cep = t.cep,
                                   numero = t.numero,
                                   parecer = t.parecer,
                                   telefone = t.telefone,
                                   complemento = t.complemento,
                                   cidade = t.cidade,
                                   estado = t.estado,
                                   site = t.site,
                                   tipo = t.tipo,
                                   status = t.status,
                                   situacao = t.situacao,
                                   ramoAtividade = t.ramoAtividade,
                                   faturamento = t.faturamento,
                                   ordemCartao = et.ordemCartao,
                                   ColunaId = et.ColunaId,
                                   Usuario = t.Usuario
                               }).ToList();

                    if (!string.IsNullOrEmpty(nomeDoCliente))
                    {
                        aux = aux.Where(c => c.nome.ToLower().Contains(nomeDoCliente.ToLower())).ToList();
                    }
                    return aux;
                }
            }
            else
            {
                using (var context = new MyDbContext())
                {
                    var aux = (from t in context.Clientes
                               join et in context.OrdemCartao on t.Id equals et.ClienteId
                               where t.RegiaoId == regiaoId
                               orderby et.ordemCartao
                               select new AuxCliente
                               {
                                   Id = t.Id,
                                   nome = t.nome,
                                   tipoPessoa = t.tipoPessoa,
                                   cpfCnpj = t.cpfCnpj,
                                   email = t.email,
                                   rua = t.rua,
                                   cep = t.cep,
                                   numero = t.numero,
                                   parecer = t.parecer,
                                   telefone = t.telefone,
                                   complemento = t.complemento,
                                   cidade = t.cidade,
                                   estado = t.estado,
                                   site = t.site,
                                   tipo = t.tipo,
                                   status = t.status,
                                   situacao = t.situacao,
                                   ramoAtividade = t.ramoAtividade,
                                   faturamento = t.faturamento,
                                   ordemCartao = et.ordemCartao,
                                   ColunaId = et.ColunaId,
                                   Usuario = t.Usuario
                               }).ToList();

                    if (!string.IsNullOrEmpty(nomeDoCliente))
                    {
                        aux = aux.Where(c => c.nome.ToLower().Contains(nomeDoCliente.ToLower())).ToList();
                    }
                    return aux;
                }
            }
        }

        [HttpGet]
        [Route("~/ListarProponenteId/{Id}")]
        public IEnumerable<CRMAudax.Models.TableCliente> ListarProponenteId(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.Clientes

                           join u in context.Usuarios on t.UsuarioId equals u.Id into usuarioJoin
                           from usuario in usuarioJoin.DefaultIfEmpty() 
                           where t.DataDelete == null && t.Id == Id

                           select new TableCliente
                           {
                               Id = t.Id,
                               nome = t.nome,
                               tipoPessoa = t.tipoPessoa,
                               cpfCnpj = t.cpfCnpj,
                               email = t.email,
                               rua = t.rua,
                               cep = t.cep,
                               numero = t.numero,
                               parecer = t.parecer,
                               telefone = t.telefone,
                               complemento = t.complemento,
                               cidade = t.cidade,
                               estado = t.estado,
                               bairro = t.bairro,
                               site = t.site,
                               tipo = t.tipo,
                               status = t.status,
                               situacao = t.situacao,
                               ramoAtividade = t.ramoAtividade,
                               faturamento = t.faturamento,
                               dataFundacaoCedente = t.dataFundacaoCedente,
                               DataCadastro = t.DataCadastro,
                               SituacaoCNPJ = t.SituacaoCNPJ,
                               Usuario = usuario,
                               celularSocio = t.celularSocio,
                               celularSocioTerceiro = t.celularSocioTerceiro,
                               celularSocioSegundo = t.celularSocioSegundo,
                               cpfSocio = t.cpfSocio,
                               cpfSocioSegundo = t.cpfSocioSegundo,
                               cpfSocioTerceiro = t.cpfSocioTerceiro,
                               contaBancaria = t.contaBancaria,
                               EmailSegundo = t.EmailSegundo,   
                               EmailSocio = t.EmailSocio,
                               EmailTerceiro = t.EmailTerceiro,
                               enderecoSocio = t.enderecoSocio,
                               enderecoSocioSegundo = t.enderecoSocioSegundo,
                               enderecoSocioTerceiro = t.enderecoSocioTerceiro,
                               socio = t.socio,
                               socioSegundo = t.socioSegundo,
                               socioTerceiro = t.socioTerceiro,
                               InscricaoEstadual = t.InscricaoEstadual,
                               nomeSocio = t.nomeSocio, 
                               justificativaReprova = t.justificativaReprova,
                               nomeResponsavel = t.nomeResponsavel,
                               nomeSocioSegundo = t.nomeSocioSegundo,
                               nomeSocioTerceiro = t.nomeSocioTerceiro,
                               DataDelete = t.DataDelete,   
                               pep = t.pep,
                               pepSegundo = t.pepSegundo,
                               pepTerceiro = t.pepTerceiro,
                               razaoSocialCedente = t.razaoSocialCedente,
                               recorreu = t.recorreu,
                               referencia = t.referencia,
                               TipoPessoaSocio = t.TipoPessoaSocio,
                               TipoPessoaSocioSegundo = t.TipoPessoaSocioSegundo,
                               TipoPessoaSocioTerceiro = t.TipoPessoaSocioTerceiro,
                               Regiao = t.Regiao,
                               RegiaoId = t.RegiaoId,
                               UsuarioId = t.UsuarioId,
                                                          
                           }).ToArray();
                return aux;
            }
        }


        [HttpPost]
        [Route("~/UploadFileContratoSocial/{IdCedente}")]
        public async System.Threading.Tasks.Task<IActionResult> UploadFileContratoSocial(long IdCedente)
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
                    using (var context2 = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        string rootPath = "www/uploads/CRM/Cedente/" + IdCedente + "/ContratoSocial";
                        try
                        {
                            await context2.LoginAsync();
                            await context2.CreateDirectoryAsync(rootPath);
                            await context2.ChangeWorkingDirectoryAsync(rootPath);

                            using (var writeStream = await context2.OpenFileWriteStreamAsync(formFile.FileName))
                            {
                                await formFile.CopyToAsync(writeStream);
                            }

                            using (var context = new MyDbContext())
                            {
                                var c = context.ContratosSociais.Add(new TableContratoSocialCedente
                                {
                                    ClienteId = IdCedente,
                                    pathContratoSocialCedente = rootPath,
                                    tipoContratoSocialCedente = formFile.ContentType,
                                    nomeContratoSocialCedente = formFile.FileName,
                                    dataEnvio = DateTime.UtcNow
                                }).Entity;
                                context.SaveChanges();
                            }

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

        [HttpPost]
        [Route("~/UploadFileComprovanteEndereco/{IdCedente}")]
        public async System.Threading.Tasks.Task<IActionResult> UploadFileComprovanteEndereco(long IdCedente)
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
                    using (var context2 = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        string rootPath = "www/uploads/CRM/Cedente/" + IdCedente + "/ComprovanteEndereco";
                        try
                        {
                            await context2.LoginAsync();
                            await context2.CreateDirectoryAsync(rootPath);
                            await context2.ChangeWorkingDirectoryAsync(rootPath);

                            using (var writeStream = await context2.OpenFileWriteStreamAsync(formFile.FileName))
                            {
                                await formFile.CopyToAsync(writeStream);
                            }

                            using (var context = new MyDbContext())
                            {
                                var c = context.ComprovantesEnderecos.Add(new TableComprovanteEnderecoCedente
                                {
                                    ClienteId = IdCedente,
                                    pathComprovanteEndereco = rootPath,
                                    tipoComprovanteRenda = formFile.ContentType,
                                    nomeComprovanteRenda = formFile.FileName,
                                    dataEnvio = DateTime.UtcNow
                                }).Entity;
                                context.SaveChanges();
                            }

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

        [HttpPost]
        [Route("~/UploadFileCartaoCNPJ/{IdCedente}")]
        public async System.Threading.Tasks.Task<IActionResult> UploadFileCartaoCNPJ(long IdCedente)
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
                    using (var context2 = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        string rootPath = "www/uploads/CRM/Cedente/" + IdCedente + "/CartaoCNPJ";
                        try
                        {
                            await context2.LoginAsync();
                            await context2.CreateDirectoryAsync(rootPath);
                            await context2.ChangeWorkingDirectoryAsync(rootPath);

                            using (var writeStream = await context2.OpenFileWriteStreamAsync(formFile.FileName))
                            {
                                await formFile.CopyToAsync(writeStream);
                            }

                            using (var context = new MyDbContext())
                            {
                                var c = context.CartoesCNPJ.Add(new TableCartaoCNPJCedente
                                {
                                    ClienteId = IdCedente,
                                    pathCartaoCNPJ = rootPath,
                                    tipoCartaoCNPJ = formFile.ContentType,
                                    nomeCartaoCNPJ = formFile.FileName,
                                    dataEnvio = DateTime.UtcNow
                                }).Entity;
                                context.SaveChanges();
                            }

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

        [HttpPost]
        [Route("~/UploadFileFaturamento/{IdCedente}")]
        public async System.Threading.Tasks.Task<IActionResult> UploadFileFaturamento(long IdCedente)
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
                    using (var context2 = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        string rootPath = "www/uploads/CRM/Cedente/" + IdCedente + "/Faturamento";
                        try
                        {
                            await context2.LoginAsync();
                            await context2.CreateDirectoryAsync(rootPath);
                            await context2.ChangeWorkingDirectoryAsync(rootPath);

                            using (var writeStream = await context2.OpenFileWriteStreamAsync(formFile.FileName))
                            {
                                await formFile.CopyToAsync(writeStream);
                            }

                            using (var context = new MyDbContext())
                            {
                                var c = context.Faturamentos.Add(new TableFaturamentoCedente
                                {
                                    ClienteId = IdCedente,
                                    pathFaturamentoCedente = rootPath,
                                    tipoFaturamentoCedente = formFile.ContentType,
                                    nomeFaturamentoCedente = formFile.FileName,
                                    dataEnvio = DateTime.UtcNow
                                }).Entity;
                                context.SaveChanges();
                            }

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

        [HttpPost]
        [Route("~/UploadFileFaturamentoFiscal/{IdCedente}")]
        public async System.Threading.Tasks.Task<IActionResult> UploadFileFaturamentoFiscal(long IdCedente)
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
                    using (var context2 = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        string rootPath = "www/uploads/CRM/Cedente/" + IdCedente + "/FaturamentoFiscal";
                        try
                        {
                            await context2.LoginAsync();
                            await context2.CreateDirectoryAsync(rootPath);
                            await context2.ChangeWorkingDirectoryAsync(rootPath);

                            using (var writeStream = await context2.OpenFileWriteStreamAsync(formFile.FileName))
                            {
                                await formFile.CopyToAsync(writeStream);
                            }

                            using (var context = new MyDbContext())
                            {
                                var c = context.FaturamentosFiscais.Add(new TableFaturamentoFiscalCedente
                                {
                                    ClienteId = IdCedente,
                                    pathFaturamentoFiscalCedente = rootPath,
                                    tipoFaturamentoFiscalCedente = formFile.ContentType,
                                    nomeFaturamentoFiscalCedente = formFile.FileName,
                                    dataEnvio = DateTime.UtcNow
                                }).Entity;
                                context.SaveChanges();
                            }

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

        [HttpPost]
        [Route("~/UploadFileImpostoRenda/{IdCedente}")]
        public async System.Threading.Tasks.Task<IActionResult> UploadFileImpostoRenda(long IdCedente)
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
                    using (var context2 = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        string rootPath = "www/uploads/CRM/Cedente/" + IdCedente + "/ImpostoDeRenda";
                        try
                        {
                            await context2.LoginAsync();
                            await context2.CreateDirectoryAsync(rootPath);
                            await context2.ChangeWorkingDirectoryAsync(rootPath);

                            using (var writeStream = await context2.OpenFileWriteStreamAsync(formFile.FileName))
                            {
                                await formFile.CopyToAsync(writeStream);
                            }

                            using (var context = new MyDbContext())
                            {
                                var c = context.ImpostosRenda.Add(new TableImpostoRendaCedente
                                {
                                    ClienteId = IdCedente,
                                    pathImpostoRendaCedente = rootPath,
                                    tipoImpostoRendaCedente = formFile.ContentType,
                                    nomeImpostoRendaCedente = formFile.FileName,
                                    dataEnvio = DateTime.UtcNow
                                }).Entity;
                                context.SaveChanges();
                            }
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

        [HttpPost]
        [Route("~/UploadFileCpfRgCnh/{IdCedente}")]
        public async System.Threading.Tasks.Task<IActionResult> UploadFileCpfRgCnh(long IdCedente)
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
                    using (var context2 = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        string rootPath = "www/uploads/CRM/Cedente/" + IdCedente + "/CpfRgCnh";
                        try
                        {
                            await context2.LoginAsync();
                            await context2.CreateDirectoryAsync(rootPath);
                            await context2.ChangeWorkingDirectoryAsync(rootPath);

                            using (var writeStream = await context2.OpenFileWriteStreamAsync(formFile.FileName))
                            {
                                await formFile.CopyToAsync(writeStream);
                            }

                            using (var context = new MyDbContext())
                            {
                                var c = context.CnpjCpfs.Add(new TableCpfRgCnh
                                {
                                    ClienteId = IdCedente,
                                    pathCpfRgCnhCedente = rootPath,
                                    tipoCpfRgCnhfCedente = formFile.ContentType,
                                    nomeCpfRgCnhCedente = formFile.FileName,
                                    dataEnvio = DateTime.UtcNow
                                }).Entity;
                                context.SaveChanges();
                            }

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


        [HttpGet]
        [Route("~/ListarFileContratoSocial/{Id}")]
        public IEnumerable<CRMAudax.Models.TableContratoSocialCedente> ListarFileContratoSocial(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.ContratosSociais

                           where t.ClienteId == Id
                           select new TableContratoSocialCedente
                           {
                               Id = t.Id,
                               nomeContratoSocialCedente = t.nomeContratoSocialCedente,
                               tipoContratoSocialCedente = t.tipoContratoSocialCedente,
                               pathContratoSocialCedente = t.pathContratoSocialCedente,
                               dataEnvio = t.dataEnvio,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/DownloadFileContratoSocial/{Id}")]
        public async Task<IActionResult> DownloadFileContratoSocial(long Id)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            using (var context = new MyDbContext())
            {
                var d = (from j in context.ContratosSociais
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
                        string arquivo = d.pathContratoSocialCedente + "/" + d.nomeContratoSocialCedente;
                        var tempFile = new FileInfo(arquivo);
                        await context2.LoginAsync();
                        using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                        {
                            byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                            var content = new System.IO.MemoryStream(buff);
                            var contentType = d.tipoContratoSocialCedente;
                            var fileName = d.nomeContratoSocialCedente;
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("~/ListarFileComprovanteEndereco/{Id}")]
        public IEnumerable<CRMAudax.Models.TableComprovanteEnderecoCedente> ListarFileComprovanteEndereco(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.ComprovantesEnderecos

                           where t.ClienteId == Id
                           select new TableComprovanteEnderecoCedente
                           {
                               Id = t.Id,
                               nomeComprovanteRenda = t.nomeComprovanteRenda,
                               tipoComprovanteRenda = t.tipoComprovanteRenda,
                               pathComprovanteEndereco = t.pathComprovanteEndereco,
                               dataEnvio = t.dataEnvio,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/DownloadFileComprovanteEndereco/{Id}")]
        public async Task<IActionResult> DownloadFileComprovanteEndereco(long Id)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            using (var context = new MyDbContext())
            {
                var d = (from j in context.ComprovantesEnderecos
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
                        string arquivo = d.pathComprovanteEndereco + "/" + d.nomeComprovanteRenda;
                        var tempFile = new FileInfo(arquivo);
                        await context2.LoginAsync();
                        using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                        {
                            byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                            var content = new System.IO.MemoryStream(buff);
                            var contentType = d.tipoComprovanteRenda;
                            var fileName = d.nomeComprovanteRenda;
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("~/ListarFileCartaoCNPJ/{Id}")]
        public IEnumerable<CRMAudax.Models.TableCartaoCNPJCedente> ListarFileCartaoCNPJ(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.CartoesCNPJ

                           where t.ClienteId == Id
                           select new TableCartaoCNPJCedente
                           {
                               Id = t.Id,
                               nomeCartaoCNPJ = t.nomeCartaoCNPJ,
                               tipoCartaoCNPJ = t.tipoCartaoCNPJ,
                               pathCartaoCNPJ = t.pathCartaoCNPJ,
                               dataEnvio = t.dataEnvio,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/DownloadFileCartaoCnpj/{Id}")]
        public async Task<IActionResult> DownloadFileCartaoCnpj(long Id)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            using (var context = new MyDbContext())
            {
                var d = (from j in context.CartoesCNPJ
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
                        string arquivo = d.pathCartaoCNPJ + "/" + d.nomeCartaoCNPJ;
                        var tempFile = new FileInfo(arquivo);
                        await context2.LoginAsync();
                        using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                        {
                            byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                            var content = new System.IO.MemoryStream(buff);
                            var contentType = d.tipoCartaoCNPJ;
                            var fileName = d.nomeCartaoCNPJ;
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("~/ListarFileFaturamento/{Id}")]
        public IEnumerable<CRMAudax.Models.TableFaturamentoCedente> ListarFileFaturamento(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.Faturamentos

                           where t.ClienteId == Id
                           select new TableFaturamentoCedente
                           {
                               Id = t.Id,
                               nomeFaturamentoCedente = t.nomeFaturamentoCedente,
                               tipoFaturamentoCedente = t.tipoFaturamentoCedente,
                               pathFaturamentoCedente = t.pathFaturamentoCedente,
                               dataEnvio = t.dataEnvio,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/DownloadFileFaturamento/{Id}")]
        public async Task<IActionResult> DownloadFileFaturamento(long Id)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            using (var context = new MyDbContext())
            {
                var d = (from j in context.Faturamentos
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
                        string arquivo = d.pathFaturamentoCedente + "/" + d.nomeFaturamentoCedente;
                        var tempFile = new FileInfo(arquivo);
                        await context2.LoginAsync();
                        using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                        {
                            byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                            var content = new System.IO.MemoryStream(buff);
                            var contentType = d.tipoFaturamentoCedente;
                            var fileName = d.nomeFaturamentoCedente;
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("~/ListarFileFaturamentoFiscal/{Id}")]
        public IEnumerable<CRMAudax.Models.TableFaturamentoFiscalCedente> ListarFileFaturamentoFiscal(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.FaturamentosFiscais

                           where t.ClienteId == Id
                           select new TableFaturamentoFiscalCedente
                           {
                               Id = t.Id,
                               nomeFaturamentoFiscalCedente = t.nomeFaturamentoFiscalCedente,
                               tipoFaturamentoFiscalCedente = t.tipoFaturamentoFiscalCedente,
                               pathFaturamentoFiscalCedente = t.pathFaturamentoFiscalCedente,
                               dataEnvio = t.dataEnvio,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/DownloadFileFaturamentoFiscal/{Id}")]
        public async Task<IActionResult> DownloadFileFaturamentoFiscal(long Id)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            using (var context = new MyDbContext())
            {
                var d = (from j in context.FaturamentosFiscais
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
                        string arquivo = d.pathFaturamentoFiscalCedente + "/" + d.nomeFaturamentoFiscalCedente;
                        var tempFile = new FileInfo(arquivo);
                        await context2.LoginAsync();
                        using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                        {
                            byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                            var content = new System.IO.MemoryStream(buff);
                            var contentType = d.tipoFaturamentoFiscalCedente;
                            var fileName = d.nomeFaturamentoFiscalCedente;
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("~/ListarFileImpostoRenda/{Id}")]
        public IEnumerable<CRMAudax.Models.TableImpostoRendaCedente> ListarFileImpostoRenda(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.ImpostosRenda

                           where t.ClienteId == Id
                           select new TableImpostoRendaCedente
                           {
                               Id = t.Id,
                               nomeImpostoRendaCedente = t.nomeImpostoRendaCedente,
                               tipoImpostoRendaCedente = t.tipoImpostoRendaCedente,
                               pathImpostoRendaCedente = t.pathImpostoRendaCedente,
                               dataEnvio = t.dataEnvio,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/DownloadFileImpostoRenda/{Id}")]
        public async Task<IActionResult> DownloadFileImpostoRenda(long Id)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            using (var context = new MyDbContext())
            {
                var d = (from j in context.ImpostosRenda
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
                        string arquivo = d.pathImpostoRendaCedente + "/" + d.nomeImpostoRendaCedente;
                        var tempFile = new FileInfo(arquivo);
                        await context2.LoginAsync();
                        using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                        {
                            byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                            var content = new System.IO.MemoryStream(buff);
                            var contentType = d.tipoImpostoRendaCedente;
                            var fileName = d.nomeImpostoRendaCedente;
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("~/ListarFileCpfRgCnh/{Id}")]
        public IEnumerable<CRMAudax.Models.TableCpfRgCnh> ListarFileCpfRgCnh(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.CnpjCpfs

                           where t.ClienteId == Id
                           select new TableCpfRgCnh
                           {
                               Id = t.Id,
                               nomeCpfRgCnhCedente = t.nomeCpfRgCnhCedente,
                               tipoCpfRgCnhfCedente = t.tipoCpfRgCnhfCedente,
                               pathCpfRgCnhCedente = t.pathCpfRgCnhCedente,
                               dataEnvio = t.dataEnvio,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/DownloadFileCpfRgCnh/{Id}")]
        public async Task<IActionResult> DownloadFileFileCpfRgCnh(long Id)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            using (var context = new MyDbContext())
            {
                var d = (from j in context.CnpjCpfs
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
                        string arquivo = d.pathCpfRgCnhCedente + "/" + d.nomeCpfRgCnhCedente;
                        var tempFile = new FileInfo(arquivo);
                        await context2.LoginAsync();
                        using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                        {
                            byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                            var content = new System.IO.MemoryStream(buff);
                            var contentType = d.tipoCpfRgCnhfCedente;
                            var fileName = d.nomeCpfRgCnhCedente;
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("~/AprovarCedente/{Id}")]
        public IActionResult AprovarCedente(long Id)
        {
            using (var context = new MyDbContext())
            {
                var clientes = (from t in context.Clientes
                                where t.Id.Equals(Id)
                                select t).ToArray().FirstOrDefault();
                if (clientes != null && 
                    clientes.rua != null && 
                    clientes.cep != null && 
                    clientes.cidade != null && 
                    clientes.numero != null && 
                    clientes.estado != null &&
                    clientes.bairro != null &&  
                    clientes.contaBancaria != null &&

                    clientes.rua != "" &&
                    clientes.cep != "" &&
                    clientes.cidade != "" &&
                    clientes.numero != "" &&
                    clientes.estado != "" &&
                    clientes.bairro != "" &&
                    clientes.contaBancaria != "")
                {
                    if(clientes.tipoPessoa == 1)
                    {
                        clientes.tipo = "Cedente";
                        clientes.situacao = "Aprovado";
                        clientes.status = true;
                        context.SaveChanges();
                    }
                    if(clientes.tipoPessoa == 2)
                    {
                        if(clientes.TipoPessoaSocio != null && 
                            clientes.nomeSocio != null && 
                            clientes.cpfSocio != null && 
                            clientes.enderecoSocio != null && 
                            clientes.celularSocio != null && 
                            clientes.pep != null && 
                            clientes.EmailSocio != null &&
                            clientes.socio != null &&

                            clientes.nomeSocio != "" &&
                            clientes.cpfSocio != "" &&
                            clientes.enderecoSocio != "" &&
                            clientes.celularSocio != "" &&
                            clientes.pep != "" &&
                            clientes.EmailSocio != "" &&
                            clientes.socio != "")
                        {
                            clientes.tipo = "Cedente";
                            clientes.situacao = "Aprovado";
                            clientes.status = true;
                            context.SaveChanges();
                        }
                        else
                        {
                            return Ok(false);
                        }
                    }
                }
                else
                {
                    return Ok(false);
                }   
            };
            return Ok(true);
        }

        [HttpPut]
        [Route("~/ReprovarCedente/{Id}/{justificativaReprova}")]
        public IActionResult CriticarSolicitacao(long Id, string justificativaReprova)
        {
            using (var context = new MyDbContext())
            {
                var cliente = (from t in context.Clientes
                               where t.Id.Equals(Id)
                               select t).ToArray().FirstOrDefault();
                if (cliente != null)
                {
                    cliente.situacao = "Reprovado";
                    cliente.justificativaReprova = justificativaReprova;
                    context.SaveChanges();
                }

                var atividade = context.Atividades.Add(new TableAtividadesCedente
                {
                    atividade = "Cedente reprovado",
                    ClienteId = Id,
                    dataAtividade = DateTime.UtcNow,
                    descricao = "Cliente reprovado pelo operador por meio do kanban, justificativa: " + justificativaReprova
                    
                }).Entity;

                context.SaveChanges();
            };
            return Ok();
        }

        [HttpPut]
        [Route("~/StatusCliente/{Id}")]
        public IActionResult StatusCliente(long Id)
        {
            using (var context = new MyDbContext())
            {
                var clientes = (from t in context.Clientes
                                where t.Id.Equals(Id)
                                select t).ToArray().FirstOrDefault();
                if (clientes != null && clientes.status == true)
                {                  
                    clientes.status = false;
                    context.SaveChanges();
                }
                else if(clientes != null && clientes.status == false)
                {
                    clientes.status = true;
                    context.SaveChanges();
                }
            };
            return Ok();
        }


        [HttpGet]
        [Route("~/ListarParecer/{Id}")]
        public CRMAudax.Models.AuxParecer ListarParecer(long Id)
        {
            using (var context = new MyDbContext())
            {
                var parecer = (from c in context.RelatoriosVisita
                               where c.ClienteId.Equals(Id)
                               select new AuxParecer
                               {
                                   Parecer = c.Parecer,
                                   DataRegistro = c.dataVisita,
                                   Tipo = "Visita"
                               })
                               .Union(
                                from q in context.RespostasCedentes                               
                                where q.PerguntaId == 61 && q.ClienteId.Equals(Id)
                                select new AuxParecer
                                {
                                    Parecer = q.RespostaAberta, 
                                    DataRegistro = q.DataResposta,
                                    Tipo = "Comitê"
                                })
                               .Union(
                                from t in context.Clientes
                                where t.Id.Equals(Id)
                                select new AuxParecer
                                { 
                                    Parecer = t.parecer, 
                                    DataRegistro = t.DataCadastro,
                                    Tipo = "Cadastro"
                                });

                return parecer.OrderByDescending(p => p.DataRegistro).FirstOrDefault();
            }      
        }

        [HttpPost]
        [Route("~/CadastrarSacado")]
        public IActionResult CadastrarSacado([FromBody] TableCliente request)
        {
            int regiao = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.System)?.Value);

            using (var context = new MyDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    long idProponente;
                    var cd = (from t in context.Clientes
                              where t.tipo == "Sacado"
                              where t.cpfCnpj.Equals(request.cpfCnpj)
                              select t).ToArray().FirstOrDefault();
                    if (cd == null)
                    {
                        var r = context.Clientes.Add(new TableCliente
                        {
                            tipoPessoa = request.tipoPessoa,
                            nome = request.nome,
                            cpfCnpj = request.cpfCnpj,
                            email = request.email,
                            telefone = request.telefone,
                            complemento = request.complemento,
                            cidade = request.cidade,
                            estado = request.estado,
                            site = request.site,
                            rua = request.rua,
                            cep = request.cep,
                            numero = request.numero,
                            parecer = request.parecer,
                            ramoAtividade = request.ramoAtividade,
                            faturamento = request.faturamento,
                            dataFundacaoCedente = request.dataFundacaoCedente,

                            tipo = "Sacado",
                            situacao = "Aprovado",
                            status = false,

                            recorreu = request.recorreu,
                            pep = request.pep,
                            socio = request.socio,
                            referencia = request.referencia,
                            cpfSocio = request.cpfSocio,
                            nomeSocio = request.nomeSocio,
                            enderecoSocio = request.enderecoSocio,
                            celularSocio = request.celularSocio,

                            DataCadastro = DateTime.Now,

                            RegiaoId = regiao,
                            UsuarioId = request.UsuarioId

                        }).Entity;

                        context.SaveChanges();

                        idProponente = r.Id;

                        context.SaveChanges();
                    }
                    else
                    {
                        idProponente = cd.Id;
                        return BadRequest("Sacado já cadastrado");
                    }
                    try
                    {
                        context.SaveChanges();
                        transaction.Commit();
                        return Ok(idProponente);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                        return BadRequest(ex);
                    }
                }
            }
        }

        [HttpPost]
        [Route("~/UploadFileDAP/{IdCedente}")]
        public async System.Threading.Tasks.Task<IActionResult> UploadFileDAP(long IdCedente)
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
                    using (var context2 = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        string rootPath = "www/uploads/CRM/Cedente/" + IdCedente + "/DAP";
                        try
                        {
                            await context2.LoginAsync();
                            await context2.CreateDirectoryAsync(rootPath);
                            await context2.ChangeWorkingDirectoryAsync(rootPath);

                            using (var writeStream = await context2.OpenFileWriteStreamAsync(formFile.FileName))
                            {
                                await formFile.CopyToAsync(writeStream);
                            }

                            using (var context = new MyDbContext())
                            {
                                var c = context.ArquivosDAP.Add(new TableArquivoDAP
                                {
                                    ClienteId = IdCedente,
                                    pathArquivoDAP = rootPath,
                                    tipoArquivoDAP = formFile.ContentType,
                                    nomeArquivoDAP = formFile.FileName,
                                    dataEnvio = DateTime.UtcNow
                                }).Entity;
                                context.SaveChanges();
                            }

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

        [HttpGet]
        [Route("~/ListarFileDAP/{Id}")]
        public IEnumerable<CRMAudax.Models.TableArquivoDAP> ListarFileDAP(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.ArquivosDAP

                           where t.ClienteId == Id
                           select new TableArquivoDAP
                           {
                               Id = t.Id,
                               nomeArquivoDAP = t.nomeArquivoDAP,
                               tipoArquivoDAP = t.tipoArquivoDAP,
                               pathArquivoDAP = t.pathArquivoDAP,
                               dataEnvio = t.dataEnvio,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/DownloadFileDAP/{Id}")]
        public async Task<IActionResult> DownloadFileDAP(long Id)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            using (var context = new MyDbContext())
            {
                var d = (from j in context.ArquivosDAP
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
                        string arquivo = d.pathArquivoDAP + "/" + d.nomeArquivoDAP;
                        var tempFile = new FileInfo(arquivo);
                        await context2.LoginAsync();
                        using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                        {
                            byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                            var content = new System.IO.MemoryStream(buff);
                            var contentType = d.tipoArquivoDAP;
                            var fileName = d.nomeArquivoDAP;
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("~/UploadFileDeclaracaoC/{IdCedente}")]
        public async System.Threading.Tasks.Task<IActionResult> UploadFileDeclaracaoC(long IdCedente)
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
                    using (var context2 = new FtpClient(new FtpClientConfiguration
                    {
                        Host = configuration.GetSection("FtpCredentials")["Host"],
                        Username = configuration.GetSection("FtpCredentials")["Username"],
                        Password = configuration.GetSection("FtpCredentials")["Password"],
                        Port = Convert.ToInt16(configuration.GetSection("FtpCredentials")["Port"]),
                    }))
                    {
                        string rootPath = "www/uploads/CRM/Cedente/" + IdCedente + "/DeclaracaoC";
                        try
                        {
                            await context2.LoginAsync();
                            await context2.CreateDirectoryAsync(rootPath);
                            await context2.ChangeWorkingDirectoryAsync(rootPath);

                            using (var writeStream = await context2.OpenFileWriteStreamAsync(formFile.FileName))
                            {
                                await formFile.CopyToAsync(writeStream);
                            }

                            using (var context = new MyDbContext())
                            {
                                var c = context.DeclaracoesCredito.Add(new TableDeclaracaoCredito
                                {
                                    ClienteId = IdCedente,
                                    pathDeclaracaoC = rootPath,
                                    tipoDeclaracaoC = formFile.ContentType,
                                    nomeDeclaracaoC = formFile.FileName,
                                    dataEnvio = DateTime.UtcNow
                                }).Entity;
                                context.SaveChanges();
                            }

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

        [HttpGet]
        [Route("~/ListarFileDeclaracaoC/{Id}")]
        public IEnumerable<CRMAudax.Models.TableArquivoDAP> ListarFileDeclaracaoC(long Id)
        {
            using (var context = new MyDbContext())
            {
                var aux = (from t in context.DeclaracoesCredito

                           where t.ClienteId == Id
                           select new TableArquivoDAP
                           {
                               Id = t.Id,
                               nomeArquivoDAP = t.nomeDeclaracaoC,
                               tipoArquivoDAP = t.tipoDeclaracaoC,
                               pathArquivoDAP = t.pathDeclaracaoC,
                               dataEnvio = t.dataEnvio,

                           }).ToArray();
                return aux;
            }
        }

        [HttpGet]
        [Route("~/DownloadFileDeclaracaoC/{Id}")]
        public async Task<IActionResult> DownloadFileDeclaracaoC(long Id)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            using (var context = new MyDbContext())
            {
                var d = (from j in context.DeclaracoesCredito
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
                        string arquivo = d.pathDeclaracaoC + "/" + d.nomeDeclaracaoC;
                        var tempFile = new FileInfo(arquivo);
                        await context2.LoginAsync();
                        using (var fileStream = await context2.OpenFileReadStreamAsync(arquivo))
                        {
                            byte[] buff = Converts.ConverteStreamToByteArray(fileStream);
                            var content = new System.IO.MemoryStream(buff);
                            var contentType = d.tipoDeclaracaoC;
                            var fileName = d.nomeDeclaracaoC;
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return BadRequest();
            }
        }

    }
}
