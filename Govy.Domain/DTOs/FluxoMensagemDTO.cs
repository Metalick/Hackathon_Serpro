using Govy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Govy.Domain.DTOs
{
   public class FluxoMensagemDTO
    {
        public int PosicaoPerguntaAnterior { get; set; }

        public string Texto { get; set; }

        public TipoFaseBot TipoFaseBot { get; set; }
        public string IdResposta { get; internal set; }
        public TipoFaseBot TipoFaseBotAnterior { get; internal set; }

        public TipoFasePassoAhPasso? TipoFasePasso_1;
        public TipoFasePassoAhPasso? TipoFasePasso_2;
        public TipoFasePassoAhPasso? TipoFasePasso_3;

    }
}
