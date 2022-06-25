﻿using System;
using System.IO;

namespace FileManager.Commands
{
    /// <summary>
    /// Команда вывода дерева каталогов и файлов.
    /// </summary>
	public class ListCommand : ICommand
    {
        /// <summary>
        /// Ключевое слово для вызова команды.
        /// </summary>
        public string KeyWord { get; }

        /// <summary>
        /// 
        /// </summary>
        public int PageNumber { get; private set; } = 1;

        /// <summary>
        /// Конструктор класса команды вывода дерева каталогов и файлов.
        /// </summary>
        /// <param name="keyWord">Ключевое слово для вызова команды.</param>
        public ListCommand(string keyWord)
        {
            KeyWord = keyWord;
        }

        /// <summary>
        /// Метод выполнения команды.
        /// </summary>
        /// <param name="command">Команда для выполнения без ключевого слова.</param>
        /// <param name="currentDir">Текущая директория.</param>
        /// <exception cref="CommandException">Возбуждается, если среди переданных значений есть null 
        /// или при некорректном вводе команды.</exception>
        public string Execute(string command, string currentDir)
        {

            if (command == null)
                throw new CommandException("Ошибка: не указана команда.");

            if (currentDir == null)
                throw new CommandException("Ошибка: не указана текущая директория.");

            currentDir = currentDir.Trim();
            command = command.Trim();
            string path;

            try
            {
                path = Path.GetFullPath(Path.Combine(currentDir, command));
            }
            catch
            {
                throw new Exception("Ошибка:  указанный путь содержит недопустимые символы.");
            }

            if (Directory.Exists(path))
            {
                PageNumber = 1;
                return GetTree(new DirectoryInfo(path));
            }

            var dir = command;

            var index = command.ToLower().LastIndexOf("-p");
            if (index == -1) throw new CommandException($"Ошибка: директория {path} не найдена.");
            if (index == 0) dir = string.Empty;
            if (index > 0) dir = command.Substring(0, index + 1).Trim(); 
            
            try
            {
                dir = Path.GetFullPath(Path.Combine(currentDir, dir));
            }
            catch
            {
                throw new Exception("Ошибка:  указанный путь содержит недопустимые символы.");
            }

            if (!Directory.Exists(dir)) throw new CommandException($"Ошибка: директория {dir} не найдена.");

            var attrs = command.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);

            if (attrs[attrs.Length - 1].ToLower() == "-p") 
                throw new CommandException("Ошибка: не указан номер страницы.");
            if (attrs.Length > 1 && attrs[attrs.Length - 2].ToLower() == "-p")
            {
                if (!int.TryParse(attrs[attrs.Length - 1], out int pageNumber))
                    throw new CommandException("Ошибка: некорректно указан номер страницы.");
                if (pageNumber < 1) 
                    throw new CommandException("Ошибка: номер страницы не может быть меньше 1.");
                PageNumber = pageNumber;
                return GetTree(new DirectoryInfo(dir));
            }
            else throw new CommandException($"Ошибка: директория {path} не найдена.");
        }

        /// <summary>
        /// Метод получения дерева каталогов.
        /// </summary>
        /// <param name="dir">Корневая директория.</param>
        /// <param name="indent">Отступ.</param>
        /// <param name="isFinalDir">Является ли сабдиректрория последней в директории.</param>
        /// <returns>Дерево каталогов и файлов.</returns>
        private string GetTree(DirectoryInfo dir, string indent = "", bool isFinalDir = true)
        {
            if (dir == null) return string.Empty;

            string result = indent + (isFinalDir ? "└" : "├") + dir.Name + "\n";
            indent += isFinalDir ? " " : "│";

            DirectoryInfo[] subDirs;
            FileInfo[] files;

            try
            {
                subDirs = dir.GetDirectories();
                files = dir.GetFiles();
            }
            catch
            {
                return result;
            }

            for (int i = 0; i < files.Length; i++)
                result += indent + (i == files.Length - 1 && subDirs.Length == 0 ? "└" : "├") + files[i].Name + "\n";

            for (int i = 0; i < subDirs.Length; i++)
                result += GetTree(subDirs[i], indent, i == subDirs.Length - 1);

            return result;
        }
    }
}
