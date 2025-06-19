namespace ProvaAdmissionalCSharpApisul.Controllers
{
  /// <summary>
  /// Define o contrato para o controller que gerencia a análise de uso dos elevadores
  /// e a exibição dos resultados.
  /// </summary>
  public interface IElevadorController
  {
    /// <summary>
    /// Executa todas as análises de uso dos elevadores e exibe os resultados no console.
    /// </summary>
    void ExibirAnaliseCompleta();
  }
}
