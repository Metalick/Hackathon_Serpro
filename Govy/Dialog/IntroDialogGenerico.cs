using Govy.Domain.Abstract;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Govy.Domain.Dialog
{
    [Serializable]
    public class IntroDialogGenerico : IDialog<object>
    {
        public IntroDialogGenerico()
        {
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            var message = await argument;
            await context.PostAsync(message.Text);

            context.Wait(MessageReceivedAsync);
        }

    }
}
