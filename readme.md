# Teste admissional prático da Apisul

Este projeto contém a solução para o teste admissional prático da Apisul, focado na análise de uso de elevadores.

**Descrição do Problema Original:**
Suponha que a administração do prédio 99a da Tecnopuc, com 16 andares e cinco elevadores, denominados A, B, C, D e E, nos convidou a aperfeiçoar o sistema de controle dos elevadores. Depois de realizado um levantamento no qual cada usuário respondia:  
 a. O elevador que utiliza com mais frequência (A, B, C, D ou E);  
 b. O andar ao qual se dirigia (0 a 15);  
 c. O período que utilizava o elevador – M: Matutino; V: Vespertino; N: Noturno.

Considerando que este possa evoluir para um sistema dinâmico, escreva o código que nos ajude a extrair as seguintes informações:  
 a. Qual é o andar menos utilizado pelos usuários;  
 b. Qual é o elevador mais frequentado e o período que se encontra maior fluxo;  
 c. Qual é o elevador menos frequentado e o período que se encontra menor fluxo;  
 d. Qual o período de maior utilização do conjunto de elevadores;  
 e. Qual o percentual de uso de cada elevador com relação a todos os serviços prestados;

---

## Solução Implementada

Esta solução foi desenvolvida em C# (.NET 8) e segue os requisitos especificados, incluindo a implementação da interface `IElevadorService` e a leitura do arquivo `input.json`.

### Estrutura do Projeto

- `C#/`: Contém o código-fonte da aplicação principal (Models, Services, Controllers, Program.cs).
- `TestsC#/`: Contém o projeto de testes unitários para a lógica de negócio (`ElevadorServiceTests.cs`).
- `input.json`: Arquivo de entrada com os dados de uso dos elevadores.
- `ProvaAdmissionalApisul.sln`: Arquivo de solução do Visual Studio/dotnet CLI que agrupa os projetos principal e de testes.

* Deve ser programado em Java ou C#.

* Para a realização do exercício você deve implementar a interface IElevadorService.

* Faça a leitura do arquivo input.json para ter acesso às entradas.

### Pré-requisitos

- .NET SDK 8.0 ou superior instalado.

### Como Construir (Build)

Navegue até o diretório raiz do projeto (`/ProvaAdmissionalApisul/`) no terminal e execute:

```bash
dotnet build
```

### Como Executar a Aplicação

Ainda no diretório raiz do projeto (`/ProvaAdmissionalApisul/`), execute:

```bash
dotnet run --project C#/ProvaAdmissionalApisul.csproj
```

O arquivo `input.json` será automaticamente copiado para o diretório de saída da aplicação durante o build.

- +### Como Executar os Testes Unitários
- +Os testes unitários foram implementados usando xUnit e FluentAssertions para garantir a correção da lógica no `ElevadorService`.
- +No diretório raiz do projeto (`/ProvaAdmissionalApisul/`), execute:
- +`bash
+dotnet test
+`
-
