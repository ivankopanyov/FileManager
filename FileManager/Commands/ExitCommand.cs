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
        /// Окно для вывода результата выполнения команды.
        /// </summary>
        public Window ResultWindow { get; }

        /// <summary>
        /// Конструктор класса команды завершения работы приложения.
        /// </summary>
        /// <param name="keyWord">Ключевое слово для вызова команды.</param>
        /// <param name="resultWindow">Окно для вывода результата выполнения команды.</param>
        /// <exception cref="NullWindowException">Возбуждается, если переданный экземпляр класса Window равен null.</exception>
        public ExitCommand(string keyWord, Window resultWindow)
        {
            KeyWord = keyWord;
            if (resultWindow == null)
                throw new NullWindowException("Переданный экземпляр класса Window не должен быть null");
            ResultWindow = resultWindow;
        }

        /// <summary>
        /// Метод выполнения команды.
        /// </summary>
        /// <param name="command">Команда для выполнения без ключевого слова.</param>
        /// <param name="currentDir">Текущая директория.</param>
        /// <exception cref="FileManagerException">Возбуждается, если среди переданных значений есть null.</exception>
        public void Execute(string command, string currentDir)
        {
            if (command == null)
                throw new FileManagerException("Ошибка: не указана команда.");

            if (!string.IsNullOrWhiteSpace(command))
                throw new FileManagerException($"Команда {KeyWord} не содержит параметра {command}.");

            ResultWindow.Content = "Завершение работы приложения...";
            KillProcess();
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

