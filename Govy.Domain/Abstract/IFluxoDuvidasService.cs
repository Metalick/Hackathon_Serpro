using Govy.Domain.DTOs;
using Govy.Domain.Entities;
using MongoDB.Driver;

namespace Govy.Domain.Abstract
{
    public interface IFluxoDuvidasService
    {
        FluxoMensagemDTO RecuperaPassoAhPassoDuvida(FluxoMensagemDTO tipoFase);

        MongoCursor<Duvida> GetAllDuvidas();

        void CriarLogDuvida(Log log);

        void CriarDuvida(Duvida duvida);
        FluxoMensagemDTO ValidaFluxoDuvidas(FluxoMensagemDTO fluxoMensagemAnterior);
    }
}
