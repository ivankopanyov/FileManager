using System;
using System.IO;
using System.Text;
using FileManager.UI;

namespace FileManager
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
        /// Окно для вывода результата выполнения команды.
        /// </summary>
        public Window ResultWindow { get; }

        /// <summary>
        /// Конструктор класса команды вывода информации о каталоге или файле.
        /// </summary>
        /// <param name="keyWord">Ключевое слово для вызова команды.</param>
        /// <param name="resultWindow">Окно для вывода результата выполнения команды.</param>
        /// <exception cref="NullWindowException">Возбуждается, если переданный экземпляр класса Window равен null.</exception>
        public InfoCommand(string keyWord, Window resultWindow)
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
        /// <exception cref="FileManagerException">Возбуждается, если среди переданных значений есть null 
        /// или при ошибке доступа.</exception>
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
                catch (Exception e)
                {
                    FileManager.WriteExceptionInfo(e);
                    throw new FileManagerException($"Не удалось получить доступ к файлу {path}.");
                }

                ResultWindow.Content = result.ToString();
                return;
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
                catch (Exception e)
                {
                    FileManager.WriteExceptionInfo(e);
                    throw new FileManagerException($"Не удалось получить доступ к директории {path}.");
                }

                ResultWindow.Content = result.ToString();
                return;
            }
            
            throw new FileManagerException($"Ошибка: директория или файл {path} не найдены.");
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
            catch (Exception e)
            {
                FileManager.WriteExceptionInfo(e);
                return -1;
            }

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    currentSize += files[i].Length;
                }
                catch (Exception e)
                {
                    FileManager.WriteExceptionInfo(e);
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

