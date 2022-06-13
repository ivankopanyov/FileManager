using System;
using System.Collections.Generic;
using System.Linq;

namespace FileManager.Commands
{
	public static class CommandsHistory
	{
		private static List<string> commands = new List<string>();
		private static int limit = 10;

		public static int Count => commands.Count();

		public static int Limit
		{
			get => limit;

			set
			{
				if (value < 0) return; //exception
				limit = value;
			}
		}

		public static void AddCommand(string command)
		{
			if (command == null) return; //exception
			commands.Insert(0, command);
			if (commands.Count() > limit)
				commands.RemoveRange(limit, commands.Count() - limit);
		}

		public static string GetCommandAt(int index)
		{
			if (commands.Count() == 0 || index < 0 || index >= commands.Count()) return ""; //exception
			return commands[index];
		}
	}
}

