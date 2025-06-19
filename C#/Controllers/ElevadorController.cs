using System;
using System.Collections.Generic;
using System.Linq; // Para string.Join
using ProvaAdmissionalCSharpApisul.Services; // Para IElevadorService

namespace ProvaAdmissionalCSharpApisul.Controllers
{
    /// <summary>
    /// Implementa o controlador para a análise de uso dos elevadores,
    /// interagindo com o serviço IElevadorService e exibindo os resultados.
    /// </summary>
    public class ElevadorController : IElevadorController
    {
        private readonly IElevadorService _elevadorService;

        /// <summary>
        /// Construtor do ElevadorController.
        /// Recebe uma instância de IElevadorService para interagir com a lógica de análise.
        /// </summary>
        /// <param name="elevadorService">Uma instância de IElevadorService.</param>
        public ElevadorController(IElevadorService elevadorService)
        {
            _elevadorService = elevadorService;
        }

        private void ExibirAndarMenosUtilizado()
        {
            // a. Qual é o andar menos utilizado pelos usuários.
            List<int> andaresMenosUtilizados = _elevadorService.andarMenosUtilizado();
            Console.WriteLine($"a) Andar(es) menos utilizado(s): {string.Join(", ", andaresMenosUtilizados)}\n");
        }

        private void ExibirElevadorMaisFrequentadoEPeriodoPico()
        {
            // b. Qual é o elevador mais frequentado e o período que se encontra maior fluxo;
            List<char> elevadoresMaisFrequentados = _elevadorService.elevadorMaisFrequentado();
            List<char> periodosMaiorFluxo = _elevadorService.periodoMaiorFluxoElevadorMaisFrequentado();

            Console.WriteLine($"b) Elevador(es) mais frequentado(s): {string.Join(", ", elevadoresMaisFrequentados)}");
            if (elevadoresMaisFrequentados.Any())
            {
                Console.WriteLine($"   Período(s) de maior fluxo do(s) elevador(es) mais frequentado(s): {string.Join(", ", periodosMaiorFluxo)}\n");
            }
            else
            {
                Console.WriteLine("b) Não foi possível determinar o elevador mais frequentado ou seu período de maior fluxo.");
            }
        }

        private void ExibirElevadorMenosFrequentadoEPeriodoVale()
        {
            // c. Qual é o elevador menos frequentado e o período que se encontra menor fluxo.
            List<char> elevadoresMenosFrequentados = _elevadorService.elevadorMenosFrequentado();
            List<char> periodosMenorFluxo = _elevadorService.periodoMenorFluxoElevadorMenosFrequentado();

            Console.WriteLine($"c) Elevador(es) menos frequentado(s): {string.Join(", ", elevadoresMenosFrequentados)}");
            if (elevadoresMenosFrequentados.Any())
            {
                Console.WriteLine($"   Período(s) de menor fluxo do(s) elevador(es) menos frequentado(s): {string.Join(", ", periodosMenorFluxo)}\n");
            }
            else
            {
                Console.WriteLine("c) Não foi possível determinar o elevador menos frequentado ou seu período de menor fluxo.");
            }
        }

        private void ExibirPeriodoMaiorUtilizacaoConjunto()
        {
            // d. Qual o período de maior utilização do conjunto de elevadores.
            List<char> periodosMaiorUtilizacao = _elevadorService.periodoMaiorUtilizacaoConjuntoElevadores();
            Console.WriteLine($"d) Período(s) de maior utilização do conjunto de elevadores: {string.Join(", ", periodosMaiorUtilizacao)}");
        }

        private void ExibirPercentuaisDeUso()
        {
            // e. Qual o percentual de uso de cada elevador com relação a todos os serviços prestados.
            Console.WriteLine("\ne) Percentual de uso de cada elevador:");
            float percA = _elevadorService.percentualDeUsoElevadorA();
            float percB = _elevadorService.percentualDeUsoElevadorB();
            float percC = _elevadorService.percentualDeUsoElevadorC();
            float percD = _elevadorService.percentualDeUsoElevadorD();
            float percE = _elevadorService.percentualDeUsoElevadorE();

            Console.WriteLine($"   - Elevador A: {percA:F2}%");
            Console.WriteLine($"   - Elevador B: {percB:F2}%");
            Console.WriteLine($"   - Elevador C: {percC:F2}%");
            Console.WriteLine($"   - Elevador D: {percD:F2}%");
            Console.WriteLine($"   - Elevador E: {percE:F2}%");
        }

        /// <inheritdoc />
        public void ExibirAnaliseCompleta()
        {
            Console.WriteLine("\n--- Resultados da Análise ---\n");
            ExibirAndarMenosUtilizado();
            ExibirElevadorMaisFrequentadoEPeriodoPico();
            ExibirElevadorMenosFrequentadoEPeriodoVale();
            ExibirPeriodoMaiorUtilizacaoConjunto();
            ExibirPercentuaisDeUso();
            Console.WriteLine("\n--- Análise Concluída ---");
        }
    }
}
