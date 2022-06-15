using System;

namespace FileManager.UI
{
    /// <summary>
    /// Возбуждается, если значение, переданное в параметр меньше 2 больше разницы высоты консоли и отступа сверху.
    /// </summary>
    public class InvalidWidthException : Exception
    {
        /// <summary>
        /// Значение, переданное в параметр ширины.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Текущее максимальное значение.
        /// </summary>
        public int Max { get; }

        /// <summary>
        /// Возбуждается, если значение, переданное в параметр меньше 2 или больше разницы ширины консоли и отступа слева.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="width">Значение, переданное в параметр ширины.</param>
        /// <param name="max">Текущее максимальное значение.</param>
        public InvalidWidthException(string message, int width, int max) : base(message)
        {
            Width = width;
            Max = max;
        }
    }
}
