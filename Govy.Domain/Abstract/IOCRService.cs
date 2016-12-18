using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Govy.Domain.Abstract
{
    public interface IOCRService
    {
        System.Threading.Tasks.Task<string> GetDocumentData(string contentUrl);
    }
}
