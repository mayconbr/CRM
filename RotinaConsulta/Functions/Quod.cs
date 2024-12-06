using CRMAudax.Models;
using CRMAudax;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static CRMAudax.Models.AuxQuod;
using System.Xml;
using System.Net.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text.RegularExpressions;
using System;


namespace RotinaConsulta.Functions
{
    public class Quod
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient1;
        private readonly HttpClient _httpClient2;

        public Quod(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient();
            _httpClient1 = clientFactory.CreateClient();
            _httpClient2 = clientFactory.CreateClient();
        }
        public void GetQuodScoreRotina(string cpf, long Id)
        {
            HttpResponseMessage response = null;
            string responseContent = string.Empty;

            TableNotificacao tableNotificacao = new TableNotificacao();
            var n = tableNotificacao;

            try
            {
                var client1 = _httpClient1;

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
                        response = client1.Send(request);
                        responseContent = response.Content.ReadAsStringAsync().Result;

                        // Log the response content
                        //Console.WriteLine($"Response Content: {responseContent}");

                        response.EnsureSuccessStatusCode();

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
                            bool verifica;

                            var ultimoScore = (from t in context.QuodScores
                                               where t.ClienteId == Id
                                               orderby t.DataScore descending
                                               select t).FirstOrDefault();

                            verifica = int.Parse(ultimoScore.Score) > int.Parse(r.Score);

                            

                           n = context.Notificacoes.Add(new TableNotificacao
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
                    Console.WriteLine("Consulta QuodScore concluída no cliente de id " + Id);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP Request Error: " + ex.Message);

                if (response != null)
                {
                    Console.WriteLine("Status Code: " + response.StatusCode);
                    Console.WriteLine("Error Content: " + responseContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);

                using (var context = new MyDbContext())
                {
                    var Log = context.LogsErroRotina.Add(new TableLogErrorRotina
                    {
                        Documento = cpf,
                        Consulta = "QuodScorePF",
                        DataConsulta = DateTime.UtcNow,
                        Erro = ex.Message

                    }).Entity;

                    context.SaveChanges();
                }
            }
        }


        public void GetPendenciasQuodPFRotina(string cpf, long Id)
        {
            HttpResponseMessage response = null;
            string responseContent = string.Empty;


            TablePendenciasQuod tablePendencias = new TablePendenciasQuod();
            var pendencias = tablePendencias;

            try
            {
                var client2 = _httpClient2;
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

                var content = new StringContent(JsonConvert.SerializeObject(jsonRequest), null, "application/json");
                request.Content = content;

                // Envio da requisição com captura detalhada de exceções
                try
                {
                    Console.WriteLine(content);
                    response = client2.Send(request); //erro aqui                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao enviar requisição para a API: " + ex.Message);
                    Console.WriteLine("Detalhes do erro: " + ex.StackTrace);

                }

                Console.WriteLine("Requisição enviada com sucesso. Verificando resposta...");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Erro: Requisição falhou com o código de status: " + response.StatusCode);
                    return;
                }

                responseContent = response.Content.ReadAsStringAsync().Result;

                // Exibe o conteúdo da resposta para depuração
                //Console.WriteLine("Conteúdo da resposta recebido: " + responseContent);

                // Verificação para resposta em XML com erro de CPF inválido
                if (responseContent.StartsWith("<?xml"))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(responseContent);

                    var statusNode = xmlDoc.SelectSingleNode("//Status");
                    var messageNode = xmlDoc.SelectSingleNode("//Message");

                    if (statusNode != null && statusNode.InnerText == "1510" && messageNode != null)
                    {
                        Console.WriteLine($"Erro: {messageNode.InnerText}");
                        return;
                    }
                }

                // Deserialização JSON e processamento de registros
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                Console.WriteLine("Resposta JSON deserializada com sucesso.");

                foreach (var record in jsonResponse.QuodReportResponseEx.Response.Records.Record)
                {
                    if (Convert.ToDecimal(record.Negative.PendenciesControlCred) > 0)
                    {
                        foreach (var pendencia in record.Negative.Apontamentos.Apontamento)
                        {
                            using (var context = new MyDbContext())
                            {
                                pendencias = context.pendenciasQuod.Add(new TablePendenciasQuod
                                {
                                    ClienteId = Id,
                                    CNPJ = pendencia.CNPJ,
                                    CompanyName = pendencia.CompanyName,
                                    Nature = pendencia.Nature,
                                    Amount = Convert.ToDecimal(pendencia.Amount),
                                    ContractNumber = pendencia.ContractNumber,
                                    ParticipantType = pendencia.ParticipantType,
                                    ApontamentoStatus = pendencia.ApontamentoStatus,
                                    DateInclued = Convert.ToDateTime(pendencia.DateIncluded),
                                    DateOcurred = Convert.ToDateTime(pendencia.DateOccurred),
                                    City = pendencia.Address.City,
                                    State = pendencia.Address.State,
                                    PendenciesControlCred = record.Negative.PendenciesControlCred
                                }).Entity;

                                context.Notificacoes.Add(new TableNotificacao
                                {
                                    DataNotificacao = DateTime.UtcNow,
                                    ClienteId = Id,
                                    Informacao = "Pendencias no sistema Quod detectadas"
                                });
                                context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não existem pendencias no Quod para o cliente " + Id);
                    }
                }
                Console.WriteLine("Consulta PendenciasQuod concluída no cliente de id " + Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro de requisição HTTP: " + ex.Message);

                using (var context = new MyDbContext())
                {
                    var Log = context.LogsErroRotina.Add(new TableLogErrorRotina
                    {
                        Documento = cpf,
                        Consulta = "PendenciasQuodPF",
                        DataConsulta = DateTime.UtcNow,
                        Erro = ex.Message

                    }).Entity;

                    context.SaveChanges();
                }
            }
        }




        public void GetQuodScorePJRotina(string cnpj, long Id)
        {

            TableQuodScore tableQuodScore = new TableQuodScore();
            var cd = tableQuodScore;

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
                     cd = (from t in context.QuodScores
                           where t.DataScore == DateTime.UtcNow
                           where t.ClienteId == Id
                           select t).ToArray().FirstOrDefault();

                    if (cd == null)
                    {
                        var response = client.Send(request);
                        response.EnsureSuccessStatusCode();

                        string responseContent = response.Content.ReadAsStringAsync().Result;
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
                        Console.WriteLine("Consulta  QuodScore concluida no cliente" + Id);
                    }
                    Console.WriteLine("Score presente em nossa base de dados, no cliente " + Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                using (var context = new MyDbContext())
                {
                    var Log = context.LogsErroRotina.Add(new TableLogErrorRotina
                    {
                        Documento =cnpj,
                        Consulta = "ScoreQuodPJ",
                        DataConsulta = DateTime.UtcNow,
                        Erro = ex.Message

                    }).Entity;

                    context.SaveChanges();
                }

            }
        }

        public void GetQuodPendenciasPJRotina(string cnpj, long Id)
        {

            TablePendenciasQuod tablePendenciasQuod = new TablePendenciasQuod();
            var r = tablePendenciasQuod;

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

                var response = client.Send(request);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.ReasonPhrase);
                }

                string responseContent = response.Content.ReadAsStringAsync().Result;
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
                                r = context.pendenciasQuod.Add(new TablePendenciasQuod
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
                                r = context.pendenciasQuod.Add(new TablePendenciasQuod
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
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Não existem pendencias referentes cliente " + Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                using (var context = new MyDbContext())
                {
                    var Log = context.LogsErroRotina.Add(new TableLogErrorRotina
                    {
                        Documento = cnpj,
                        Consulta = "QuodPendenciasPJ",
                        DataConsulta = DateTime.UtcNow,
                        Erro = ex.Message

                    }).Entity;

                    context.SaveChanges();
                }
            }
        }
    }
}

