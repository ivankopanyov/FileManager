using System;
using System.IO;
using System.Text;

namespace FileManager.Commands
{
    /// <summary>
    /// Команда для вывода информации о каталоге или файле.
    /// </summary>
	public class InfoCommand : ICommand
    {
        /// <summary>
        /// Ключевое слово для вызова команды.
        /// </summary>
        public string KeyWord { get; }

        /// <summary>
        /// Конструктор класса команды вывода информации о каталоге или файле.
        /// </summary>
        /// <param name="keyWord">Ключевое слово для вызова команды.</param>
        public InfoCommand(string keyWord)
        {
            KeyWord = keyWord;
        }

        /// <summary>
        /// Метод выполнения команды.
        /// </summary>
        /// <param name="command">Команда для выполнения без ключевого слова.</param>
        /// <param name="currentDir">Текущая директория.</param>
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

            if (File.Exists(path))
            {
                var file = new FileInfo(path);
                StringBuilder result = new StringBuilder();

                try
                {
                    result.Append($"Имя файла:         {file.Name}\n");
                    result.Append($"Путь к файлу:      {file.FullName}\n");
                    result.Append($"Размер файла:      {ToKB(file.Length)} KB\n");
                    result.Append($"Время создания:    {file.CreationTime.ToString("dd.MM.yyyy hh:mm:ss")}\n");
                    result.Append($"Время изменения:   {file.LastWriteTime.ToString("dd.MM.yyyy hh:mm:ss")}\n");
                    result.Append($"Tолько для чтения: {(file.IsReadOnly ? "Да" : "Нет")}\n");
                }
                catch
                {
                    throw new Exception($"Не удалось получить доступ к файлу {path}.");
                }

                return result.ToString();
            }

            if (Directory.Exists(path))
            {
                var dir = new DirectoryInfo(path);
                StringBuilder result = new StringBuilder();

                try
                {
                    result.Append($"Имя директории:    {dir.Name}\n");
                    result.Append($"Путь к директории: {dir.FullName}\n");
                    var size = GetDirectorySize(dir);
                    result.Append($"Размер директории: {(size == -1 ? "Неизвестно" : (ToKB(size) + " KB"))}\n");
                    result.Append($"Время создания:    {dir.CreationTime.ToString("dd.MM.yyyy hh:mm:ss")}\n");
                    result.Append($"Время изменения:   {dir.LastWriteTime.ToString("dd.MM.yyyy hh:mm:ss")}\n");
                }
                catch
                {
                    throw new Exception($"Не удалось получить доступ к директории {path}.");
                }

                return result.ToString();
            }
            
            throw new CommandException($"Ошибка: директория или файл {path} не найдены.");
        }

        /// <summary>
        /// Рекурсивный метод получения размера каталога.
        /// </summary>
        /// <param name="dir">Директория для вычисления размера.</param>
        /// <returns>Размер каталога. При неудачной попытке вычисления возвращает -1.</returns>
        private long GetDirectorySize(DirectoryInfo dir)
        {
            long currentSize = 0;
            DirectoryInfo[] subDirs;
            FileInfo[] files;

            try
            {
                subDirs = dir.GetDirectories();
                files = dir.GetFiles();
            }
            catch
            {
                return -1;
            }

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    currentSize += files[i].Length;
                }
                catch
                {
                    return -1;
                }
            }

            for (int i = 0; i < subDirs.Length; i++)
            {
                var result = GetDirectorySize(subDirs[i]);
                if (result == -1) return result;
                currentSize += result;
            }

            return currentSize;
        }

        /// <summary>
        /// Переводит байты в килобайты и преобразует результат в форматированную строку.
        /// </summary>
        /// <param name="value">Колличество байт.</param>
        /// <returns>Форматированная строка с коллиством килобайт.</returns>
        static string ToKB(long value)
        {
            return (value / 1024).ToString("N0", System.Globalization.CultureInfo.CreateSpecificCulture("ru-RU"));
        }
    }
}

