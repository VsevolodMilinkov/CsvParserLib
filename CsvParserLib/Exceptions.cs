using System;

namespace CsvParserLib
{
    #region Parser Class exceptions

    public class EmptyColumnNameException : ArgumentException
    {
        public EmptyColumnNameException() : base("Получено пустое имя столбца для парсинга!")
        {
        }
    }

    public class EmptyExpressionException : ArgumentException
    {
        public EmptyExpressionException() : base("Получена пустая строка вместо выражения для поиска!")
        {
        }
    }

    public class EmptyOutputFileNameException : ArgumentException
    {
        public EmptyOutputFileNameException() : base("Введите имя исходящего файла!")
        {
        }
    }

    public class InputFileDoesntExistException : ArgumentException
    {
        public InputFileDoesntExistException() : base("Введите имя входящего файла!")
        {
        }
    }

    public class InputFileIsEmptyException : ArgumentException
    {
        public InputFileIsEmptyException(string inputPath) : base($"Входящий файл \"{inputPath}\" пуст!")
        {
        }
    }

    public class InputFileIsBusyException : ArgumentException
    {
        public InputFileIsBusyException(string inputPath) : base(
            $"Входящий файл \"{inputPath}\" занят другим процессом!")
        {
        }
    }

    public class OutputFileIsBusyException : ArgumentException
    {
        public OutputFileIsBusyException(string outputPath) : base(
            $"Исходящий файл \"{outputPath}\" занят другим процессом.")
        {
        }
    }

    public class ExpressionTypeMismatchException : ArgumentException
    {
        public ExpressionTypeMismatchException(string columnName, object expression) : base(
            $"Указанное значение для парсинга (\"{expression}\") не соответстует типу данных в столбце \"{columnName}\"")
        {
        }
    }

    #endregion

    #region Column class exceptions

    public class MissingColumnTypeException : ArgumentException
    {
        public MissingColumnTypeException(string s) : base($"У столбца \"{s}\" не указан тип данных!")
        {
        }
    }

    public class MissingColumnNameException : ArgumentException
    {
        public MissingColumnNameException() : base("Указано пустое имя столбца!")
        {
        }
    }

    public class CsvHeaderIsEmptyException : ArgumentException
    {
        public CsvHeaderIsEmptyException() : base("Получена пустая строка вместо заголовка Csv файла!")
        {
        }
    }

    public class UnsupportedColumnTypeException : ArgumentException
    {
        public UnsupportedColumnTypeException(string columnTypeString) : base(
            $"Неподдерживаемый тип искомого столбца - \"{columnTypeString}\".")
        {
        }
    }

    public class CsvHeaderNoColumnNameException : ArgumentNullException
    {
        public CsvHeaderNoColumnNameException() : base(
            "Получена пустая строка вместо имени искомого столбца - укажите имя столбца.")
        {
        }
    }

    public class ExcessInfoInColumnException : ArgumentException
    {
        public ExcessInfoInColumnException(string csvHeader, char headerSplitter, string[] colArray) : base(
            $"В заголовке \"{csvHeader}\" при разборе искомого столбца получено более двух атрибутов " +
            $"(ожидается лишь имя без пробелов и тип через знак \"{headerSplitter}\", массив - \"{string.Join(' ', colArray)}\")")
        {
        }
    }

    public class NoTypeInColumnException : ArgumentException
    {
        public NoTypeInColumnException(string csvHeader, string columnName) : base(
            $"В заголовке файла искомый столбец \"{columnName}\" не имеет типа! (заголовок - \"{csvHeader}\")")
        {
        }
    }

    public class EmptyColumnException : Exception
    {
        public EmptyColumnException(int index) : base($"Пустой столбец с индексом {index} в заголовке csv файла!")
        {
        }
    }

    public class NoSuchColumnInCsvHeaderException : ArgumentException
    {
        public NoSuchColumnInCsvHeaderException(string columnName, string csvHeader) : base(
            $"Не удалось найти столбец \"{columnName}\" в заголовке csv файла (заголовок - \"{csvHeader}\").")
        {
        }
    }

    #endregion
}