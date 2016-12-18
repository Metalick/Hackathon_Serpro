using Govy.Domain.Abstract;
using Govy.Domain.DTOs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Govy.Domain.Dialog
{
    [Serializable]
    public class IntroDialogSaudacao : IDialog<object>
    {
        private ISaudacaoService _saudacaoService;
        private StateClient _stateClient;
        private List<FluxoMensagemDTO> _fluxosMensagens;

        public IntroDialogSaudacao(ISaudacaoService saudacaoService, 
            StateClient stateClient, 
            List<FluxoMensagemDTO> fluxosMensagens)
        {
            _saudacaoService = saudacaoService;
            _fluxosMensagens = fluxosMensagens;
            _stateClient = stateClient;

        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            var saudacao = _saudacaoService.RecuperaSaudacaoSegundaria();
         //   SalvaFluxoMensagem(argument.GetAwaiter().GetResult(), new FluxoMensagemDTO {Texto = "Teste 123 ", Tipo = Enums.TipoFaseBot.Saudacao });
            await context.PostAsync(saudacao.Texto);
            context.Wait(MessageReceivedAsync);
        }

        public  void SalvaFluxoMensagem(IMessageActivity activity, FluxoMensagemDTO fluxo)
        {
            _fluxosMensagens.Add(fluxo);
            BotData userData =  _stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);
            userData =  _stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);
            userData.SetProperty<List<FluxoMensagemDTO>>("Texto", _fluxosMensagens);
             _stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
        }
    }
}
