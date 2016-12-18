using Govy.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Govy.Domain.DTOs;

namespace Govy.Domain.Services
{
    public class FluxoFinalizacaoService : IFluxoFinalizacaoService
    {
        public FluxoFinalizacaoService()
        {

        }

        public string MensagemDeFinalizacao()
        {
            var mensagens = RecuperaMensagensFinalizacao();
            var pos = Utils.NumeroAleatorioUtil.RecuperaNumeroAleatorio(mensagens.Count);
            return mensagens[pos];
        }

        public List<string> RecuperaMensagensFinalizacao()
        {
            var lst = new List<string>();

            lst.Add("Obrigado, Tchau");

            return lst;
        }

        public FluxoMensagemDTO ValidaFluxoFinalizar(FluxoMensagemDTO fluxoAtual)
        {
            var palavrasChaves = FluxoFinalizarPalavraChave();

            var palavrasFluxoDuvidas = palavrasChaves
                                            .Where(x => fluxoAtual.Texto.Contains(x)).ToList();

            if (palavrasFluxoDuvidas.Any())
            {
                fluxoAtual.TipoFaseBotAnterior = fluxoAtual.TipoFaseBot;
                fluxoAtual.TipoFaseBot = Enums.TipoFaseBot.FluxoFinalizacao;
            }

            return fluxoAtual;
        }

        public List<string> FluxoFinalizarPalavraChave()
        {
            var lst = new List<string>();

            lst.Add("tchau");
            lst.Add("t++");

            return lst;
        }
    }
}
