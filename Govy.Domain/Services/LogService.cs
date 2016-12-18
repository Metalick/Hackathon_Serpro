using Govy.Domain.Abstract;
using Govy.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Configuration;

namespace Govy.Domain.Services
{
    public class LogService : ILogService
    {
        private MongoCollection<Log> logCollection;
        public LogService()
        {
            var conn = ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString;
            var client = new MongoClient(conn);
            var server = client.GetServer();
            var db = server.GetDatabase("Govy");
            logCollection = db.GetCollection<Log>("Log");
        }
        public MongoCursor<Log> GetAllLog()
        {            
            var allLog = logCollection.FindAll().SetSortOrder(SortBy<Log>.Ascending(x => x.Pergunta));
            return allLog;
        }

        public void AtualizarLog(Log _log)
        {
            FluxoDuvidasService _duv = new FluxoDuvidasService();
            _duv.CriarDuvida(new Entities.Duvida() { Pergunta = _log.Pergunta, Resposta = _log.Resposta });
            DeletarLog(_log.Id);
        }

        public Log GetLogById(string id)
        {
            return logCollection.FindOneById(new ObjectId(id));
        }

        public void CriarLog(Log _log)
        {
            logCollection.Insert(_log);
        }

        public void DeletarLog(string id)
        {
            logCollection.Remove(Query.EQ("_id", new ObjectId(id)));
        }
    }
}
