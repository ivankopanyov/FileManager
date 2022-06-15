using System;

namespace FileManager.UI
{
    /// <summary>
    /// Возбуждается, если значение, переданное в параметр меньше 0 или больше или равно максимальному значению.
    /// </summary>
    public class InvalidPositionException : Exception
    {
        /// <summary>
        /// Название параметра.
        /// </summary>
        public string ParamName { get; }

        /// <summary>
        /// Значение, переданное в параметр.
        /// </summary>
        public string ParamNameMax { get; }

        /// <summary>
        /// Название параметра, хранящего максимальное значение.
        /// </summary>
        public int ParamValue { get; }

        /// <summary>
        /// Значение параметра, хранящего максимальное значение.
        /// </summary>
        public int ParamValueMax { get; }

        /// <summary>
        /// Возбуждается, если значение, переданное в параметр меньше 0 или больше или равно максимальному значению.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="paramName">Название параметра.</param>
        /// <param name="paramValue">Значение, переданное в параметр.</param>
        /// <param name="paramNameMax">Название параметра, хранящего максимальное значение.</param>
        /// <param name="paramValueMax">Значение параметра, хранящего максимальное значение.</param>
        public InvalidPositionException(string message, string paramName, int paramValue, 
            string paramNameMax, int paramValueMax) : base(message)
        {
            ParamName = paramName;
            ParamValue = paramValue;
            ParamNameMax = paramNameMax;
            ParamValueMax = paramValueMax;
        }
    }
}
