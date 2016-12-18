using Govy.Domain.Abstract;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Govy.Domain.Dialog
{
    [Serializable]
    public class IntroDialogFluxoDeclaracaoSimplificada : IDialog<object>
    {

        public IntroDialogFluxoDeclaracaoSimplificada()
        {
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            context.Wait(MessageReceivedAsync);
        }

    }
}
