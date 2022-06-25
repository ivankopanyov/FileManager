using System;

namespace FileManager.UI
{
    /// <summary>
    /// Возбуждается, если значение, переданное в параметр меньше 2 или меньше колличества всех дочерних окон + 1
    /// или больше разницы высоты консоли и отступа сверху.
    /// </summary>
    public class InvalidHeightException : Exception
    {
        /// <summary>
        /// Значение, переданное в параметр высоты.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Текущее минимальное значение.
        /// </summary>
        public int Min { get; }

        /// <summary>
        /// Текущее максимальное значение.
        /// </summary>
        public int Max { get; }

        /// <summary>
        /// Возбуждается, если значение, переданное в параметр меньше 2 или меньше колличества всех дочерних окон + 1
        /// или больше разницы высоты консоли и отступа сверху.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="height">Значение, переданное в параметр высоты.</param>
        /// <param name="min">Текущее минимальное значение.</param>
        /// <param name="max">Текущее максимальное значение.</param>
        public InvalidHeightException(string message, int height, int min, int max) : base(message)
        {
            Height = height;
            Min = min;
            Max = max;
        }
    }
}
