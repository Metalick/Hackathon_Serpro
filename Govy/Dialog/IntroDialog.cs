using Govy.Domain.Abstract;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Text;
using Govy.Domain.DTOs;

namespace Govy.Domain.Dialog
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        public IOCRService _ocr;

        public EchoDialog(IOCRService ocr)
        {            
            _ocr = ocr;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            // Recupera o arquivo anexado pelo usuário
            var activity = argument.GetAwaiter().GetResult().Attachments?.FirstOrDefault(a => a.ContentType.Contains("image"));

            // Retorna o o documento IRRF Processado
            var retornoOcr = await _ocr.GetDocumentData(activity.ContentUrl);
            var message = await argument;
            
            await context.PostAsync(retornoOcr);
            context.Wait(MessageReceivedAsync);
        }

    //private static async Task<Stream> GetImageStream(ConnectorClient connector, Attachment imageAttachment)
    //{
    //    using (var httpClient = new HttpClient())
    //    {
    //        // The Skype attachment URLs are secured by JwtToken,
    //        // you should set the JwtToken of your bot as the authorization header for the GET request your bot initiates to fetch the image.
    //        // https://github.com/Microsoft/BotBuilder/issues/662
    //        var uri = new Uri(imageAttachment.ContentUrl);
    //        if (uri.Host.EndsWith("skype.com") && uri.Scheme == "https")
    //        {
    //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync(connector));
    //            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
    //        }
    //        else
    //        {
    //            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(imageAttachment.ContentType));
    //        }

    //        return await httpClient.GetStreamAsync(uri);
    //    }
    //}
}
}
