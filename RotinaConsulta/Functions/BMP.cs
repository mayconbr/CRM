using CRMAudax.Models;
using CRMAudax;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace RotinaConsulta.Functions
{
    public class BMP
    {
        public async Task<string> GetTokenBancoBMP()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://auth.moneyp.com.br/connect/token");
                request.Headers.Add("Cache-Control", "no-cache");
                var collection = new List<KeyValuePair<string, string>>();
                collection.Add(new("grant_type", "client_credentials"));
                collection.Add(new("client_id", ""));
                collection.Add(new("scope", "bmp.digital.api.full.access"));
                collection.Add(new("client_assertion", ""));
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

                    //ConsultaSCR();

                    return (accessToken);
                }
                else
                {
                    return ("Erro em autenticar no banco BMP");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

   
        //public async Task<string> ConsultaSCR(string token)
        //{
        //    try
        //    {
        //        var data = DateTime.UtcNow.AddMonths(-2);
        //        int mes = data.Month;
        //        int ano = data.Year;

        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.bmpdigital.moneyp.com.br/Bureau/MultiConsultaSCR");
        //        request.Headers.Add("IdempotencyKey", "");
        //        request.Headers.Add("Authorization", "Bearer " + token);
        //        var content = new StringContent($"{{\"consulta\": {{\"documento\": \"{cpfcnpj}\",\"dataBaseMes\": \"{mes}\",\"dataBaseAno\": \"{ano}\"}}}}", null, "application/json");
        //        request.Content = content;
        //        var response = await client.SendAsync(request);
        //        response.EnsureSuccessStatusCode();
        //        string responseContent = await response.Content.ReadAsStringAsync();
        //        return responseContent;


        //        dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

        //        string codigoDoCliente = jsonResponse.resumoDoCliente.codigoDoCliente;
        //        string dataBaseConsultada = jsonResponse.resumoDoCliente.dataBaseConsultada;
        //        string dataInicioRelacionamento = jsonResponse.resumoDoCliente.dataInicioRelacionamento;

        //        string carteiraVencerAte30diasVencidosAte14dias = jsonResponse.resumoDoClienteTraduzido.carteiraVencerAte30diasVencidosAte14dias;
        //        string carteiraVencer31a60dias = jsonResponse.resumoDoClienteTraduzido.carteiraVencer31a60dias;
        //        string carteiraVencer61a90dias = jsonResponse.resumoDoClienteTraduzido.carteiraVencer61a90dias;
        //        string carteiraVencer91a180dias = jsonResponse.resumoDoClienteTraduzido.carteiraVencer91a180dias;
        //        string carteiraVencer181a360dias = jsonResponse.resumoDoClienteTraduzido.carteiraVencer181a360dias;
        //        string carteiraVencerPrazoIndeterminado = jsonResponse.resumoDoClienteTraduzido.carteiraVencerPrazoIndeterminado;
        //        string responsabilidadeTotal = jsonResponse.resumoDoClienteTraduzido.responsabilidadeTotal;
        //        string creditosaLiberar = jsonResponse.resumoDoClienteTraduzido.creditosaLiberar;
        //        string limitesdeCredito = jsonResponse.resumoDoClienteTraduzido.limitesdeCredito;
        //        string riscoTotal = jsonResponse.resumoDoClienteTraduzido.riscoTotal;

        //        string qtdeOperacoesDiscordancia = jsonResponse.resumoDoClienteTraduzido.qtdeOperacoesDiscordancia;
        //        string vlrOperacoesDiscordancia = jsonResponse.resumoDoClienteTraduzido.vlrOperacoesDiscordancia;
        //        string qtdeOperacoesSobJudice = jsonResponse.resumoDoClienteTraduzido.qtdeOperacoesSobJudice;
        //        string vlrOperacoesSobJudice = jsonResponse.resumoDoClienteTraduzido.vlrOperacoesSobJudice;

        //        string carteiraVencido = jsonResponse.resumoDoClienteTraduzido.carteiraVencido;
        //        string carteiraVencer = jsonResponse.resumoDoClienteTraduzido.carteiraVencer;
           

        //        using (var context = new MyDbContext())
        //        {
        //            var r = context.endividamentoSCR.Add(new TableEndividamentoSCR
        //            {
        //                ClienteId = Id,
        //                codigoDoCliente = codigoDoCliente,
        //                dataBaseConsultada = dataBaseConsultada,
        //                dataInicioRelacionamento = dataInicioRelacionamento,
        //                carteiraVencerAte30diasVencidosAte14dias = carteiraVencerAte30diasVencidosAte14dias,
        //                carteiraVencer31a60dias = carteiraVencer31a60dias,
        //                carteiraVencer61a90dias = carteiraVencer61a90dias,
        //                carteiraVencer91a180dias = carteiraVencer91a180dias,
        //                carteiraVencer181a360dias = carteiraVencer181a360dias,
        //                carteiraVencerPrazoIndeterminado = carteiraVencerPrazoIndeterminado,
        //                responsabilidadeTotal = responsabilidadeTotal,
        //                creditosaLiberar = creditosaLiberar,
        //                limitesdeCredito = limitesdeCredito,
        //                riscoTotal = riscoTotal,
        //                qtdeOperacoesDiscordancia = qtdeOperacoesDiscordancia,
        //                vlrOperacoesDiscordancia = vlrOperacoesDiscordancia,
        //                qtdeOperacoesSobJudice = qtdeOperacoesSobJudice,
        //                vlrOperacoesSobJudice = vlrOperacoesSobJudice,

        //                DataConsulta = DateTime.UtcNow,
        //                carteiraVencido = carteiraVencido,
        //                carteiraVencer = carteiraVencer

        //            }).Entity;
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //    return ("Endividamento consultado com sucesso no banco BMP");
        //}


    }
}
