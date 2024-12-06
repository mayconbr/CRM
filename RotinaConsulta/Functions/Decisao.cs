using CRMAudax.Models;
using CRMAudax;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace RotinaConsulta.Functions
{
    public class Decisao
    {
        private readonly HttpClient _httpClient;

        public Decisao(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient();
        }

        public async Task GetDecisaoScoreRotina(string cpf, long Id)
        {

            TableDecisaoScore tableDecisaoScore = new TableDecisaoScore();
            var r = tableDecisaoScore;

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
                        var response =client.Send(request);
                        response.EnsureSuccessStatusCode();

                        string responseContent = response.Content.ReadAsStringAsync().Result;
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

                         r = context.DecisaoScores.Add(new TableDecisaoScore
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
                    Console.WriteLine("Consulta  DecisaoScore concluida  no cliente de id" + Id);
                }
            }
            catch (Exception ex)
            {
                using (var context = new MyDbContext())
                {
                    var Log = context.LogsErroRotina.Add(new TableLogErrorRotina
                    {
                        Documento = cpf,
                        Consulta = "DecisaoScorePF",
                        DataConsulta = DateTime.UtcNow,
                        Erro = ex.Message

                    }).Entity;

                    context.SaveChanges();
                }
            }
        }

        public void GetDecisaoPendenciasPFRotina(string cpf, long Id)
        {
            TablePendenciasDecisao tablePendenciasDecisao = new TablePendenciasDecisao();
            var d = tablePendenciasDecisao;
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Get, "https://consulta.distribuidor.digital/api/consulta?logon=565306&senha=gpaudax03api&consulta=73&tipo_documento=F&documento=" + cpf);

                using (var context = new MyDbContext())
                {

                    var response = client.Send(request);
                    response.EnsureSuccessStatusCode();

                    string responseContent = response.Content.ReadAsStringAsync().Result;
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

                                d = context.pendenciasDecisao.Add(new TablePendenciasDecisao
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

                                d = context.pendenciasDecisao.Add(new TablePendenciasDecisao
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

                                    d = context.pendenciasDecisao.Add(new TablePendenciasDecisao
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

                        Console.WriteLine("Pendencias no sistema Decisão consultados no cliente " + Id);
                    }
                    else
                    {
                        Console.WriteLine("Não existem pendencias referentes ao cliente" + Id);
                    }
                }
            }
            catch (Exception ex)
            {
                using (var context = new MyDbContext())
                {
                    var Log = context.LogsErroRotina.Add(new TableLogErrorRotina
                    {
                        Documento = cpf,
                        Consulta = "DecisaoPendenciasPF",
                        DataConsulta = DateTime.UtcNow,
                        Erro = ex.Message

                    }).Entity;

                    context.SaveChanges();
                }
            }
        }

        public void GetDecisaoScoreProtestosPJRotina(string cnpj, long Id)
        {
            TableDecisaoScore tableDecisaoScore = new TableDecisaoScore();
            var cd = tableDecisaoScore;

            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Get, "https://consulta.distribuidor.digital/api/consulta?logon=565306&senha=gpaudax03api&consulta=75&tipo_documento=J&documento=" + cnpj);

                using (var context = new MyDbContext())
                {
                    cd = (from t in context.DecisaoScores
                          where t.DataScore == DateTime.UtcNow
                          where t.ClienteId == Id
                          select t).ToArray().FirstOrDefault();

                    if (cd == null)
                    {

                        var response = client.Send(request);
                        response.EnsureSuccessStatusCode();

                        string responseContent = response.Content.ReadAsStringAsync().Result;
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
                            Console.WriteLine("Score vazio na base decisão no cliente " + Id);
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

                                var n = context.Notificacoes.Add(new TableNotificacao
                                {
                                    DataNotificacao = DateTime.UtcNow,
                                    ClienteId = Id,
                                    Informacao = "Protesto no sistema Decisão detectado",

                                }).Entity;
                                context.SaveChanges();
                            }
                        }
                        Console.WriteLine("Consulta  DecisãoScore e protestos concluida no cliente de id" + Id);
                    }
                    Console.WriteLine("Score presente em nossa base de dados, no cliente " + Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Não foi possivel atualiza o score Decisao e protestos, no cliente " + Id);

                using (var context = new MyDbContext())
                {
                    var Log = context.LogsErroRotina.Add(new TableLogErrorRotina
                    {
                        Documento = cnpj,
                        Consulta = "QuodScoreProtestosPJ",
                        DataConsulta = DateTime.UtcNow,
                        Erro = ex.Message

                    }).Entity;

                    context.SaveChanges();

                }
            }
        }

        public void GetDecisaoPendenciasPJRotina(string cnpj, long Id)
        {
            TablePendenciasDecisao tablePendenciasDecisao = new TablePendenciasDecisao();
            var cd = tablePendenciasDecisao;
            try
            {
                var client = _httpClient;

                var request = new HttpRequestMessage(HttpMethod.Get, "https://consulta.distribuidor.digital/api/consulta?logon=565306&senha=gpaudax03api&consulta=72&tipo_documento=J&documento=" + cnpj);

                using (var context = new MyDbContext())
                {

                    var response = client.Send(request);
                    response.EnsureSuccessStatusCode();

                    string responseContent = response.Content.ReadAsStringAsync().Result;
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
                        Console.WriteLine("Pendencias no sistema decisão consultados, cliente " + Id);
                    }
                    else
                    {
                       Console.WriteLine("Não existem pendencias referentes a esse documento");
                    }
                }
            }
            catch (Exception ex)
            {
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
