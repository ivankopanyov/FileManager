namespace FileManager.Commands
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
		/// Метод выполнения команды.
		/// </summary>
		/// <param name="command">Атрибуты команды.</param>
		/// <param name="currentDir">Текущая директория файлового менеджера.</param>
		string Execute(string command, string currentDir);
	}
}

