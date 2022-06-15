using System;
using System.Collections.Generic;
using System.Linq;

namespace FileManager.UI
{
    /// <summary>
    /// Класс, описывающий окно для вывода контента в консоль.
    /// </summary>
    public class Window
    {
        /// <summary>
        /// Высота окна относительно родительского окна.
        /// </summary>
        private double weigth = 1;

        /// <summary>
        /// Дочерние окна.
        /// </summary>
        private List<Window> windows = new List<Window>();

        /// <summary>
        /// Позиция курсора для ввода.
        /// </summary>
        private (int left, int top) cursorPosition;

        /// <summary>
        /// Максимальное кооличество символов для ввода.
        /// </summary>
        private int limit;

        /// <summary>
        /// Контент, выводимый внутри окна.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Размер окна без учета рамки.
        /// </summary>
        public (int width, int height) ContentSize { get; private set; }

        /// <summary>
        /// Высота окна относительно родительского окна.
        /// </summary>
        /// <exception cref="InvalidWeigthException">Возбуждается, если переданное значение меньше 0.</exception>
        public double Weigth
        {
            get => weigth;

            set
            {
                if (value < 0) 
                    throw new InvalidWeigthException("Значение weigth не должно быть меньше 0.", value);
                weigth = value;
            }
        }

        /// <summary>
        /// Сумма параметров weigth всех дочерних окон.
        /// </summary>
        public double WeigthSum
        {
            get 
            {
                if (windows == null || windows.Count() == 0) return 0;
                double weigthSum = 0;
                foreach (Window window in windows)
                    weigthSum += window.Weigth;
                return weigthSum;
            }
        }

        /// <summary>
        /// Добавление дочернего окна.
        /// </summary>
        /// <param name="window">Дочернее окно.</param>
        /// <exception cref="NullWindowException">Возбуждает, если переданный экземпляр класса Window равен null.</exception>
        public void AddWindow(Window window)
        {
            if (window == null) 
                throw new NullWindowException("Переданный экземпляр класса Window равен null.");
            windows.Add(window);
        }

        /// <summary>
        /// Отрисовка окна и всех его дочерних окон в консоли.
        /// </summary>
        /// <param name="left">Отступ слева.</param>
        /// <param name="top">Отступ сверху.</param>
        /// <param name="width">Ширина окна.</param>
        /// <param name="height">Высота окна.</param>
        /// <exception cref="InvalidPositionException">Возбуждается, если отступы имеют недопустимые значения.</exception>
        /// <exception cref="InvalidWidthException">Возбуждается, если ширина имеет недопустимое значения.</exception>
        /// <exception cref="InvalidHeightException">Возбуждается, если высота имеет недопустимое значения.</exception>
        public void Draw(int left, int top, int width, int height)
        {
            var result = new string[height];

            GetStackWindows(result, width, height, left, top);

            string resultStr = string.Join(new string(' ', left), result);

            Console.CursorLeft = left;
            Console.CursorTop = top;
            Console.Write(resultStr);
        }

        /// <summary>
        /// Ввод пользователем значения в поле.
        /// </summary>
        /// <returns>Возвращает введенное значение.</returns>
        public string Read(string[] defaultValues = null)
        {
            Console.CursorLeft = cursorPosition.left;
            Console.CursorTop = cursorPosition.top;
            var value = string.Empty;
            int index = -1;

            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter) break;

                if (defaultValues != null && defaultValues.Length > 0 &&
                    (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.DownArrow))
                {
                    if (key.Key == ConsoleKey.UpArrow)
                        index = index >= defaultValues.Length - 1 ? 0 : index + 1;
                    else index = index <= 0 ? defaultValues.Length - 1 : index - 1;

                    if (value.Length > 0)
                    {
                        for (int i = 0; i < value.Length; i++) 
                            Console.Write("\b \b");
                        value = string.Empty;
                    }

                    value = defaultValues == null ? string.Empty : defaultValues[index];

                    Console.Write(value);
                    continue;
                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (value != string.Empty)
                    {
                        value = value.Substring(0, value.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (value.Length < limit && !char.IsControl(key.KeyChar))
                {
                    Console.Write(key.KeyChar);
                    value += key.KeyChar;
                }
            }

            return value;
        }

        /// <summary>
        /// Метод отрисовки дочерних окон.
        /// </summary>
        /// <param name="result">Массив строк для хранения отрисовки окна.</param>
        /// <param name="width">Ширина окна.</param>
        /// <param name="height">Высота окна.</param>
        /// <param name="left">Отступ слева.</param>
        /// <param name="top">Отступ сверху.</param>
        /// <exception cref="InvalidPositionException">Возбуждается, если отступы имеют недопустимые значения.</exception>
        /// <exception cref="InvalidWidthException">Возбуждается, если ширина имеет недопустимое значения.</exception>
        /// <exception cref="InvalidHeightException">Возбуждается, если высота имеет недопустимое значения.</exception>
        private void GetStackWindows(string[] result, int width, int height, int left, int top)
        {
            if (left < 0 || left >= Console.WindowWidth) 
                throw new InvalidPositionException("Left меньше 0 или больше или равен ширине консоли.", 
                    "left", left, "Console.WindowWidth", Console.WindowWidth);

            if (top < 0 || top >= Console.WindowHeight)
                throw new InvalidPositionException("Top меньше 0 или больше или равен высоте консоли.",
                    "top", top, "Console.WindowHeight", Console.WindowHeight);

            if (width < 2 || width > Console.WindowWidth - left)
                throw new InvalidWidthException("Width меньше 2 или больше разницы ширины консоли и отступа слева.",
                    width, Console.WindowWidth - left);

            if (height < windows.Count() + 1 || height < 2 || height > Console.WindowHeight - top)
                throw new InvalidHeightException("Height меньше 2 или меньше колличества всех дочерних окон + 1 или больше разницы высоты консоли и отступа сверху.",
                    height, windows.Count() + 1 > 2 ? windows.Count() + 1 : 2, Console.WindowHeight - top);

            ContentSize = (width - 2, height - 2);

            var heightWindow = height;

            var content = Content != null ? Content.Split('\n').ToList() : new List<string>();

            cursorPosition.top = top + (content.Count() > 0 ? content.Count() : 1);
            cursorPosition.left = left +
                (content.Count() > 0 && content[content.Count() - 1].Length > 0 ? content[content.Count() - 1].Length + 1 : 1);
            limit = width - cursorPosition.left - 2;


            for (int i = 0; i < height; i++)
            {
                string items = i == 0 ? "╔═╗" : "║ ║";
                if (i == height - 1) items = "╚═╝";

                string contentLine;

                if (i == 0 || i == height - 1 || content.Count() < i)
                    contentLine = new string(items[1], width - 2);
                else if (content[i - 1].Length > width - 2)
                {
                    contentLine = content[i - 1].Substring(0, width - 2);
                    content.Insert(i, content[i - 1].Substring(width - 2, content[i - 1].Length - (width - 2)));
                }
                else if (content[i - 1].Length < width - 2)
                    contentLine = content[i - 1] + new string(' ', width - 2 - content[i - 1].Length);
                else
                    contentLine = content[i - 1];

                result[i + top] = TransformBorders(result[i + top], items[0] + contentLine + items[2]) + "\n";
            }

            var sum = 0;

            for (int i = 0; i < windows.Count(); i++)
            {
                int heightWithoutBorder = heightWindow - windows.Count() - 1;
                height = (int)Math.Round(windows[i].weigth / WeigthSum * heightWithoutBorder) + 2;
                if (i == windows.Count() - 1)
                    height = heightWithoutBorder - sum + 2;
                else
                    sum += height - 2;

                windows[i].GetStackWindows(result, width, height, left, top);

                top += height - 1;
            }
        }

        /// <summary>
        /// Метод перерисовки рамки окна.
        /// </summary>
        /// <param name="oldBorder">Текущая рамка.</param>
        /// <param name="newBorder">Новая рамка.</param>
        /// <returns>Результат трансформации рамки.</returns>
        private string TransformBorders(string oldBorder, string newBorder)
        {
            if (string.IsNullOrEmpty(oldBorder)) return newBorder;
            if (string.IsNullOrEmpty(newBorder)) return oldBorder;

            var result = string.Empty;

            for (int i = 0; i < Math.Max(oldBorder.Length, newBorder.Length); i++)
            {
                if (i < oldBorder.Length && i < newBorder.Length) result += BorderItemsSum(oldBorder[i], newBorder[i]);
                else if (i > oldBorder.Length && i < newBorder.Length) result += newBorder[1];
                else if (i < oldBorder.Length && i > newBorder.Length) result += oldBorder[1];
            }

            return result;
        }

        /// <summary>
        /// Метод сложения элементов рамки.
        /// </summary>
        /// <param name="oldBorderItem">Старый элемент рамки.</param>
        /// <param name="newBorderItem">Новый элемент рамки.</param>
        /// <returns>Результат сложения элементов рамки.</returns>
        private char BorderItemsSum(char oldBorderItem, char newBorderItem)
        {
            var borderItems = new Dictionary<char, uint>()
            {
                { ' ', 0b_0000},
                { '═', 0b_0101},
                { '║', 0b_1010},
                { '╔', 0b_0110},
                { '╗', 0b_0011},
                { '╚', 0b_1100},
                { '╝', 0b_1001},
                { '╠', 0b_1110},
                { '╣', 0b_1011},
                { '╦', 0b_0111},
                { '╩', 0b_1101},
                { '╬', 0b_1111}
            };

            foreach (var bordeItem in borderItems)
                if (borderItems.ContainsKey(oldBorderItem) && borderItems.ContainsKey(newBorderItem) && 
                    bordeItem.Value == (borderItems[oldBorderItem] | borderItems[newBorderItem]))
                    return bordeItem.Key;

            return newBorderItem;
        }
    }
}
