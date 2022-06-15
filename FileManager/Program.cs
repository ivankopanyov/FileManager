using System;
using System.IO;
using FileManager.UI;
using FileManager.Utils;

namespace FileManager
{
    class Program
    {
        /// <summary>
        /// Массив, хранящий набор комманд для файлового менеджера.
        /// </summary>
        private static ICommand[] commands;

        /// <summary>
        /// Основное окно файлового менеджера.
        /// </summary>
        private static Window mainWindow = new Window();

        /// <summary>
        /// Окно для вывода дерева каталогов и файлов.
        /// </summary>
        private static Window treeWindow = new Window();

        /// <summary>
        /// Окно для вывода инфомации и сообщении об ошибках.
        /// </summary>
        private static Window infoWindow = new Window();

        /// <summary>
        /// Окно для вывода текущей директори и ввода команд пользователем.
        /// </summary>
        private static Window commandWindow = new Window();

        /// <summary>
        /// Файл для сохранения текущего состояния окон.
        /// </summary>
        private static string savingFileName = "data.xml";

        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            FixedWindowSize(120, 40);

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Cyan;

#if DEBUG
            Console.Title = Properties.Settings.Default.AppNameDebug;
#else
            Console.Title = Properties.Settings.Default.AppName;
#endif

            commands = new ICommand[]
            {
                new ChangeDirectoryCommand("cd", commandWindow),
                new ListCommand("ls", treeWindow),
                new CopyCommand("cp", infoWindow),
                new RemoveCommand("rm", infoWindow),
                new InfoCommand("info", infoWindow),
                new ExitCommand("exit", infoWindow)
            };

            commandWindow.Content = FileManager.GetShortPath(Directory.GetCurrentDirectory()) + ">";

            var data = FileManager.LoadState(savingFileName);
            if (data != null && data.Length == 3)
            {
                treeWindow.Content = data[0];
                infoWindow.Content = data[1];
                commandWindow.Content = data[2];
            }

            mainWindow.AddWindow(treeWindow);
            mainWindow.AddWindow(infoWindow);
            mainWindow.AddWindow(commandWindow);

            while (true)
            {
                SetWindowsSize();
                mainWindow.Draw(0, 0, Console.WindowWidth - 1, Console.WindowHeight - 1);
                var inputCommand = commandWindow.Read(FileManager.Commands);
                FileManager.AddCommand(inputCommand);
                infoWindow.Content = string.Empty;
                var inputCommandArray = inputCommand.Trim().Split();
                if (inputCommandArray.Length == 0)
                {
                    infoWindow.Content = "Ошибка: Команда не указана";
                    continue;
                }

                var command = Array.Find(commands, c => c.KeyWord == inputCommandArray[0].ToLower());
                if (command == null)
                {
                    infoWindow.Content = $"Ошибка: Команда {inputCommandArray[0]} не найдена";
                    continue;
                }

                try
                {
                    var attrs = string.Join(" ", Shift(inputCommandArray));
                    var currentDir = commandWindow.Content.Replace(">", string.Empty);
                    command.Execute(attrs, currentDir);

                    if (command.GetType() != typeof(ExitCommand))
                        FileManager.SaveState(savingFileName, 
                            new string[] { treeWindow.Content, infoWindow.Content, commandWindow.Content });
                }
                catch (FileManagerException e)
                {
                    infoWindow.Content = e.Message;
                }
                catch (Exception e)
                {
                    infoWindow.Content = e.Message;
                    FileManager.WriteExceptionInfo(e);
                }
            }
        }

        /// <summary>
        /// Метод рассчета размера окон.
        /// </summary>
        private static void SetWindowsSize()
        {
            if (Properties.Settings.Default.LineCounts < 1) Properties.Settings.Default.LineCounts = 1;
            else if (Properties.Settings.Default.LineCounts > Console.WindowHeight - 17)
                Properties.Settings.Default.LineCounts = Console.WindowHeight - 17;
            Properties.Settings.Default.Save();

            treeWindow.Weigth = Properties.Settings.Default.LineCounts + 1;
            infoWindow.Weigth = Console.WindowHeight - Properties.Settings.Default.LineCounts - 7;
            commandWindow.Weigth = 1;
        }

        /// <summary>
        /// Метод установки фиксированного размера консоли.
        /// </summary>
        /// <param name="width">Ширина консоли.</param>
        /// <param name="height">Высота консоли.</param>
        private static void FixedWindowSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            IntPtr handle = API.GetConsoleWindow();
            IntPtr sysMenu = API.GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                API.DeleteMenu(sysMenu, API.SC_MINIMIZE, API.MF_BYCOMMAND);
                API.DeleteMenu(sysMenu, API.SC_MAXIMIZE, API.MF_BYCOMMAND);
                API.DeleteMenu(sysMenu, API.SC_SIZE, API.MF_BYCOMMAND);
            }
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
    }
}
