using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FileManager
{
    public class Command
    {
        private const uint MAX_PATH = 255;

        public ICommand LastCommand { get; private set; }

        private List<ICommand> commands = new List<ICommand>()
        {
            new ChangeDirectoryCommand("cd"),
            new ListCommand("ls"),
            new CopyCommand("cp"),
            new RemoveCommand("rm"),
            new InfoCommand("info"),
            new ExitCommand("exit")
        };

        public string Execute(string inputCommand, string currentDir)
        {
            var inputCommandArray = inputCommand.Trim().Split();
            if (inputCommandArray.Length == 0) 
                throw new FileManagerException("Ошибка: Команда не указана");

            var command = commands.Find(c => c.KeyWord.ToLower() == inputCommandArray[0].ToLower());

            if (command == null)
                throw new FileManagerException($"Ошибка: Команда {inputCommandArray[0]} не найдена");

            var attrs = string.Join(" ", Shift(inputCommandArray));
            LastCommand = command;
            return command.Execute(attrs, currentDir);
        }

        /// <summary>
        /// Удаляет первый элемент массива.
        /// </summary>
        /// <param name="array">Массив для удалаения первого элемента.</param>
        /// <returns>Новый массив с удаленным первым элементом из переданного массива.</returns>
        private static string[] Shift(string[] array)
        {
            if (array == null || array.Length == 0) return array;
            if (array.Length == 1) return new string[0];

            var result = new string[array.Length - 1];
            Array.Copy(array, 1, result, 0, array.Length - 1);

            return result;
        }

        /// <summary>
        /// Сокращает отображаемый путь.
        /// </summary>
        /// <param name="path">Путь для сокращения.</param>
        /// <returns>Сокращенный путь.</returns>
        public static string GetShortPath(string path)
        {
            StringBuilder shortPathName = new StringBuilder((int)MAX_PATH);
            GetShortPathName(path, shortPathName, MAX_PATH);
            return shortPathName.ToString();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetShortPathName(
           [MarshalAs(UnmanagedType.LPTStr)]
           string lpszLongPath,
           [MarshalAs(UnmanagedType.LPTStr)]
           StringBuilder lpszShortPath,
           uint cchBuffer);
    }
}
