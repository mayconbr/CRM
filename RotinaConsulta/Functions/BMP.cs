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
        //        request.Headers.Add("IdempotencyKey", "6c490988-8d84-46dd-873e-78053e1dad1a");
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
