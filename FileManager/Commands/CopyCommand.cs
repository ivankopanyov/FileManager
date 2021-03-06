using System;
using System.IO;
using FileManager.UI;

namespace FileManager
{
    /// <summary>
    /// Команда копирования каталогов и файлов.
    /// </summary>
    public class CopyCommand : ICommand
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
        /// Конструктор класса команды копирования каталогов и файлов.
        /// </summary>
        /// <param name="keyWord">Ключевое слово для вызова команды.</param>
        /// <param name="resultWindow">Окно для вывода результата выполнения команды.</param>
        /// <exception cref="NullWindowException">Возбуждается, если переданный экземпляр класса Window равен null.</exception>
        public CopyCommand(string keyWord, Window resultWindow)
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
        /// или при неудачном копировании.</exception>
        public void Execute(string command, string currentDir)
        {
            if (command == null)
                throw new FileManagerException("Ошибка: не указана команда.");

            if (currentDir == null) 
                throw new FileManagerException("Ошибка: не указана текущая директория.");

            var attrs = command.Split();

            (string source, string dest) copyData = (null, null);
            bool isDir = false;

            for (int i = 0; i < attrs.Length; i++)
            {
                string source;
                try
                {
                    source = Path.GetFullPath(Path.Combine(currentDir, string.Join(" ", attrs, 0, i + 1).Trim()));
                }
                catch (Exception e)
                {
                    FileManager.WriteExceptionInfo(e);
                    throw new FileManagerException("Ошибка: указанный путь содержит недопустимые символы.");
                }

                if (!Directory.Exists(source) && i == attrs.Length)
                { 
                    copyData = (currentDir, source);
                    isDir = true;
                }
                else if (Directory.Exists(source) && i < attrs.Length)
                {
                    try
                    {
                        var dest = Path.GetFullPath(Path.Combine(currentDir, string.Join(" ", attrs, i + 1, attrs.Length - i - 1).Trim()));
                        if (string.IsNullOrEmpty(dest))
                        {
                            copyData = (currentDir, source);
                            isDir = true;
                        }
                        else if (!string.IsNullOrEmpty(dest) && !Directory.Exists(dest))
                        {
                            copyData = (source, dest);
                            isDir = true;
                        }
                    }
                    catch (Exception e)
                    {
                        FileManager.WriteExceptionInfo(e);
                    }
                }

                if (File.Exists(source) && i < attrs.Length)
                {
                    try
                    {
                        var dest = Path.GetFullPath(Path.Combine(currentDir, string.Join(" ", attrs, i + 1, attrs.Length - i - 1).Trim()));
                        if (!string.IsNullOrEmpty(dest) && !File.Exists(dest))
                        { 
                            copyData = (source, dest);
                            isDir = false;
                        }
                    }
                    catch (Exception e)
                    {
                        FileManager.WriteExceptionInfo(e);
                    }
                }
            }

            if (copyData.source == null)
                throw new FileManagerException("Ошибка: не корректно указаны данные для копирования.");

            if (isDir)
            {
                try
                {
                    CopyDirectory(new DirectoryInfo(copyData.source), new DirectoryInfo(copyData.dest));
                    ResultWindow.Content = $"Директория {copyData.source} успешно скопирована в {copyData.dest}.";
                    return;
                }
                catch (Exception e)
                {
                    FileManager.WriteExceptionInfo(e);
                    throw new FileManagerException($"Ошибка: не удалось скопировать директорию {copyData.source} в {copyData.dest}.");
                }
            }

            try
            {
                File.Copy(copyData.source, copyData.dest);
                ResultWindow.Content = $"Файл {copyData.source} успешно скопирована в {copyData.dest}.";
            }
            catch (Exception e)
            {
                FileManager.WriteExceptionInfo(e);
                throw new FileManagerException($"Ошибка: не удалось скопировать файл {copyData.source} в {copyData.dest}.");
            }
        }

        /// <summary>
        /// Рекурсивный метод копирования директории.
        /// </summary>
        /// <param name="source">Директория для копирования.</param>
        /// <param name="dest">Новая директория.</param>
        /// <exception cref="FileManagerException">Возбуждается, если не удалось скопировать директорию или файл.</exception>
        private void CopyDirectory(DirectoryInfo source, DirectoryInfo dest)
        {
            Directory.CreateDirectory(dest.FullName);

            DirectoryInfo[] subDirs;
            FileInfo[] files;

            try
            {
                subDirs = source.GetDirectories();
                files = source.GetFiles();
            }
            catch (Exception e)
            {
                FileManager.WriteExceptionInfo(e);
                throw new FileManagerException("Ошибка копирования");
            }

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    File.Copy(files[i].FullName, dest.FullName);
                }
                catch (Exception e)
                {
                    FileManager.WriteExceptionInfo(e);
                    throw new FileManagerException("Ошибка копирования");
                }
            }

            for (int i = 0; i < subDirs.Length; i++)
            {
                try
                {
                    var dir = Path.GetFullPath(Path.Combine(dest.FullName, subDirs[i].Name));
                    CopyDirectory(subDirs[i], new DirectoryInfo(dir));
                }
                catch (Exception e)
                {
                    FileManager.WriteExceptionInfo(e);
                    throw new FileManagerException("Ошибка копирования");
                }
            }
                
        }
    }
}

