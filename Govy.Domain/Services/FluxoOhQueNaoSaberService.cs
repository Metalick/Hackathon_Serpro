using System;
using Govy.Domain.Abstract;
using Govy.Domain.DTOs;
using Govy.Domain.Enums;

namespace Govy.Domain.Services
{
    public class FluxoOhQueNaoSaberService : IFluxoOhQueNaoSaberService
    {
        public FluxoOhQueNaoSaberService()
        {

        }

        public void SalvaPergunta(string pergunta)
        {
            // Salva pergunta no banco de dados

        }

        public FluxoMensagemDTO ValidaOhQueNaoSouver(object fluxoMensagemAnterior)
        {
            throw new NotImplementedException();
        }
    }
}
