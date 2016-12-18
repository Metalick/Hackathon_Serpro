using Govy.Domain.Abstract;
using Govy.Domain.DTOs;
using Govy.Domain.Entities;
using Govy.Domain.Enums;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using System.Linq;
namespace Govy.Domain.Services
{
    public class FluxoDuvidasService : IFluxoDuvidasService
    {
        private MongoCollection<Duvida> DuvidaCollection;

        public FluxoDuvidasService()
        {
            var conn = System.Configuration.ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString;
            var client = new MongoClient(conn);
            var server = client.GetServer();
            var db = server.GetDatabase("Govy");
            DuvidaCollection = db.GetCollection<Duvida>("Duvida");
        }

        public FluxoMensagemDTO RecuperaPassoAhPassoDuvida(FluxoMensagemDTO tipoFase)
        {
            if (string.IsNullOrEmpty(tipoFase.IdResposta))
            {
                tipoFase.Texto = MensagemPadrao();
                tipoFase.TipoFaseBot = TipoFaseBot.FluxoDuvidas;
                return tipoFase;
            }
            // Deve recuperar respostas do banco de dados
            tipoFase.Texto = GetAllDuvidas().Where(x => string.Equals(x.Id, tipoFase.IdResposta)).Select(x=>x.Resposta).First();

            return tipoFase;
        }

        public string MensagemPadrao()
        {
            return "Qual é a sua dúvida ?";
        }

        

        public MongoCursor<Duvida> GetAllDuvidas()
        {
            return DuvidaCollection.FindAll().SetSortOrder(SortBy<Duvida>.Ascending(x => x.Pergunta));
        }

        public void CriarLogDuvida(Log log)
        {
            LogService _log = new LogService();
            _log.CriarLog(log);
        }

        public void CriarDuvida(Duvida duvida)
        {
            DuvidaCollection.Insert(duvida);
        }

        public FluxoMensagemDTO ValidaFluxoDuvidas(FluxoMensagemDTO fluxoMensagemAnterior)
        {

            var palavrasChaves = GetAllDuvidas().ToList();

            var palavrasFluxoDuvidas = palavrasChaves
                                            .Where(x => !string.IsNullOrEmpty(x.Resposta) 
                                            && string.Equals(fluxoMensagemAnterior.Texto, x.Pergunta.ToLower())).ToList();

            if (palavrasFluxoDuvidas.Any())
            {
                fluxoMensagemAnterior.IdResposta = palavrasFluxoDuvidas.First().Id;
            }
            else
            {
                CriarLogDuvida(new Log {Pergunta = fluxoMensagemAnterior.Texto, Resposta = "", Count = 1});
                fluxoMensagemAnterior.Texto = "Não temos essa resposta, mas embreve poderemos te responder.";
                fluxoMensagemAnterior.TipoFaseBotAnterior = fluxoMensagemAnterior.TipoFaseBot;
                fluxoMensagemAnterior.TipoFaseBot = TipoFaseBot.OhQueNaoSouber;
            }

            return fluxoMensagemAnterior;
        }

        public List<string> DuvidasLogPalavraChave()
        {
            var lst = new List<string>();
            lst.Add("qual tipo de ...");
            lst.Add("mas o que significa isso ?");

            return lst;
        }
    }
}
