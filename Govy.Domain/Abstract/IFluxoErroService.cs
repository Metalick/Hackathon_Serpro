using Govy.Domain.DTOs;

namespace Govy.Domain.Abstract
{
    public interface IFluxoErroService
    {
        FluxoMensagemDTO MontaErro(FluxoMensagemDTO tipoFase);
    }
}
