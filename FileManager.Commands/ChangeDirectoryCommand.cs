using System.IO;

namespace FileManager.Commands
{
	public class ChangeDirectoryCommand : ICommand
	{
        public string Name { get; }

        public ChangeDirectoryCommand(string name)
        {
            Name = name;
        }

        public (string tree, string info, string command) Execute(string currentDir, string command)
        {
            var path = Path.GetFullPath(Path.Combine(currentDir.Trim(), command.Trim()));
            if (Directory.Exists(path)) return (null, string.Empty, path);
            return (null, $"Ошибка: директория {path} не найдена!", null);
        }
    }
}

