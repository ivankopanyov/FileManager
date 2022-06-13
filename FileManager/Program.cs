using System;
using FileManager.GUI;
using FileManager.Commands;
using System.IO;

namespace FileManager
{
    class Program
    {
        private static string currentDir = Directory.GetCurrentDirectory();

        private static ICommand[] commands = new ICommand[]
        {
            new ChangeDirectoryCommand("cd"),
            new ListCommand("ls"),
            new CopyCommand("cp"),
            new RemoveCommand("rm"),
            new InfoCommand("file")
        };

        private static void Main(string[] args)
        {
            int widthWindow = Console.WindowWidth - 1;
            int heightWindow = Console.WindowHeight - 1;

            Console.SetWindowSize(widthWindow + 1, heightWindow + 1);
            Console.SetBufferSize(widthWindow + 1, heightWindow + 1);

            int treeWindowHeight = (int)Math.Round((heightWindow - 4) * 0.6);

            var mainWindow = new Window(Window.StackDirection.Vertical, 0);
            var treeWindow = new Window(Window.StackDirection.Vertical, treeWindowHeight);
            var infoWindow = new Window(Window.StackDirection.Vertical, heightWindow - treeWindowHeight - 5);
            var commandWindow = new Window(Window.StackDirection.Vertical, 1);
            commandWindow.Content = currentDir + ">";

            mainWindow.AddWindow(treeWindow);
            mainWindow.AddWindow(infoWindow);
            mainWindow.AddWindow(commandWindow);

            mainWindow.Draw(0, 0, widthWindow, heightWindow);

            while (true)
            {
                var inputCommand = commandWindow.Read();
                var inputCommandArray = inputCommand.Trim().Split();
                if (inputCommandArray.Length == 0)
                {
                    infoWindow.Content = "Ошибка: Команда не указана";
                    mainWindow.Draw(0, 0, widthWindow, heightWindow);
                    continue;
                }

                var command = Array.Find(commands, c => c.Name == inputCommandArray[0].ToLower());
                if (command == null)
                {
                    infoWindow.Content = $"Ошибка: Команда {inputCommandArray[0]} не найдена";
                    mainWindow.Draw(0, 0, widthWindow, heightWindow);
                    continue;
                }

                var result = command.Execute(currentDir, string.Join(" ", Shift(inputCommandArray)));

                if (result.tree != null) treeWindow.Content = result.tree;
                if (result.info != null) infoWindow.Content = result.info;
                if (result.command != null)
                {
                    currentDir = result.command;
                    commandWindow.Content = result.command + ">";
                }

                mainWindow.Draw(0, 0, widthWindow, heightWindow);
            }
        }

        /// <summary>
        /// Удаляет первый элемент массива.
        /// </summary>
        /// <param name="array">Массива для удалаения первого элемента.</param>
        /// <returns>Новый массив с удаленным первым элементом из переданного массива.</returns>
        private static string[] Shift(string[] array)
        {
            if (array == null || array.Length == 0) return array;
            if (array.Length == 1) return new string[0];

            var result = new string[array.Length - 1];
            Array.Copy(array, 1, result, 0, array.Length - 1);

            return result;
        }
    }
}
