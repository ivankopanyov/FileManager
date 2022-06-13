using System;

namespace FileManager.Commands
{
	public class CopyCommand : ICommand
	{
        public string Name { get; }

        public CopyCommand(string name)
        {
            Name = name;
        }

        public (string tree, string info, string command) Execute(string currentDir, string command)
        {
            return (null, null, null);
        }
    }
}

