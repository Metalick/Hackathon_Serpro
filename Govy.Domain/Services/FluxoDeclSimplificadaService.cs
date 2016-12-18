using Govy.Domain.Abstract;
using Govy.Domain.DTOs;
using Govy.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Govy.Domain.Services
{
    public class FluxoDeclSimplificadaService : IFluxoDeclSimplificadaService
    {
        private IOCRService _ocrService;

        public FluxoDeclSimplificadaService(IOCRService ocrService)
        {
            _ocrService = ocrService;
        }

        public FluxoMensagemDTO RecuperaFluxoDeclSimplificada(FluxoMensagemDTO tipoFase, string path)
        {
            if (!tipoFase.TipoFasePasso_1.HasValue)
            {

                tipoFase.Texto = MensagemPadrao();
                tipoFase.TipoFaseBot = TipoFaseBot.FluxoDeclaracaoSimplificada;
                tipoFase.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_5;
                return tipoFase;
            }

            switch (tipoFase.TipoFasePasso_1)
            {
                case TipoFasePassoAhPasso.Fase_1:
                    tipoFase.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_2;
                    tipoFase.Texto = "Então, tira uma foto dele ou me manda uma imagem dele para eu ver se te encontro";
                    break;

                case TipoFasePassoAhPasso.Fase_2:
                    tipoFase.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_3;
                    var texto = _ocrService.GetDocumentData(path).GetAwaiter().GetResult();
                    tipoFase.Texto = texto;// Achei você ...
                    break;

                case TipoFasePassoAhPasso.Fase_3:
                    tipoFase.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_4;
                    tipoFase.Texto = "Você mudou de endereço ou quer atualizar suas informações cadastrais ? ";
                    break;

                case TipoFasePassoAhPasso.Fase_4:
                    tipoFase.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_6;
                    tipoFase.Texto = "Ok. Posso mandar sua declaração ? ";
                    break;

                case TipoFasePassoAhPasso.Fase_5:
                    tipoFase.Texto = "Você possui seu informe de rendimentos desse ano ?";
                    tipoFase.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_1;
                    break;

                case TipoFasePassoAhPasso.Fase_6:
                    tipoFase.TipoFaseBot = TipoFaseBot.FluxoFinalizacao;
                    tipoFase.Texto = "Sua declaração foi enviada com sucesso. Obrigado por colaborar ..";
                    break;

                default:
                    break;
            }

            return tipoFase;
        }

        public string MensagemPadrao()
        {
            return "Simplificada, Composta ou de Insenção ?";
        }

        public FluxoMensagemDTO ValidaFluxoDeclaracaoSimplificada(FluxoMensagemDTO fluxoMensagemAnterior, string path)
        {

            var palavrasChaves = DescSimplificadaOperacionalPalavraChave();

            var palavrasFluxoDuvidas = palavrasChaves
                                            .Where(x => fluxoMensagemAnterior.Texto.Contains(x.Texto)).ToList();

            if (palavrasFluxoDuvidas.Any())
            {
                var primeiraPalavraChave = palavrasFluxoDuvidas.First();

                if (primeiraPalavraChave.Identificador == 1)
                {
                    if (fluxoMensagemAnterior.TipoFasePasso_1.HasValue
                        && fluxoMensagemAnterior.TipoFasePasso_1 == TipoFasePassoAhPasso.Fase_1)
                    {
                        fluxoMensagemAnterior.TipoFaseBot = TipoFaseBot.FluxoErro;
                        fluxoMensagemAnterior.Texto = "HUmm... Ainda não fazendo isso...";

                    }
                    else if (fluxoMensagemAnterior.TipoFasePasso_1.HasValue
                      && fluxoMensagemAnterior.TipoFasePasso_1 == TipoFasePassoAhPasso.Fase_3)
                    {
                        fluxoMensagemAnterior.TipoFaseBot = TipoFaseBot.FluxoErro;
                        fluxoMensagemAnterior.Texto = "Foi mal, ainda não estou pronto para atender sua solicitação...";
                    }
                    else if (fluxoMensagemAnterior.TipoFasePasso_1.HasValue
                     && fluxoMensagemAnterior.TipoFasePasso_1 == TipoFasePassoAhPasso.Fase_4)
                    {
                        fluxoMensagemAnterior.TipoFaseBot = TipoFaseBot.FluxoErro;
                        fluxoMensagemAnterior.Texto = "Foi mal, ainda não estou pronto para atender sua solicitação...";
                    }
                    if (fluxoMensagemAnterior.TipoFasePasso_1.HasValue
                     && fluxoMensagemAnterior.TipoFasePasso_1 == TipoFasePassoAhPasso.Fase_4)
                    {
                        fluxoMensagemAnterior.TipoFaseBot = TipoFaseBot.FluxoErro;
                        fluxoMensagemAnterior.Texto = "Foi mal, ainda não estou pronto para atender sua solicitação...";
                    }
                }
                else if (primeiraPalavraChave.Identificador == 2)
                {

                    if (!fluxoMensagemAnterior.TipoFasePasso_1.HasValue)
                    {
                        fluxoMensagemAnterior.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_1;
                    }

                    if (fluxoMensagemAnterior.TipoFasePasso_1.HasValue
                        && fluxoMensagemAnterior.TipoFasePasso_1 == TipoFasePassoAhPasso.Fase_3)
                    {
                        fluxoMensagemAnterior.TipoFasePasso_1 = TipoFasePassoAhPasso.Fase_4;
                    }

                }
                else if (primeiraPalavraChave.Identificador == 3)
                {
                    fluxoMensagemAnterior.TipoFaseBot = TipoFaseBot.FluxoErro;
                    fluxoMensagemAnterior.Texto = "Desculpe, mas ainda não estou pronto para tabalhar com isso ... (sou estágiario)";
                }
            }
            else
           if (fluxoMensagemAnterior.TipoFasePasso_1.HasValue
              && fluxoMensagemAnterior.TipoFasePasso_1 == TipoFasePassoAhPasso.Fase_2)
            {
                if (string.IsNullOrEmpty(path))
                {
                    fluxoMensagemAnterior.TipoFaseBot = TipoFaseBot.FluxoErro;
                    fluxoMensagemAnterior.Texto = "Não entendi. Me envia a imagem novamente";
                }
            }
            else
            {
                fluxoMensagemAnterior.Texto = "Não entendi. Me responda novamente. " + MensagemPadrao();
                fluxoMensagemAnterior.TipoFaseBotAnterior = fluxoMensagemAnterior.TipoFaseBot;
                fluxoMensagemAnterior.TipoFaseBot = TipoFaseBot.OhQueNaoSouber;
            }


            return fluxoMensagemAnterior;
        }

        public List<PalavraChaveDTO> DescSimplificadaOperacionalPalavraChave()
        {
            var lst = new List<PalavraChaveDTO>();
            lst.Add(new PalavraChaveDTO { Identificador = 1, Texto = "nao" });
            lst.Add(new PalavraChaveDTO { Identificador = 1, Texto = "não" });
            lst.Add(new PalavraChaveDTO { Identificador = 1, Texto = "n" });
            lst.Add(new PalavraChaveDTO { Identificador = 1, Texto = "negativo" });
            lst.Add(new PalavraChaveDTO { Identificador = 2, Texto = "sim" });
            lst.Add(new PalavraChaveDTO { Identificador = 2, Texto = "s" });
            lst.Add(new PalavraChaveDTO { Identificador = 2, Texto = "ahan" });
            lst.Add(new PalavraChaveDTO { Identificador = 2, Texto = "aham" });
            lst.Add(new PalavraChaveDTO { Identificador = 2, Texto = "claro" });
            lst.Add(new PalavraChaveDTO { Identificador = 2, Texto = "tenho" });
            lst.Add(new PalavraChaveDTO { Identificador = 3, Texto = "simplificado" });
            lst.Add(new PalavraChaveDTO { Identificador = 3, Texto = "simplificada" });
            lst.Add(new PalavraChaveDTO { Identificador = 4, Texto = "completo" });
            lst.Add(new PalavraChaveDTO { Identificador = 4, Texto = "completa" });
            lst.Add(new PalavraChaveDTO { Identificador = 5, Texto = "isenção" });
            lst.Add(new PalavraChaveDTO { Identificador = 5, Texto = "isencao" });




            return lst;
        }
    }
}
