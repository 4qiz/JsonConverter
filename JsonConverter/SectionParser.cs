
using System.Text.RegularExpressions;

namespace JsonConverter
{
    /// <summary>
    /// Методы для разбора иерархической структуры разделов.
    /// </summary>
    public static class SectionParser
    {
        /// <summary>
        /// распознавание заголовков разделов
        /// </summary>
        private static readonly Regex HeaderRegex = new(@"^(\d+(?:\.\d+)*)(\s+.+)", RegexOptions.Compiled);

        /// <summary>
        /// Разбирает текстовый поток и создает дерево разделов в виде иерархии узлов.
        /// </summary>
        /// <param name="reader">Поток чтения строк из текстового файла.</param>
        /// <returns>Корневой узел дерева разделов.</returns>
        public static SectionNode Parse(StreamReader reader)
        {
            var root = new SectionNode("0", "root");
            var stack = new Stack<(string Number, SectionNode Node)>();
            stack.Push(("0", root));

            string? line;
            SectionNode? currentNode = null;

            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Length == 0)
                    continue;

                if (TryParseHeader(line, out var number, out var title))
                {
                    var newNode = new SectionNode(number, $"{number} {title}");

                    
                    while (stack.Count > 1 && !IsParent(stack.Peek().Number, number))
                    {
                        stack.Pop();
                    }

                    var parent = stack.Peek().Node;
                    parent.Children.Add(newNode);
                    stack.Push((number, newNode));
                    currentNode = newNode;
                }
                else if (currentNode != null)
                {
                    AppendContent(currentNode, line);
                }
            }

            return root;
        }

        /// <summary>
        /// Пытается распознать строку как заголовок раздела
        /// </summary>
        /// <param name="line">Строка текста</param>
        /// <param name="number">Выходной параметр: номер раздела</param>
        /// <param name="title">Выходной параметр: заголовок раздела</param>
        /// <returns><c>true</c>, если строка является заголовком раздела; иначе <c>false</c>.</returns>
        private static bool TryParseHeader(string line, out string number, out string title)
        {
            var match = HeaderRegex.Match(line);
            if (match.Success)
            {
                number = match.Groups[1].Value;
                title = match.Groups[2].Value.Trim();
                return true;
            }

            number = string.Empty;
            title = string.Empty;
            return false;
        }

        /// <summary>
        /// Добавляет строку содержимого к текущему узлу.
        /// </summary>
        /// <param name="node">Узел, к которому добавляется содержимое.</param>
        /// <param name="line">Строка содержимого.</param>
        private static void AppendContent(SectionNode node, string line)
        {
            if (node.Content.Length > 0)
                node.ContentBuilder.AppendLine(); 

            node.ContentBuilder.Append(line);
        }

        /// <summary>
        /// Определяет, является ли один номер раздела родителем другого.
        /// </summary>
        /// <param name="parent">Номер предполагаемого родительского раздела.</param>
        /// <param name="child">Номер предполагаемого дочернего раздела.</param>
        /// <returns><c>true</c>, если <paramref name="parent"/> является родителем <paramref name="child"/>; иначе <c>false</c>.</returns>
        private static bool IsParent(string parent, string child)
        {
            return child.StartsWith(parent + ".");
        }
    }
}
