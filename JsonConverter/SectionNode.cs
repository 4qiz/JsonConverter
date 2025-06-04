
using System.Text;
using System.Text.Json.Serialization;

namespace JsonConverter
{
    /// <summary>
    /// узел раздела с возможными дочерними разделами и содержимым.
    /// </summary>
    public class SectionNode
    {
        /// <summary>
        /// Номер раздела
        /// </summary>
        public string Number { get; }

        /// <summary>
        /// Полный заголовок раздела
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Содержимое раздела. 
        /// </summary>
        [JsonIgnore]
        public StringBuilder ContentBuilder { get; } = new();

        /// <summary>
        /// Дочерние разделы.
        /// </summary>
        public List<SectionNode> Children { get; } = [];

        /// <summary>
        /// Возвращает содержимое раздела в виде строки.
        /// </summary>
        public string Content => ContentBuilder.ToString();

        /// <summary>
        /// Создает новый узел раздела.
        /// </summary>
        /// <param name="number">Номер раздела.</param>
        /// <param name="title">Полный заголовок раздела.</param>
        public SectionNode(string number, string title)
        {
            Number = number;
            Title = title;
        }

        /// <summary>
        /// Преобразует узел и его дочерние элементы в совместимый с JSON формат.
        /// </summary>
        /// <returns>Строка содержимого или вложенный объект в зависимости от структуры.</returns>
        public object ToJsonCompatible()
        {
            if (Children.Count == 0)
            {
                return Content;
            }

            var dict = new Dictionary<string, object>(Children.Count);
            foreach (var child in Children)
            {
                dict[child.Title] = child.ToJsonCompatible();
            }
            return dict;
        }
    }
}
