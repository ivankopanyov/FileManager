using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.UI
{
    /// <summary>
    /// Возбуждается, если значение, переданное в параметр weigth меньше 0.
    /// </summary>
    public class InvalidWeigthException : Exception
    {
        /// <summary>
        /// Значение, переданное в параметр weigth.
        /// </summary>
        public double Weigth { get; }

        /// <summary>
        /// Возбуждается, если значение, переданное в параметр weigth меньше 0.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="weigth">Значение, переданное в параметр weigth.</param>
        public InvalidWeigthException(string message, double weigth) : base(message)
        {
            Weigth = weigth;
        }
    }
}
