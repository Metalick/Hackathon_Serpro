using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Govy.Domain.DTOs
{
    public class DeclaracaoSimplificada
    {
        public string Cpf { get; set; }
        public string NomeCompleto { get; set; }
        public string TotalRendimentos { get; set; }
        public string ImpostoRetidoNaFonte { get; set; }
        public string IndenizacoesPorContrato { get; set; }
        public string DecimoTerceiro { get; set; }
        public string TotalRendimentosTributaveis { get; set; }
        public string Cnpj { get; set; }
        public string NomeEmpresa { get; set; }

        public DeclaracaoSimplificada(string textoOCR)
        {
            try
            {
                // Recuperar o CPF do texto recebido padrao A
                textoOCR = textoOCR.ToLower().Replace("\r\n", "");
                Cpf                     = textoOCR.Substring(textoOCR.IndexOf("cpf ") + 4, 14);
                NomeCompleto            = textoOCR.Substring(textoOCR.IndexOf("nome completo  ") + 4, 14);
                var numeros             = textoOCR.Substring(textoOCR.IndexOf("valores em reais ") , textoOCR.Length- textoOCR.IndexOf("valores em reais ")).Split(' ');
                TotalRendimentos        = numeros[3];
                ImpostoRetidoNaFonte    = numeros[7];
                IndenizacoesPorContrato = numeros[15];
                DecimoTerceiro          = numeros[40];
                TotalRendimentosTributaveis = numeros[130];
                Cnpj                    = numeros[120];
            }
            catch(Exception ex)
            {
                throw new Exception("Desculpe mas esse arquivo possui alguns problemas que dificultam a leitura. Tente enviar uma imagem diferente.");
            }
        }

    }
}
