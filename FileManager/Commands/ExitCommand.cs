using System.Diagnostics;
using System.Threading.Tasks;
using FileManager.UI;

namespace FileManager
{
    /// <summary>
    /// Команда завершения работы приложения.
    /// </summary>
    public class ExitCommand : ICommand
    {
        /// <summary>
        /// Ключевое слово для вызова команды.
        /// </summary>
        public string KeyWord { get; }

        /// <summary>
        /// Конструктор класса команды завершения работы приложения.
        /// </summary>
        /// <param name="keyWord">Ключевое слово для вызова команды.</param>
        public ExitCommand(string keyWord)
        {
            KeyWord = keyWord;
        }

        /// <summary>
        /// Метод выполнения команды.
        /// </summary>
        /// <param name="command">Команда для выполнения без ключевого слова.</param>
        /// <param name="currentDir">Текущая директория.</param>
        /// <exception cref="CommandException">Возбуждается, если среди переданных значений есть null.</exception>
        public string Execute(string command, string currentDir)
        {
            if (command == null)
                throw new CommandException("Ошибка: не указана команда.");

            if (!string.IsNullOrWhiteSpace(command))
                throw new CommandException($"Команда {KeyWord} не содержит параметра {command}.");

            KillProcess();
            return "Завершение работы приложения...";
        }

        /// <summary>
        /// Завершает работу приложения через секунду после вызова.
        /// </summary>
        /// <returns></returns>
        private async void KillProcess()
        {
            await Task.Delay(1000);
            Process.GetCurrentProcess().Kill();
        }
    }
}

