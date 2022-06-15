using FileManager.UI;

namespace FileManager
{
	/// <summary>
	/// Интерфейс для реализации команды файлового менеджера.
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// Ключевое слово для вызова команды.
		/// </summary>
		string KeyWord { get; }

		/// <summary>
		/// Окно для вывода результата выполнения команды.
		/// </summary>
		Window ResultWindow { get; }

		/// <summary>
		/// Метод выполнения команды.
		/// </summary>
		/// <param name="command">Атрибуты команды.</param>
		/// <param name="currentDir">Текущая директория файлового менеджера.</param>
		void Execute(string command, string currentDir);
	}
}

