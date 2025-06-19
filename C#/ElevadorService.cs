using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProvaAdmissionalCSharpApisul;
using ProvaAdmissionalCSharpApisul.Models;

namespace ProvaAdmissionalCSharpApisul
{
  public class ElevadorService : IElevadorService
  {
    // Campo privado para armazenar a lista de usos de elevadores
    private readonly List<UsoElevador> _usosElevadores;
    private const int TotalAndares = 16; // Constante para o número total de andares (0 a 15)

    /// <summary>
    /// Construtor da classe ElevadorService.
    /// Recebe a lista de usos de elevadores (já convertida do JSON) e a armazena internamente.
    /// </summary>
    /// <param name="usosElevadores">A lista de usos de elevadores processados.</param>
    public ElevadorService(List<UsoElevador> usosElevadores)
    {
      _usosElevadores = usosElevadores ?? new List<UsoElevador>(); // Se a lista for nula, inicializa com uma lista vazia para evitar erros futuros.
    }

    /// <inheritdoc />
    public List<int> andarMenosUtilizado()
    {
      // Se não houver dados de uso, todos os andares (0-15) são considerados "menos utilizados".
      if (_usosElevadores.Count == 0)
      {
        return [.. Enumerable.Range(0, TotalAndares)];
      }

      // Inicializa um dicionário para contar o uso de cada andar.
      // Todos os andares de 0 a 15 começam com 0 usos.
      var contagemUsoPorAndar = Enumerable.Range(0, TotalAndares).ToDictionary(andar => andar, andar => 0);

      //Contabiliza os usos reais de cada andar.
      foreach (var uso in _usosElevadores)
      {
        // Verifica se o andar do registro é válido (0-15) antes de tentar acessar o dicionário.
        if (contagemUsoPorAndar.ContainsKey(uso.Andar))
        {
          contagemUsoPorAndar[uso.Andar]++;
        }
      }
      //Encontra a menor contagem de uso entre todos os andares.
      int menorContagem = contagemUsoPorAndar.Values.Min();
        
      //Seleciona todos os andares que têm essa menor contagem e os ordena.
      return [.. contagemUsoPorAndar.Where(par => par.Value == menorContagem) // exemplo: [{0, 2}, {1, 2}, ...]
                                  .Select(par => par.Key) // ex: [0, 1, ...] (os andares)
                                  .OrderBy(andar => andar)];
    }

    /// <inheritdoc />
    public List<char> elevadorMaisFrequentado()
    {
      // Se não houver dados de uso, retorna uma lista vazia.
      if (_usosElevadores.Count == 0)
      {
        return [];
      }

      // Agrupa os usos por elevador e conta a frequência de cada um. ex: [{ Elevador.A, Contagem = 5 }]
      var contagemPorElevador = _usosElevadores
          .GroupBy(uso => uso.Elevador) // Agrupa pela propriedade Elevador (enum Models.Elevador) ex: [{Elevador.A, ...usos}]
          .Select(grupo => new { Elevador = grupo.Key, Contagem = grupo.Count() })
          .ToList();

      // Encontra a maior contagem de uso.
      int maxContagem = contagemPorElevador.Max(item => item.Contagem);

      // Seleciona os elevadores que têm essa contagem máxima, converte para char e ordena.
      return [.. contagemPorElevador.Where(item => item.Contagem == maxContagem) // Filtra pelos mais frequentes
                                .Select(item => item.Elevador.ToString()[0]) // Converte o enum Elevador para char (ex: Elevador.A -> "A" -> 'A')
                                .OrderBy(c => c)];
    }

    /// <inheritdoc />
    public List<char> periodoMaiorFluxoElevadorMaisFrequentado()
    {
      // Lista de caracteres dos elevadores mais frequentados.
      List<char> elevadoresMaisFrequentados = elevadorMaisFrequentado();

      // Se não houver elevadores mais frequentados, retorna lista vazia.
      if (elevadoresMaisFrequentados.Count == 0)
      {
        return [];
      }

      // Converte os caracteres de volta para nosso enum Models.Elevador para facilitar a filtragem. ex: ['A', 'B'] -> [Elevador.A, Elevador.B]
      List<Elevador> elevadoresMaisFreqEnums = [.. elevadoresMaisFrequentados.Select(c => Enum.Parse<Elevador>(c.ToString(), true))];

      var periodosDePico = new HashSet<char>(); // Um objeto vazio com HashSet para evitar duplicatas.

      // Analisa cada elevador mais frequentado.
      foreach (var elevadorEnum in elevadoresMaisFreqEnums)
      {
        var usosDoElevadorEspecifico = _usosElevadores.Where(u => u.Elevador == elevadorEnum);

        var contagemPorPeriodo = usosDoElevadorEspecifico
            .GroupBy(u => u.Periodo) //ex: Agrupa os usos por período { Periodo.Manha: {UsoElevador, UsoElevador, ... }, ...}
            .Select(g => new { Periodo = g.Key, Contagem = g.Count() }) // ex: [{ Periodo.Manha, Contagem = 10 }, { Periodo.Tarde, Contagem = 15 }, ...]
            .ToList();
        
        if (contagemPorPeriodo.Count == 0) continue;

        int maxContagemPeriodo = contagemPorPeriodo.Max(p => p.Contagem); // Encontra a maior contagem de uso para o elevador atual.
        contagemPorPeriodo.Where(p => p.Contagem == maxContagemPeriodo) // ex: { Periodo = Periodo.Tarde, Contagem = 15 }
                          .Select(p => p.Periodo.ToString()[0]) // Converte Models.Periodo para char
                          .ToList()
                          .ForEach(c => periodosDePico.Add(c)); //ex: { 'T' }
      }
      // Ordena e retorna a lista de períodos de pico.
      return [.. periodosDePico.OrderBy(c => c)];
    }

    /// <inheritdoc />
    public List<char> elevadorMenosFrequentado()
    {
      // Obtém todos os elevadores definidos no enum Models.Elevador.
      var todosOsElevadores = Enum.GetValues<Elevador>();

      // Se não houver dados de uso, todos os elevadores são "menos frequentados" com 0 usos.
      if (_usosElevadores.Count == 0)
      {
        return [.. todosOsElevadores.Select(e => e.ToString()[0]).OrderBy(c => c)]; // ex: ['A', 'B', 'C', 'D', 'E']
      }

      // Contabiliza os usos para cada elevador, incluindo aqueles com 0 usos.
      var contagemPorElevador = todosOsElevadores
          .Select(elevador => new
          {
            Elevador = elevador,
            Contagem = _usosElevadores.Count(uso => uso.Elevador == elevador)
          }) // ex: [{ Elevador.A, Contagem = 5 }, { Elevador.B, Contagem = 3 }, ...]
          .ToList();

      // Encontra a menor contagem de uso.
      int minContagem = contagemPorElevador.Min(item => item.Contagem);

      // Seleciona os elevadores que têm essa contagem mínima, converte para char e ordena.
      return [.. contagemPorElevador.Where(item => item.Contagem == minContagem)
                                .Select(item => item.Elevador.ToString()[0])
                                .OrderBy(c => c)];
    }

    /// <inheritdoc />
    public List<char> periodoMenorFluxoElevadorMenosFrequentado()
    {
      // Obtém a lista de caracteres dos elevadores menos frequentados.
      List<char> elevadoresMenosFreqChars = elevadorMenosFrequentado();

      // Se não houver elevadores menos frequentados, retorna lista vazia.
      if (elevadoresMenosFreqChars.Count == 0)
      {
        return [];
      }

      // Converte os caracteres de volta para nosso enum Models.Elevador.
      List<Elevador> elevadoresMenosFreqEnums = [.. elevadoresMenosFreqChars.Select(c => Enum.Parse<Elevador>(c.ToString(), true))];

      var periodosDeMenorFluxo = new HashSet<char>();

      var todosOsPeriodos = Enum.GetValues<Periodo>(); // Obtém M, V, N

      // Analisa cada elevador menos frequentado.
      foreach (var elevadorEnum in elevadoresMenosFreqEnums)
      {
        // Filtra os usos apenas para o elevador atual.
        var usosDoElevadorEspecifico = _usosElevadores.Where(u => u.Elevador == elevadorEnum).ToList();

        // Contabiliza os usos para cada período (M, V, N), incluindo períodos com 0 usos para este elevador.
        var contagemPorPeriodo = todosOsPeriodos
            .Select(periodo => new
            {
              Periodo = periodo,
              Contagem = usosDoElevadorEspecifico.Count(u => u.Periodo == periodo)
            })
            .ToList();

        int minContagemPeriodo = contagemPorPeriodo.Min(p => p.Contagem);

        contagemPorPeriodo.Where(p => p.Contagem == minContagemPeriodo)
                          .Select(p => p.Periodo.ToString()[0]) // Converte Models.Periodo para char
                          .ToList()
                          .ForEach(c => periodosDeMenorFluxo.Add(c));
      }
      // Ordena e retorna a lista de períodos de menor fluxo.
      return [.. periodosDeMenorFluxo.OrderBy(c => c)];
    }

    /// <inheritdoc />
    public List<char> periodoMaiorUtilizacaoConjuntoElevadores()
    {
      // Se não houver dados de uso, retorna uma lista vazia.
      if (_usosElevadores.Count == 0)
      {
        return [];
      }

      // Agrupa todos os usos por período e conta a frequência de cada um.
      var contagemPorPeriodo = _usosElevadores
          .GroupBy(uso => uso.Periodo) // Agrupa pela propriedade Periodo (nosso enum Models.Periodo)
          .Select(grupo => new { Periodo = grupo.Key, Contagem = grupo.Count() }) //ex: [{ Periodo.Manha, Contagem = 10 }, ...]
          .ToList();

      // Encontra a maior contagem de uso entre os períodos.
      int maxContagem = contagemPorPeriodo.Max(item => item.Contagem);

      // Seleciona os períodos que têm essa contagem máxima, converte para char e ordena.
      return [.. contagemPorPeriodo.Where(item => item.Contagem == maxContagem)
                                .Select(item => item.Periodo.ToString()[0]) // Converte o enum Periodo para char
                                .OrderBy(c => c)];
    }

    /// <summary>
    /// Método auxiliar para calcular o percentual de uso de um elevador específico.
    /// </summary>
    /// <param name="elevador">O enum Models.Elevador para o qual calcular o percentual.</param>
    /// <returns>O percentual de uso como float.</returns>
    private float CalcularPercentual(Elevador elevador)
    {
        // Se não houver dados de uso, o percentual é 0.
        if (_usosElevadores.Count == 0)
        {
            return 0.0f;
        }

        float totalUsos = _usosElevadores.Count;
        
        // Evita divisão por zero, embora _usosElevadores.Count == 0 já trate a maioria dos casos.
        if (totalUsos == 0)
        {
            return 0.0f;
        }

        // Conta quantos usos são para o elevador específico.
        float usosDoElevador = _usosElevadores.Count(u => u.Elevador == elevador);

        // Calcula o percentual. Multiplica por 100.0f para garantir que o resultado seja float.
        return usosDoElevador / totalUsos * 100.0f;
    }

    /// <inheritdoc />
    public float percentualDeUsoElevadorA()
    {
      return CalcularPercentual(Elevador.A);
    }

    /// <inheritdoc />
    public float percentualDeUsoElevadorB()
    {
      return CalcularPercentual(Elevador.B);
    }

    /// <inheritdoc />
    public float percentualDeUsoElevadorC()
    {
      return CalcularPercentual(Elevador.C);
    }

    /// <inheritdoc />
    public float percentualDeUsoElevadorD()
    {
      return CalcularPercentual(Elevador.D);
    }

    /// <inheritdoc />
    public float percentualDeUsoElevadorE()
    {
      return CalcularPercentual(Elevador.E);
    }
  }

}