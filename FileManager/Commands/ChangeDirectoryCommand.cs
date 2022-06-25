using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using FileManager.UI;

namespace FileManager
{
    /// <summary>
    /// Команда файлового менеджера для изменения текущей директории.
    /// </summary>
	public class ChangeDirectoryCommand : ICommand
    {
        /// <summary>
        /// Ключевое слово для вызова команды.
        /// </summary>
        public string KeyWord { get; }

        /// <summary>
        /// Конструктор класса команды изменения текущей директории.
        /// </summary>
        /// <param name="keyWord">Ключевое слово для вызова команды.</param>
        public ChangeDirectoryCommand(string keyWord)
        {
            KeyWord = keyWord;
        }

        /// <summary>
        /// Метод выполнения команды.
        /// </summary>
        /// <param name="command">Команда для выполнения без ключевого слова.</param>
        /// <param name="currentDir">Текущая директория.</param>
        /// <exception cref="CommandException">Возбуждается, если среди переданных значений есть null.</exception>
        public string Execute(string command, string currentDir)
        {
            if (command == null)
                throw new CommandException("Ошибка: не указана команда.");

            if (currentDir == null)
                throw new CommandException("Ошибка: не указана текущая директория.");

            string path;
            try
            {
                path = Path.GetFullPath(Path.Combine(currentDir.Trim(), command.Trim()));

            }
            catch (Exception e)
            {
                FileManager.WriteExceptionInfo(e);
                throw new CommandException("Ошибка:  указанный путь содержит недопустимые символы.");
            }

            if (!Directory.Exists(path)) throw new CommandException($"Ошибка: директория {path} не найдена.");
            return CommandLine.GetShortPath(path) + ">";
        }
    }
}

