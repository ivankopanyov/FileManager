using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FileManager.Utils;

namespace FileManager
{
	/// <summary>
	/// Статический класс вспомогательных методов для работы с файловым менеджером.
	/// </summary>
	public static class FileManager
	{
		/// <summary>
		/// Список, хранящий историю введенных команд.
		/// </summary>
		private static List<string> commands = new List<string>();

		/// <summary>
		/// Возвращает массив, содержащий копию списка введенных команд.
		/// </summary>
		public static string[] Commands => commands.ToArray();

		/// <summary>
		/// Свойство, возвращающее и записывающее колличество выводимых элементов на страницу в дереве каталогов.
		/// Записывает и считывает из настроек пользователя.
		/// </summary>
		private static int Limit
		{ 
			get => Properties.Settings.Default.LimitCommands;

			set	=> Properties.Settings.Default.LimitCommands = value;
		}

		/// <summary>
		/// Добавляет команду в историю команд.
		/// </summary>
		/// <param name="command">Команда.</param>
		public static void AddCommand(string command)
		{
			UpdateCommandsList();

			if (Limit == 0 || string.IsNullOrWhiteSpace(command) || commands.FindIndex(c => c.ToLower() == command.Trim().ToLower()) != -1) 
				return;
			commands.Insert(0, command.Trim());
			if (commands.Count() > Properties.Settings.Default.LimitCommands)
				commands.RemoveRange(Properties.Settings.Default.LimitCommands, commands.Count() - Properties.Settings.Default.LimitCommands);
		}

		/// <summary>
		/// Запись текущего отображаемого контента окон в файл.
		/// </summary>
		/// <param name="fileName">Файл для записи.</param>
		/// <param name="data">Массив данных для записи.</param>
		public static void SaveState(string fileName, params string[] data)
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(string[]));
				using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
				{
					xmlSerializer.Serialize(fs, data);
				}
			}
			catch (Exception e)
			{
				WriteExceptionInfo(e);
			}
		}

		/// <summary>
		/// Чтение из файла последенего сохраненного контента окон.
		/// </summary>
		/// <param name="fileName">Файл для чтения</param>
		/// <returns>Массив строк, считанных из файла.</returns>
		public static string[] LoadState(string fileName)
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(string[]));
				using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
				{
					return xmlSerializer.Deserialize(fs) as string[];
				}
			}
			catch (Exception e)
			{
				WriteExceptionInfo(e);
			}
			return null;
		}

		/// <summary>
		/// Записывает в файл сообщения об исключениях.
		/// </summary>
		/// <param name="e">Возбужденное исключение.</param>
		public static void WriteExceptionInfo(Exception e)
		{
			string dirName = "errors";
			string fileName = "random_name_exception.txt";
			try
			{
				if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
				using (StreamWriter sw = new StreamWriter(Path.Combine(dirName, fileName), true))
				{
					sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss ") + e.Message + '\n' + e.StackTrace);
				}
			}
			catch
			{ 
			
			}
		}

		/// <summary>
		/// Обновляет список команд после изменения лимита списка.
		/// </summary>
		private static void UpdateCommandsList()
		{
			if (Limit < 0) Limit = 0;

			if (commands.Count() > Limit)
				commands.RemoveRange(Limit, commands.Count() - Limit);
		}
    }
}
