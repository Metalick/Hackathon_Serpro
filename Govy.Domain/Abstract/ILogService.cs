using Govy.Domain.Entities;
using MongoDB.Driver;

namespace Govy.Domain.Abstract
{
    public interface ILogService
    {
        MongoCursor<Log> GetAllLog();

        void AtualizarLog(Log _log);

        Log GetLogById(string id);

        void CriarLog(Log _log);

        void DeletarLog(string id);
    }
}
