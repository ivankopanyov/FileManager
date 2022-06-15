using System;
using System.IO;
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
        /// Окно для вывода результата выполнения команды.
        /// </summary>
        public Window ResultWindow { get; }

        /// <summary>
        /// Конструктор класса команды изменения текущей директории.
        /// </summary>
        /// <param name="keyWord">Ключевое слово для вызова команды.</param>
        /// <param name="resultWindow">Окно для вывода результата выполнения команды.</param>
        /// <exception cref="NullWindowException">Возбуждается, если переданный экземпляр класса Window равен null.</exception>
        public ChangeDirectoryCommand(string keyWord, Window resultWindow)
        {
            KeyWord = keyWord;
            if (resultWindow == null)
                throw new NullWindowException("Переданный экземпляр класса Window не должен быть null");
            ResultWindow = resultWindow;
        }

        /// <summary>
        /// Метод выполнения команды.
        /// </summary>
        /// <param name="command">Команда для выполнения без ключевого слова.</param>
        /// <param name="currentDir">Текущая директория.</param>
        /// <exception cref="FileManagerException">Возбуждается, если среди переданных значений есть null.</exception>
        public void Execute(string command, string currentDir)
        {
            if (command == null)
                throw new FileManagerException("Ошибка: не указана команда.");

            if (currentDir == null)
                throw new FileManagerException("Ошибка: не указана текущая директория.");

            string path;
            try
            {
                path = Path.GetFullPath(Path.Combine(currentDir.Trim(), command.Trim()));

            }
            catch (Exception e)
            {
                FileManager.WriteExceptionInfo(e);
                throw new FileManagerException("Ошибка:  указанный путь содержит недопустимые символы.");
            }

            if (!Directory.Exists(path)) throw new FileManagerException($"Ошибка: директория {path} не найдена.");
            ResultWindow.Content = FileManager.GetShortPath(path) + ">";
        }
    }
}

