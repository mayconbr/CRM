using CRMAudax.Models;
using CRMAudax;
using Newtonsoft.Json;

namespace RotinaConsulta.Functions
{
    internal class BrasilAPI
    {
        public void BrasilAPICNPJ(string CNPJ, long Id)
        {
            TableCliente tableCliente = new TableCliente();
            var cliente = tableCliente;

            try
            {
                string CNPJReplace = CNPJ.Replace(".", "").Replace("-", "").Replace("/", "");

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://brasilapi.com.br/api/cnpj/v1/" + CNPJReplace);
                var response = client.Send(request);
                response.EnsureSuccessStatusCode();

                string responseContent = response.Content.ReadAsStringAsync().Result;
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                string situacao = jsonResponse.descricao_situacao_cadastral;

                using (var context = new MyDbContext())
                {
                     cliente = (from t in context.Clientes
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
                using (var context = new MyDbContext())
                {
                    var Log = context.LogsErroRotina.Add(new TableLogErrorRotina
                    {
                        Documento = CNPJ,
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
