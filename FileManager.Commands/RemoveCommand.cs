namespace FileManager.Commands
{
	public class RemoveCommand : ICommand
    {
        public string Name { get; }

        public RemoveCommand(string name)
        {
            Name = name;
        }

        public (string tree, string info, string command) Execute(string currentDir, string commandAttr)
        {
            return (null, null, null);
        }
    }
}

