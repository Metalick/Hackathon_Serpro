using Govy.Domain.DTOs;

namespace Govy.Domain.Abstract
{
    public  interface IFluxoDeclSimplificadaService
    {
        FluxoMensagemDTO RecuperaFluxoDeclSimplificada(FluxoMensagemDTO tipoFase, string path);
        FluxoMensagemDTO ValidaFluxoDeclaracaoSimplificada(FluxoMensagemDTO fluxoAnterior,string path);
    }
}
