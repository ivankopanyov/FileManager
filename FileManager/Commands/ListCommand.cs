using System;
using System.IO;
using System.Threading.Tasks;
using FileManager.UI;

namespace FileManager
{
    /// <summary>
    /// Команда вывода дерева каталогов и файлов.
    /// </summary>
	public class ListCommand : ICommand
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
        /// Конструктор класса команды вывода дерева каталогов и файлов.
        /// </summary>
        /// <param name="keyWord">Ключевое слово для вызова команды.</param>
        /// <param name="resultWindow">Окно для вывода результата выполнения команды.</param>
        /// <exception cref="NullWindowException">Возбуждается, если переданный экземпляр класса Window равен null.</exception>
        public ListCommand(string keyWord, Window resultWindow)
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
        /// или при некорректном вводе команды.</exception>
        public void Execute(string command, string currentDir)
        {

            if (command == null)
                throw new FileManagerException("Ошибка: не указана команда.");

            if (currentDir == null)
                throw new FileManagerException("Ошибка: не указана текущая директория.");

            currentDir = currentDir.Trim();
            command = command.Trim();
            string path;

            try
            {
                path = Path.GetFullPath(Path.Combine(currentDir, command));
            }
            catch (Exception e)
            {
                FileManager.WriteExceptionInfo(e);
                throw new FileManagerException("Ошибка:  указанный путь содержит недопустимые символы.");
            }

            if (Directory.Exists(path))
            {
                ResultWindow.Content = GetTree(path, 1);
                return;
            }

            var dir = command;

            var index = command.ToLower().LastIndexOf("-p");
            if (index == -1) throw new FileManagerException($"Ошибка: директория {path} не найдена.");
            if (index == 0) dir = string.Empty;
            if (index > 0) dir = command.Substring(0, index + 1).Trim(); 
            
            try
            {
                dir = Path.GetFullPath(Path.Combine(currentDir, dir));
            }
            catch (Exception e)
            {
                FileManager.WriteExceptionInfo(e);
                throw new FileManagerException("Ошибка:  указанный путь содержит недопустимые символы.");
            }

            if (!Directory.Exists(dir)) throw new FileManagerException($"Ошибка: директория {dir} не найдена.");

            var attrs = command.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);

            if (attrs[attrs.Length - 1].ToLower() == "-p") 
                throw new FileManagerException("Ошибка: не указан номер страницы.");
            if (attrs.Length > 1 && attrs[attrs.Length - 2].ToLower() == "-p")
            {
                if (!int.TryParse(attrs[attrs.Length - 1], out int pageNumber))
                    throw new FileManagerException("Ошибка: некорректно указан номер страницы.");
                if (pageNumber < 1) 
                    throw new FileManagerException("Ошибка: номер страницы не может быть меньше 1.");
                ResultWindow.Content = GetTree(dir, pageNumber);
            }
            else throw new FileManagerException($"Ошибка: директория {path} не найдена.");
        }

        /// <summary>
        /// Метод получения страницы дерева каталогов.
        /// </summary>
        /// <param name="dir">Корневая директория.</param>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <returns>Страница дерева каталогов.</returns>
        private string GetTree(string dir, int pageNumber)
        {
            var tree = GetTree(new DirectoryInfo(dir));
            var treeArray = tree.Split('\n');
            var pageLines = ResultWindow.ContentSize.height - 1;
            var total = treeArray.Length / pageLines + 1;
            if (pageNumber > total) pageNumber = total;
            var startIndex = pageLines * (pageNumber - 1);
            var count = treeArray.Length - startIndex < pageLines ? treeArray.Length - startIndex : pageLines;
            var result = string.Join("\n", treeArray, startIndex, count);
            var footer = $"-= {pageNumber} of {total} =-";
            result += new string('\n', pageLines + 1 - count) 
                + new string(' ', (ResultWindow.ContentSize.width - footer.Length) / 2) + footer;
            return result;
        }

        /// <summary>
        /// Метод получения дерева каталогов.
        /// </summary>
        /// <param name="dir">Корневая директория.</param>
        /// <param name="indent">Отступ.</param>
        /// <param name="isFinalDir">Является ли сабдиректрория последней в директории.</param>
        /// <returns>Дерево каталогов и файлов.</returns>
        private string GetTree(DirectoryInfo dir, string indent = "", bool isFinalDir = true)
        {
            if (dir == null) return string.Empty;

            string result = indent + (isFinalDir ? "└" : "├") + dir.Name + "\n";
            indent += isFinalDir ? " " : "│";

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
                return result;
            }

            for (int i = 0; i < files.Length; i++)
                result += indent + (i == files.Length - 1 && subDirs.Length == 0 ? "└" : "├") + files[i].Name + "\n";

            for (int i = 0; i < subDirs.Length; i++)
                result += GetTree(subDirs[i], indent, i == subDirs.Length - 1);

            return result;
        }
    }
}

