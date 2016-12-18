using System;

namespace Govy.Domain.Utils
{
    public static class NumeroAleatorioUtil
    {
        public static int RecuperaNumeroAleatorio(int valorTotal)
        {
            Random random = new Random();
            return random.Next(valorTotal);
        }
    }
}
