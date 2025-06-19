namespace ProvaAdmissionalCSharpApisul.Models
{
    /// <summary>
    /// Representa um registro individual de utilização de um elevador,
    /// utilizando os enums Elevador e Periodo para tipagem forte interna.
    /// Esta classe será usada pela lógica de serviço após a conversão dos dados do JSON.
    /// </summary>
    public class UsoElevador
    {
        /// <summary>
        /// O andar ao qual o usuário se dirigiu (0 a 15).
        /// </summary>
        public int Andar { get; set; }

        /// <summary>
        /// O elevador que o usuário utilizou.
        /// </summary>
        public Elevador Elevador { get; set; }

        /// <summary>
        /// O período em que o elevador foi utilizado.
        /// </summary>
        public Periodo Periodo { get; set; }
    }
}