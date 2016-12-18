using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Govy.Domain.Enums;

namespace Govy.Domain.DTOs
{
    public class MensagemDTO
    {
        public string Texto { get; set; }
        public string TextoAnterior { get; set; }
        public TipoFaseBot TipoFaseBot { get; set; }
        public TipoFaseBot TipoFaseBotAnterior { get; set; }
        public TipoFasePassoAhPasso? TipoFasePasso_1;
        public TipoFasePassoAhPasso? TipoFasePasso_2;
        public TipoFasePassoAhPasso? TipoFasePasso_3;
        public TipoFasePassoAhPasso? TipoFasePasso_1Anterior;
        public TipoFasePassoAhPasso? TipoFasePasso_2Anterior;
        public TipoFasePassoAhPasso? TipoFasePasso_3Anterior;
    }
}
