
using System.Text.Json;

namespace JsonConverter
{
    public static class SectionConverter
    {
        /// <summary>
        /// Преобразует структурированный текстовый файл в JSON и сохраняет его.
        /// </summary>
        /// <param name="inputFilePath">Путь к входному текстовому файлу.</param>
        /// <param name="outputFilePath">Путь к выходному JSON-файлу.</param>
        public static void ConvertToJson(string inputFilePath, string outputFilePath)
        {
            using var reader = new StreamReader(inputFilePath);
            var root = SectionParser.Parse(reader);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(root.ToJsonCompatible(), options);
            File.WriteAllText(outputFilePath, json);
        }
    }
}
