using Govy.Domain.Abstract;
using Govy.Domain.DTOs;
using Govy.Domain.Enums;
using Govy.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Govy.Domain.Services
{
    [Serializable]
    public class SaudacaoService : ISaudacaoService
    {
        public SaudacaoService()
        {

        }

        public SaudacaoDTO RecuperaSaudacaoInicial()
        {

            var lst = new List<SaudacaoDTO>();

            lst.Add(new SaudacaoDTO
            {
                Texto = "Olá, Bom dia, tudo bem com você ?"
            });

            lst.Add(new SaudacaoDTO
            {
                Texto = "Oi, Tudo bem ?"
            });

            lst.Add(new SaudacaoDTO
            {
                Texto = "Oi, beleza ?"
            });

            lst.Add(new SaudacaoDTO
            {
                Texto = "Ei, como vai você ?"
            });

            var numeroAleatorio = NumeroAleatorioUtil.RecuperaNumeroAleatorio(lst.Count);
            return lst[numeroAleatorio];
        }

        public SaudacaoDTO RecuperaSaudacaoSegundaria()
        {
            var lst = new List<SaudacaoDTO>();

            lst.Add(new SaudacaoDTO
            {
                Id = 1,
                Texto = "Tudo bem, obrigado por perguntar. No que posso te ajudar ?"
            });

            lst.Add(new SaudacaoDTO
            {
                Id = 2,
                Texto = "Tudo certo. Como posso te ajudar ?"
            });

            lst.Add(new SaudacaoDTO
            {
                Id = 3,
                Texto = "Tudo bom. O que vamos fazer hoje ?"
            });

            lst.Add(new SaudacaoDTO
            {
                Id = 4,
                Texto = "Por aqui vai tudo bem. Como posso te ajudar ?"
            });

            var numeroAleatorio = NumeroAleatorioUtil.RecuperaNumeroAleatorio(lst.Count);

            return lst[numeroAleatorio];
        }

        public FluxoMensagemDTO ValidaParametro(FluxoMensagemDTO faseAnterior)
        {
            throw new NotImplementedException();
        }

        public FluxoMensagemDTO ValidaSaudacao(FluxoMensagemDTO fluxoAnterior)
        {
            fluxoAnterior.Texto = fluxoAnterior.Texto.ToLower();
            var palavrasChaves = DuvidasPalavraChave();
            bool ehFluxoDuvidas = false;
            bool ehFluxoDescSimplificada = false;
            var palavrasFluxoDuvidas = palavrasChaves
                                            .Where(x => fluxoAnterior.Texto.Contains(x)).ToList();

            ehFluxoDuvidas = palavrasFluxoDuvidas.Any();

            palavrasChaves = DescSimplificadaPalavraChave();

            var FluxoDescSimp = palavrasChaves
                                            .Where(x => fluxoAnterior.Texto.Contains(x)).ToList();
            ehFluxoDescSimplificada = FluxoDescSimp.Any();

            if (ehFluxoDuvidas || ehFluxoDescSimplificada)
            {
                if (ehFluxoDescSimplificada && ehFluxoDuvidas)
                {

                }
                else
                    if (ehFluxoDuvidas)
                {
                    fluxoAnterior.TipoFaseBot = TipoFaseBot.FluxoDuvidas;
                }
                else if (ehFluxoDescSimplificada)
                {
                    fluxoAnterior.TipoFaseBot = TipoFaseBot.FluxoDeclaracaoSimplificada;
                }
            }else
            {
                var saudacaoPalavraChave = SaudacaoPalavrasChaves();

                palavrasChaves = saudacaoPalavraChave
                    .Where(x => fluxoAnterior.Texto.Contains(x))
                    .ToList();

                if (!palavrasChaves.Any())
                {
                    fluxoAnterior.Texto = "Não consegui entender. Repita para mim";
                    fluxoAnterior.TipoFaseBot = TipoFaseBot.FluxoErro;
                }
            }

            return fluxoAnterior;
        }

        public List<string> SaudacaoPalavrasChaves()
        {

            var lst = new List<string>();
            lst.Add("voce");
            lst.Add("você");
            lst.Add("sim");
            return lst;

        }

        public List<string> DuvidasPalavraChave()
        {
            var lst = new List<string>();
            lst.Add("duvida");
            lst.Add("dúvida");
            lst.Add("ajuda");

            return lst;
        }

        public List<string> DescSimplificadaPalavraChave()
        {
            var lst = new List<string>();
            lst.Add("eu quero fazer minha decla");
            lst.Add("quero fazer minha decla");
            lst.Add("gostaria de fazer uma decl");
            lst.Add("iniciar");
            lst.Add("começar");
            lst.Add("comecar");

            return lst;
        }
    }
}
