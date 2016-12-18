using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using MongoRepository;
using Govy.Domain;
using Govy.Domain.Abstract;
using Microsoft.Bot.Builder.Dialogs;
using Govy.Domain.Dialog;
using Govy.Domain.Services;
using Govy.Domain.DTOs;
using Govy.Domain.Enums;

namespace Govy
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        static MongoRepository<Customer> customerrepo;
        private ISaudacaoService _saudacaoService;
        private IFluxoDuvidasService _fluxoDuvidasService;
        private IFluxoDeclSimplificadaService _fluxoDeclSimplificadaService;
        private IFluxoErroService _fluxoErroService;
        private IFluxoOhQueNaoSaberService _fluxoOhQueNaoSaberService;
        private IFluxoVoltarService _fluxoVoltarService;
        private IFluxoFinalizacaoService _fluxoFinalizacaoService;
        public IOCRService _ocr;

        public MessagesController()
        {
            _fluxoDuvidasService = new FluxoDuvidasService();
            _saudacaoService = new SaudacaoService();
            _ocr = new OCRService();
            _fluxoDeclSimplificadaService = new FluxoDeclSimplificadaService(_ocr);
            _fluxoOhQueNaoSaberService = new FluxoOhQueNaoSaberService();
            _fluxoVoltarService = new FluxoVoltarService();
            _fluxoFinalizacaoService = new FluxoFinalizacaoService();
            _fluxoErroService = new FluxoErroService();
            
            customerrepo = new MongoRepository<Customer>();
        }

        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            try
            {
                // check if activity is of type message
                if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
                {

                    StateClient stateClient = activity.GetStateClient();
                    BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);
                    var mensagem = userData.GetProperty<MensagemDTO>("Mensagem");

                    mensagem.TipoFasePasso_1Anterior = mensagem.TipoFasePasso_1;
                    mensagem.TipoFasePasso_2Anterior = mensagem.TipoFasePasso_2;
                    mensagem.TipoFasePasso_3Anterior = mensagem.TipoFasePasso_3;
                    mensagem.TipoFaseBotAnterior = mensagem.TipoFaseBot;
                    mensagem.TextoAnterior = mensagem.Texto;
                    mensagem.Texto = activity.Text;

                    var faseAnterior = new FluxoMensagemDTO
                    {
                        Texto = string.IsNullOrEmpty(mensagem.Texto) ? "" : mensagem.Texto,
                        TipoFaseBot = mensagem.TipoFaseBot,
                        TipoFasePasso_1 = mensagem.TipoFasePasso_1,
                        TipoFasePasso_2 = mensagem.TipoFasePasso_2,
                        TipoFasePasso_3 = mensagem.TipoFasePasso_3,
                    };

                    faseAnterior.Texto = faseAnterior.Texto.ToLower();

                    FluxoMensagemDTO faseAtual = await RecuperaFluxoAtual(faseAnterior, activity);

                    var fluxoRetorno = await RecuperaFluxoRetorno(faseAtual, activity);

                    mensagem.TipoFasePasso_1 = fluxoRetorno.TipoFasePasso_1;
                    mensagem.TipoFasePasso_2 = fluxoRetorno.TipoFasePasso_2;
                    mensagem.TipoFasePasso_3 = fluxoRetorno.TipoFasePasso_3;
                    mensagem.TipoFaseBotAnterior = mensagem.TipoFaseBot;
                    mensagem.TipoFaseBot = fluxoRetorno.TipoFaseBot;
                    mensagem.Texto = fluxoRetorno.Texto;

                    userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);
                    userData.SetProperty<MensagemDTO>("Mensagem", mensagem);
                    stateClient.BotState.SetUserData(activity.ChannelId, activity.From.Id, userData);

                    activity.Text = mensagem.Texto;

                    await Conversation.SendAsync(activity, () => new IntroDialogGenerico());
                }
                else
                {
                    HandleSystemMessage(activity);
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                Activity reply = activity.CreateReply(ex.Message);
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                connector.Conversations.ReplyToActivity(reply);
                return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
            }
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {

                var saudacao = _saudacaoService.RecuperaSaudacaoInicial();
                Activity reply = message.CreateReply(saudacao.Texto);
                ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                StateClient stateClient = message.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(message.ChannelId, message.From.Id);
                userData = stateClient.BotState.GetUserData(message.ChannelId, message.From.Id);

                var mensagemTemp = new MensagemDTO { TipoFaseBot = TipoFaseBot.Saudacao, Texto = saudacao.Texto };
                userData.SetProperty<MensagemDTO>("Mensagem", mensagemTemp);
                stateClient.BotState.SetUserData(message.ChannelId, message.From.Id, userData);

                connector.Conversations.ReplyToActivity(reply);

            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        private static void DumpData()
        {
            //Print all data
            Console.WriteLine("Currently in our database:");
            foreach (Customer c in customerrepo)
            {
                Console.WriteLine("{0}\t{1}\t has {2} products",
                    c.FirstName, c.LastName, c.Products.Count);
                foreach (Product p in c.Products)
                    Console.WriteLine("\t{0} priced ${1:N2}", p.Name, p.Price);
                Console.WriteLine("\tTOTAL: ${0:N2}", c.Products.Sum(p => p.Price));
            }
            Console.WriteLine(new string('=', 50));
        }

        private async Task<FluxoMensagemDTO> RecuperaFluxoAtual(FluxoMensagemDTO fluxoAnterior, Activity activity)
        {

            fluxoAnterior.Texto = fluxoAnterior.Texto ?? "";

            fluxoAnterior.Texto = fluxoAnterior.Texto.ToLower();
            //fluxoAnterior.TipoFaseBotAnterior = fluxoAnterior.TipoFaseBot;

            var fluxoAtual = new FluxoMensagemDTO();

            switch (fluxoAnterior.TipoFaseBot)
            {
                case TipoFaseBot.Saudacao:

                    fluxoAtual = _saudacaoService.ValidaSaudacao(fluxoAnterior);

                    break;
                case TipoFaseBot.FluxoDuvidas:
                    fluxoAtual = _fluxoDuvidasService.ValidaFluxoDuvidas(fluxoAnterior);
                    break;

                case TipoFaseBot.AcompanhamentoDiario:
                    // tipoFluxo = _fluxoAcompanhamentoDiario.ValidaAcompanhamentoDiario(fluxoMensagemAnterior);
                    break;

                case TipoFaseBot.FluxoDeclaracaoSimplificada:

                    string pathArquivo = "";
                    if (activity.Attachments != null)
                    {
                        pathArquivo = activity.Attachments[0].ContentUrl;
                    }


                    fluxoAtual = _fluxoDeclSimplificadaService.ValidaFluxoDeclaracaoSimplificada(fluxoAnterior, pathArquivo);
                    break;

                case TipoFaseBot.OhQueNaoSouber:
                    // fluxoAtual = _fluxoOhQueNaoSaberService.ValidaOhQueNaoSouver(fluxoAnterior);
                    break;

                case TipoFaseBot.FluxoFinalizacao:
                    fluxoAnterior.TipoFaseBot = fluxoAnterior.TipoFaseBotAnterior;
                    fluxoAnterior.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_1;
                    //  fluxoAtual = _fluxoFinalizacaoService.ValidaFluxoFinalizacao(fluxoAnterior);
                    break;

                default:
                    fluxoAtual = fluxoAnterior;
                    break;
            }

            fluxoAtual = _fluxoVoltarService.ValidaFluxoVoltar(fluxoAtual);
            fluxoAtual = _fluxoFinalizacaoService.ValidaFluxoFinalizar(fluxoAtual);
            return fluxoAtual;
        }

        private async Task<FluxoMensagemDTO> RecuperaFluxoRetorno(FluxoMensagemDTO fluxoAtual, Activity activity)
        {
            var fluxoRetorno = fluxoAtual;
            switch (fluxoAtual.TipoFaseBot)
            {
                case TipoFaseBot.Saudacao:
                    fluxoRetorno = new FluxoMensagemDTO
                    {
                        Texto = _saudacaoService.RecuperaSaudacaoSegundaria().Texto,
                        TipoFaseBot = fluxoAtual.TipoFaseBot
                    };

                    break;

                case TipoFaseBot.FluxoDuvidas:
                    fluxoRetorno = _fluxoDuvidasService.RecuperaPassoAhPassoDuvida(fluxoAtual);

                    break;

                case TipoFaseBot.AcompanhamentoDiario:

                    break;

                case TipoFaseBot.FluxoDeclaracaoSimplificada:

                    string pathArquivo = "";
                    if (activity.Attachments != null)
                    {
                        pathArquivo = activity.Attachments[0].ContentUrl;
                    }

                    if (fluxoRetorno.TipoFasePasso_1.HasValue
                        && fluxoRetorno.TipoFasePasso_1 == TipoFasePassoAhPasso.Fase_2
                        && !string.IsNullOrEmpty(pathArquivo))
                    {
                        var aa =  await _ocr.GetDocumentData(pathArquivo);
                        fluxoAtual.Texto = aa;
                        fluxoAtual.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_3;
                    }
                    else
                    {
                        fluxoRetorno = _fluxoDeclSimplificadaService.RecuperaFluxoDeclSimplificada(fluxoAtual, pathArquivo);
                    }
                    break;

                case TipoFaseBot.OhQueNaoSouber:
                    fluxoRetorno.TipoFaseBot = fluxoAtual.TipoFaseBotAnterior;
                    fluxoRetorno.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_1;
                    break;

                case TipoFaseBot.FluxoFinalizacao:

                    fluxoRetorno.Texto = _fluxoFinalizacaoService.MensagemDeFinalizacao();

                    break;
                case TipoFaseBot.FluxoErro:
                    fluxoRetorno = _fluxoErroService.MontaErro(fluxoAtual);
                    break;
            }

            return fluxoRetorno;
        }
    }
}