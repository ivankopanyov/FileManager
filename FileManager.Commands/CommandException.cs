using System;

namespace FileManager.Commands
{
    /// <summary>
    /// Возбуждается при некорректном вводе команд и атрибутов и ошибке доступа.
    /// </summary>
    public class CommandException : Exception
    {
        /// <summary>
        /// Возбуждается при некорректном вводе команд и атрибутов и ошибке доступа.
        /// </summary>
        /// <param name="message">Сообщение об ошибке для вывода в консоль.</param>
        public CommandException(string message) : base(message)
        {

        }
    }
}
