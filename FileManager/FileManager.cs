using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace FileManager
{
	/// <summary>
	/// Статический класс вспомогательных методов для работы с файловым менеджером.
	/// </summary>
	public static class FileManager
	{
		public const int MF_BYCOMMAND = 0x00000000;
		public const int SC_CLOSE = 0xF060;
		public const int SC_MINIMIZE = 0xF020;
		public const int SC_MAXIMIZE = 0xF030;
		public const int SC_SIZE = 0xF000;

		[DllImport("user32.dll")]
		public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		public static extern IntPtr GetConsoleWindow();

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
    }
}
