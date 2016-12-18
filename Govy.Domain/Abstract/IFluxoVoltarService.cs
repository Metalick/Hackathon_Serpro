using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Govy.Domain.DTOs;

namespace Govy.Domain.Abstract
{
    public interface IFluxoVoltarService
    {
        FluxoMensagemDTO ValidaFluxoVoltar(FluxoMensagemDTO fluxoAtual);
    }
}
