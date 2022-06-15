using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.UI
{
    /// <summary>
    /// Возбуждается, если переданный экземпляр класса Window равен null.
    /// </summary>
    public class NullWindowException : Exception
    {
        /// <summary>
        /// Возбуждается, если переданный экземпляр класса Window равен null.
        /// </summary>
        /// <param name="message"> Сообщение исключения.</param>
        public NullWindowException(string message) : base(message)
        {

        }
    }
}
