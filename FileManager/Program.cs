using System;
using System.IO;
using FileManager.UI;
using FileManager.Commands;

namespace FileManager
{
    class Program
    {
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

            commandWindow.Content = CommandLine.GetShortPath(Directory.GetCurrentDirectory()) + ">";
            
            var data = FileManager.LoadState(savingFileName);
            if (data != null && data.Length == 3)
            {
                treeWindow.Content = data[0];
                infoWindow.Content = data[1];
                commandWindow.Content = data[2];
            }

            treeWindow.showPageNumber = true;

            mainWindow.AddWindow(treeWindow);
            mainWindow.AddWindow(infoWindow);
            mainWindow.AddWindow(commandWindow);

            var command = new CommandLine();

            while (true)
            {
                SetWindowsSize();
                mainWindow.Draw(0, 0, Console.WindowWidth - 1, Console.WindowHeight - 1);
                var inputCommand = commandWindow.Read(command.History);
                infoWindow.Content = string.Empty;

                try
                {
                    var currentDir = commandWindow.Content.Replace(">", string.Empty);
                    var result = command.Execute(inputCommand, currentDir);

                    Window window = infoWindow;
                    if (command.LastCommand.GetType() == typeof(ListCommand))
                    {
                        window = treeWindow;
                        window.PageNumber = ((ListCommand)command.LastCommand).PageNumber;
                    }
                    else if (command.LastCommand.GetType() == typeof(ChangeDirectoryCommand))
                        window = commandWindow;
                    
                    if (command.LastCommand.GetType() != typeof(ExitCommand))
                        FileManager.SaveState(savingFileName,
                            new string[] { treeWindow.Content, infoWindow.Content, commandWindow.Content });

                    window.Content = result;
                }
                catch (CommandException e)
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

            IntPtr handle = FileManager.GetConsoleWindow();
            IntPtr sysMenu = FileManager.GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                FileManager.DeleteMenu(sysMenu, FileManager.SC_MINIMIZE, FileManager.MF_BYCOMMAND);
                FileManager.DeleteMenu(sysMenu, FileManager.SC_MAXIMIZE, FileManager.MF_BYCOMMAND);
                FileManager.DeleteMenu(sysMenu, FileManager.SC_SIZE, FileManager.MF_BYCOMMAND);
            }
        }
    }
}
