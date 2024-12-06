using CRMAudax.Controllers;
using Quartz;
using System.Configuration;

namespace CRMAudax.Job
{
    public class NotificacaoJob : IJob
    {
        private readonly IHttpClientFactory _clientFactory;

        public NotificacaoJob(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            if (configuration.GetSection("RotinaNotificacao")["Ativo"] == "true")
            {
                Console.WriteLine("Rotina Iniciada");

                NotificacaoController notificacaoController = new NotificacaoController(_clientFactory);

                notificacaoController.RotinaDeConsultas();

                Console.WriteLine("Rotina Finalizada");
            }           
        }
    }
}
