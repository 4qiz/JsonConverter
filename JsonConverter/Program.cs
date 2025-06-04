

namespace JsonConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFile = GetInputFilePath(args);
                string outputFile = GetOutputFilePath(args);

                SectionConverter.ConvertToJson(inputFile, outputFile);

                Console.WriteLine($"\nПреобразование завершено.\nJSON сохранён в: {Path.GetFullPath(outputFile)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Получает путь к входному файлу из аргументов или от пользователя.
        /// </summary>
        private static string GetInputFilePath(string[] args)
        {
            string? inputFile = args.Length >= 1 ? args[0] : Prompt("Введите путь к входному файлу:");

            if (string.IsNullOrWhiteSpace(inputFile) || !File.Exists(inputFile))
                throw new FileNotFoundException("Указанный входной файл не существует.", inputFile);

            return inputFile;
        }

        /// <summary>
        /// Получает путь к выходному файлу из аргументов или от пользователя.
        /// </summary>
        private static string GetOutputFilePath(string[] args)
        {
            if (args.Length >= 2)
                return args[1];

            string? outputFile = Prompt("Введите путь к выходному JSON-файлу (по умолчанию: output.json):");
            return string.IsNullOrWhiteSpace(outputFile) ? "output.json" : outputFile;
        }

        /// <summary>
        /// Запрашивает ввод строки у пользователя.
        /// </summary>
        private static string Prompt(string message)
        {
            Console.Write(message + " ");
            return Console.ReadLine() ?? "";
        }
    }
}
