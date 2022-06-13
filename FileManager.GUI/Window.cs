using System;
using System.Collections.Generic;
using System.Linq;

namespace FileManager.GUI
{
    public class Window
    {
        private double weigth;

        private List<Window> windows = new List<Window>();

        private (int left, int top) cursorPosition;

        private int limit;

        public string Content { get; set; } = string.Empty;

        public enum StackDirection
        {
            Horizontal,
            Vertical
        }

        public StackDirection Direction { get; }

        public double Weigth
        {
            get => weigth;

            set
            {
                if (value <= 0)
                    return; //excaption
                weigth = value;
            }
        }

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

        public Window(StackDirection stackDirection, double weigth)
        {
            Direction = stackDirection;
            Weigth = weigth; // exception
        }

        public void AddWindow(Window window)
        {
            if (window == null) return; //exception
            windows.Add(window);
        }

        public void Draw(int left, int top, int width, int height)
        {
            var result = new string[height];

            if (Direction == StackDirection.Horizontal) GetPrintedHorizontalStack(result, width, height, left, top);
            else GetPrintedVerticalStack(result, width, height, left, top);

            string resultStr = string.Join(new string(' ', left), result);

            Console.CursorLeft = left;
            Console.CursorTop = top;
            Console.Write(resultStr);
        }

        /// <summary>
        /// Ввод пользователем значения в поле.
        /// </summary>
        /// <returns>Возвращает введенное значение.</returns>
        public string Read()
        {
            Console.CursorLeft = cursorPosition.left;
            Console.CursorTop = cursorPosition.top;
            var value = string.Empty;

            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter) break;

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (value != string.Empty)
                    {
                        value = value.Substring(0, value.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (value.Length < limit && key.Key != ConsoleKey.Tab && key.Key != ConsoleKey.LeftArrow
                    && key.Key != ConsoleKey.RightArrow && key.Key != ConsoleKey.UpArrow && key.Key != ConsoleKey.DownArrow)
                {
                    Console.Write(key.KeyChar);
                    value += key.KeyChar;
                }
            }

            return value;
        }

        private void GetPrintedVerticalStack(string[] result, int width, int height, int left, int top)
        {
            var heightWindow = height;

            var content = Content != null ? Content.Split('\n') : new string[0];

            cursorPosition.top = top + (content.Length > 0 ? content.Length : 1);
            cursorPosition.left = left +
                (content.Length > 0 && content[content.Length - 1].Length > 0 ? content[content.Length - 1].Length + 1 : 1);
            limit = width - cursorPosition.left - 2;


            for (int i = 0; i < height; i++)
            {
                string items = i == 0 ? "╔═╗" : "║ ║";
                if (i == height - 1) items = "╚═╝";

                string contentLine;

                if (i == 0 || i == height - 1 || content.Length < i)
                    contentLine = new string(items[1], width - 2);
                else if (content[i - 1].Length > width - 2)
                    contentLine = content[i - 1].Substring(0, width - 2);
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

                if (windows[i].Direction == StackDirection.Horizontal)
                    windows[i].GetPrintedHorizontalStack(result, width, height, left, top);
                else windows[i].GetPrintedVerticalStack(result, width, height, left, top);

                top += height - 1;
            }
        }

        private void GetPrintedHorizontalStack(string[] result, int width, int height, int left, int top)
        {
        }

        private string TransformBorders(string border1, string border2)
        {
            if (string.IsNullOrEmpty(border1) && string.IsNullOrEmpty(border2)) 
                return string.Empty; //exception

            if (string.IsNullOrEmpty(border1)) return border2;
            if (string.IsNullOrEmpty(border2)) return border1;

            var result = string.Empty;

            for (int i = 0; i < Math.Max(border1.Length, border2.Length); i++)
            {
                if (i < border1.Length && i < border2.Length) result += AngleSum(border1[i], border2[i]);
                else if (i > border1.Length && i < border2.Length) result += border2[1];
                else if (i < border1.Length && i > border2.Length) result += border1[1];
            }

            return result;
        }

        private char AngleSum(char borderItem1, char borderItem2)
        {
            var borderItems
                = new Dictionary<char, uint>()
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
                if (borderItems.ContainsKey(borderItem1) && borderItems.ContainsKey(borderItem2) && 
                    bordeItem.Value == (borderItems[borderItem1] | borderItems[borderItem2]))
                    return bordeItem.Key;

            return borderItem2;
        }
    }
}
