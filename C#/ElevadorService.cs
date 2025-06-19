using System;
using System.Collections.Generic;
using ProvaAdmissionalCSharpApisul.Interfaces;
using ProvaAdmissionalCSharpApisul.Models;

namespace ProvaAdmissionalCSharpApisul
{
  public class ElevadorService : IElevadorService
  {
    // Campo privado para armazenar a lista de usos de elevadores
    private readonly List<UsoElevador> _usosElevadores;

    /// <summary>
    /// Construtor da classe ElevadorService.
    /// Recebe a lista de usos de elevadores (já convertida do JSON) e a armazena internamente.
    /// </summary>
    /// <param name="usosElevadores">A lista de usos de elevadores processados.</param>
    public ElevadorService(List<UsoElevador> usosElevadores)
    {
        _usosElevadores = usosElevadores ?? new List<UsoElevador>(); // Se a lista for nula, inicializa com uma lista vazia para evitar erros futuros.
    }
    public List<int> andarMenosUtilizado()
    {
      // Implementação do método
      throw new NotImplementedException();
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