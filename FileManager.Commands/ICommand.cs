namespace FileManager.Commands
{
	public interface ICommand
	{
		string Name { get; }

		(string tree, string info, string command) Execute(string currentDir, string command);
	}
}

