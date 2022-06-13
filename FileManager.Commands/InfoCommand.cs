namespace FileManager.Commands
{
	public class InfoCommand : ICommand
    {
        public string Name { get; }

        public InfoCommand(string name)
        {
            Name = name;
        }

        public (string tree, string info, string command) Execute(string currentDir, string command)
        {
            return (null, null, null);
        }
    }
}

