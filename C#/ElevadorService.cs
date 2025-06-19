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
      if (!_usosElevadores.Any())
      {
        return Enumerable.Range(0, TotalAndares).ToList();
      }

      // Inicializa um dicionário para contar o uso de cada andar.
      // Todos os andares de 0 a 15 começam com 0 usos.
      var contagemUsoPorAndar = Enumerable.Range(0, TotalAndares).ToDictionary(andar => andar, andar => 0);
      Console.WriteLine("Contagem de uso por andar inicializada com 0 para todos os andares.", contagemUsoPorAndar);

      //Contabiliza os usos reais de cada andar.
      foreach (var uso in _usosElevadores)
      {
        // Verifica se o andar do registro é válido (0-15) antes de tentar acessar o dicionário.
        // Embora o dicionário já contenha todos os andares, esta é uma boa prática de defesa.
        if (contagemUsoPorAndar.ContainsKey(uso.Andar))
        {
          contagemUsoPorAndar[uso.Andar]++;
        }
      }
        //Encontra a menor contagem de uso entre todos os andares.
        int menorContagem = contagemUsoPorAndar.Values.Min();

        //Seleciona todos os andares que têm essa menor contagem e os ordena.
        return contagemUsoPorAndar.Where(par => par.Value == menorContagem)
                                  .Select(par => par.Key)
                                  .OrderBy(andar => andar)
                                  .ToList();
    }

    public List<char> elevadorMaisFrequentado()
    {
      // Implementação do método
      throw new NotImplementedException();
    }

    public List<char> periodoMaiorFluxoElevadorMaisFrequentado()
    {
      // Implementação do método
      throw new NotImplementedException();
    }

    public List<char> elevadorMenosFrequentado()
    {
      // Implementação do método
      throw new NotImplementedException();
    }

    public List<char> periodoMenorFluxoElevadorMenosFrequentado()
    {
      // Implementação do método
      throw new NotImplementedException();
    }

    public List<char> periodoMaiorUtilizacaoConjuntoElevadores()
    {
      // Implementação do método
      throw new NotImplementedException();
    }

    public float percentualDeUsoElevadorA()
    {
      // Implementação do método
      throw new NotImplementedException();
    }

    public float percentualDeUsoElevadorB()
    {
      // Implementação do método
      throw new NotImplementedException();
    }

    public float percentualDeUsoElevadorC()
    {
      // Implementação do método
      throw new NotImplementedException();
    }

    public float percentualDeUsoElevadorD()
    {
      // Implementação do método
      throw new NotImplementedException();
    }

    public float percentualDeUsoElevadorE()
    {
      // Implementação do método
      throw new NotImplementedException();
    }
  }

}