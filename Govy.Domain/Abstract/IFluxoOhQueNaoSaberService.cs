using Govy.Domain.DTOs;

namespace Govy.Domain.Abstract
{
    public interface IFluxoOhQueNaoSaberService
    {
        void SalvaPergunta(string pergunta);
        FluxoMensagemDTO ValidaOhQueNaoSouver(object fluxoMensagemAnterior);
    }
}
