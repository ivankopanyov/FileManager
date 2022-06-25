using System;
using System.IO;
using System.Text;

namespace FileManager.Commands
{
    /// <summary>
    /// Команда удаления каталогов и файлов.
    /// </summary>
	public class RemoveCommand : ICommand
    {
        /// <summary>
        /// Ключевое слово для вызова команды.
        /// </summary>
        public string KeyWord { get; }

        /// <summary>
        /// Конструктор класса команды удаления каталогов и файлов.
        /// </summary>
        /// <param name="keyWord">Ключевое слово для вызова команды.</param>
        public RemoveCommand(string keyWord)
        {
            KeyWord = keyWord;
        }

        /// <summary>
        /// Метод выполнения команды.
        /// </summary>
        /// <param name="command">Команда для выполнения без ключевого слова.</param>
        /// <param name="currentDir">Текущая директория.</param>
        /// <exception cref="CommandException">Возбуждается, если среди переданных значений есть null 
        /// или при ошибке доступа.</exception>
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
            catch
            {
                throw new Exception("Ошибка:  указанный путь содержит недопустимые символы.");
            }

            if (!Directory.Exists(path) && !File.Exists(path)) 
                throw new CommandException($"Ошибка: директория или файл {path} не найдены.");

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    throw new Exception($"Ошибка: не удалось удалить файл {path}");
                }
                return $"Файл {path} успешно удален.";
            }

            var logs = new StringBuilder();
            RemoveDirectory(new DirectoryInfo(path), logs);

            var logsStr = logs.ToString();

            if (logsStr != string.Empty)
                throw new CommandException(logsStr);

            return $"Директория {path} успешно удалена.";
        }

        /// <summary>
        /// Рекурсивный метод удаления директории.
        /// </summary>
        /// <param name="dir">Директоия для удаоения.</param>
        /// <param name="logs">Динамическая строка для записи ошибок в ходе выполнения удаления.</param>
        /// <returns>Возвращает</returns>
        private StringBuilder RemoveDirectory(DirectoryInfo dir, StringBuilder logs)
        {
            DirectoryInfo[] subDirs;
            FileInfo[] files;

            try
            {
                subDirs = dir.GetDirectories();
                files = dir.GetFiles();
            }
            catch
            {
                logs.Append($"Ошибка: не удалось получить доступ к содержимому каталога {dir.FullName}\n");
                return logs;
            }

            for (int i = 0; i < files.Length; i++) {
                try
                {
                    File.Delete(files[i].FullName);
                }
                catch
                {
                    logs.Append($"Ошибка: не удалось удалить файл {files[i].FullName}\n");
                }
            }

            for (int i = 0; i < subDirs.Length; i++)
                RemoveDirectory(subDirs[i], logs);

            try
            {
                Directory.Delete(dir.FullName);
            }
            catch
            {
                logs.Append($"Ошибка: не удалось удалить директорию {dir.FullName}\n");
            }

            return logs;
        }
    }
}

