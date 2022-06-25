using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FileManager.Commands
{
    public class CommandLine
    {
        private const uint MAX_PATH = 255;

        private int limitHistory = 10;

        /// <summary>
        /// Список, хранящий историю введенных команд.
        /// </summary>
        private static List<string> history = new List<string>();

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

        /// <summary>
        /// Свойство, возвращающее и записывающее колличество выводимых элементов на страницу в дереве каталогов.
        /// Записывает и считывает из настроек пользователя.
        /// </summary>
        private int LimitHistory
        {
            get => limitHistory;

            set => limitHistory = value;
        }

        /// <summary>
        /// Возвращает массив, содержащий копию списка введенных команд.
        /// </summary>
        public string[] History => history.ToArray();

        /// <summary>
        /// Добавляет команду в историю команд.
        /// </summary>
        /// <param name="command">Команда.</param>
        public void AddToHistory(string command)
        {
            UpdateHistory();

            if (LimitHistory == 0 || string.IsNullOrWhiteSpace(command) || history.FindIndex(c => c.ToLower() == command.Trim().ToLower()) != -1)
                return;
            history.Insert(0, command.Trim());
            if (history.Count() > limitHistory)
                history.RemoveRange(limitHistory, history.Count() - limitHistory);
        }

        /// <summary>
        /// Обновляет список команд после изменения лимита списка.
        /// </summary>
        private void UpdateHistory()
        {
            if (LimitHistory < 0) LimitHistory = 0;

            if (history.Count() > LimitHistory)
                history.RemoveRange(LimitHistory, history.Count() - LimitHistory);
        }

        public string Execute(string inputCommand, string currentDir)
        {
            AddToHistory(inputCommand);
            var inputCommandArray = inputCommand.Trim().Split();
            if (inputCommandArray.Length == 0) 
                throw new CommandException("Ошибка: Команда не указана");

            var command = commands.Find(c => c.KeyWord.ToLower() == inputCommandArray[0].ToLower());

            if (command == null)
                throw new CommandException($"Ошибка: Команда {inputCommandArray[0]} не найдена");

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
