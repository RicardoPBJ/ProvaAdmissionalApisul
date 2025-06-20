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

        #region Testes para o método elevadorMenosFrequentado
        [Fact]
        public void ElevadorMenosFrequentado_SemDados_DeveRetornarTodosElevadoresOrdenados()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>();
            var service = new ElevadorService(dadosDeUso);
            var elevadoresEsperados = new List<char> { 'A', 'B', 'C', 'D', 'E' };

            // Act
            List<char> resultado = service.elevadorMenosFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(elevadoresEsperados);
        }

        [Fact]
        public void ElevadorMenosFrequentado_ComUmClaramenteMenosUsado_DeveRetornarCorreto()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M }, // A: 3
                new UsoElevador { Elevador = Elevador.A, Andar = 2, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.A, Andar = 3, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.B, Andar = 1, Periodo = Periodo.V }, // B: 2
                new UsoElevador { Elevador = Elevador.B, Andar = 2, Periodo = Periodo.V },
                new UsoElevador { Elevador = Elevador.C, Andar = 1, Periodo = Periodo.N }  // C: 1 (Menos usado)
            };
            var service = new ElevadorService(dadosDeUso);
            // Elevadores D e E não foram usados (0 usos), C foi usado 1 vez.
            // Portanto, D e E são os menos usados.
            var elevadoresEsperados = new List<char> { 'D', 'E' };

            // Act
            List<char> resultado = service.elevadorMenosFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(elevadoresEsperados);
        }

        [Fact]
        public void ElevadorMenosFrequentado_ComEmpateNosMenosUsados_DeveRetornarTodosEmpatadosOrdenados()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M }, // A: 2
                new UsoElevador { Elevador = Elevador.A, Andar = 2, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.B, Andar = 1, Periodo = Periodo.V }, // B: 1
                new UsoElevador { Elevador = Elevador.C, Andar = 1, Periodo = Periodo.N }  // C: 1
            };
            // D e E têm 0 usos. B e C têm 1 uso. A tem 2 usos.
            // D e E são os menos usados.
            var service = new ElevadorService(dadosDeUso);
            var elevadoresEsperados = new List<char> { 'D', 'E' };

            // Act
            List<char> resultado = service.elevadorMenosFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(elevadoresEsperados);
        }

        [Fact]
        public void ElevadorMenosFrequentado_TodosUsadosMesmaQuantidade_DeveRetornarTodos()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.B, Andar = 1, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.C, Andar = 1, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.D, Andar = 1, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.E, Andar = 1, Periodo = Periodo.M }
            };
            var service = new ElevadorService(dadosDeUso);
            var elevadoresEsperados = new List<char> { 'A', 'B', 'C', 'D', 'E' };

            // Act
            List<char> resultado = service.elevadorMenosFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(elevadoresEsperados);
        }
        #endregion

        #region Testes para o método periodoMenorFluxoElevadorMenosFrequentado

        [Fact]
        public void PeriodoMenorFluxoElevadorMenosFrequentado_SemDados_DeveRetornarTodosPeriodos()
        {
            // Arrange
            // Se não há dados, elevadorMenosFrequentado retorna A,B,C,D,E.
            // Para cada um deles, não há usos, então todos os períodos (M,V,N) têm 0 usos.
            // O método deve retornar a união desses períodos (M,V,N).
            var dadosDeUso = new List<UsoElevador>();
            var service = new ElevadorService(dadosDeUso);
            var periodosEsperados = new List<char> { 'M', 'N', 'V' }; // Ordem alfabética

            // Act
            List<char> resultado = service.periodoMenorFluxoElevadorMenosFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(periodosEsperados);
        }

        [Fact]
        public void PeriodoMenorFluxoElevadorMenosFrequentado_UmMenosFrequentadoComPeriodoMenorFluxoClaro_DeveRetornarCorreto()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                // Elevador A (mais usado)
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M },
                // Elevador B (menos usado)
                new UsoElevador { Elevador = Elevador.B, Andar = 1, Periodo = Periodo.M }, // B: M=1
                new UsoElevador { Elevador = Elevador.B, Andar = 1, Periodo = Periodo.V }  // B: V=1
                // Elevador B é um dos menos frequentados (junto com C,D,E que tem 0 usos).
                // Para B: M=1, V=1, N=0. Período de menor fluxo para B é N.
                // Para C,D,E: M=0, V=0, N=0. Períodos de menor fluxo são M,V,N.
                // A união é M,V,N.
            };
            var service = new ElevadorService(dadosDeUso);
            var periodosEsperados = new List<char> { 'M', 'N', 'V' };

            // Act
            List<char> resultado = service.periodoMenorFluxoElevadorMenosFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(periodosEsperados);
        }

        [Fact]
        public void PeriodoMenorFluxoElevadorMenosFrequentado_EmpateMenosFrequentadosComPeriodosDiferentes_DeveRetornarUniaoOrdenada()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                // Elevador A (mais usado)
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M },
                // Elevadores B e C (menos usados, empatados com 1 uso cada)
                // D e E têm 0 usos, então são os menos frequentados.
                // Para D e E, os períodos de menor fluxo são M, V, N (0 usos cada).
                new UsoElevador { Elevador = Elevador.B, Andar = 1, Periodo = Periodo.M }, // B: M=1, V=0, N=0. Menor fluxo para B: V, N
                new UsoElevador { Elevador = Elevador.C, Andar = 1, Periodo = Periodo.V }  // C: M=0, V=1, N=0. Menor fluxo para C: M, N
            };
            // Elevadores D e E são os menos frequentados (0 usos).
            // Para D: M=0, V=0, N=0. Menor fluxo: M,V,N
            // Para E: M=0, V=0, N=0. Menor fluxo: M,V,N
            // A união dos períodos de menor fluxo é M, V, N.
            var service = new ElevadorService(dadosDeUso);
            var periodosEsperados = new List<char> { 'M', 'N', 'V' };

            // Act
            List<char> resultado = service.periodoMenorFluxoElevadorMenosFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(periodosEsperados);
        }

        [Fact]
        public void PeriodoMenorFluxoElevadorMenosFrequentado_MenosFrequentadoComTodosPeriodosZeroUsos_DeveRetornarTodosPeriodos()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                // Elevador A usado algumas vezes
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.A, Andar = 2, Periodo = Periodo.V },
                // Elevador B não é usado (0 usos), então é o menos frequentado junto com C,D,E.
                // Para B, C, D, E: M=0, V=0, N=0. Menor fluxo para eles é M,V,N.
            };
            var service = new ElevadorService(dadosDeUso);
            var periodosEsperados = new List<char> { 'M', 'N', 'V' };

            // Act
            List<char> resultado = service.periodoMenorFluxoElevadorMenosFrequentado();

            // Assert
            resultado.Should().BeEquivalentTo(periodosEsperados);
        }
        #endregion

        #region Testes para o método periodoMaiorUtilizacaoConjuntoElevadores

        [Fact]
        public void PeriodoMaiorUtilizacaoConjuntoElevadores_SemDados_DeveRetornarListaVazia()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>();
            var service = new ElevadorService(dadosDeUso);
            var periodosEsperados = new List<char>();

            // Act
            List<char> resultado = service.periodoMaiorUtilizacaoConjuntoElevadores();

            // Assert
            resultado.Should().BeEquivalentTo(periodosEsperados);
        }

        [Fact]
        public void PeriodoMaiorUtilizacaoConjuntoElevadores_ComUmPeriodoMaisUtilizado_DeveRetornarCorreto()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M }, // M: 3
                new UsoElevador { Elevador = Elevador.B, Andar = 2, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.C, Andar = 3, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.V }, // V: 2
                new UsoElevador { Elevador = Elevador.D, Andar = 2, Periodo = Periodo.V },
                new UsoElevador { Elevador = Elevador.E, Andar = 1, Periodo = Periodo.N }  // N: 1
            };
            var service = new ElevadorService(dadosDeUso);
            var periodosEsperados = new List<char> { 'M' };

            // Act
            List<char> resultado = service.periodoMaiorUtilizacaoConjuntoElevadores();

            // Assert
            resultado.Should().BeEquivalentTo(periodosEsperados);
        }

        [Fact]
        public void PeriodoMaiorUtilizacaoConjuntoElevadores_ComEmpateNosPeriodos_DeveRetornarTodosEmpatadosOrdenados()
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>
            {
                new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M }, // M: 2
                new UsoElevador { Elevador = Elevador.B, Andar = 2, Periodo = Periodo.M },
                new UsoElevador { Elevador = Elevador.C, Andar = 1, Periodo = Periodo.V }, // V: 2
                new UsoElevador { Elevador = Elevador.D, Andar = 2, Periodo = Periodo.V },
                new UsoElevador { Elevador = Elevador.E, Andar = 1, Periodo = Periodo.N }  // N: 1
            };
            var service = new ElevadorService(dadosDeUso);
            var periodosEsperados = new List<char> { 'M', 'V' }; // Esperado em ordem alfabética

            // Act
            List<char> resultado = service.periodoMaiorUtilizacaoConjuntoElevadores();

            // Assert
            resultado.Should().BeEquivalentTo(periodosEsperados);
        }
        #endregion

        #region Testes para os métodos de percentual de uso dos elevadores

        [Theory]
        [InlineData(0, 0.0f)] // Sem usos, percentual deve ser 0
        [InlineData(1, 100.0f)] // Apenas 1 uso do elevador A, percentual deve ser 100%
        [InlineData(2, 100.0f)] // Apenas 2 usos do elevador A
        [InlineData(3, 66.67f)] // 2 usos de A, 1 de outro, percentual de A = 2/3 * 100
        [InlineData(4, 50.0f)]  // 2 usos de A, 2 de outros, percentual de A = 2/4 * 100
        public void PercentualDeUsoElevadores_ComDiferentesUsos_DeveRetornarCorreto(int totalUsos, float percentualEsperado)
        {
            // Arrange
            var dadosDeUso = new List<UsoElevador>();
            if (totalUsos > 0)
            {
                dadosDeUso.Add(new UsoElevador { Elevador = Elevador.A, Andar = 1, Periodo = Periodo.M });
                if (totalUsos > 1)
                {
                    dadosDeUso.Add(new UsoElevador { Elevador = Elevador.A, Andar = 2, Periodo = Periodo.V });
                }
                // Adiciona usos de outros elevadores se totalUsos > 2
                for (int i = 2; i < totalUsos; i++)
                {
                    // Alterna entre B, C, D, E para variar os usos
                    var elevador = (Elevador)((i % 4) + 1); // (i%4) gera 0, 1, 2, 3. +1 mapeia para B, C, D, E (1, 2, 3, 4)
                    dadosDeUso.Add(new UsoElevador { Elevador = elevador, Andar = i, Periodo = Periodo.N });
                }
            }
            var service = new ElevadorService(dadosDeUso);

            // Act
            float resultado = service.percentualDeUsoElevadorA();

            // Assert
            resultado.Should().BeApproximately(percentualEsperado, 0.01f); // Usa precisão de 2 casas decimais
        }
        #endregion
    }
    #endregion
}