using Xunit;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ProvaAdmissionalCSharpApisul.Models;
using ProvaAdmissionalCSharpApisul.Services;

namespace ProvaAdmissionalApisul.Tests
{
    #region Testes para o serviço ElevadorService
    public class ElevadorServiceTests
    {
        #region Testes para o método andarMenosUtilizado
        [Fact]
        public void AndarMenosUtilizado_QuandoNaoHaUsos_DeveRetornarTodosOsAndares()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>();
            var service = new ElevadorService(dadosDeUso);
            var andaresEsperados = Enumerable.Range(0, 16).ToList(); // 0 a 15

            // Act
            List<int> resultado = service.andarMenosUtilizado();

            // Assert
            resultado.OrderBy(x => x).Should().BeEquivalentTo(andaresEsperados.OrderBy(x => x));
        }

        [Fact]
        public void AndarMenosUtilizado_ComUmAndarNaoUtilizado_DeveRetornarApenasEsseAndar()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Andar = 0, Elevador = Elevador.A, Periodo = Periodo.M },
                new UsoElevador { Andar = 1, Elevador = Elevador.A, Periodo = Periodo.M },
                // Andar 2 não é usado, outros andares de 3 a 15 também não.
            };
            var service = new ElevadorService(dadosDeUso);
            // Espera-se que os andares 2 a 15 sejam os menos utilizados (0 usos)
            var andaresEsperados = Enumerable.Range(2, 14).ToList(); // 2 a 15

            // Act
            List<int> resultado = service.andarMenosUtilizado();

            // Assert
            resultado.OrderBy(x => x).Should().BeEquivalentTo(andaresEsperados.OrderBy(x => x));
        }

        [Fact]
        public void AndarMenosUtilizado_ComTodosAndaresUtilizadosUmaVezExcetoUm_DeveRetornarAndarCorreto()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>();
            for (int i = 0; i < 16; i++)
            {
                if (i != 5) // Andar 5 será o menos utilizado (0 usos)
                {
                    dadosDeUso.Add(new UsoElevador { Andar = i, Elevador = Elevador.A, Periodo = Periodo.M });
                }
            }
            var service = new ElevadorService(dadosDeUso);
            var andaresEsperados = new List<int> { 5 };

            // Act
            List<int> resultado = service.andarMenosUtilizado();

            // Assert
            resultado.Should().BeEquivalentTo(andaresEsperados);
        }

        [Fact]
        public void AndarMenosUtilizado_ComEmpateNaMenorUtilizacao_DeveRetornarTodosEmpatados()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Andar = 0, Elevador = Elevador.A, Periodo = Periodo.M }, // 1 uso
                new UsoElevador { Andar = 1, Elevador = Elevador.A, Periodo = Periodo.M }, // 2 usos
                new UsoElevador { Andar = 1, Elevador = Elevador.B, Periodo = Periodo.V },
                new UsoElevador { Andar = 2, Elevador = Elevador.C, Periodo = Periodo.N }, // 1 uso
                new UsoElevador { Andar = 3, Elevador = Elevador.D, Periodo = Periodo.M }, // 2 usos
                new UsoElevador { Andar = 3, Elevador = Elevador.E, Periodo = Periodo.V },
                // Andar 0 e 2 têm 1 uso. Andares 4-15 têm 0 usos.
            };
            var service = new ElevadorService(dadosDeUso);
            // Espera-se que os andares 4 a 15 sejam os menos utilizados (0 usos)
            var andaresEsperados = Enumerable.Range(4, 12).ToList();

            // Act
            List<int> resultado = service.andarMenosUtilizado();

            // Assert
            resultado.OrderBy(x => x).Should().BeEquivalentTo(andaresEsperados.OrderBy(x => x));
        }
        #endregion

        #region Testes para o método elevadorMaisFrequentado

        [Fact]
        public void ElevadorMaisFrequentado_ComDadosSimples_DeveRetornarElevadorCorreto()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Andar = 0, Elevador = Elevador.A, Periodo = Periodo.M }, // A: 2
                new UsoElevador { Andar = 1, Elevador = Elevador.A, Periodo = Periodo.M },
                new UsoElevador { Andar = 1, Elevador = Elevador.B, Periodo = Periodo.V }, // B: 1
                new UsoElevador { Andar = 2, Elevador = Elevador.C, Periodo = Periodo.N }  // C: 1
            };
            var service = new ElevadorService(dadosDeUso);
            var elevadoresEsperados = new List<char> { 'A' };

            // Act
            List<char> resultado = service.elevadorMaisFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(elevadoresEsperados);
        }

        [Fact]
        public void ElevadorMaisFrequentado_ComEmpate_DeveRetornarAmbosOrdenados()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Andar = 0, Elevador = Elevador.A, Periodo = Periodo.M }, // A: 2
                new UsoElevador { Andar = 1, Elevador = Elevador.A, Periodo = Periodo.M },
                new UsoElevador { Andar = 1, Elevador = Elevador.B, Periodo = Periodo.V }, // B: 2
                new UsoElevador { Andar = 2, Elevador = Elevador.B, Periodo = Periodo.N },
                new UsoElevador { Andar = 3, Elevador = Elevador.C, Periodo = Periodo.M }  // C: 1
            };
            var service = new ElevadorService(dadosDeUso);
            var elevadoresEsperados = new List<char> { 'A', 'B' }; // Esperado em ordem alfabética

            // Act
            List<char> resultado = service.elevadorMaisFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(elevadoresEsperados);
        }

        [Fact]
        public void ElevadorMaisFrequentado_SemDados_DeveRetornarListaVazia()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>();
            var service = new ElevadorService(dadosDeUso);
            var elevadoresEsperados = new List<char>();

            // Act
            List<char> resultado = service.elevadorMaisFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(elevadoresEsperados);
        }
        #endregion

        #region Testes para o método periodoMaiorFluxoElevadorMaisFrequentado
        [Fact]
        public void PeriodoMaiorFluxoElevadorMaisFrequentado_ComUmMaisFrequentado_DeveRetornarPeriodoCorreto()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Andar = 0, Elevador = Elevador.A, Periodo = Periodo.M }, // A: M=2, V=1
                new UsoElevador { Andar = 1, Elevador = Elevador.A, Periodo = Periodo.M },
                new UsoElevador { Andar = 2, Elevador = Elevador.A, Periodo = Periodo.V },
                new UsoElevador { Andar = 3, Elevador = Elevador.B, Periodo = Periodo.N }  // B: N=1
            };
            // Elevador A é o mais frequentado. Período de maior fluxo para A é M.
            var service = new ElevadorService(dadosDeUso);
            var periodosEsperados = new List<char> { 'M' };

            // Act
            List<char> resultado = service.periodoMaiorFluxoElevadorMaisFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(periodosEsperados);
        }

        [Fact]
        public void PeriodoMaiorFluxoElevadorMaisFrequentado_ComEmpateNosMaisFrequentados_DeveRetornarUniaoDosPeriodosDePicoOrdenados()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Andar = 0, Elevador = Elevador.A, Periodo = Periodo.M }, // A: M=2 (Pico A)
                new UsoElevador { Andar = 1, Elevador = Elevador.A, Periodo = Periodo.M },
                new UsoElevador { Andar = 2, Elevador = Elevador.A, Periodo = Periodo.V }, // A: V=1
                new UsoElevador { Andar = 3, Elevador = Elevador.B, Periodo = Periodo.N }, // B: N=2 (Pico B)
                new UsoElevador { Andar = 4, Elevador = Elevador.B, Periodo = Periodo.N },
                new UsoElevador { Andar = 5, Elevador = Elevador.B, Periodo = Periodo.M }  // B: M=1
            };
            // Elevadores A e B são os mais frequentados (3 usos cada).
            // Pico de A é M. Pico de B é N.
            var service = new ElevadorService(dadosDeUso);
            var periodosEsperados = new List<char> { 'M', 'N' }; // Esperado em ordem alfabética

            // Act
            List<char> resultado = service.periodoMaiorFluxoElevadorMaisFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(periodosEsperados);
        }
        #endregion

    }
    #endregion
}