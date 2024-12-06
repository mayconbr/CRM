using System.Diagnostics;
using System.ServiceProcess;
using CRMAudax.Models;
using CRMAudax.Tools;
using RotinaConsulta.Functions;
using CRMAudax;
using Microsoft.Extensions.DependencyInjection;
using static CRMAudax.Models.AuxQuod;
using System.Linq;
using System;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ConsoleToService
{
    public class MyService : ServiceBase
    {
        private bool _finish = false;
        private bool _isRunning = false;
        private EventLog _eventLog;
        private Thread _workerThread;
        private IHttpClientFactory _clientFactory;

        public bool IsConsoleMode { get; set; }

        public MyService(bool isConsoleMode = false)
        {
            IsConsoleMode = isConsoleMode;
            this.ServiceName = "RotinaAtualizacaoCRM";

            if (!IsConsoleMode)
            {
                _eventLog = new EventLog();
                if (!EventLog.SourceExists(this.ServiceName))
                {
                    EventLog.CreateEventSource(this.ServiceName, "Application");
                }
                _eventLog.Source = this.ServiceName;
                _eventLog.Log = "Application";
            }
        }

        // Método público para iniciar o serviço no modo console
        public void StartService(string[] args)
        {
            OnStart(args);  // Chama o OnStart protegido internamente
        }

        // Método público para parar o serviço no modo console
        public void StopService()
        {
            OnStop();  // Chama o OnStop protegido internamente
        }

        protected override void OnStart(string[] args)
        {
            WriteMessage("RotinaAtualizacaoCRM started.");
            _isRunning = true;

            // Inicia o thread de trabalho
            _workerThread = new Thread(WorkerFunction);
            _workerThread.Start();
        }

        protected override void OnStop()
        {
            WriteMessage("RotinaAtualizacaoCRM stopped.");
            _isRunning = false;

            // Espera o thread de trabalho terminar
            if (_workerThread != null && _workerThread.IsAlive)
            {
                _workerThread.Join();
            }
        }

        public async Task<string> GetPublicIpAsync()
        {
            try
            {
                var cliente = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.ipify.org");
                var response = await cliente.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var ip = await response.Content.ReadAsStringAsync();
                WriteMessage(ip, EventLogEntryType.Information);
                return ip;
            }
            catch (HttpRequestException ex)
            {
                WriteMessage($"Erro ao obter o IP público: {ex.Message}", EventLogEntryType.Information);
                return null;
            }
        }

        private async void WorkerFunction()
        {
            try
            {
                while (_isRunning)
                {
                    if (DateTime.UtcNow.Day == 04)
                    {
                        int ExecuteDay = 04;
                        int ExecuteHour = 8;

                        WriteMessage("RotinaAtualizacaoCRM dia " + ExecuteDay.ToString() + " detectado, verificando IP");

                        string ipPublico = GetPublicIpAsync().Result;

                        if (ipPublico == "177.92.203.86")
                        {
                            WriteMessage("RotinaAtualizacaoCRM verificará a condição, condição " + _finish);

                            if (DateTime.UtcNow.Day == ExecuteDay && DateTime.UtcNow.Hour >= ExecuteHour && !_finish)
                            {
                                WriteMessage("RotinaAtualizacaoCRM iniciou no dia " + ExecuteDay.ToString() + " às " + DateTime.Now.ToString());

                                // Código da rotina principal
                                try
                                {
                                    await ExecutaRotina(); 
                                }
                                catch (Exception ex)
                                {
                                    //throw new Exception(ex.Message);
                                }

                            }
                            else if (DateTime.UtcNow.Day != ExecuteDay)
                            {
                                WriteMessage("Nao vai rodar de novo");
                                _finish = false;
                            }
                            else
                            {
                                if (DateTime.UtcNow.Day == ExecuteDay && _finish == true)
                                {
                                    WriteMessage("Hoje já rodou!");
                                }
                                else
                                {
                                    WriteMessage("Hoje não roda!");
                                }
                            }

                            // Espera por um segundo antes de executar novamente
                            Thread.Sleep(10000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteMessage("Error: " + ex.Message, EventLogEntryType.Error);
            }
        }


        public async Task ExecutaRotina()
        {
            string ConsultaAtual = "";
            try
            {
                //BMP bmp = new BMP();
                //var token = bmp.GetTokenBancoBMP();

                WriteMessage("RotinaAtualizacaoCRM em execução " + DateTime.Now.ToString());

                WriteMessage("RotinaAtualizacaoCRM iniciou e sera mandado o email " + DateTime.Now.ToString());

                // Adicione aqui a lógica do processo fornecido
                EmailTask.SendFormatedMail("INICIO DE ROTINA",
                "TI - GPAUDAX", "mbruzolato@gmail.com",
                "HORARIO DE INICIO DO SERVIDOR:" + DateTime.Now,
                false
                );

                using (var context = new MyDbContext())
                {
                    List<string> listcnpj = new List<string>();

                    //listcnpj.Add("22.414.848/0001-08");
                    //listcnpj.Add("27.080.279/0001-17");
                    //listcnpj.Add("04.055.900/0001-97");

                    //listcnpj.Add("35.413.063/0001-54");
                    //listcnpj.Add("41.330.142/0001-02");
                    //listcnpj.Add("41.823.919/0001-62");

                    //listcnpj.Add("37.227.902/0001-75");
                    //listcnpj.Add("17.866.400/0001-87");
                    //listcnpj.Add("40.669.108/0001-96");

                    var cnpjs = (from t in context.Clientes
                                 where t.DataDelete == null
                                 where t.tipo != "Sacado"
                                 where t.status == true
                                 where t.cpfCnpj.Length == 18
                                 where !listcnpj.Contains(t.cpfCnpj)
                                 select new TableCliente
                                 {
                                     Id = t.Id,
                                     cpfCnpj = t.cpfCnpj,
                                 }).ToArray();

                    WriteMessage(cnpjs.Count() + " CNPJs encontrados");
                    foreach (var cnpj in cnpjs)
                    {
                        WriteMessage(cnpj.cpfCnpj);
                    }


                    List<string> listcpf = new List<string>();

                    //listcpf.Add("030.001.136-90");
                    //listcpf.Add("122.320.156-29");
                    //listcpf.Add("082.768.056-20");

                    //listcpf.Add("110.370.576-86");
                    //listcpf.Add("096.506.286-49");
                    //listcpf.Add("041.268.956-12");

                    //listcpf.Add("046.031.896-99");
                    //listcpf.Add("313.954.146-53");
                    //listcpf.Add("396.513.126-53");

                    //listcpf.Add("125.576.206-37");
                    //listcpf.Add("013.794.256-70");
                    //listcpf.Add("257.078.598-96");


                    var cpfs = (from t in context.Clientes
                                where t.DataDelete == null
                                where t.tipo != "Sacado"
                                where t.status == true
                                where t.cpfCnpj.Length == 14
                                where !listcpf.Contains(t.cpfCnpj)
                                select new TableCliente
                                {
                                    Id = t.Id,
                                    cpfCnpj = t.cpfCnpj,
                                }).ToArray();

                    WriteMessage(cpfs.Count() + " CPFs encontrados");

                    foreach (var cpf in cpfs)
                    {
                        WriteMessage(cpf.cpfCnpj);
                    }

                    var InicioRotina = context.StatusRotinas.Add(new TableStatusRotina
                    {
                        DataInicio = DateTime.Now,
                        QtdCpf = cpfs.Count(),
                        QtdCnpj = cnpjs.Count(),
                        QuodProtestosPJuridica = 0,
                        QuodProtestosPFisica = 0,
                        QuodPendenciasPJuiridica = 0,
                        QuodPendenciasPFisica = 0,
                        QuodScorePJuridica = 0,
                        QuodScorePFisica = 0,
                        DecisaoProtestosPJuridica = 0,
                        DecisaoProtestosPFisica = 0,
                        DecisaoPendenciasPJuridica = 0,
                        DecisaoPendenciasPFisica = 0,
                        DecisaoScorePJuridica = 0,
                        DecisaoScorePFisica = 0,
                        ConsultaCNPJ = 0
                    }).Entity;

                    context.SaveChanges();

                    WriteMessage("Rotina criada no banco");

                    var StatusRotina = (from t in context.StatusRotinas
                                        where t.Id == InicioRotina.Id
                                        select t).FirstOrDefault();

                    var QuodProtestosPFisica = 0;
                    var QuodPendenciasPFisica = 0;
                    var QuodScorePFisica = 0;
                    var DecisaoProtestosPFisica = 0;
                    var DecisaoPendenciasPFisica = 0;
                    var DecisaoScorePFisica = 0;

                    var services1 = new ServiceCollection();
                    services1.AddHttpClient();
                    var serviceProvider = services1.BuildServiceProvider();

                    _clientFactory = serviceProvider.GetService<IHttpClientFactory>();                 

                    foreach (var cpf in cpfs)
                    {
                        var CpfReplace = cpf.cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                        ConsultaAtual = CpfReplace;

                        if (StatusRotina != null)
                        {
                            try
                            {
                                WriteMessage("Início da consulta ScoreQuod PF " + DateTime.Now);
                                Quod quod1 = new Quod(_clientFactory);
                                quod1.GetQuodScoreRotina(CpfReplace, cpf.Id);
                                StatusRotina.QuodScorePFisica = QuodScorePFisica++;
                                context.SaveChanges();                                
                            }
                            finally
                            {
                                WriteMessage("Fim da consulta ScoreQuod PF");
                            }


                            try
                            {
                                WriteMessage("Início da consulta Pendencias Quod PF " + DateTime.Now); //aqui 75 colocar thow
                                Quod quod2 = new Quod(_clientFactory);
                                quod2.GetPendenciasQuodPFRotina(CpfReplace, cpf.Id);
                                StatusRotina.QuodPendenciasPFisica = QuodPendenciasPFisica++;
                                context.SaveChanges();                            
                            }
                            finally
                            {
                                WriteMessage("Fim da consulta Pendencias Quod PF " + DateTime.Now);
                            }

                            try
                            {

                                WriteMessage("Início da consulta ScoreDecisao PF " + DateTime.Now);
                                Decisao decisao1 = new Decisao(_clientFactory);
                                await decisao1.GetDecisaoScoreRotina(CpfReplace, cpf.Id);
                                StatusRotina.DecisaoScorePFisica = DecisaoScorePFisica++;
                                context.SaveChanges();
                            }
                            finally
                            {
                                WriteMessage("Fim da consulta ScoreDecisao PF " + DateTime.Now);
                            }

                            try
                            {
                                WriteMessage("Início da consulta Pendencia Decisao PF " + DateTime.Now);
                                Decisao decisao2 = new Decisao(_clientFactory);
                                decisao2.GetDecisaoPendenciasPFRotina(CpfReplace, cpf.Id);
                                StatusRotina.DecisaoPendenciasPFisica = DecisaoPendenciasPFisica++;
                                context.SaveChanges();                                

                            }
                            finally
                            {
                                WriteMessage("Fim da consulta Pendencia Decisao PF " + DateTime.Now);
                            }
                        }
                    }

                    var QuodProtestosPJuridica = 0;
                    var QuodPendenciasPJuiridica = 0;
                    var QuodScorePJuridica = 0;
                    var DecisaoProtestosPJuridica = 0;
                    var DecisaoPendenciasPJuridica = 0;
                    var DecisaoScorePJuridica = 0;

                    foreach (var cnpj in cnpjs)
                    {
                        var CnpjReplace = cnpj.cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                        ConsultaAtual = CnpjReplace;

                        try
                        {
                            WriteMessage("Início da consulta API Brasil " + DateTime.Now);
                            BrasilAPI brasilAPI = new BrasilAPI();
                            brasilAPI.BrasilAPICNPJ(cnpj.cpfCnpj, cnpj.Id);
                            context.SaveChanges();                            
                        }
                        finally
                        {
                            WriteMessage("Fim da consulta API Brasil " + DateTime.Now);
                        }

                        try
                        {
                            WriteMessage("Início da consulta ScoreQuod PJ" + DateTime.Now);
                            Quod quod1 = new Quod(_clientFactory);
                            quod1.GetQuodScorePJRotina(CnpjReplace, cnpj.Id);
                            StatusRotina.QuodScorePJuridica = QuodScorePJuridica++;
                            context.SaveChanges();                           
                        }
                        finally
                        {
                            WriteMessage("Fim da consulta ScoreQuod PJ" + DateTime.Now);
                        }

                        try
                        {
                            WriteMessage("Início da consulta Pendencias Quod PJ " + DateTime.Now);
                            Quod quod2 = new Quod(_clientFactory);
                            quod2.GetQuodPendenciasPJRotina(CnpjReplace, cnpj.Id);
                            StatusRotina.DecisaoPendenciasPJuridica = DecisaoPendenciasPJuridica++;
                            context.SaveChanges();
                        }
                        finally
                        {
                            WriteMessage("Fim da consulta Pendencias Quod PJ " + DateTime.Now);
                        }

                        try
                        {
                            WriteMessage("Início da consulta ScoreDecisao PJ" + DateTime.Now);
                            Decisao decisao1 = new Decisao(_clientFactory);
                            decisao1.GetDecisaoScoreProtestosPJRotina(CnpjReplace, cnpj.Id);
                            StatusRotina.DecisaoScorePJuridica = DecisaoScorePJuridica++;
                            context.SaveChanges();
                        }
                        finally
                        {
                            WriteMessage("Fim da consulta ScoreDecisao PJ" + DateTime.Now);
                        }

                        try
                        {
                            WriteMessage("Início da consulta Pendencias Decisao PJ " + DateTime.Now);
                            Decisao decisao2 = new Decisao(_clientFactory);
                            decisao2.GetDecisaoPendenciasPJRotina(CnpjReplace, cnpj.Id);
                            StatusRotina.DecisaoPendenciasPJuridica = DecisaoPendenciasPJuridica++;
                            context.SaveChanges();
                        }
                        finally
                        {
                            WriteMessage("Fim da consulta Pendencias Decisao PJ " + DateTime.Now); 
                        }
                    }

                    var clientes = (from t in context.Clientes
                                    where t.DataDelete == null
                                    where t.status == true
                                    select new TableCliente
                                    {
                                        Id = t.Id,
                                        cpfCnpj = t.cpfCnpj,
                                    }).ToArray();

                    foreach (var cliente in clientes)
                    {
                        var clientesReplace = cliente.cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                        if (cliente.tipo != "Sacado")
                        {
                            WriteMessage("Sacado ignorado para consulta " + cliente.Id);
                        }
                    }


                    if (StatusRotina != null)
                    {
                        StatusRotina.DataFinal = DateTime.Now;
                        context.SaveChanges();
                    }

                    EmailTask.SendFormatedMail("FINAL DE ROTINA",
                        "TI - GPAUDAX", "mbruzolato@gmail.com",
                        "HORARIO DE INICIO DO SERVIDOR:" + DateTime.Now.ToString(),
                        false
                    );

                    WriteMessage("Rotina executada com sucesso, base de dados atualizada em todas as APIs");
                }
                _finish = true;
                EmailTask.SendFormatedMail("FINAL DE ROTINA",
                                           "TI - GPAUDAX", "mbruzolato@gmail.com",
                                           "HORARIO DE INICIO DO SERVIDOR: " + DateTime.Now,
                                           false
                );
            }
            catch (Exception ex)
            {
                _finish = true;
                WriteMessage("Erro CPF/CMPJ: " + ConsultaAtual + "-" + ex.Message, EventLogEntryType.Error);
                EmailTask.SendFormatedMail("FINAL DE ROTINA - COM ERRO",
                                           "TI - GPAUDAX", "mbruzolato@gmail.com",
                                           "HORARIO DE INICIO DO SERVIDOR: " + DateTime.Now + " - Erro: " + ex.Message + " - Consulta Atual " + ConsultaAtual,
                                           false
                );
            }
        }


        void WriteMessage(string message, EventLogEntryType type = EventLogEntryType.Information)
        {
            if (IsConsoleMode)
            {
                // Exibe a mensagem no console
                Console.WriteLine(message);
            }
            else
            {
                // Grava no Event Viewer
                _eventLog?.WriteEntry(message, type);
            }
        }
    }

    static class Program
    {
        static bool IsConsoleMode { get; set; }

        static void Main(string[] args)
        {
            #if DEBUG
            IsConsoleMode = true;
            Console.WriteLine("Executando em modo de console no ambiente de debug.");
            #else
            IsConsoleMode = args.Contains("--console");
            #endif

            if (IsConsoleMode)
            {
                Console.WriteLine("Aplicação em modo Console.");
                var service = new MyService(isConsoleMode: true);
                service.StartService(args); 
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new MyService() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }

  
}
