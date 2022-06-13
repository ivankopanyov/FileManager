using System;
using System.IO;

namespace FileManager.Commands
{
	public class ListCommand : ICommand
    {
        public string Name { get; }

        public ListCommand(string name)
        {
            Name = name;
        }

        public (string tree, string info, string command) Execute(string currentDir, string command)
        {
            var path = Path.GetFullPath(Path.Combine(currentDir.Trim(), command.Trim()));

            if (Directory.Exists(path)) 
                return (GetTree(new DirectoryInfo(path)), string.Empty, null);

            return (null, $"Ошибка: директория {path} не найдена!", null);
        }

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
            catch (Exception)
            {
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

