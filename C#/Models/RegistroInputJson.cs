namespace ProvaAdmissionalApisul.Models
{
    /// <summary>
    /// Representa a estrutura de um único registro conforme definido no arquivo input.json.
    /// Esta classe é usada especificamente para desserializar os dados brutos do JSON.
    /// </summary>
    public class RegistroInputJson
    {
        /// <summary>
        /// o JsonPropertyName é usado para mapear a propriedade C# para o nome da propriedade no JSON.
        /// O andar ao qual o usuário se dirigiu.
        /// Mapeado da propriedade "andar" no JSON.
        /// </summary>
        [JsonPropertyName("andar")]
        public int Andar { get; set; }

        /// <summary>
        /// O elevador que o usuário utilizou (representado como string no JSON: "A", "B", etc.).
        /// Mapeado da propriedade "elevador" no JSON.
        /// </summary>
        [JsonPropertyName("elevador")]
        public string Elevador { get; set; } // Mantido como string para corresponder ao JSON

        /// <summary>
        /// O período em que o elevador foi utilizado (representado como string no JSON: "M", "V", "N").
        /// Mapeado da propriedade "turno" no JSON.
        /// </summary>
        [JsonPropertyName("turno")]
        public string Turno { get; set; } // Mantido como string para corresponder ao JSON
    }
}