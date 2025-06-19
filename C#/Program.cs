using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json; // Para JsonSerializer
using ProvaAdmissionalCSharpApisul.Models;
using ProvaAdmissionalCSharpApisul;

public class Program
{
    /// <summary>
    /// Método principal que inicia o programa, carrega os dados do arquivo JSON e executa a análise de uso dos elevadores.
    /// </summary>
    public static void Main(string[] args)
    {
        Console.WriteLine("Sistema de Análise de Uso de Elevadores - Prova Admissional Apisul");
        Console.WriteLine("------------------------------------------------------------");

        // Caminho para o arquivo de entrada JSON.
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.json");

        // Verifica se o arquivo existe no diretório de saída
        if (!File.Exists(jsonFilePath))
        {
            Console.WriteLine($"Erro: Arquivo de entrada não encontrado em '{jsonFilePath}'");
            Console.WriteLine("Verifique o caminho e se o arquivo input.json existe e está configurado para ser copiado para o diretório de saída.");
            return;
        }

        // Carregar e processar os dados do arquivo JSON
        List<UsoElevador>? dadosDeUsoProcessados = CarregarEProcessarDados(jsonFilePath);

        // Verifica se os dados foram carregados e processados corretamente
        if (dadosDeUsoProcessados == null)
        {
            Console.WriteLine("Não foi possível carregar ou processar os dados do arquivo de entrada.");
            return;
        }

        // Verifica se a lista de dados processados está vazia
        if (dadosDeUsoProcessados.Count == 0)
        {
            Console.WriteLine("Nenhum dado de uso válido foi encontrado no arquivo de entrada ou o arquivo está vazio.");
        }

        // Cria uma instância do serviço de elevadores com os dados processados
        IElevadorService elevadorService = new ElevadorService(dadosDeUsoProcessados);

        Console.WriteLine($"\n{dadosDeUsoProcessados.Count} registros de uso carregados e processados.");

        // Testando o método andarMenosUtilizado()
        List<int> andaresMenosUtilizados = elevadorService.andarMenosUtilizado();
        Console.WriteLine("\n--- Resultados ---\n");
        Console.WriteLine($"a) Andar menos utilizado: {string.Join(", ", andaresMenosUtilizados)}");

        // Testando o método elevadorMaisFrequentado()
        List<char> elevadoresMaisFrequentados = elevadorService.elevadorMaisFrequentado();
        Console.WriteLine($"b) Elevador mais frequentado: {string.Join(", ", elevadoresMaisFrequentados)}");

        // Testando o método periodoMaiorFluxoElevadorMaisFrequentado()
        List<char> periodosMaiorFluxo = elevadorService.periodoMaiorFluxoElevadorMaisFrequentado();
        Console.WriteLine($"c) Período de maior fluxo do elevador mais frequentado: {string.Join(", ", periodosMaiorFluxo)}");

        // Testando o método elevadorMenosFrequentado()
        List<char> elevadoresMenosFrequentados = elevadorService.elevadorMenosFrequentado();
        Console.WriteLine($"d) Elevador menos frequentado: {string.Join(", ", elevadoresMenosFrequentados)}");

        // Testando o método periodoMenorFluxoElevadorMenosFrequentado()
        List<char> periodosMenorFluxo = elevadorService.periodoMenorFluxoElevadorMenosFrequentado();
        Console.WriteLine($"e) Período de menor fluxo do elevador menos frequentado: {string.Join(", ", periodosMenorFluxo)}");

        // Testando o método periodoMaiorUtilizacaoConjuntoElevadores()
        List<char> periodoMaiorUtilizacao = elevadorService.periodoMaiorUtilizacaoConjuntoElevadores();
        Console.WriteLine($"f) Período de maior utilização do conjunto de elevadores: {string.Join(", ", periodoMaiorUtilizacao)}");

        Console.WriteLine("\n------------------------------------------------------------");
    }

    /// <summary>
    /// Carrega os dados do arquivo JSON, desserializa e converte para o modelo interno UsoElevador.
    /// </summary>
    /// <param name="filePath">O caminho para o arquivo JSON.</param>
    /// <returns>Uma lista de UsoElevador ou null em caso de erro crítico de leitura/desserialização.</returns>
    public static List<UsoElevador>? CarregarEProcessarDados(string filePath)
    {
        try
        {
            // Lê o conteúdo do arquivo JSON
            string jsonData = File.ReadAllText(filePath);
            // Desserializa o JSON para uma lista de RegistroInputJson
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; 
            List<RegistroInputJson>? registrosJson = JsonSerializer.Deserialize<List<RegistroInputJson>>(jsonData, options);

            if (registrosJson == null) return new List<UsoElevador>(); // Retorna lista vazia se a desserialização resultar em nulo

            // Converter para o nosso modelo interno UsoElevador (com enums)
            return [.. registrosJson.Select(regJson =>
            {
                // Converte as strings do JSON para os enums definidos
                Enum.TryParse<Elevador>(regJson.Elevador, true, out Elevador elevadorEnum);
                Enum.TryParse<Periodo>(regJson.Turno, true, out Periodo periodoEnum);
                return new UsoElevador { Andar = regJson.Andar, Elevador = elevadorEnum, Periodo = periodoEnum };
            })];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ao carregar ou processar os dados: {ex.Message}");
            return null;
        }
    }
}
