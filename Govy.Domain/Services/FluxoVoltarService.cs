using System;
using Govy.Domain.Abstract;
using Govy.Domain.DTOs;
using System.Collections.Generic;
using System.Linq;
namespace Govy.Domain.Services
{
    public class FluxoVoltarService : IFluxoVoltarService
    {
        public FluxoVoltarService()
        {

        }

        public FluxoMensagemDTO MontaErro(FluxoMensagemDTO tipoFase)
        {

            var fluxoAtual = tipoFase;

            fluxoAtual.TipoFaseBot = tipoFase.TipoFaseBotAnterior;

            return tipoFase;
        }

        public FluxoMensagemDTO ValidaFluxoVoltar(FluxoMensagemDTO fluxoMensagemAnterior)
        {
            var palavrasChaves = VolarPalavraChave();

            var palavrasFluxoDuvidas = palavrasChaves
                                            .Where(x => fluxoMensagemAnterior.Texto.Contains(x)).ToList();

            if (palavrasFluxoDuvidas.Any())
            {
                fluxoMensagemAnterior.TipoFaseBot = fluxoMensagemAnterior.TipoFaseBotAnterior;
            }

            return fluxoMensagemAnterior;
        }

        public List<string> VolarPalavraChave()
        {
            var lst = new List<string>();
            lst.Add("voltar");
            lst.Add("retroceder");
            lst.Add("anterior");

            return lst;
        }

    }
}
