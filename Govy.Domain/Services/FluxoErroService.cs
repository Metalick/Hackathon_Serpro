using Govy.Domain.Abstract;
using Govy.Domain.DTOs;

namespace Govy.Domain.Services
{
    public class FluxoErroService : IFluxoErroService
    {
        public FluxoErroService()
        {

        }

        public FluxoMensagemDTO MontaErro(FluxoMensagemDTO tipoFase)
        {
            var fluxoAtual = tipoFase;

            fluxoAtual.TipoFaseBot = tipoFase.TipoFaseBotAnterior == 0 ? Enums.TipoFaseBot.Saudacao : tipoFase.TipoFaseBotAnterior;

            return tipoFase;
        }       
    }
}
