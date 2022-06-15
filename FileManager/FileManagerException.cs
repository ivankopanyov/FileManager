using System;

namespace FileManager
{
    /// <summary>
    /// Возбуждается при некорректном вводе команд и атрибутов и ошибке доступа.
    /// </summary>
    public class FileManagerException : Exception
    {
        /// <summary>
        /// Возбуждается при некорректном вводе команд и атрибутов и ошибке доступа.
        /// </summary>
        /// <param name="message">Сообщение об ошибке для вывода в консоль.</param>
        public FileManagerException(string message) : base(message)
        {

        }
    }
}
