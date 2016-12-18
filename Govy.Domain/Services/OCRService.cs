
using Govy.Domain.Abstract;
using Govy.Domain.DTOs;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;
using System.Net;
using System.Text;

namespace Govy.Domain.Services
{
    public class OCRService : IOCRService
    {
        public async System.Threading.Tasks.Task<string> GetDocumentData(string contentUrl)
        {
            OcrResults text;

            // Faz o download do arquivo.
            var arquivoFisico = new WebClient().DownloadData(contentUrl);

            // Envia o arquivo para a API Cognitiva.
            var client = new VisionServiceClient("568afbc6fc674f0eb22685becd56bfd6");
            using (Stream imgStream = new MemoryStream(arquivoFisico))
            {
                text = await client.RecognizeTextAsync(imgStream);
            }
            var objImposto = new DeclaracaoSimplificada(LogOcrResults(text));

            return "Achei você. Aproveitei e lí os dados desse ano. Confirma para mim. Seus dados estão corretos?  \r\nCPF: " + objImposto.Cpf + "\nNome: " + objImposto.NomeCompleto + "\nTotal de Rendimentos: " + objImposto.TotalRendimentos + "\nNome: " + objImposto.TotalRendimentosTributaveis + "\nTotal Rendimentos Tributáveis: " + objImposto.TotalRendimentosTributaveis + "\nDécimo Terceio: " + objImposto.DecimoTerceiro + "\nCNPJ:  " + objImposto.Cnpj;
        }

        protected string LogOcrResults(OcrResults results)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (results != null && results.Regions != null)
            {
                stringBuilder.Append(" ");
                stringBuilder.AppendLine();
                foreach (var item in results.Regions)
                {
                    foreach (var line in item.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            stringBuilder.Append(word.Text);
                            stringBuilder.Append(" ");
                        }
                        stringBuilder.AppendLine();
                    }
                    stringBuilder.AppendLine();
                }
            }
            return stringBuilder.ToString();
        }
    }
}
