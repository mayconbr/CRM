using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using CRMAudax.Models;
using System.Dynamic;

namespace CRMAudax.Controllers
{
    public class RelatorioController : Controller
    {

        public IActionResult Relatorios()
        {
            return View();
        }
        public IActionResult GerenciaPJ()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Documentos = RelatorioDocumentosPJ();
            return View(mymodel);
        }
        public IActionResult GerenciaPF()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Documentos = RelatorioDocumentosPF();
            return View(mymodel);
        }

        [HttpGet]
        [Route("~/RelatorioDocumentosPJ")]
        public IEnumerable<CRMAudax.Models.AuxCliente> RelatorioDocumentosPJ()
        {
            using (var context = new MyDbContext())
            {
                var documentos = (from cliente in context.Clientes

                                  join contrato in context.ContratosSociais on cliente.Id equals contrato.ClienteId into clienteContratos
                                  from contrato in clienteContratos.DefaultIfEmpty()

                                  join endereco in context.ComprovantesEnderecos on cliente.Id equals endereco.ClienteId into clienteEnderecos
                                  from endereco in clienteEnderecos.DefaultIfEmpty()

                                  join cartao in context.CartoesCNPJ on cliente.Id equals cartao.ClienteId into clienteCartoes
                                  from cartao in clienteCartoes.DefaultIfEmpty()

                                  join faturamento in context.Faturamentos on cliente.Id equals faturamento.ClienteId into clientesFaturamentos
                                  from faturamento in clientesFaturamentos.DefaultIfEmpty()

                                  join fiscal in context.FaturamentosFiscais on cliente.Id equals fiscal.ClienteId into clientesFiscais
                                  from fiscal in clientesFiscais.DefaultIfEmpty()

                                  join documento in context.CnpjCpfs on cliente.Id equals documento.ClienteId into clientesDocumento
                                  from documento in clientesDocumento.DefaultIfEmpty()

                                  join Declaracao in context.DeclaracoesCredito on cliente.Id equals Declaracao.ClienteId into clientesDeclaracao
                                  from declaracao in clientesDeclaracao.DefaultIfEmpty()

                                  where cliente.tipoPessoa == 2
                                  where cliente.tipo != "Sacado"
                                  select new
                                  {
                                      cliente.Id,
                                      cliente.nome,
                                      cliente.tipo,
                                      cliente.Regiao,
                                      TemContrato = contrato != null,
                                      TemEndereco = endereco != null,
                                      TemCartaoCnpj = cartao != null,
                                      TemFaturamento = faturamento != null,
                                      TemFiscal = fiscal != null,
                                      TemDocumento = documento != null,
                                      TemDeclaracao = declaracao != null,
                                  }).ToArray();

                var groupedDocumentos = documentos.GroupBy(d => d.Id).Select(g => new AuxCliente
                {
                    nome = g.First().nome,
                    tipo = g.First().tipo,
                    Regiao = g.First().Regiao,
                    TemContrato = g.Any(x => x.TemContrato),
                    TemEndereco = g.Any(x => x.TemEndereco),
                    TemCartaoCnpj = g.Any(x => x.TemCartaoCnpj),
                    TemFaturamento = g.Any(x => x.TemFaturamento),
                    TemFiscal = g.Any(x => x.TemFiscal),
                    TemDocumento = g.Any(x => x.TemDocumento),
                    TemDeclaracao = g.Any(x => x.TemDeclaracao),
                    TotalTrueVariables = new[] { g.Any(x => x.TemDeclaracao), g.Any(x => x.TemDocumento), g.Any(x => x.TemContrato), g.Any(x => x.TemEndereco), g.Any(x => x.TemCartaoCnpj), g.Any(x => x.TemFaturamento), g.Any(x => x.TemFiscal) }.Count(t => t)
                });

                var sortedDocumentos = groupedDocumentos.OrderBy(d => d.TotalTrueVariables);

                return sortedDocumentos.ToArray();
            }
        }



        [HttpGet]
        [Route("~/RelatorioDocumentosPF")]
        public IEnumerable<CRMAudax.Models.AuxCliente> RelatorioDocumentosPF()
        {
            using (var context = new MyDbContext())
            {
                var documentos = (from cliente in context.Clientes

                                  join contrato in context.ContratosSociais on cliente.Id equals contrato.ClienteId into clienteContratos
                                  from contrato in clienteContratos.DefaultIfEmpty()

                                  join endereco in context.ComprovantesEnderecos on cliente.Id equals endereco.ClienteId into clienteEnderecos
                                  from endereco in clienteEnderecos.DefaultIfEmpty()

                                  join cartao in context.CartoesCNPJ on cliente.Id equals cartao.ClienteId into clienteCartoes
                                  from cartao in clienteCartoes.DefaultIfEmpty()

                                  join faturamento in context.Faturamentos on cliente.Id equals faturamento.ClienteId into clientesFaturamentos
                                  from faturamento in clientesFaturamentos.DefaultIfEmpty()

                                  join fiscal in context.FaturamentosFiscais on cliente.Id equals fiscal.ClienteId into clientesFiscais
                                  from fiscal in clientesFiscais.DefaultIfEmpty()

                                  join imposto in context.ImpostosRenda on cliente.Id equals imposto.ClienteId into clientesImposto
                                  from imposto in clientesImposto.DefaultIfEmpty()

                                  join documento in context.CnpjCpfs on cliente.Id equals documento.ClienteId into clientesDocumento
                                  from documento in clientesDocumento.DefaultIfEmpty()

                                  join dap in context.ArquivosDAP on cliente.Id equals dap.ClienteId into clientesDap
                                  from dap in clientesDap.DefaultIfEmpty()

                                  join Declaracao in context.DeclaracoesCredito on cliente.Id equals Declaracao.ClienteId into clientesDeclaracao
                                  from Declaracao in clientesDeclaracao.DefaultIfEmpty()

                                  where cliente.tipoPessoa == 1
                                  where cliente.tipo != "Sacado"
                                  select new
                                  {
                                      cliente.Id,
                                      cliente.nome,
                                      cliente.tipo,
                                      cliente.Regiao,
                                      TemEndereco = endereco != null,
                                      TemFaturamento = faturamento != null,
                                      TemFiscal = fiscal != null,
                                      TemImposto = imposto != null,
                                      TemDocumento = documento != null,
                                      TemDeclaracao = Declaracao != null,
                                      TemDAP = dap != null

                                  }).ToArray();

                var groupedDocumentos = documentos.GroupBy(d => d.Id).Select(g => new AuxCliente
                {
                    nome = g.First().nome,
                    tipo = g.First().tipo,
                    Regiao = g.First().Regiao,
                    TemEndereco = g.Any(x => x.TemEndereco),
                    TemFaturamento = g.Any(x => x.TemFaturamento),
                    TemFiscal = g.Any(x => x.TemFiscal),
                    TemImposto = g.Any(x => x.TemImposto),
                    TemDocumento = g.Any(x => x.TemDocumento),
                    TemDeclaracao = g.Any(x => x.TemDeclaracao),
                    TemDAP = g.Any(x => x.TemDAP),

                    TotalTrueVariables = new[] { g.Any(x => x.TemDAP), g.Any(x => x.TemDeclaracao), g.Any(x => x.TemEndereco), g.Any(x => x.TemFaturamento), g.Any(x => x.TemFiscal), g.Any(x => x.TemImposto), g.Any(x => x.TemDocumento) }.Count(t => t)
                });

                var sortedDocumentos = groupedDocumentos.OrderBy(d => d.TotalTrueVariables);

                return sortedDocumentos.ToArray();
            }
        }


        [HttpGet]
        [Route("~/RelatorioAssinaturaDigital/{inicio}/{final}")]
        public IActionResult RelatorioAssinaturaDigital(string inicio, string final)
        {
            using (var context = new MyDbContext())
            {

                var clientes = (from cliente in context.Clientes
                                where cliente.DataCadastro >= Convert.ToDateTime(inicio)
                                where cliente.DataCadastro <= Convert.ToDateTime(final)
                                select cliente).ToArray();


                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Securitizadora S.A.");

                    int row = 1;

                    var corAzulClaro = Color.FromArgb(28, 69, 135);


                    worksheet.Cells[row, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 12].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 13].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 15].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 16].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 18].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 19].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 20].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 21].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 22].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 23].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 24].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 26].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 27].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 28].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 29].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 30].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 31].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                    worksheet.Cells[row, 32].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 33].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 34].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                    worksheet.Cells[row, 1].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255)); // Define a cor da fonte como branco
                    worksheet.Cells[row, 2].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 3].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 4].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 5].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 6].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 7].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 8].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 9].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 10].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 11].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 12].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 13].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 14].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 15].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 16].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 17].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 18].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 19].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 20].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 21].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 22].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 23].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 24].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 25].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 26].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 27].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 28].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 29].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 30].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 31].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));

                    worksheet.Cells[row, 32].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 33].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    worksheet.Cells[row, 34].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));


                    for (int i = 1; i <= 34; i++)
                    {
                        worksheet.Column(i).Width = 25;
                    }

                    worksheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 4].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 5].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 6].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 7].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 8].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 9].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 10].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 11].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 12].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 13].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 14].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 15].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 16].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 17].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 18].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 19].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 20].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 21].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 22].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 23].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 24].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 25].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 26].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 27].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 28].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 29].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 30].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 31].Style.Fill.BackgroundColor.SetColor(corAzulClaro);

                    worksheet.Cells[row, 32].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 33].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    worksheet.Cells[row, 34].Style.Fill.BackgroundColor.SetColor(corAzulClaro);

                    worksheet.Cells[row, 1].Value = "CNPJ EMPRESA";
                    worksheet.Cells[row, 2].Value = "Nome Jurídico";
                    worksheet.Cells[row, 3].Value = "Nome Fantasia";
                    worksheet.Cells[row, 4].Value = "Tipo Pessoa";
                    worksheet.Cells[row, 5].Value = "CPF/CNPJ";
                    worksheet.Cells[row, 6].Value = "Inscrição Estadual";
                    worksheet.Cells[row, 7].Value = "Endereço completo";
                    worksheet.Cells[row, 8].Value = "Bairro";                       //bairro
                    worksheet.Cells[row, 9].Value = "Cidade";
                    worksheet.Cells[row, 10].Value = "Estado";
                    worksheet.Cells[row, 11].Value = "CEP";
                    worksheet.Cells[row, 12].Value = "Telefone";
                    worksheet.Cells[row, 13].Value = "Email";
                    worksheet.Cells[row, 14].Value = "Nome Socio 1";
                    worksheet.Cells[row, 15].Value = "Tipo Socio 1";
                    worksheet.Cells[row, 16].Value = "CPF/CNPJ 1";
                    worksheet.Cells[row, 17].Value = "Email Socio 1";
                    worksheet.Cells[row, 18].Value = "Celular Socio 1";
                    worksheet.Cells[row, 19].Value = "Função Socio 1";
                    worksheet.Cells[row, 20].Value = "Nome Socio 2";
                    worksheet.Cells[row, 21].Value = "Tipo Socio 2";
                    worksheet.Cells[row, 22].Value = "CPF/CNPJ 2";
                    worksheet.Cells[row, 23].Value = "Email Socio 2";
                    worksheet.Cells[row, 24].Value = "Celular Socio 2";
                    worksheet.Cells[row, 25].Value = "Função Socio 2";
                    worksheet.Cells[row, 26].Value = "Nome Socio 3";
                    worksheet.Cells[row, 27].Value = "Tipo Socio 3";
                    worksheet.Cells[row, 28].Value = "CPF/CNPJ 3";
                    worksheet.Cells[row, 29].Value = "Email Socio 3";
                    worksheet.Cells[row, 30].Value = "Celular Socio 3";
                    worksheet.Cells[row, 31].Value = "Função Socio 3";

                    worksheet.Cells[row, 32].Value = "Assinatura socio 1";
                    worksheet.Cells[row, 33].Value = "Assinatura socio 2";
                    worksheet.Cells[row, 34].Value = "Assinatura socio 3";


                    foreach (var item in clientes)
                    {
                        if (item.tipo == "Cedente")
                        {
                            row++;
                            worksheet.Cells[row, 1].Value = "44750853000116";
                            worksheet.Cells[row, 2].Value = item.razaoSocialCedente;
                            worksheet.Cells[row, 3].Value = item.nome;
                            worksheet.Cells[row, 5].Value = item.cpfCnpj.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                            worksheet.Cells[row, 6].Value = item.InscricaoEstadual != null ? item.InscricaoEstadual.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.InscricaoEstadual;
                            worksheet.Cells[row, 7].Value = item.rua + " " + item.numero + " " + item.complemento;
                            worksheet.Cells[row, 8].Value = item.bairro;
                            worksheet.Cells[row, 9].Value = item.cidade;
                            worksheet.Cells[row, 10].Value = item.estado;
                            worksheet.Cells[row, 11].Value = item.cep.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                            worksheet.Cells[row, 12].Value = item.telefone.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                            worksheet.Cells[row, 13].Value = item.email;
                            worksheet.Cells[row, 14].Value = item.nomeSocio;
                            worksheet.Cells[row, 16].Value = item.cpfSocio != null ? item.cpfSocio.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.cpfSocio;
                            worksheet.Cells[row, 17].Value = item.EmailSocio;
                            worksheet.Cells[row, 18].Value = item.celularSocio != null ? item.celularSocio.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.celularSocio;
                            worksheet.Cells[row, 20].Value = item.nomeSocioSegundo;
                            worksheet.Cells[row, 22].Value = item.cpfSocioSegundo != null ? item.cpfSocioSegundo.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.cpfSocioSegundo;
                            worksheet.Cells[row, 23].Value = item.EmailSegundo;
                            worksheet.Cells[row, 24].Value = item.celularSocioSegundo != null ? item.celularSocioSegundo.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.celularSocioSegundo;
                            worksheet.Cells[row, 26].Value = item.nomeSocioTerceiro;
                            worksheet.Cells[row, 28].Value = item.cpfSocioTerceiro != null ? item.cpfSocioTerceiro.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.cpfSocioTerceiro;
                            worksheet.Cells[row, 29].Value = item.EmailTerceiro;
                            worksheet.Cells[row, 30].Value = item.celularSocioTerceiro != null ? item.celularSocioTerceiro.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.celularSocioTerceiro;

                            if (item.tipoPessoa == 1)
                            {
                                worksheet.Cells[row, 4].Value = "F";
                            }
                            if (item.tipoPessoa == 2)
                            {
                                worksheet.Cells[row, 4].Value = "J";
                            }

                            //Tipo Pessoa socio 1
                            if (item.cpfSocio != null)
                            {
                                string valor = item.cpfSocio.ToString().Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                                if (valor.Length > 0 && valor.Length == 11)
                                {
                                    worksheet.Cells[row, 15].Value = "F";
                                }
                                if (valor.Length == 14)
                                {
                                    worksheet.Cells[row, 15].Value = "J";
                                }
                            }

                            //Tipo socio 2

                            if (item.cpfSocioSegundo != null)
                            {
                                string valor = item.cpfSocioSegundo.ToString().Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                                if (valor.Length > 0 && valor.Length == 11)
                                {
                                    worksheet.Cells[row, 21].Value = "F";
                                }
                                if (valor.Length == 14)
                                {
                                    worksheet.Cells[row, 21].Value = "J";
                                }
                            }

                            //Tipo Socio 3
                            if (item.cpfSocioTerceiro != null)
                            {
                                string valor = item.cpfSocioTerceiro.ToString().Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                                if (valor.Length > 0 && valor.Length == 11)
                                {
                                    worksheet.Cells[row, 27].Value = "F";
                                }
                                if (valor.Length == 14)
                                {
                                    worksheet.Cells[row, 27].Value = "J";
                                }
                            }

                            if (item.nomeSocioSegundo != null && item.nomeSocioSegundo.Length > 1)
                            {
                                worksheet.Cells[row, 25].Value = "REPRESENTANTE";
                            }
                            if (item.nomeSocioTerceiro != null && item.nomeSocioTerceiro.Length > 1)
                            {
                                worksheet.Cells[row, 31].Value = "REPRESENTANTE";
                            }
                            if (item.nomeSocio != null && item.nomeSocio.Length > 1)
                            {
                                worksheet.Cells[row, 19].Value = "AVALISTA, EMITENTE, ENDOSSANTE, REPRESENTANTE";
                            }

                            if(item.tipoPessoa != 2)
                            {
                                worksheet.Cells[row, 14].Value = item.nome;
                                worksheet.Cells[row, 15].Value = "F";
                                worksheet.Cells[row, 16].Value = item.cpfCnpj;
                                worksheet.Cells[row, 17].Value = item.email;
                                worksheet.Cells[row, 18].Value = item.telefone;
                                worksheet.Cells[row, 19].Value = "AVALISTA, EMITENTE, ENDOSSANTE, REPRESENTANTE";
                            }

                            //Assinatura socios

                            if(item.AssinaturaSocio != null)
                            {
                                worksheet.Cells[row, 32].Value = "Sim";
                            }

                            if (item.AssinaturaSocioSegundo != null)
                            {
                                worksheet.Cells[row, 33].Value = "Sim";
                            }

                            if (item.AssinaturaSocioTerceiro != null)
                            {
                                worksheet.Cells[row, 34].Value = "Sim";
                            }

                        }
                    }

                    var worksheet2 = package.Workbook.Worksheets.Add("G.P. FACTORING SOCIEDADE DE FOMENTO MERCANTIL LTDA");
                    int row2 = 1;

                    for (int i = 1; i <= 34; i++)
                    {
                        worksheet2.Cells[row2, i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    }

                    for (int i = 1; i <= 34; i++)
                    {
                        worksheet2.Cells[row2, i].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255)); // Define a cor da fonte como branco
                    }

                    for (int i = 1; i <= 34; i++)
                    {
                        worksheet2.Column(i).Width = 25;
                    }

                    for (int i = 1; i <= 34; i++)
                    {
                        worksheet2.Cells[row2, i].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    }

                    worksheet2.Cells[row2, 1].Value = "CNPJ EMPRESA";
                    worksheet2.Cells[row2, 2].Value = "Nome Jurídico";
                    worksheet2.Cells[row2, 3].Value = "Nome Fantasia";
                    worksheet2.Cells[row2, 4].Value = "Tipo Pessoa";
                    worksheet2.Cells[row2, 5].Value = "CPF/CNPJ";
                    worksheet2.Cells[row2, 6].Value = "Inscrição Estadual";
                    worksheet2.Cells[row2, 7].Value = "Endereço completo";
                    worksheet2.Cells[row2, 8].Value = "Bairro";
                    worksheet2.Cells[row2, 9].Value = "Cidade";
                    worksheet2.Cells[row2, 10].Value = "Estado";
                    worksheet2.Cells[row2, 11].Value = "CEP";
                    worksheet2.Cells[row2, 12].Value = "Telefone";
                    worksheet2.Cells[row2, 13].Value = "Email";
                    worksheet2.Cells[row2, 14].Value = "Nome Socio 1";
                    worksheet2.Cells[row2, 15].Value = "Tipo Socio 1";
                    worksheet2.Cells[row2, 16].Value = "CPF/CNPJ 1";
                    worksheet2.Cells[row2, 17].Value = "Email Socio 1";
                    worksheet2.Cells[row2, 18].Value = "Celular Socio 1";
                    worksheet2.Cells[row2, 19].Value = "Função Socio 1";
                    worksheet2.Cells[row2, 20].Value = "Nome Socio 2";
                    worksheet2.Cells[row2, 21].Value = "Tipo Socio 2";
                    worksheet2.Cells[row2, 22].Value = "CPF/CNPJ 2";
                    worksheet2.Cells[row2, 23].Value = "Email Socio 2";
                    worksheet2.Cells[row2, 24].Value = "Celular Socio 2";
                    worksheet2.Cells[row2, 25].Value = "Função Socio 2";
                    worksheet2.Cells[row2, 26].Value = "Nome Socio 3";
                    worksheet2.Cells[row2, 27].Value = "Tipo Socio 3";
                    worksheet2.Cells[row2, 28].Value = "CPF/CNPJ 3";
                    worksheet2.Cells[row2, 29].Value = "Email Socio 3";
                    worksheet2.Cells[row2, 30].Value = "Celular Socio 3";
                    worksheet2.Cells[row2, 31].Value = "Função Socio 3";

                    worksheet2.Cells[row2, 32].Value = "Assinatura socio 1";
                    worksheet2.Cells[row2, 33].Value = "Assinatura socio 2";
                    worksheet2.Cells[row2, 34].Value = "Assinatura socio 3";

                    foreach (var item in clientes)
                    {
                        if (item.tipo == "Cedente")
                        {
                            row2++;
                            worksheet2.Cells[row2, 1].Value = "32598094000139";
                            worksheet2.Cells[row2, 2].Value = item.razaoSocialCedente;
                            worksheet2.Cells[row2, 3].Value = item.nome;
                            worksheet2.Cells[row2, 5].Value = item.cpfCnpj.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                            worksheet2.Cells[row2, 6].Value = item.InscricaoEstadual != null ? item.InscricaoEstadual.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.InscricaoEstadual;
                            worksheet2.Cells[row2, 7].Value = item.rua + " " + item.numero + " " + item.complemento;
                            worksheet2.Cells[row2, 8].Value = item.bairro;
                            worksheet2.Cells[row2, 9].Value = item.cidade;
                            worksheet2.Cells[row2, 10].Value = item.estado;
                            worksheet2.Cells[row2, 11].Value = item.cep.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                            worksheet2.Cells[row2, 12].Value = item.telefone.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                            worksheet2.Cells[row2, 13].Value = item.email;
                            worksheet2.Cells[row2, 14].Value = item.nomeSocio;
                            worksheet2.Cells[row2, 16].Value = item.cpfSocio != null ? item.cpfSocio.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.cpfSocio;
                            worksheet2.Cells[row2, 17].Value = item.EmailSocio;
                            worksheet2.Cells[row2, 18].Value = item.celularSocio != null ? item.celularSocio.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.celularSocio;
                            worksheet2.Cells[row2, 20].Value = item.nomeSocioSegundo;
                            worksheet2.Cells[row2, 22].Value = item.cpfSocioSegundo != null ? item.cpfSocioSegundo.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.cpfSocioSegundo;
                            worksheet2.Cells[row2, 23].Value = item.EmailSegundo;
                            worksheet2.Cells[row2, 24].Value = item.celularSocioSegundo != null ? item.celularSocioSegundo.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.celularSocioSegundo;
                            worksheet2.Cells[row2, 26].Value = item.nomeSocioTerceiro;
                            worksheet2.Cells[row2, 28].Value = item.cpfSocioTerceiro != null ? item.cpfSocioTerceiro.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.cpfSocioTerceiro;
                            worksheet2.Cells[row2, 29].Value = item.EmailTerceiro;
                            worksheet2.Cells[row2, 30].Value = item.celularSocioTerceiro != null ? item.celularSocioTerceiro.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.celularSocioTerceiro;

                            if (item.tipoPessoa == 1)
                            {
                                worksheet2.Cells[row2, 4].Value = "F";
                            }
                            if (item.tipoPessoa == 2)
                            {
                                worksheet2.Cells[row2, 4].Value = "J";
                            }


                            //Tipo Pessoa socio 1
                            if (item.cpfSocio != null)
                            {
                                string valor = item.cpfSocio.ToString().Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                                if (valor.Length > 0 && valor.Length == 11)
                                {
                                    worksheet2.Cells[row2, 15].Value = "F";
                                }
                                if (valor.Length == 14)
                                {
                                    worksheet2.Cells[row2, 15].Value = "J";
                                }
                            }

                            //Tipo socio 2

                            if (item.cpfSocioSegundo != null)
                            {
                                string valor = item.cpfSocioSegundo.ToString().Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                                if (valor.Length > 0 && valor.Length == 11)
                                {
                                    worksheet2.Cells[row2, 21].Value = "F";
                                }
                                if (valor.Length == 14)
                                {
                                    worksheet2.Cells[row2, 21].Value = "J";
                                }
                            }

                            //Tipo Socio 3
                            if (item.cpfSocioTerceiro != null)
                            {
                                string valor = item.cpfSocioTerceiro.ToString().Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                                if (valor.Length > 0 && valor.Length == 11)
                                {
                                    worksheet2.Cells[row2, 27].Value = "F";
                                }
                                if (valor.Length == 14)
                                {
                                    worksheet2.Cells[row2, 27].Value = "J";
                                }
                            }


                            if (item.nomeSocioSegundo != null && item.nomeSocioSegundo.Length > 1)
                            {
                                worksheet2.Cells[row2, 25].Value = "REPRESENTANTE";
                            }
                            if (item.nomeSocioTerceiro != null && item.nomeSocioTerceiro.Length > 1)
                            {
                                worksheet2.Cells[row2, 31].Value = "REPRESENTANTE";
                            }
                            if (item.nomeSocio != null && item.nomeSocio.Length > 1)
                            {
                                worksheet2.Cells[row2, 19].Value = "AVALISTA, EMITENTE, ENDOSSANTE, REPRESENTANTE";
                            }

                            if (item.tipoPessoa != 2)
                            {
                                worksheet2.Cells[row2, 14].Value = item.nome;
                                worksheet2.Cells[row2, 15].Value = "F";
                                worksheet2.Cells[row2, 16].Value = item.cpfCnpj;
                                worksheet2.Cells[row2, 17].Value = item.email;
                                worksheet2.Cells[row2, 18].Value = item.telefone;
                                worksheet2.Cells[row2, 19].Value = "AVALISTA, EMITENTE, ENDOSSANTE, REPRESENTANTE";
                            }

                            //Assinatura socios

                            if (item.AssinaturaSocio != null)
                            {
                                worksheet2.Cells[row2, 32].Value = "Sim";
                            }

                            if (item.AssinaturaSocioSegundo != null)
                            {
                                worksheet2.Cells[row2, 33].Value = "Sim";
                            }

                            if (item.AssinaturaSocioTerceiro != null)
                            {
                                worksheet2.Cells[row2, 34].Value = "Sim";
                            }

                        }
                    }

                    var worksheet3 = package.Workbook.Worksheets.Add("GP AUDAX ");
                    int row3 = 1;

                    for (int i = 1; i <= 34; i++)
                    {
                        worksheet3.Cells[row3, i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    }

                    for (int i = 1; i <= 34; i++)
                    {
                        worksheet3.Cells[row3, i].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255)); // Define a cor da fonte como branco
                    }

                    for (int i = 1; i <= 34; i++)
                    {
                        worksheet3.Column(i).Width = 25;
                    }

                    for (int i = 1; i <= 34; i++)
                    {
                        worksheet3.Cells[row3, i].Style.Fill.BackgroundColor.SetColor(corAzulClaro);
                    }

                    worksheet3.Cells[row3, 1].Value = "CNPJ EMPRESA";
                    worksheet3.Cells[row3, 2].Value = "Nome Jurídico";
                    worksheet3.Cells[row3, 3].Value = "Nome Fantasia";
                    worksheet3.Cells[row3, 4].Value = "Tipo Pessoa";
                    worksheet3.Cells[row3, 5].Value = "CPF/CNPJ";
                    worksheet3.Cells[row3, 6].Value = "Inscrição Estadual";
                    worksheet3.Cells[row3, 7].Value = "Endereço completo";
                    worksheet3.Cells[row3, 8].Value = "Bairro";
                    worksheet3.Cells[row3, 9].Value = "Cidade";
                    worksheet3.Cells[row3, 10].Value = "Estado";
                    worksheet3.Cells[row3, 11].Value = "CEP";
                    worksheet3.Cells[row3, 12].Value = "Telefone";
                    worksheet3.Cells[row3, 13].Value = "Email";
                    worksheet3.Cells[row3, 14].Value = "Nome Socio 1";
                    worksheet3.Cells[row3, 15].Value = "Tipo Socio 1";
                    worksheet3.Cells[row3, 16].Value = "CPF/CNPJ 1";
                    worksheet3.Cells[row3, 17].Value = "Email Socio 1";
                    worksheet3.Cells[row3, 18].Value = "Celular Socio 1";
                    worksheet3.Cells[row3, 19].Value = "Função Socio 1";
                    worksheet3.Cells[row3, 20].Value = "Nome Socio 2";
                    worksheet3.Cells[row3, 21].Value = "Tipo Socio 2";
                    worksheet3.Cells[row3, 22].Value = "CPF/CNPJ 2";
                    worksheet3.Cells[row3, 23].Value = "Email Socio 2";
                    worksheet3.Cells[row3, 24].Value = "Celular Socio 2";
                    worksheet3.Cells[row3, 25].Value = "Função Socio 2";
                    worksheet3.Cells[row3, 26].Value = "Nome Socio 3";
                    worksheet3.Cells[row3, 27].Value = "Tipo Socio 3";
                    worksheet3.Cells[row3, 28].Value = "CPF/CNPJ 3";
                    worksheet3.Cells[row3, 29].Value = "Email Socio 3";
                    worksheet3.Cells[row3, 30].Value = "Celular Socio 3";
                    worksheet3.Cells[row3, 31].Value = "Função Socio 3";

                    worksheet3.Cells[row3, 32].Value = "Assinatura socio 1";
                    worksheet3.Cells[row3, 33].Value = "Assinatura socio 2";
                    worksheet3.Cells[row3, 34].Value = "Assinatura socio 3";

                    foreach (var item in clientes)
                    {
                        if (item.tipo == "Cedente")
                        {
                            row3++;
                            worksheet3.Cells[row3, 1].Value = "50541180000197";
                            worksheet3.Cells[row3, 2].Value = item.razaoSocialCedente;
                            worksheet3.Cells[row3, 3].Value = item.nome;
                            worksheet3.Cells[row3, 5].Value = item.cpfCnpj.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                            worksheet3.Cells[row3, 6].Value = item.InscricaoEstadual != null ? item.InscricaoEstadual.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.InscricaoEstadual;
                            worksheet3.Cells[row3, 7].Value = item.rua + " " + item.numero + " " + item.complemento;
                            worksheet3.Cells[row3, 8].Value = item.bairro;
                            worksheet3.Cells[row3, 9].Value = item.cidade;
                            worksheet3.Cells[row3, 10].Value = item.estado;
                            worksheet3.Cells[row3, 11].Value = item.cep.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                            worksheet3.Cells[row3, 12].Value = item.telefone.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                            worksheet3.Cells[row3, 13].Value = item.email;
                            worksheet3.Cells[row3, 14].Value = item.nomeSocio;
                            worksheet3.Cells[row3, 16].Value = item.cpfSocio != null ? item.cpfSocio.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.cpfSocio;
                            worksheet3.Cells[row3, 17].Value = item.EmailSocio;
                            worksheet3.Cells[row3, 18].Value = item.celularSocio != null ? item.celularSocio.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.celularSocio;
                            worksheet3.Cells[row3, 20].Value = item.nomeSocioSegundo;
                            worksheet3.Cells[row3, 22].Value = item.cpfSocioSegundo != null ? item.cpfSocioSegundo.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.cpfSocioSegundo;
                            worksheet3.Cells[row3, 23].Value = item.EmailSegundo;
                            worksheet3.Cells[row3, 24].Value = item.celularSocioSegundo != null ? item.celularSocioSegundo.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.celularSocioSegundo;
                            worksheet3.Cells[row3, 26].Value = item.nomeSocioTerceiro;
                            worksheet3.Cells[row3, 28].Value = item.cpfSocioTerceiro != null ? item.cpfSocioTerceiro.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.cpfSocioTerceiro;
                            worksheet3.Cells[row3, 29].Value = item.EmailTerceiro;
                            worksheet3.Cells[row3, 30].Value = item.celularSocioTerceiro != null ? item.celularSocioTerceiro.Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "") : item.celularSocioTerceiro;

                            if (item.tipoPessoa == 1)
                            {
                                worksheet3.Cells[row3, 4].Value = "F";
                            }
                            if (item.tipoPessoa == 2)
                            {
                                worksheet3.Cells[row3, 4].Value = "J";
                            }

                            //Tipo Pessoa socio 1
                            if (item.cpfSocio != null)
                            {
                                string valor = item.cpfSocio.ToString().Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                                if (valor.Length > 0 && valor.Length == 11)
                                {
                                    worksheet3.Cells[row3, 15].Value = "F";
                                }
                                if (valor.Length == 14)
                                {
                                    worksheet3.Cells[row3, 15].Value = "J";
                                }
                            }

                            //Tipo socio 2

                            if (item.cpfSocioSegundo != null)
                            {
                                string valor = item.cpfSocioSegundo.ToString().Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                                if (valor.Length > 0 && valor.Length == 11)
                                {
                                    worksheet3.Cells[row3, 21].Value = "F";
                                }
                                if (valor.Length == 14)
                                {
                                    worksheet3.Cells[row3, 21].Value = "J";
                                }
                            }

                            //Tipo Socio 3
                            if (item.cpfSocioTerceiro != null)
                            {
                                string valor = item.cpfSocioTerceiro.ToString().Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "").Replace("/", "");
                                if (valor.Length > 0 && valor.Length == 11)
                                {
                                    worksheet3.Cells[row3, 27].Value = "F";
                                }
                                if (valor.Length == 14)
                                {
                                    worksheet3.Cells[row3, 27].Value = "J";
                                }
                            }


                            if (item.nomeSocioSegundo != null && item.nomeSocioSegundo.Length > 1)
                            {
                                worksheet3.Cells[row3, 25].Value = "REPRESENTANTE";
                            }
                            if (item.nomeSocioTerceiro != null && item.nomeSocioTerceiro.Length > 1)
                            {
                                worksheet3.Cells[row3, 31].Value = "REPRESENTANTE";
                            }
                            if (item.nomeSocio != null && item.nomeSocio.Length > 1)
                            {
                                worksheet3.Cells[row3, 19].Value = "AVALISTA, EMITENTE, ENDOSSANTE, REPRESENTANTE";
                            }

                            if (item.tipoPessoa != 2)
                            {
                                worksheet3.Cells[row3, 14].Value = item.nome;
                                worksheet3.Cells[row3, 15].Value = "F";
                                worksheet3.Cells[row3, 16].Value = item.cpfCnpj;
                                worksheet3.Cells[row3, 17].Value = item.email;
                                worksheet3.Cells[row3, 18].Value = item.telefone;
                                worksheet3.Cells[row3, 19].Value = "AVALISTA, EMITENTE, ENDOSSANTE, REPRESENTANTE";
                            }

                            //Assinatura socios

                            if (item.AssinaturaSocio != null)
                            {
                                worksheet3.Cells[row3, 32].Value = "Sim";
                            }

                            if (item.AssinaturaSocioSegundo != null)
                            {
                                worksheet3.Cells[row3, 33].Value = "Sim";
                            }

                            if (item.AssinaturaSocioTerceiro != null)
                            {
                                worksheet3.Cells[row3, 34].Value = "Sim";
                            }
                        }
                    }

                    worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    var stream = new MemoryStream(package.GetAsByteArray());

                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AssinaturaDigitalCRM.xlsx");
                }
            };

        }
    }
}
