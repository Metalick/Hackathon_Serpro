using Govy.Domain.DTOs;

namespace Govy.Domain.Abstract
{
    public interface ISaudacaoService
    {
        SaudacaoDTO RecuperaSaudacaoInicial();
        SaudacaoDTO RecuperaSaudacaoSegundaria();
        FluxoMensagemDTO ValidaParametro(FluxoMensagemDTO faseAnterior);
        FluxoMensagemDTO ValidaSaudacao(FluxoMensagemDTO fluxoMensagemAnterior);
    }
}
