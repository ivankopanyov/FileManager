# FileManager  
#### Итоговый проэкт по курсу "Введение в C#"
Консольное приложение для работы с файлами и директориями.
##### Список команд.
- [cd](#cd)  -  изменение текущей директории
- [ls](#ls) - вывод дерева каталогов и файлов
- [cp](#cp) - копирование каталога или файла
- [rm](#rm) - удаление каталога или файла
- [info](#info) - вывод информации о каталоге или файле
- [exit](#exit) - завершение работы приложения

> Команды, их аттрибуды и пути вводятся через пробел без кавычек и прочих разделителей. Путь указывается относительно текущей директории. Для перехода в директорию выше используйте две точки. Для навигации по истории введенных команд используйте клавиши вниз и вверх. В конфигурационном файле задается настройка колличества выводимых элементов на страницу и колличество хранимых в истории команд.

# cd
##### Изменение текущей директории.
В качестве аргумента указывается путь директории относительно текущей директории.

Переход из директории FileManager в Documents:
```sh
C:\ProgramFiles\FileManager> cd ..\..\Users\Иван Копанев\Documents  
```

# ls
##### Вывод дерева каталогов и файлов.
В качестве аргумента указывается путь корневой директории относительно текущей директории. Если параметр не указан, выводится дерево для текущей директории.

##### -p
Параметр для указания номера страницы дерева. Если параметр не указан выводит первую страницу.

Вывод второй страницы дерева каталогов и файлов из директории Documents:
```sh
C:\ProgramFiles\FileManager> ls ..\..\Users\Иван Копанев\Documents -p 2
```

# cp
##### Копирование каталога или файла.
В качестве аргумента указываются пути исходного и нового каталога или файла. Если исходный путь не указан в качестве исходного каталога используется текущая директория.

Копирование file.txt из директории FileManager в директорию Documents:
```sh
C:\ProgramFiles\FileManager> cp file.txt ..\..\Users\Иван Копанев\Documents\file.txt
```

Копирование директории FileManager в директорию Documents:
```sh
C:\ProgramFiles\FileManager> cp ..\..\Users\Иван Копанев\Documents\FileManager
```

# rm
#### Удаление каталога или файла.
В качестве аргумента указывается путь каталога или файла для удаления. Если путь не указан удаляется текущая директория.

Удаление file.txt из директории Documents:
```sh
C:\ProgramFiles\FileManager> rm ..\..\Users\Иван Копанев\Documents\file.txt
```

Удаление директории errors:
```sh
C:\ProgramFiles\FileManager\errors> rm
```

# info
#### Вывод информации о каталоге или файле.
В качестве аргумента указывается путь каталога или файла для вывода информации. Если путь не указан выводится информация о текущей директории.

Вывод информации о файле file.txt:
```sh
C:\ProgramFiles\FileManager> info ..\..\Users\Иван Копанев\Documents\file.txt
```

Вывод информации о директории FileManager:
```sh
C:\ProgramFiles\FileManager> info
```

# exit
#### Завершение работы приложения.
```sh
C:\ProgramFiles\FileManager> exit
```