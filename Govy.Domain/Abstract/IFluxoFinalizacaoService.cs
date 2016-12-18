using Govy.Domain.DTOs;

namespace Govy.Domain.Abstract
{
    public interface IFluxoFinalizacaoService
    {
        string MensagemDeFinalizacao();
        FluxoMensagemDTO ValidaFluxoFinalizar(FluxoMensagemDTO fluxoAtual);
    }
}
