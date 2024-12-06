using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;
using System.Diagnostics;
using CRMAudax.Models;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.IO;
using CoreFtp;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using static CRMAudax.Models.AuxQuod;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;
using CRMAudax.Models.ViewModels;

namespace CRMAudax.Controllers
{
    //[Authorize]

    public class APIController : Controller
    {
        public ActionResult API(long Id)
        {
            dynamic mymodel = new ExpandoObject();

            mymodel.Cedentes = new CedenteController().ListarProponenteId(Id);
            mymodel.NFEs = new NFEController().ListarNFE(Id);

            return View(mymodel);
        }

        private readonly HttpClient _httpClient;

        public APIController(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient();
        }

        [HttpGet]
        [Route("~/GetToken")]
        public async Task<string> GetToken()
        {
            string url = "https://gateway.apiserpro.serpro.gov.br/token";
            string clientId = "V0ozYW0yMlJmY0JCejkyRW5mbmdPMmdHQjc0YTo5SFVnVXJvZHQ2V3M2d3Q0UGNiOUlmTlRMRE1h";

            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientId);

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeAnonymousType(responseContent, new { access_token = "" });
                    string accessToken = jsonResponse.access_token;
                    return accessToken;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("~/GetQuodScore/{cpf}/{Id}")]
        public async Task<IActionResult> GetQuodScore(string cpf, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.quod.com.br/WsQuodAPI/QuodReport?ver_=2.0");

                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("32598094000139@esp:Q6BVLL2uv"));

                //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("hmlgpfactoring@esp:71pIWStl"));

                request.Headers.Add("Authorization", "Basic " + credentials);

                var jsonRequest = new
                {
                    QuodReportRequest = new
                    {
                        Options = new
                        {
                            IncludeBestInfo = "false",
                            IncludeCreditRiskIndicators = "false",
                            IncludeCreditRiskData = "false",
                            IncludeQuodScore = "true",
                            IncludeCreditLinesData = "false",
                        },
                        SearchBy = new
                        {
                            CPF = cpf
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(jsonRequest), null, "application/json");
                request.Content = content;

                using (var context = new MyDbContext())
                {
                    var cd = (from t in context.QuodScores
                              where t.DataScore.Date == DateTime.UtcNow.Date
                              where t.ClienteId == Id
                              select t).FirstOrDefault();

                    if (cd == null)
                    {
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        string responseContent = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JsonConvert.DeserializeAnonymousType(
                            responseContent,
                            new
                            {
                                QuodReportResponseEx = new
                                {
                                    Response = new
                                    {
                                        Records = new
                                        {
                                            Record = new[] {
                                                new {
                                                    QuodScore = new {
                                                        Score = 0
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            });

                        int score = jsonResponse.QuodReportResponseEx.Response.Records.Record[0].QuodScore.Score;

                        var r = context.QuodScores.Add(new TableQuodScore
                        {
                            CPFCNPJ = cpf,
                            DataScore = DateTime.UtcNow,
                            Score = score.ToString(),
                            ClienteId = Id

                        }).Entity;
                        context.SaveChanges();

                        if (cd != null && cd.Score != score.ToString())
                        {
                            bool verifica = false;

                            var ultimoScore = (from t in context.QuodScores
                                               where t.ClienteId == Id
                                               orderby t.DataScore descending
                                               select t).FirstOrDefault();

                            if (int.Parse(ultimoScore.Score) > int.Parse(r.Score))
                            {
                                verifica = true;
                            }
                            else
                            {
                                verifica = false;
                            }


                            var n = context.Notificacoes.Add(new TableNotificacao
                            {
                                DataNotificacao = DateTime.UtcNow,
                                ClienteId = Id,
                                Informacao = "Variação de score Quod detectada",
                                Score = true,
                                Status = verifica

                            }).Entity;
                            context.SaveChanges();
                        }
                    }
                    return Ok();
                }
            }
            catch (HttpRequestException ex)
            {
                using (var context = new MyDbContext())
                {
                    var cd = (from t in context.QuodScores
                              where t.ClienteId == Id
                              orderby t.Id descending
                              select t).ToArray().FirstOrDefault();

                    if (cd != null)
                    {
                        var r = context.QuodScores.Add(new TableQuodScore
                        {
                            CPFCNPJ = cpf,
                            DataScore = DateTime.UtcNow,
                            Score = cd.Score,
                            ClienteId = Id

                        }).Entity;
                    }

                    context.SaveChanges();
                }

                return BadRequest(ex);
            }
        }

        [HttpGet("~/Tracert/{hostname}")]
        public string Get(string hostname)
        {
            string result = "";

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "tracert";
                process.StartInfo.Arguments = hostname;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                }
            }

            return result + " - IP: " + HttpContext.Connection.RemoteIpAddress;
        }

        [HttpGet]
        [Route("~/GetQuodScorePJ/{cnpj}/{Id}")]
        public async Task<IActionResult> GetQuodScorePJ(string cnpj, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.quod.com.br/WsQuodPJ/ReportPJ?ver_=1.4");

                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("32598094000139@esp:Q6BVLL2uv"));

                //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("hmlgpfactoring@esp:71pIWStl"));

                request.Headers.Add("Authorization", "Basic " + credentials);

                var jsonRequest = new
                {
                    ReportPJRequest = new
                    {
                        Options = new
                        {
                            IncludeBestInfo = "false",
                            IncludeCreditRiskIndicators = "false",
                            IncludeCreditRiskData = "false",
                            IncludeQuodScore = "true",
                            IncludeCreditLinesData = "false",
                            IncludeInterpretaData = "false",
                            IncludeLawSuitData = "false",
                            IncludePartnershipData = "false",
                            IncludeCcfData = "false"
                        },
                        SearchBy = new
                        {
                            CNPJs = new
                            {
                                CNPJ = cnpj
                            }
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(jsonRequest), null, "application/json");
                request.Content = content;

                using (var context = new MyDbContext())
                {
                    var cd = (from t in context.QuodScores
                              where t.DataScore == DateTime.UtcNow
                              where t.ClienteId == Id
                              select t).ToArray().FirstOrDefault();

                    if (cd == null)
                    {
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        string responseContent = await response.Content.ReadAsStringAsync();
                        dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                        if (jsonResponse.ReportPJResponseEx != null && jsonResponse.ReportPJResponseEx.Response != null && jsonResponse.ReportPJResponseEx.Response.Records.ReportPJOutput[0] != null && jsonResponse.ReportPJResponseEx.Response.Records.ReportPJOutput[0].ScorePJ != null && jsonResponse.ReportPJResponseEx.Response.Records.ReportPJOutput[0].ScorePJ.Score != null)
                        {
                            string score = jsonResponse.ReportPJResponseEx.Response.Records.ReportPJOutput[0].ScorePJ.Score;

                            var r = context.QuodScores.Add(new TableQuodScore
                            {
                                CPFCNPJ = cnpj,
                                DataScore = DateTime.UtcNow,
                                Score = score,
                                ClienteId = Id

                            }).Entity;
                            context.SaveChanges();

                            string notScore = (from t in context.QuodScores
                                               where t.ClienteId == Id
                                               orderby t.DataScore descending
                                               select t.Score).ToArray().FirstOrDefault();

                            if (notScore != score.ToString())
                            {
                                bool verifica = false;

                                var ultimoScore = (from t in context.QuodScores
                                                   where t.ClienteId == Id
                                                   orderby t.DataScore descending
                                                   select t).FirstOrDefault();

                                if (int.Parse(ultimoScore.Score) > int.Parse(r.Score))
                                {
                                    verifica = true;
                                }
                                else
                                {
                                    verifica = false;
                                }


                                var n = context.Notificacoes.Add(new TableNotificacao
                                {
                                    DataNotificacao = DateTime.UtcNow,
                                    ClienteId = Id,
                                    Informacao = "Variação de score Quod detectada",
                                    Score = true,
                                    Status = verifica

                                }).Entity;
                                context.SaveChanges();
                            }
                        }
                        return Ok();
                    }
                    return Conflict("Score presente em nossa base de dados, aguarde algum tempo para realizar a contulta novamente");
                }
            }
            catch (HttpRequestException ex)
            {
                using (var context = new MyDbContext())
                {
                    var cd = (from t in context.QuodScores
                              where t.ClienteId == Id
                              orderby t.Id descending
                              select t).ToArray().FirstOrDefault();

                    if (cd != null)
                    {
                        var r = context.QuodScores.Add(new TableQuodScore
                        {
                            CPFCNPJ = cnpj,
                            DataScore = DateTime.UtcNow,
                            Score = cd.Score,
                            ClienteId = Id

                        }).Entity;
                    }

                    context.SaveChanges();
                }
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("~/GetDecisaoScore/{cpf}/{Id}")]
        public async Task<IActionResult> GetDecisaoScore(string cpf, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Get, "https://consulta.distribuidor.digital/api/consulta?logon=565306&senha=gpaudax03api&consulta=67&tipo_documento=F&documento=" + cpf);

                using (var context = new MyDbContext())
                {
                    var cd = (from t in context.DecisaoScores
                              where t.DataScore == DateTime.UtcNow
                              where t.ClienteId == Id
                              select t).ToArray().FirstOrDefault();

                    if (cd == null)
                    {
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        string responseContent = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JsonConvert.DeserializeAnonymousType(
                            responseContent,
                            new
                            {
                                score_6_meses = new
                                {
                                    classe_risco = "",
                                    esclarecimento = "",
                                    probabilidade_inadimplencia = "",
                                    score = "",
                                }
                            });

                        string score = jsonResponse.score_6_meses.score;
                        string esclarecimento = jsonResponse.score_6_meses.esclarecimento;
                        string probabilidadeInadimplecia = jsonResponse.score_6_meses.probabilidade_inadimplencia;
                        string claseRisco = jsonResponse.score_6_meses.classe_risco;

                        var r = context.DecisaoScores.Add(new TableDecisaoScore
                        {
                            CPFCNPJ = cpf,
                            DataScore = DateTime.UtcNow,
                            Score = score,
                            ClienteId = Id,
                            ClasseRisco = claseRisco,
                            Esclarecimento = esclarecimento,
                            ProbabilidadeInadimplencia = probabilidadeInadimplecia,

                        }).Entity;
                        context.SaveChanges();

                        var sd = (from t in context.DecisaoScores
                                  where t.ClienteId == Id
                                  select t).ToArray().FirstOrDefault();

                        if (sd.Score != score)
                        {
                            bool verifica = false;

                            var ultimoScore = (from t in context.DecisaoScores
                                               where t.ClienteId == Id
                                               orderby t.DataScore descending
                                               select t).FirstOrDefault();

                            if (int.Parse(ultimoScore.Score) > int.Parse(r.Score))
                            {
                                verifica = true;
                            }
                            else
                            {
                                verifica = false;
                            }


                            var n = context.Notificacoes.Add(new TableNotificacao
                            {
                                DataNotificacao = DateTime.UtcNow,
                                ClienteId = Id,
                                Informacao = "Variação de score Decisão detectada",
                                Score = true,
                                Status = verifica

                            }).Entity;
                            context.SaveChanges();
                        }

                    }
                    return Ok();
                }
            }
            catch (HttpRequestException ex)
            {
                using (var context = new MyDbContext())
                {
                    var cd = (from t in context.DecisaoScores
                              where t.ClienteId == Id
                              orderby t.Id descending
                              select t).ToArray().FirstOrDefault();

                    if (cd != null)
                    {
                        var r = context.DecisaoScores.Add(new TableDecisaoScore
                        {
                            CPFCNPJ = cpf,
                            DataScore = DateTime.UtcNow,
                            Score = cd.Score,
                            ClienteId = Id,
                            ClasseRisco = cd.Score,
                            Esclarecimento = cd.Esclarecimento,
                            ProbabilidadeInadimplencia = cd.ProbabilidadeInadimplencia,
                            QuantidadeProtestos = cd.QuantidadeProtestos,
                            UltimaOcorrenciaProtestos = cd.UltimaOcorrenciaProtestos,
                            ValorTotalProtestos = cd.ValorTotalProtestos

                        }).Entity;
                        context.SaveChanges();
                    }
                    return BadRequest(ex);
                }
            }
        }

        [HttpGet]
        [Route("~/GetDecisaoScorePJ/{cnpj}/{Id}")]
        public async Task<IActionResult> GetDecisaoScorePJ(string cnpj, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Get, "https://consulta.distribuidor.digital/api/consulta?logon=565306&senha=gpaudax03api&consulta=75&tipo_documento=J&documento=" + cnpj);

                using (var context = new MyDbContext())
                {
                    var cd = (from t in context.DecisaoScores
                              where t.DataScore == DateTime.UtcNow
                              where t.ClienteId == Id
                              select t).ToArray().FirstOrDefault();

                    if (cd == null)
                    {

                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        string responseContent = await response.Content.ReadAsStringAsync();
                        dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                        string score = null;
                        string classeRisco = null;
                        string esclarecimento = null;
                        string probabilidadeInadimplencia = null;
                        string quantidadeProtestos = null;
                        string ultimaOcorrencia = null;
                        string valorTotalProtestos = null;

                        if (jsonResponse.score_12_meses != null)
                        {
                            if (jsonResponse.score_12_meses.score != null)
                            {
                                score = jsonResponse.score_12_meses.score;
                            }
                            if (jsonResponse.score_12_meses.classe_risco != null)
                            {
                                classeRisco = jsonResponse.score_12_meses.classe_risco;
                            }
                            if (jsonResponse.score_12_meses.esclarecimento != null)
                            {
                                esclarecimento = jsonResponse.score_12_meses.esclarecimento;
                            }
                            if (jsonResponse.score_12_meses.probabilidade_inadimplencia != null)
                            {
                                probabilidadeInadimplencia = jsonResponse.score_12_meses.probabilidade_inadimplencia;
                            }
                            if (jsonResponse.resumo_ocorrencias.protesto.quantidade != null)
                            {
                                quantidadeProtestos = jsonResponse.resumo_ocorrencias.protesto.quantidade;
                            }
                            if (jsonResponse.resumo_ocorrencias.protesto.ultima_ocorrencia != null)
                            {
                                ultimaOcorrencia = jsonResponse.resumo_ocorrencias.protesto.ultima_ocorrencia;
                            }
                            if (jsonResponse.resumo_ocorrencias.protesto.valor != null)
                            {
                                valorTotalProtestos = jsonResponse.resumo_ocorrencias.protesto.valor;
                                if (valorTotalProtestos == "-")
                                {
                                    valorTotalProtestos = "0";
                                }
                            }
                        }
                        else
                        {
                            return Conflict("Score vazio na base decisão");
                        }

                        var r = context.DecisaoScores.Add(new TableDecisaoScore
                        {
                            CPFCNPJ = cnpj,
                            DataScore = DateTime.UtcNow,
                            Score = score,
                            ClienteId = Id,
                            ClasseRisco = classeRisco,
                            Esclarecimento = esclarecimento,
                            ProbabilidadeInadimplencia = probabilidadeInadimplencia,
                            QuantidadeProtestos = Convert.ToInt64(quantidadeProtestos),
                            UltimaOcorrenciaProtestos = ultimaOcorrencia,
                            ValorTotalProtestos = Convert.ToDecimal(valorTotalProtestos)

                        }).Entity;
                        context.SaveChanges();

                        var sd = (from t in context.DecisaoScores
                                  where t.ClienteId == Id
                                  select t).ToArray().FirstOrDefault();

                        if (sd.Score != score)
                        {
                            bool verifica = false;

                            var ultimoScore = (from t in context.DecisaoScores
                                               where t.ClienteId == Id
                                               orderby t.DataScore descending
                                               select t).FirstOrDefault();

                            if (int.Parse(ultimoScore.Score) > int.Parse(r.Score))
                            {
                                verifica = true;
                            }
                            else
                            {
                                verifica = false;
                            }

                            var n = context.Notificacoes.Add(new TableNotificacao
                            {
                                DataNotificacao = DateTime.UtcNow,
                                ClienteId = Id,
                                Informacao = "Variação de score Decisão detectada",
                                Score = true,
                                Status = verifica

                            }).Entity;
                            context.SaveChanges();
                        }

                        if (jsonResponse.protesto != null && Convert.ToInt16(jsonResponse.protesto.num_registros) > 1)
                        {
                            foreach (var registro in jsonResponse.protesto.registros)
                            {
                                if (registro != null)
                                {

                                    string cartorio = registro.cartorio;
                                    string cidade = registro.cidade;
                                    string data = registro.data;
                                    string moeda = registro.moeda;
                                    string uf = registro.uf;
                                    string valor = registro.valor;

                                    var d = context.protestosDecisao.Add(new TableProtestosDecisao
                                    {
                                        Cartorio = cartorio,
                                        Cidade = cidade,
                                        Data = Convert.ToDateTime(data),
                                        Moeda = moeda,
                                        Uf = uf,
                                        Valor = Convert.ToDecimal(valor),
                                        ClienteId = Id

                                    }).Entity;
                                    context.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            if (jsonResponse.protesto.registros != null)
                            {
                                string cartorio = jsonResponse.protesto.registros.cartorio;
                                string cidade = jsonResponse.protesto.registros.cidade;
                                string data = jsonResponse.protesto.registros.data;
                                string moeda = jsonResponse.protesto.registros.moeda;
                                string uf = jsonResponse.protesto.registros.uf;
                                string valor = jsonResponse.protesto.registros.valor;

                                var d = context.protestosDecisao.Add(new TableProtestosDecisao
                                {
                                    Cartorio = cartorio,
                                    Cidade = cidade,
                                    Data = Convert.ToDateTime(data),
                                    Moeda = moeda,
                                    Uf = uf,
                                    Valor = Convert.ToDecimal(valor),
                                    ClienteId = Id

                                }).Entity;
                                context.SaveChanges();
                            }
                        }
                        return Ok();
                    }
                    return Conflict("Score presente em nossa base de dados, aguarde algum tempo para realizar a contulta novamente");
                }
            }
            catch (HttpRequestException ex)
            {
                using (var context = new MyDbContext())
                {
                    var cd = (from t in context.DecisaoScores
                              where t.ClienteId == Id
                              orderby t.Id descending
                              select t).ToArray().FirstOrDefault();

                    if (cd != null)
                    {
                        var r = context.DecisaoScores.Add(new TableDecisaoScore
                        {
                            CPFCNPJ = cnpj,
                            DataScore = DateTime.UtcNow,
                            Score = cd.Score,
                            ClienteId = Id,
                            ClasseRisco = cd.Score,
                            Esclarecimento = cd.Esclarecimento,
                            ProbabilidadeInadimplencia = cd.ProbabilidadeInadimplencia,
                            QuantidadeProtestos = cd.QuantidadeProtestos,
                            UltimaOcorrenciaProtestos = cd.UltimaOcorrenciaProtestos,
                            ValorTotalProtestos = cd.ValorTotalProtestos
                        }).Entity;
                        context.SaveChanges();
                    }
                    return Conflict("Não foi possivel atualizar o score decisão");
                }

            }
        }

        [HttpGet]
        [Route("~/GetDecisaoProtestosPJ/{cnpj}/{Id}")]
        public async Task<IActionResult> GetDecisaoProtestosPJ(string cnpj, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Get, "https://consulta.distribuidor.digital/api/consulta?logon=565306&senha=gpaudax03api&consulta=91&tipo_documento=J&documento=" + cnpj);

                using (var context = new MyDbContext())
                {
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                    if (Convert.ToInt16(jsonResponse.protestos.num_registros) > 0)
                    {
                        if (jsonResponse.protestos.ocorrencia is Newtonsoft.Json.Linq.JArray)
                        {
                            foreach (var registro in jsonResponse.protestos.ocorrencia)
                            {
                                string cartorio = registro.cartorio;
                                string cidade = registro.cidade;
                                string data = registro.data;
                                string moeda = registro.moeda;
                                string tipo = registro.tipo;
                                string uf = registro.uf;
                                string valor = registro.valorn;

                                var d = context.protestosDecisao.Add(new TableProtestosDecisao
                                {
                                    Cartorio = cartorio,
                                    Cidade = cidade,
                                    Data = DateTime.ParseExact(data, "yyyyMMdd", null),
                                    Moeda = moeda,
                                    Uf = uf,
                                    Valor = Convert.ToDecimal(valor),
                                    ClienteId = Id

                                }).Entity;
                                context.SaveChanges();

                                var n = context.Notificacoes.Add(new TableNotificacao
                                {
                                    DataNotificacao = DateTime.UtcNow,
                                    ClienteId = Id,
                                    Informacao = "Protestos no sistema Decisão apontados"

                                }).Entity;
                                context.SaveChanges();

                            }
                        }
                        else
                        {
                            string cartorio = jsonResponse.protestos.ocorrencia.cartorio;
                            string cidade = jsonResponse.protestos.ocorrencia.cidade;
                            string data = jsonResponse.protestos.ocorrencia.data;
                            string moeda = jsonResponse.protestos.ocorrencia.moeda;
                            string tipo = jsonResponse.protestos.ocorrencia.tipo;
                            string uf = jsonResponse.protestos.ocorrencia.uf;
                            string valor = jsonResponse.protestos.ocorrencia.valorn;

                            var d = context.protestosDecisao.Add(new TableProtestosDecisao
                            {
                                Cartorio = cartorio,
                                Cidade = cidade,
                                Data = DateTime.ParseExact(data, "yyyyMMdd", null),
                                Moeda = moeda,
                                Uf = uf,
                                Valor = Convert.ToDecimal(valor),
                                ClienteId = Id

                            }).Entity;
                            context.SaveChanges();

                            var n = context.Notificacoes.Add(new TableNotificacao
                            {
                                DataNotificacao = DateTime.UtcNow,
                                ClienteId = Id,
                                Informacao = "Protestos no sistema Decisão apontados"

                            }).Entity;
                            context.SaveChanges();
                        }
                        string Cheio = "Protestos  no sistema decisão consultados";
                        return Ok(Cheio);
                    }
                    else
                    {
                        string Vazio = "Não existem protestos referentes a esse documento";
                        return Ok(Vazio);
                    }
                }
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("~/GetDecisaoProtestosPF/{cpf}/{Id}")]
        public async Task<IActionResult> GetDecisaoProtestosPF(string cpf, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Get, "https://consulta.distribuidor.digital/api/consulta?logon=565306&senha=gpaudax03api&consulta=91&tipo_documento=F&documento=" + cpf);

                using (var context = new MyDbContext())
                {
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                    if (Convert.ToInt16(jsonResponse.protestos.num_registros) > 1)
                    {
                        foreach (var registro in jsonResponse.protestos.ocorrencia)
                        {
                            string cartorio = registro.cartorio;
                            string cidade = registro.cidade;
                            string data = registro.data;
                            string moeda = registro.moeda;
                            string uf = registro.uf;
                            string valor = registro.valorn;

                            var d = context.protestosDecisao.Add(new TableProtestosDecisao
                            {
                                Cartorio = cartorio,
                                Cidade = cidade,
                                Data = DateTime.ParseExact(data, "yyyyMMdd", null),
                                Moeda = moeda,
                                Uf = uf,
                                Valor = Convert.ToDecimal(valor),
                                ClienteId = Id

                            }).Entity;
                            context.SaveChanges();

                            var n = context.Notificacoes.Add(new TableNotificacao
                            {
                                DataNotificacao = DateTime.UtcNow,
                                ClienteId = Id,
                                Informacao = "Protestos no sistema Decisão apontados"

                            }).Entity;
                            context.SaveChanges();

                        }
                        string Cheio = "Protestos  no sistema decisão consultados";
                        return Ok(Cheio);
                    }
                    else if (Convert.ToInt16(jsonResponse.protestos.num_registros) == 1)
                    {
                        string cartorio = jsonResponse.protestos.ocorrencia.cartorio;
                        string cidade = jsonResponse.protestos.ocorrencia.cidade;
                        string data = jsonResponse.protestos.ocorrencia.data;
                        string moeda = jsonResponse.protestos.ocorrencia.moeda;
                        string uf = jsonResponse.protestos.ocorrencia.uf;
                        string valor = jsonResponse.protestos.ocorrencia.valorn;

                        var d = context.protestosDecisao.Add(new TableProtestosDecisao
                        {
                            Cartorio = cartorio,
                            Cidade = cidade,
                            Data = DateTime.ParseExact(data, "yyyyMMdd", null),
                            Moeda = moeda,
                            Uf = uf,
                            Valor = Convert.ToDecimal(valor),
                            ClienteId = Id

                        }).Entity;
                        context.SaveChanges();

                        var n = context.Notificacoes.Add(new TableNotificacao
                        {
                            DataNotificacao = DateTime.UtcNow,
                            ClienteId = Id,
                            Informacao = "Protestos no sistema Decisão apontados"

                        }).Entity;
                        context.SaveChanges();

                        string Cheio = "Protestos  no sistema decisão consultados";
                        return Ok(Cheio);
                    }
                    else
                    {
                        string Vazio = "Não existem protestos referentes a esse documento";
                        return Ok(Vazio);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("~/GetProtestosQuodPF/{cpf}/{Id}")]
        public async Task<IActionResult> GetProtestosQuodPF(string cpf, long Id)
        {
            var DataAtual = DateTime.UtcNow.ToString("yy/MM/yyyy");
            try
            {
                using (var context = new MyDbContext())
                {
                    var cd = (from t in context.protestosQuod
                              where t.Data == Convert.ToDateTime(DataAtual)
                              where t.ClienteId == Id
                              select t).ToArray().FirstOrDefault();


                    var client = _httpClient;

                    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.quod.com.br/WsQuodAPI/QuodReport?ver_=2.0");
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("32598094000139@esp:Q6BVLL2uv"));
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("hmlgpfactoring@esp:71pIWStl"));
                    request.Headers.Add("Authorization", "Basic " + credentials);

                    var jsonRequest = new
                    {
                        QuodReportRequest = new
                        {
                            Options = new
                            {
                                IncludeBestInfo = "false",
                                IncludeCreditRiskIndicators = "false",
                                IncludeCreditRiskData = "true",
                                IncludeQuodScore = "false",
                                IncludeCreditLinesData = "false",
                            },
                            SearchBy = new
                            {
                                CPF = cpf
                            }
                        }
                    };
                    request.Content = new StringContent(JsonConvert.SerializeObject(jsonRequest), Encoding.UTF8, "application/json");

                    var response = await client.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                    }

                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                    if (cd == null)
                    {
                        if (jsonResponse.QuodReportResponseEx.Response.Records.Record != null)
                        {
                            foreach (var record in jsonResponse.QuodReportResponseEx.Response.Records.Record)
                            {
                                if (record.Protests != null && record.Protests.Protest != null)
                                {
                                    foreach (var protest in record.Protests.Protest)
                                    {
                                        foreach (var cartorio in protest.conteudo.cartorio)
                                        {
                                            var r = context.protestosQuod.Add(new TableProtestosQuod
                                            {
                                                NomeCartorio = cartorio.nome,
                                                Endereco = cartorio.endereco,
                                                Cidade = cartorio.cidade,
                                                Valor = Convert.ToDecimal(cartorio.valor_protestado),
                                                CPFCNPJ = cpf,
                                                Data = Convert.ToDateTime(DataAtual),
                                                ClienteId = Id
                                            }).Entity;
                                            context.SaveChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    return Ok("Não existem protestos referentes a esse documento");
                                }
                            }
                            return Ok("Protestos no sistema Quod consultados");
                        }
                        else
                        {
                            return Ok("Não existem protestos referentes a esse documento");
                        }

                    }
                    return Ok("Protestos já consultados");
                }
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("~/GetDecisaoPendenciasPJ/{cnpj}/{Id}")]
        public async Task<IActionResult> GetDecisaoPendenciasPJ(string cnpj, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Get, "https://consulta.distribuidor.digital/api/consulta?logon=565306&senha=gpaudax03api&consulta=72&tipo_documento=J&documento=" + cnpj);

                using (var context = new MyDbContext())
                {

                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                    if (Convert.ToInt16(jsonResponse.ccf_achei.num_registros) > 0 || Convert.ToInt16(jsonResponse.registro_pefin_serasa.num_registros) > 0)
                    {
                        if (Convert.ToInt16(jsonResponse.ccf_achei.num_registros) > 0)
                        {
                            foreach (var registro in jsonResponse.ccf_achei.registros)
                            {
                                string agencia = registro.agencia;
                                string banco = registro.banco;
                                string dataOcorrencia = registro.data_ocorrencia;
                                string motivo = registro.motivo;
                                string origem = registro.origem;

                                var d = context.pendenciasDecisao.Add(new TablePendenciasDecisao
                                {
                                    ClienteId = Id,
                                    Agencia = agencia,
                                    Banco = banco,
                                    Data = Convert.ToDateTime(dataOcorrencia),
                                    Motivo = motivo,
                                    Origem = origem,
                                    Contagem = 1

                                }).Entity;
                                context.SaveChanges();

                                var n = context.Notificacoes.Add(new TableNotificacao
                                {
                                    DataNotificacao = DateTime.UtcNow,
                                    ClienteId = Id,
                                    Informacao = "Pendencias no sistema Decisão apontados"

                                }).Entity;
                                context.SaveChanges();
                            }
                        }
                        if (Convert.ToInt16(jsonResponse.registro_pefin_serasa.num_registros) > 0)
                        {
                            if (Convert.ToInt16(jsonResponse.registro_pefin_serasa.num_registros) == 1)
                            {
                                string avalista = jsonResponse.registro_pefin_serasa.registros.avalista;
                                string contrato = jsonResponse.registro_pefin_serasa.registros.contrato;
                                string dataOcorrencia = jsonResponse.registro_pefin_serasa.registros.data_ocorrencia;
                                string modalidade = jsonResponse.registro_pefin_serasa.registros.modalidade;
                                string origem = jsonResponse.registro_pefin_serasa.registros.origem;
                                string valor = jsonResponse.registro_pefin_serasa.registros.valor;

                                var d = context.pendenciasDecisao.Add(new TablePendenciasDecisao
                                {
                                    ClienteId = Id,
                                    Avalista = avalista,
                                    Contrato = contrato,
                                    Data = Convert.ToDateTime(dataOcorrencia),
                                    Modalidade = modalidade,
                                    Origem = origem,
                                    Valor = Convert.ToDecimal(valor),
                                    Contagem = 1

                                }).Entity;
                                context.SaveChanges();
                            }
                            else
                            {
                                foreach (var registro in jsonResponse.registro_pefin_serasa.registros)
                                {
                                    string avalista = registro.avalista;
                                    string contrato = registro.contrato;
                                    string dataOcorrencia = registro.data_ocorrencia;
                                    string modalidade = registro.modalidade;
                                    string origem = registro.origem;
                                    string valor = registro.valor;

                                    var d = context.pendenciasDecisao.Add(new TablePendenciasDecisao
                                    {
                                        ClienteId = Id,
                                        Avalista = avalista,
                                        Contrato = contrato,
                                        Data = Convert.ToDateTime(dataOcorrencia),
                                        Modalidade = modalidade,
                                        Origem = origem,
                                        Valor = Convert.ToDecimal(valor),
                                        Contagem = 1

                                    }).Entity;
                                    context.SaveChanges();
                                }
                            }
                        }
                        return Ok("Pendencias no sistema decisão consultados");
                    }
                    else
                    {
                        return Ok("Não existem pendencias referentes a esse documento");
                    }
                }
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("~/GetDecisaoPendenciasPF/{cpf}/{Id}")]
        public async Task<IActionResult> GetDecisaoPendenciasPF(string cpf, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Get, "https://consulta.distribuidor.digital/api/consulta?logon=565306&senha=gpaudax03api&consulta=73&tipo_documento=F&documento=" + cpf);

                using (var context = new MyDbContext())
                {

                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                    if (Convert.ToInt16(jsonResponse.ccf_achei.num_registros) > 0 || Convert.ToInt16(jsonResponse.registro_pefin_serasa.num_registros) > 0)
                    {
                        if (Convert.ToInt16(jsonResponse.ccf_achei.num_registros) > 0)
                        {
                            foreach (var registro in jsonResponse.ccf_achei.registros)
                            {
                                string agencia = registro.agencia;
                                string banco = registro.banco;
                                string dataOcorrencia = registro.data_ocorrencia;
                                string motivo = registro.motivo;
                                string origem = registro.origem;

                                var d = context.pendenciasDecisao.Add(new TablePendenciasDecisao
                                {
                                    ClienteId = Id,
                                    Agencia = agencia,
                                    Banco = banco,
                                    Data = Convert.ToDateTime(dataOcorrencia),
                                    Motivo = motivo,
                                    Origem = origem,
                                    Contagem = 1

                                }).Entity;
                                context.SaveChanges();
                            }
                        }
                        if (Convert.ToInt16(jsonResponse.registro_pefin_serasa.num_registros) > 0)
                        {
                            if (Convert.ToInt16(jsonResponse.registro_pefin_serasa.num_registros) == 1)
                            {
                                string avalista = jsonResponse.registro_pefin_serasa.registros.avalista;
                                string contrato = jsonResponse.registro_pefin_serasa.registros.contrato;
                                string dataOcorrencia = jsonResponse.registro_pefin_serasa.registros.data_ocorrencia;
                                string modalidade = jsonResponse.registro_pefin_serasa.registros.modalidade;
                                string origem = jsonResponse.registro_pefin_serasa.registros.origem;
                                string valor = jsonResponse.registro_pefin_serasa.registros.valor;

                                var d = context.pendenciasDecisao.Add(new TablePendenciasDecisao
                                {
                                    ClienteId = Id,
                                    Avalista = avalista,
                                    Contrato = contrato,
                                    Data = Convert.ToDateTime(dataOcorrencia),
                                    Modalidade = modalidade,
                                    Origem = origem,
                                    Valor = Convert.ToDecimal(valor),
                                    Contagem = 1

                                }).Entity;
                                context.SaveChanges();
                            }
                            else
                            {
                                foreach (var registro in jsonResponse.registro_pefin_serasa.registros)
                                {
                                    string avalista = registro.avalista;
                                    string contrato = registro.contrato;
                                    string dataOcorrencia = registro.data_ocorrencia;
                                    string modalidade = registro.modalidade;
                                    string origem = registro.origem;
                                    string valor = registro.valor;

                                    var d = context.pendenciasDecisao.Add(new TablePendenciasDecisao
                                    {
                                        ClienteId = Id,
                                        Avalista = avalista,
                                        Contrato = contrato,
                                        Data = Convert.ToDateTime(dataOcorrencia),
                                        Modalidade = modalidade,
                                        Origem = origem,
                                        Valor = Convert.ToDecimal(valor),
                                        Contagem = 1

                                    }).Entity;
                                    context.SaveChanges();
                                }
                            }

                        }

                        var n = context.Notificacoes.Add(new TableNotificacao
                        {
                            DataNotificacao = DateTime.UtcNow,
                            ClienteId = Id,
                            Informacao = "Pendencias no  sistema Decisão detectadas"

                        }).Entity;
                        context.SaveChanges();

                        return Ok("Pendencias no sistema Decisão consultados");
                    }
                    else
                    {
                        return Ok("Não existem pendencias referentes a esse documento");
                    }
                }
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("~/GetPendenciasQuodPF/{cpf}/{Id}")]
        public async Task<IActionResult> GetPendenciasQuodPF(string cpf, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.quod.com.br/WsQuodAPI/QuodReport?ver_=2.0");
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("32598094000139@esp:Q6BVLL2uv"));
                //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("hmlgpfactoring@esp:71pIWStl"));
                request.Headers.Add("Authorization", "Basic " + credentials);

                var jsonRequest = new
                {
                    QuodReportRequest = new
                    {
                        Options = new
                        {
                            IncludeBestInfo = "false",
                            IncludeCreditRiskIndicators = "false",
                            IncludeCreditRiskData = "true",
                            IncludeQuodScore = "false",
                            IncludeCreditLinesData = "false",
                        },
                        SearchBy = new
                        {
                            CPF = cpf
                        }
                    }
                };
                request.Content = new StringContent(JsonConvert.SerializeObject(jsonRequest), Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                foreach (var record in jsonResponse.QuodReportResponseEx.Response.Records.Record)
                {
                    if (Convert.ToDecimal(record.Negative.PendenciesControlCred) > 0)
                    {
                        foreach (var pendencia in record.Negative.Apontamentos.Apontamento)
                        {
                            string cnpjPend = pendencia.CNPJ;
                            string Company = pendencia.CompanyName;
                            string nature = pendencia.Nature;
                            string amount = pendencia.Amount;
                            string contract = pendencia.ContractNumber;
                            string participant = pendencia.ParticipantType;
                            string statusAp = pendencia.ApontamentoStatus;
                            string dateIncluded = pendencia.DateIncluded.Day + "/" + pendencia.DateIncluded.Month + "/" + pendencia.DateIncluded.Year;
                            string dateOccurred = pendencia.DateOccurred.Day + "/" + pendencia.DateOccurred.Month + "/" + pendencia.DateOccurred.Year;
                            string city = pendencia.Address.City;
                            string state = pendencia.Address.State;

                            using (var context = new MyDbContext())
                            {
                                var r = context.pendenciasQuod.Add(new TablePendenciasQuod
                                {
                                    ClienteId = Id,
                                    CNPJ = cnpjPend,
                                    CompanyName = Company,
                                    Nature = nature,
                                    Amount = Convert.ToDecimal(amount),
                                    ContractNumber = contract,
                                    ParticipantType = participant,
                                    ApontamentoStatus = statusAp,
                                    DateInclued = Convert.ToDateTime(dateIncluded),
                                    DateOcurred = Convert.ToDateTime(dateOccurred),
                                    City = city,
                                    State = state,
                                    PendenciesControlCred = record.Negative.PendenciesControlCred

                                }).Entity;
                                context.SaveChanges();

                                var n = context.Notificacoes.Add(new TableNotificacao
                                {
                                    DataNotificacao = DateTime.UtcNow,
                                    ClienteId = Id,
                                    Informacao = "Pendencias no  sistema Quod detectadas"

                                }).Entity;
                                context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        return Ok("Não existem pendencias referentes a esse documento");
                    }
                }
                return Ok("Pendencias no sistema Quod consultados");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("~/GetQuodPendenciasPJ/{cnpj}/{Id}")]
        public async Task<IActionResult> GetQuodPendenciasPJ(string cnpj, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.quod.com.br/WsQuodPJ/ReportPJ?ver_=1.4");

                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("32598094000139@esp:Q6BVLL2uv"));

                request.Headers.Add("Authorization", "Basic " + credentials);

                var jsonRequest = new
                {
                    ReportPJRequest = new
                    {
                        Options = new
                        {
                            IncludeBestInfo = "0",
                            IncludeCreditRiskIndicators = "0",
                            IncludeCreditRiskData = "1",
                            IncludeQuodScore = "0",
                        },
                        SearchBy = new
                        {
                            CNPJs = new
                            {
                                CNPJ = cnpj
                            }
                        }
                    }
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(jsonRequest), Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
                bool consulta1 = false;
                bool consulta2 = false;

                foreach (var record in jsonResponse.ReportPJResponseEx.Response.Records.ReportPJOutput)
                {
                    if (record.Negative != null && record.Negative.CcfApontamentos != null && record.Negative.CcfApontamentos.CcfApontamento != null)
                    {
                        foreach (var ccf in record.Negative.CcfApontamentos.CcfApontamento)
                        {
                            string cnpjPend = ccf.CpfCnpj;
                            string comarca = ccf.ReportingNameBank;
                            string dateDistribution = ccf.DateLastBounce.Day + "/" + ccf.DateLastBounce.Month + "/" + ccf.DateLastBounce.Year;

                            using (var context = new MyDbContext())
                            {
                                var r = context.pendenciasQuod.Add(new TablePendenciasQuod
                                {
                                    ClienteId = Id,
                                    CNPJ = cnpjPend,
                                    Comarca = comarca,
                                    Forum = "0",
                                    Vara = "0",
                                    Name = "0",
                                    ProcessType = "0",
                                    JusticeType = "0",
                                    Amount = 0,
                                    DateInclued = Convert.ToDateTime(dateDistribution),
                                    City = "0",
                                    State = "0",
                                    ProcessAuthor = "0",
                                    ProcessNumber = "0"

                                }).Entity;
                                context.SaveChanges();

                                var n = context.Notificacoes.Add(new TableNotificacao
                                {
                                    DataNotificacao = DateTime.UtcNow,
                                    ClienteId = Id,
                                    Informacao = "Pendencias no  sistema Quod detectadas"

                                }).Entity;

                                consulta1 = true;

                                context.SaveChanges();
                            }
                        }
                    }

                    if (record.Negative != null && record.Negative.LawSuitApontamentos != null && record.Negative.LawSuitApontamentos.LawSuitApontamento != null)
                    {
                        foreach (var pendencia in record.Negative.LawSuitApontamentos.LawSuitApontamento)
                        {
                            string cnpjPend = pendencia.CpfCnpj;
                            string comarca = pendencia.Comarca;
                            string forum = pendencia.Forum;
                            string vara = pendencia.Vara;
                            string name = pendencia.Name;
                            string processType = pendencia.ProcessType;
                            string amountLawsuit = pendencia.AmountLawsuit;
                            string dateDistribution = pendencia.DateDistribution.Day + "/" + pendencia.DateDistribution.Month + "/" + pendencia.DateDistribution.Year;
                            string city = pendencia.City;
                            string state = pendencia.State;
                            string justiceType = pendencia.JusticeType;
                            string processAuthor = pendencia.ProcessAuthor;
                            string processNumber = pendencia.ProcessNumber;

                            using (var context = new MyDbContext())
                            {
                                var r = context.pendenciasQuod.Add(new TablePendenciasQuod
                                {
                                    ClienteId = Id,
                                    CNPJ = cnpjPend,
                                    Comarca = comarca,
                                    Forum = forum,
                                    Vara = vara,
                                    Name = name,
                                    ProcessType = processType,
                                    JusticeType = justiceType,
                                    Amount = Convert.ToDecimal(amountLawsuit),
                                    DateInclued = Convert.ToDateTime(dateDistribution),
                                    City = city,
                                    State = state,
                                    ProcessAuthor = processAuthor,
                                    ProcessNumber = processNumber

                                }).Entity;
                                context.SaveChanges();

                                var n = context.Notificacoes.Add(new TableNotificacao
                                {
                                    DataNotificacao = DateTime.UtcNow,
                                    ClienteId = Id,
                                    Informacao = "Pendencias no  sistema Quod detectadas"

                                }).Entity;

                                consulta2 = true;

                                context.SaveChanges();
                            }
                        }
                    }

                    if (consulta1 || consulta2)
                    {
                        return Ok();
                    }
                    else
                    {
                        return Conflict("Não existem pendencias referentes a esse documento"); //ATENCAO - Utlizar Conflict para mensagens de erros personalizadas
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex); //ATENCAO - retornar ex para exibir mensagem de erro quando existir erro
            }
            return BadRequest();
        }

        //Resolver essa palhaçada, não achei um json que retorna o que precisa. PF OK 
        [HttpGet]
        [Route("~/GetProtestosQuodPJ/{cpf}/{Id}")]
        public async Task<IActionResult> GetProtestosQuodPJ(string cnpj, long Id)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.quod.com.br/WsQuodAPI/QuodReport?ver_=2.0");
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("32598094000139@esp:Q6BVLL2uv"));
                // string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("hmlgpfactoring@esp:71pIWStl"));
                request.Headers.Add("Authorization", "Basic " + credentials);

                var jsonRequest = new
                {
                    ReportPJRequest = new
                    {
                        Options = new
                        {
                            IncludeBestInfo = "false",
                            IncludeCreditRiskIndicators = "false",
                            IncludeCreditRiskData = "true",
                            IncludeQuodScore = "false",
                            IncludeCreditLinesData = "false",
                            IncludeInterpretaData = "false",
                            IncludeLawSuitData = "false",
                            IncludePartnershipData = "false",
                            IncludeCcfData = "false"
                        },
                        SearchBy = new
                        {
                            CNPJs = new
                            {
                                CNPJ = cnpj
                            }
                        }
                    }
                };
                request.Content = new StringContent(JsonConvert.SerializeObject(jsonRequest), Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                using (var context = new MyDbContext())
                {
                    var cd = (from t in context.protestosQuod
                              where t.Data == DateTime.UtcNow
                              where t.ClienteId == Id
                              select t).ToArray().FirstOrDefault();

                    if (cd == null)
                    {
                        foreach (var record in jsonResponse.QuodReportResponseEx.Response.Records.ReportPJOutput)
                        {

                            if (record.Protests != null && record.Protests.Protest.Count > 0)
                            {

                                foreach (var protest in record.Protests.Protest)
                                {
                                    foreach (var cartorio in protest.conteudo.cartorio)
                                    {
                                        var r = context.protestosQuod.Add(new TableProtestosQuod
                                        {
                                            NomeCartorio = cartorio.nome,
                                            Endereco = cartorio.endereco,
                                            Cidade = cartorio.cidade,
                                            Valor = Convert.ToDecimal(cartorio.valor_protestado),
                                            CPFCNPJ = cnpj,
                                            Data = DateTime.UtcNow,
                                            ClienteId = Id
                                        }).Entity;
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }
                        return Ok("Protestos no sistema Quod consultados");
                    }
                    else
                    {
                        return Ok("Não existem protestos referentes a esse documento");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("~/GetTokenBancoBMP")]
        public async Task<string> GetTokenBancoBMP()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://auth.moneyp.com.br/connect/token");
                request.Headers.Add("Cache-Control", "no-cache");
                var collection = new List<KeyValuePair<string, string>>();
                collection.Add(new("grant_type", "client_credentials"));
                collection.Add(new("client_id", "bmp.digital.api.gp_securitizadora_scr"));
                collection.Add(new("scope", "bmp.digital.api.full.access"));
                collection.Add(new("client_assertion", "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI4MzVlMWY0Zi02Njg5LTQ2YTUtYmQ2NS04ZTE4OWZhMjNiMGIiLCJzdWIiOiJibXAuZGlnaXRhbC5hcGkuZ3Bfc2VjdXJpdGl6YWRvcmFfc2NyIiwiaWF0IjoxNzAyNDg4MzI3LCJuYmYiOjE3MDI0ODgzMjcsImV4cCI6MTk3MDMzNzMwNiwiaXNzIjoiYm1wLmRpZ2l0YWwuYXBpLmdwX3NlY3VyaXRpemFkb3JhX3NjciIsImF1ZCI6Imh0dHBzOi8vYXV0aC5tb25leXAuY29tLmJyL2Nvbm5lY3QvdG9rZW4ifQ.WD9dahGsl1SumCh8ewZTQfDOve2gXS-sW--I6uLQKEsh9aFKgVQhDx_OeOk7wadHep2mvhHe7vxRFyPyl0cc2W6KwyTcwOpcFNOk61yKmCAh34tsRdtezd9WtSSX1WLFFdMES9Rr7NKImgRQ4Mfb4rr1LWcTt18H7SmsTA5E2-eYLL0m5jaBB6Jyo0K87SWjAHv1_s8neA-H8LT2FiUpRGWiqAKGvs8eeGJYcf6TLFhdarYbQRbwyjCoaoAqhB0Qr5Qg_cmQL3pDJaDKe1s9qeU01oMzny5drOHAgOJVDmUXZIuQeEV9WPQ5XnJ_BhePvqT3uSFN5y6sOfxrkjm1yQ"));
                collection.Add(new("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer"));
                var content = new FormUrlEncodedContent(collection);
                request.Content = content;
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeAnonymousType(responseContent,
                        new
                        {
                            access_token = ""
                        });

                    string accessToken = jsonResponse.access_token;

                    return (accessToken);
                }
                else
                {
                    return ("Erro em autenticar no banco BMP");
                }
            }
            catch
            {
                BadRequest();
            }
            return null;
        }

        [HttpGet]
        [Route("~/ConsultaSCRTeste/{cpfcnpj}/{Id}")]
        public async Task<string> ConsultaSCRTeste(string cpfcnpj, long Id)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://auth.moneyp.com.br/connect/token");
                request.Headers.Add("Cache-Control", "no-cache");
                var collection = new List<KeyValuePair<string, string>>();
                collection.Add(new("grant_type", "client_credentials"));
                collection.Add(new("client_id", "bmp.digital.api.gp_securitizadora_scr"));
                collection.Add(new("scope", "bmp.digital.api.full.access"));
                collection.Add(new("client_assertion", "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI4MzVlMWY0Zi02Njg5LTQ2YTUtYmQ2NS04ZTE4OWZhMjNiMGIiLCJzdWIiOiJibXAuZGlnaXRhbC5hcGkuZ3Bfc2VjdXJpdGl6YWRvcmFfc2NyIiwiaWF0IjoxNzAyNDg4MzI3LCJuYmYiOjE3MDI0ODgzMjcsImV4cCI6MTk3MDMzNzMwNiwiaXNzIjoiYm1wLmRpZ2l0YWwuYXBpLmdwX3NlY3VyaXRpemFkb3JhX3NjciIsImF1ZCI6Imh0dHBzOi8vYXV0aC5tb25leXAuY29tLmJyL2Nvbm5lY3QvdG9rZW4ifQ.WD9dahGsl1SumCh8ewZTQfDOve2gXS-sW--I6uLQKEsh9aFKgVQhDx_OeOk7wadHep2mvhHe7vxRFyPyl0cc2W6KwyTcwOpcFNOk61yKmCAh34tsRdtezd9WtSSX1WLFFdMES9Rr7NKImgRQ4Mfb4rr1LWcTt18H7SmsTA5E2-eYLL0m5jaBB6Jyo0K87SWjAHv1_s8neA-H8LT2FiUpRGWiqAKGvs8eeGJYcf6TLFhdarYbQRbwyjCoaoAqhB0Qr5Qg_cmQL3pDJaDKe1s9qeU01oMzny5drOHAgOJVDmUXZIuQeEV9WPQ5XnJ_BhePvqT3uSFN5y6sOfxrkjm1yQ"));
                collection.Add(new("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer"));
                var content = new FormUrlEncodedContent(collection);
                request.Content = content;
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeAnonymousType(responseContent,
                        new
                        {
                            access_token = ""
                        });

                    string accessToken = jsonResponse.access_token;

                    try
                    {
                        var data = DateTime.UtcNow.AddMonths(-2);
                        int mes = data.Month;
                        int ano = data.Year;

                        var client2 = new HttpClient();
                        var request2 = new HttpRequestMessage(HttpMethod.Post, "https://api.bmpdigital.moneyp.com.br/Bureau/ConsultarSCR");
                        request2.Headers.Add("IdempotencyKey", "6c490988-8d84-46dd-873e-78053e1dad1a");
                        request2.Headers.Add("Authorization", "Bearer " + accessToken);
                        var content2 = new StringContent($"{{\"consulta\": {{\"documento\": \"{cpfcnpj}\",\"dataBaseMes\": \"{mes}\",\"dataBaseAno\": \"{ano}\"}}}}", null, "application/json");
                        request2.Content = content2;
                        var response2 = await client2.SendAsync(request2);
                        response2.EnsureSuccessStatusCode();
                        string responseContent2 = await response2.Content.ReadAsStringAsync();

                        dynamic jsonResponse2 = JsonConvert.DeserializeObject(responseContent);

                        return (responseContent2);

                        string codigoDoCliente = jsonResponse2.resumoDoCliente.codigoDoCliente;
                        string dataBaseConsultada = jsonResponse2.resumoDoCliente.dataBaseConsultada;
                        string dataInicioRelacionamento = jsonResponse2.resumoDoCliente.dataInicioRelacionamento;

                        string carteiraVencerAte30diasVencidosAte14dias = jsonResponse2.resumoDoClienteTraduzido.carteiraVencerAte30diasVencidosAte14dias;
                        string carteiraVencer31a60dias = jsonResponse2.resumoDoClienteTraduzido.carteiraVencer31a60dias;
                        string carteiraVencer61a90dias = jsonResponse2.resumoDoClienteTraduzido.carteiraVencer61a90dias;
                        string carteiraVencer91a180dias = jsonResponse2.resumoDoClienteTraduzido.carteiraVencer91a180dias;
                        string carteiraVencer181a360dias = jsonResponse2.resumoDoClienteTraduzido.carteiraVencer181a360dias;
                        string carteiraVencerPrazoIndeterminado = jsonResponse2.resumoDoClienteTraduzido.carteiraVencerPrazoIndeterminado;
                        string responsabilidadeTotal = jsonResponse2.resumoDoClienteTraduzido.responsabilidadeTotal;
                        string creditosaLiberar = jsonResponse2.resumoDoClienteTraduzido.creditosaLiberar;
                        string limitesdeCredito = jsonResponse2.resumoDoClienteTraduzido.limitesdeCredito;
                        string riscoTotal = jsonResponse2.resumoDoClienteTraduzido.riscoTotal;

                        string qtdeOperacoesDiscordancia = jsonResponse2.resumoDoClienteTraduzido.qtdeOperacoesDiscordancia;
                        string vlrOperacoesDiscordancia = jsonResponse2.resumoDoClienteTraduzido.vlrOperacoesDiscordancia;
                        string qtdeOperacoesSobJudice = jsonResponse2.resumoDoClienteTraduzido.qtdeOperacoesSobJudice;
                        string vlrOperacoesSobJudice = jsonResponse2.resumoDoClienteTraduzido.vlrOperacoesSobJudice;

                        string carteiraVencido = jsonResponse2.resumoDoClienteTraduzido.carteiraVencido;
                        string carteiraVencer = jsonResponse2.resumoDoClienteTraduzido.carteiraVencer;

                        //    using (var context = new MyDbContext())
                        //    {
                        //        var r = context.endividamentoSCR.Add(new TableEndividamentoSCR
                        //        {
                        //            ClienteId = Id,
                        //            codigoDoCliente = codigoDoCliente,
                        //            dataBaseConsultada = dataBaseConsultada,
                        //            dataInicioRelacionamento = dataInicioRelacionamento,
                        //            carteiraVencerAte30diasVencidosAte14dias = carteiraVencerAte30diasVencidosAte14dias,
                        //            carteiraVencer31a60dias = carteiraVencer31a60dias,
                        //            carteiraVencer61a90dias = carteiraVencer61a90dias,
                        //            carteiraVencer91a180dias = carteiraVencer91a180dias,
                        //            carteiraVencer181a360dias = carteiraVencer181a360dias,
                        //            carteiraVencerPrazoIndeterminado = carteiraVencerPrazoIndeterminado,
                        //            responsabilidadeTotal = responsabilidadeTotal,
                        //            creditosaLiberar = creditosaLiberar,
                        //            limitesdeCredito = limitesdeCredito,
                        //            riscoTotal = riscoTotal,
                        //            qtdeOperacoesDiscordancia = qtdeOperacoesDiscordancia,
                        //            vlrOperacoesDiscordancia = vlrOperacoesDiscordancia,
                        //            qtdeOperacoesSobJudice = qtdeOperacoesSobJudice,
                        //            vlrOperacoesSobJudice = vlrOperacoesSobJudice,

                        //            DataConsulta = DateTime.UtcNow,
                        //            carteiraVencido = carteiraVencido,
                        //            carteiraVencer = carteiraVencer

                        //        }).Entity;
                        //        context.SaveChanges();
                        //    }
                    }
                    catch (Exception e)
                    {
                        BadRequest(e.Message);
                    }
                }
                else
                {
                    return ("Erro em autenticar no banco BMP");
                }
            }
            catch
            {
                BadRequest();
            }
            return null;
        }

        [HttpGet]
        [Route("~/ConsultaSCR/{token}/{cpfcnpj}/{Id}")]
        public async Task<string> ConsultaSCR(string token, string cpfcnpj, long Id)
        {
            try
            {
                var data = DateTime.UtcNow.AddMonths(-2);
                int mes = data.Month;
                int ano = data.Year;

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.bmpdigital.moneyp.com.br/Bureau/ConsultarSCR");
                request.Headers.Add("IdempotencyKey", "6c490988-8d84-46dd-873e-78053e1dad1a");
                request.Headers.Add("Authorization", "Bearer " + token);
                var content = new StringContent($"{{\"consulta\": {{\"documento\": \"{cpfcnpj}\",\"dataBaseMes\": \"{mes}\",\"dataBaseAno\": \"{ano}\"}}}}", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                //return responseContent;

                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                string codigoDoCliente = jsonResponse.resumoDoCliente.codigoDoCliente;
                string dataBaseConsultada = jsonResponse.resumoDoCliente.dataBaseConsultada;
                string dataInicioRelacionamento = jsonResponse.resumoDoCliente.dataInicioRelacionamento;

                string carteiraVencerAte30diasVencidosAte14dias = jsonResponse.resumoDoClienteTraduzido.carteiraVencerAte30diasVencidosAte14dias;
                string carteiraVencer31a60dias = jsonResponse.resumoDoClienteTraduzido.carteiraVencer31a60dias;
                string carteiraVencer61a90dias = jsonResponse.resumoDoClienteTraduzido.carteiraVencer61a90dias;
                string carteiraVencer91a180dias = jsonResponse.resumoDoClienteTraduzido.carteiraVencer91a180dias;
                string carteiraVencer181a360dias = jsonResponse.resumoDoClienteTraduzido.carteiraVencer181a360dias;
                string carteiraVencerPrazoIndeterminado = jsonResponse.resumoDoClienteTraduzido.carteiraVencerPrazoIndeterminado;
                string responsabilidadeTotal = jsonResponse.resumoDoClienteTraduzido.responsabilidadeTotal;
                string creditosaLiberar = jsonResponse.resumoDoClienteTraduzido.creditosaLiberar;
                string limitesdeCredito = jsonResponse.resumoDoClienteTraduzido.limitesdeCredito;
                string riscoTotal = jsonResponse.resumoDoClienteTraduzido.riscoTotal;

                string qtdeOperacoesDiscordancia = jsonResponse.resumoDoClienteTraduzido.qtdeOperacoesDiscordancia;
                string vlrOperacoesDiscordancia = jsonResponse.resumoDoClienteTraduzido.vlrOperacoesDiscordancia;
                string qtdeOperacoesSobJudice = jsonResponse.resumoDoClienteTraduzido.qtdeOperacoesSobJudice;
                string vlrOperacoesSobJudice = jsonResponse.resumoDoClienteTraduzido.vlrOperacoesSobJudice;

                string carteiraVencido = jsonResponse.resumoDoClienteTraduzido.carteiraVencido;
                string carteiraVencer = jsonResponse.resumoDoClienteTraduzido.carteiraVencer;

                //if(jsonResponse.resumoModalidade != null || jsonResponse.resumoModalidade != "" || Convert.ToInt32(jsonResponse.resumoModalidade).Count() > 0)
                //{
                //    var mod = new List<ResumoModalidade>();  

                //    foreach(var modalidade in jsonResponse.resumoModalidade)
                //    {
                //        var aux = new ResumoModalidade
                //        {
                //            tipo = modalidade.tipo,
                //            subdominio = modalidade.subdominio,
                //            dominio = modalidade.dominio,
                //            modalidade = modalidade.modalidade,
                //            valorVencimento = modalidade.valorVencimento.ToFloat()
                //        };

                //        if(mod.FirstOrDefault(x => x.tipo.Equals(aux.tipo) && x.subdominio.Equals(aux.subdominio)) == null)
                //        {
                //            mod.Add(aux);
                //        }

                //    }

                //}

                using (var context = new MyDbContext())
                {
                    var r = context.endividamentoSCR.Add(new TableEndividamentoSCR
                    {
                        ClienteId = Id,
                        codigoDoCliente = codigoDoCliente,
                        dataBaseConsultada = dataBaseConsultada,
                        dataInicioRelacionamento = dataInicioRelacionamento,
                        carteiraVencerAte30diasVencidosAte14dias = carteiraVencerAte30diasVencidosAte14dias,
                        carteiraVencer31a60dias = carteiraVencer31a60dias,
                        carteiraVencer61a90dias = carteiraVencer61a90dias,
                        carteiraVencer91a180dias = carteiraVencer91a180dias,
                        carteiraVencer181a360dias = carteiraVencer181a360dias,
                        carteiraVencerPrazoIndeterminado = carteiraVencerPrazoIndeterminado,
                        responsabilidadeTotal = responsabilidadeTotal,
                        creditosaLiberar = creditosaLiberar,
                        limitesdeCredito = limitesdeCredito,
                        riscoTotal = riscoTotal,
                        qtdeOperacoesDiscordancia = qtdeOperacoesDiscordancia,
                        vlrOperacoesDiscordancia = vlrOperacoesDiscordancia,
                        qtdeOperacoesSobJudice = qtdeOperacoesSobJudice,
                        vlrOperacoesSobJudice = vlrOperacoesSobJudice,

                        DataConsulta = DateTime.UtcNow,
                        carteiraVencido = carteiraVencido,
                        carteiraVencer = carteiraVencer

                    }).Entity;
                    context.SaveChanges();
                }
            }
            catch
            {
                BadRequest();
            }
            return ("Endividamento consultado com sucesso no banco BMP");
        }

        [HttpGet]
        [Route("~/GetNFEs/{numero}")]
        public async Task GetNFEs(string numero)
        {
            try
            {
                var client = new HttpClient();

                string token = await GetToken();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var nfeResponse = await client.GetAsync($"https://gateway.apiserpro.serpro.gov.br/consulta-nfe-df/api/v1/nfe/{numero}");

                if (!nfeResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Nota Fiscal não encontrada na Serpro");
                }

                var nfeData = await nfeResponse.Content.ReadAsStringAsync();
                var dataObject = JObject.Parse(nfeData);
                var cStat = dataObject["nfeProc"]["protNFe"]["infProt"]["cStat"].ToString();

                if (cStat == "101")
                {
                    var status = "Desativado";
                    NFEController nfecontroller = new NFEController();
                    nfecontroller.UpdateStatusNFE(status, numero);
                }
            }
            catch (Exception e)
            {
                BadRequest();
            }
        }

        [HttpGet]
        [Route("~/GetSCR/{cpfcnpj}/{Id}")]
        public async Task GetSCR(string cpfcnpj, long Id)
        {
            try
            {
                var client = new HttpClient();

                string token = await GetTokenBancoBMP();

                await ConsultaSCR(token, cpfcnpj, Id);

            }
            catch (Exception ex)
            {
                BadRequest();
            }
        }

        [HttpGet]
        [Route("~/SituacaoCNPJ/{CNPJ}/{Id}")]
        public async Task SituacaoCNPJ(string CNPJ, long Id)
        {
            try
            {
                string CNPJReplace = CNPJ.Replace(".", "").Replace("-", "").Replace("/", "");

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://brasilapi.com.br/api/cnpj/v1/" + CNPJReplace);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                string situacao = jsonResponse.descricao_situacao_cadastral;

                using (var context = new MyDbContext())
                {
                    var cliente = (from t in context.Clientes
                                   where t.Id.Equals(Id)
                                   select t).ToArray().FirstOrDefault();

                    if (cliente != null)
                    {
                        cliente.SituacaoCNPJ = situacao;

                        if (situacao == "BAIXADA")
                        {
                            cliente.status = false;

                            var n = context.Notificacoes.Add(new TableNotificacao
                            {
                                DataNotificacao = DateTime.UtcNow,
                                ClienteId = Id,
                                Informacao = "Situação do CNPJ consta como BAIXADA na receita federal"

                            }).Entity;
                        }
                        context.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                BadRequest();
            }
        }

        [HttpPost]
        [Route("~/DecideConsulta/{IdSacado}/{documento}/{quod}/{decisaoscore}/{decisaopendencia}/{decisaoprotesto}/{scr}")]
        public async Task DecideConsulta(long IdSacado, string documento, bool quod, bool decisaoscore, bool decisaopendencia, bool decisaoprotesto, bool scr)
        {

            string documentoSemPontos = documento.Replace(".", "").Replace("-", "").Replace("/", "");

            if (documentoSemPontos.Length == 11)
            {
                if (quod == true)
                {
                    await GetQuodInfoSacadoPF(documentoSemPontos, IdSacado);
                }
                if (decisaoscore == true)
                {
                    await GetDecisaoScore(documentoSemPontos, IdSacado);
                }
                if (decisaopendencia == true)
                {
                    await GetDecisaoPendenciasPF(documentoSemPontos, IdSacado);
                }
                if (decisaoprotesto == true)
                {
                    await GetDecisaoProtestosPF(documentoSemPontos, IdSacado);
                }
                if (scr == true)
                {
                    await GetSCR(documentoSemPontos, IdSacado);
                }
            }

            if (documentoSemPontos.Length == 14)
            {
                if (quod == true)
                {
                    await GetQuodInfoSacadoPJ(documentoSemPontos, IdSacado);
                }
                if (decisaoscore == true)
                {
                    await GetDecisaoScorePJ(documentoSemPontos, IdSacado);
                }
                if (decisaopendencia == true)
                {
                    await GetDecisaoPendenciasPJ(documentoSemPontos, IdSacado);
                }
                if (decisaoprotesto == true)
                {
                    await GetDecisaoProtestosPJ(documentoSemPontos, IdSacado);
                }
                if (scr == true)
                {
                    await GetSCR(documentoSemPontos, IdSacado);
                }
            }
        }

        [HttpGet]
        [Route("~/GetInfoLemit/{cpf}")]
        public async Task<IActionResult> GetInfoLemit(string cpf)
        {
            string cpfReplace = cpf.Replace(".", "").Replace("-", "").Replace("/", "");

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.lemit.com.br/api/v1/consulta/pessoa");

            request.Headers.Add("Authorization", "Bearer yrKqYsNZn2IuSZ4a4Yv72bBbgw6xmOmXdoKrnZVR");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("documento", cpfReplace));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

            string cpjLemit = jsonResponse.pessoa.cpf;
            string nomeLemit = jsonResponse.pessoa.nome;
            string data = jsonResponse.pessoa.data_nascimento;

            string telefoneLemit = null;

            string logradouro = null;
            string complementoLemit = null;
            string cepLemit = null;
            string estadoLemit = null;
            string numeroLemit = null;
            string cidadeLemit = null;
            string emailLemit = null;


            if (jsonResponse.pessoa.celulares != null && jsonResponse.pessoa.celulares.Count > 0)
            {
                string ddd = jsonResponse.pessoa.celulares[jsonResponse.pessoa.celulares.Count - 1].ddd;
                string numeroTelefone = jsonResponse.pessoa.celulares[jsonResponse.pessoa.celulares.Count - 1].numero;
                telefoneLemit = "(" + ddd + ")" + numeroTelefone;

            }
            if (jsonResponse.pessoa.enderecos != null && jsonResponse.pessoa.enderecos.Count > 0)
            {
                logradouro = jsonResponse.pessoa.enderecos[jsonResponse.pessoa.enderecos.Count - 1].logradouro;
                complementoLemit = jsonResponse.pessoa.enderecos[jsonResponse.pessoa.enderecos.Count - 1].complemento;
                cepLemit = jsonResponse.pessoa.enderecos[jsonResponse.pessoa.enderecos.Count - 1].cep;
                estadoLemit = jsonResponse.pessoa.enderecos[jsonResponse.pessoa.enderecos.Count - 1].uf;
                numeroLemit = jsonResponse.pessoa.enderecos[jsonResponse.pessoa.enderecos.Count - 1].numero;
                cidadeLemit = jsonResponse.pessoa.enderecos[jsonResponse.pessoa.enderecos.Count - 1].cidade;
            }
            if (jsonResponse.pessoa.emails != null && jsonResponse.pessoa.emails.Count > 0)
            {
                emailLemit = jsonResponse.pessoa.emails[jsonResponse.pessoa.emails.Count - 1].email;
            }

            var pessoa = new
            {
                cpf = cpjLemit,
                nome = nomeLemit,
                nascimento = data,
                rua = logradouro,
                complemento = complementoLemit,
                cep = cepLemit,
                estado = estadoLemit,
                numero = numeroLemit,
                cidade = cidadeLemit,
                email = emailLemit,
                telefone = telefoneLemit
            };

            return Ok(pessoa);
        }


        [HttpPost]
        [Route("~/GetQuodInfoSacadoPF/{cpf}/{Id}")]
        public async Task<IActionResult> GetQuodInfoSacadoPF(string cpf, long Id)
        {
            List<Apontamento> pendencias = new List<Apontamento>();

            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.quod.com.br/WsQuodAPI/QuodReport?ver_=2.0");
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("32598094000139@esp:Q6BVLL2uv"));
                request.Headers.Add("Authorization", "Basic " + credentials);

                var jsonRequest = new
                {
                    QuodReportRequest = new
                    {
                        Options = new
                        {
                            IncludeBestInfo = "false",
                            IncludeCreditRiskIndicators = "true",
                            IncludeCreditRiskData = "true",
                            IncludeQuodScore = "true",
                            IncludeCreditLinesData = "false",
                        },
                        SearchBy = new
                        {
                            CPF = cpf
                        }
                    }
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(jsonRequest), Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();

                AuxQuod.Root jsonResponse = JsonConvert.DeserializeObject<AuxQuod.Root>(responseContent);

                if (jsonResponse.QuodReportResponseEx.Response.Records.Record != null)
                {
                    foreach (var registro in jsonResponse.QuodReportResponseEx.Response.Records.Record)
                    {
                        if (registro != null && registro.Negative != null && registro.Negative.PendenciesControlCred != 0)
                        {
                            foreach (var pendencia in registro.Negative.Apontamentos.Apontamento)
                            {

                                Date DateIncluded = new Date
                                {
                                    Year = pendencia.DateIncluded.Year,
                                    Month = pendencia.DateIncluded.Month,
                                    Day = pendencia.DateIncluded.Day
                                };

                                Date DateOccurred = new Date
                                {
                                    Year = pendencia.DateIncluded.Year,
                                    Month = pendencia.DateIncluded.Month,
                                    Day = pendencia.DateOccurred.Day
                                };

                                Address Address = new Address
                                {
                                    City = pendencia.Address.City,
                                    State = pendencia.Address.State
                                };

                                Apontamento Pendencia = new Apontamento
                                {
                                    CNPJ = pendencia.CNPJ,
                                    CompanyName = pendencia.CompanyName,
                                    Nature = pendencia.Nature,
                                    Amount = pendencia.Amount,
                                    ContractNumber = pendencia.ContractNumber,
                                    ParticipantType = pendencia.ParticipantType,
                                    ApontamentoStatus = pendencia.ApontamentoStatus,
                                    DateIncluded = DateIncluded,
                                    DateOccurred = DateOccurred,
                                    Address = Address
                                };

                                pendencias.Add(Pendencia);
                            }
                        }

                        var cadastro = new AuxCadastro
                        {
                            QuodScore = registro.QuodScore.Score,
                            QuodPendencias = null,
                        };

                        if (registro != null && registro.Negative != null && registro.Negative.PendenciesControlCred != 0)
                        {
                            cadastro.QuodPendencias = pendencias;
                        }

                        using (var context = new MyDbContext())
                        {
                            using (var transaction = context.Database.BeginTransaction())
                            {
                                var score = context.QuodScores.Add(new TableQuodScore
                                {
                                    CPFCNPJ = cpf,
                                    DataScore = DateTime.UtcNow,
                                    Score = cadastro.QuodScore.ToString(),
                                    ClienteId = Id

                                }).Entity;
                                context.SaveChanges();

                                if (cadastro.QuodPendencias != null)
                                {
                                    foreach (var ocorrencia in cadastro.QuodPendencias)
                                    {
                                        int yearDateOccurred = int.Parse(ocorrencia.DateOccurred.Year);
                                        int monthDateOccurred = int.Parse(ocorrencia.DateOccurred.Month);
                                        int dayDateOccurred = int.Parse(ocorrencia.DateOccurred.Day);

                                        int yearDateInclued = int.Parse(ocorrencia.DateIncluded.Year);
                                        int monthDateInclued = int.Parse(ocorrencia.DateIncluded.Month);
                                        int dayDateInclued = int.Parse(ocorrencia.DateIncluded.Day);

                                        DateTime dateTimeDateOccurred = new DateTime(yearDateOccurred, monthDateOccurred, dayDateOccurred);
                                        DateTime dateTimeDateInclued = new DateTime(yearDateInclued, monthDateInclued, dayDateInclued);

                                        var pendenciaDB = context.pendenciasQuod.Add(new TablePendenciasQuod
                                        {
                                            ClienteId = Id,
                                            CNPJ = cpf,
                                            CompanyName = ocorrencia.CompanyName,
                                            Nature = ocorrencia.Nature,
                                            Amount = Convert.ToDecimal(ocorrencia.Amount),
                                            ContractNumber = ocorrencia.ContractNumber,
                                            ParticipantType = ocorrencia.ParticipantType,
                                            ApontamentoStatus = ocorrencia.ApontamentoStatus,
                                            DateInclued = dateTimeDateInclued,
                                            DateOcurred = dateTimeDateOccurred,
                                            City = ocorrencia.Address.City,
                                            State = ocorrencia.Address.State,

                                        }).Entity;
                                        context.SaveChanges();
                                    }
                                }

                                try
                                {
                                    transaction.Commit();
                                    return Ok();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    throw ex;
                                    return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Details = "Detalhes adicionais sobre o erro" });
                                }
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Details = "Detalhes adicionais sobre o erro" });
            }
        }



        [HttpPost]
        [Route("~/GetQuodInfoSacadoPJ/{cnpj}/{Id}")]
        public async Task<IActionResult> GetQuodInfoSacadoPJ(string cnpj, long Id)
        {
            List<Apontamento> pendencias = new List<Apontamento>();

            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.quod.com.br/WsQuodPJ/ReportPJ?ver_=1.4");

                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("32598094000139@esp:Q6BVLL2uv"));

                request.Headers.Add("Authorization", "Basic " + credentials);

                var jsonRequest = new
                {
                    ReportPJRequest = new
                    {
                        Options = new
                        {
                            IncludeBestInfo = "false",
                            IncludeCreditRiskIndicators = "true",
                            IncludeCreditRiskData = "true",
                            IncludeQuodScore = "true",
                            IncludeCreditLinesData = "false",
                            IncludeInterpretaData = "false",
                            IncludeLawSuitData = "false",
                            IncludePartnershipData = "false",
                            IncludeCcfData = "false"
                        },
                        SearchBy = new
                        {
                            CNPJs = new
                            {
                                CNPJ = cnpj
                            }
                        }
                    }
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(jsonRequest), Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);

                string responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                string score = jsonResponse.ReportPJResponseEx.Response.Records.ReportPJOutput[0].ScorePJ.Score;

                if (jsonResponse.ReportPJResponseEx.Response.Records.ReportPJOutput != null)
                {
                    foreach (var registro in jsonResponse.ReportPJResponseEx.Response.Records.ReportPJOutput)
                    {
                        if (registro != null && registro.Negative != null && registro.Negative.PendenciesControlCred != 0)
                        {
                            foreach (var pendencia in registro.Negative.Apontamentos.Apontamento)
                            {
                                Date DateIncluded = new Date
                                {
                                    Year = pendencia.DateIncluded.Year,
                                    Month = pendencia.DateIncluded.Month,
                                    Day = pendencia.DateIncluded.Day
                                };

                                Date DateOccurred = new Date
                                {
                                    Year = pendencia.DateIncluded.Year,
                                    Month = pendencia.DateIncluded.Month,
                                    Day = pendencia.DateOccurred.Day
                                };

                                Address Address = new Address
                                {
                                    City = pendencia.Address.City,
                                    State = pendencia.Address.State
                                };

                                Apontamento Pendencia = new Apontamento
                                {
                                    CNPJ = pendencia.CNPJ,
                                    CompanyName = pendencia.CompanyName,
                                    Nature = pendencia.Nature,
                                    Amount = pendencia.Amount,
                                    ContractNumber = pendencia.ContractNumber,
                                    ParticipantType = pendencia.ParticipantType,
                                    ApontamentoStatus = pendencia.ApontamentoStatus,
                                    DateIncluded = DateIncluded,
                                    DateOccurred = DateOccurred,
                                    Address = Address
                                };

                                pendencias.Add(Pendencia);
                            }
                        }

                        var cadastro = new AuxCadastro
                        {
                            QuodScore = registro.ScorePJ.Score,
                            QuodPendencias = null,
                        };

                        if (registro != null && registro.Negative != null && registro.Negative.PendenciesControlCred != 0)
                        {
                            cadastro.QuodPendencias = pendencias;
                        }


                        using (var context = new MyDbContext())
                        {
                            using (var transaction = context.Database.BeginTransaction())
                            {
                                var scoreDB = context.QuodScores.Add(new TableQuodScore
                                {
                                    CPFCNPJ = cnpj,
                                    DataScore = DateTime.UtcNow,
                                    Score = score.ToString(),
                                    ClienteId = Id

                                }).Entity;
                                context.SaveChanges();

                                if (cadastro.QuodPendencias != null)
                                {
                                    foreach (var ocorrencia in cadastro.QuodPendencias)
                                    {
                                        int yearDateOccurred = int.Parse(ocorrencia.DateOccurred.Year);
                                        int monthDateOccurred = int.Parse(ocorrencia.DateOccurred.Month);
                                        int dayDateOccurred = int.Parse(ocorrencia.DateOccurred.Day);

                                        int yearDateInclued = int.Parse(ocorrencia.DateIncluded.Year);
                                        int monthDateInclued = int.Parse(ocorrencia.DateIncluded.Month);
                                        int dayDateInclued = int.Parse(ocorrencia.DateIncluded.Day);

                                        DateTime dateTimeDateOccurred = new DateTime(yearDateOccurred, monthDateOccurred, dayDateOccurred);
                                        DateTime dateTimeDateInclued = new DateTime(yearDateInclued, monthDateInclued, dayDateInclued);

                                        var pendenciaDB = context.pendenciasQuod.Add(new TablePendenciasQuod
                                        {
                                            ClienteId = Id,
                                            CNPJ = cnpj,
                                            CompanyName = ocorrencia.CompanyName,
                                            Nature = ocorrencia.Nature,
                                            Amount = Convert.ToDecimal(ocorrencia.Amount),
                                            ContractNumber = ocorrencia.ContractNumber,
                                            ParticipantType = ocorrencia.ParticipantType,
                                            ApontamentoStatus = ocorrencia.ApontamentoStatus,
                                            DateInclued = dateTimeDateInclued,
                                            DateOcurred = dateTimeDateOccurred,
                                            City = ocorrencia.Address.City,
                                            State = ocorrencia.Address.State,

                                        }).Entity;
                                        context.SaveChanges();
                                    }
                                }

                                try
                                {
                                    transaction.Commit();
                                    return Ok();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    throw ex;
                                    return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Details = "Detalhes adicionais sobre o erro" });
                                }
                            }
                        }
                    }
                }
                return Ok();

            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost]
        [Route("~/ConfereCPFiadoGarantido/{cpf}")]
        public async Task<IActionResult> ConfereCPFiadoGarantido(string cpf)
        {
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.quod.com.br/WsQuodAPI/QuodReport?ver_=2.0");
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("32598094000139@esp:Q6BVLL2uv"));
                request.Headers.Add("Authorization", "Basic " + credentials);

                var jsonRequest = new
                {
                    QuodReportRequest = new
                    {
                        Options = new
                        {
                            IncludeBestInfo = "true",
                            IncludeCreditRiskIndicators = "true",
                            IncludeCreditRiskData = "true",
                            IncludeQuodScore = "true",
                            IncludeCreditLinesData = "false",
                        },
                        SearchBy = new
                        {
                            CPF = cpf
                        }
                    }
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(jsonRequest), Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                return Ok(responseContent);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Details = "Detalhes adicionais sobre o erro" });
            }

        }


    }
}
