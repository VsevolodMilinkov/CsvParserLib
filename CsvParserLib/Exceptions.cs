using System;
using System.Runtime.Serialization;

namespace CsvParserLib
{
    #region Parser Class exceptions

    public class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
        }
    }
    public class EmptyColumnNameException : ParserException
    {
        public EmptyColumnNameException() : base("Получено пустое имя столбца для парсинга!")
        {
        }
    }

    public class EmptyExpressionException : ParserException
    {
        public EmptyExpressionException() : base("Получена пустая строка вместо выражения для поиска!")
        {
        }
    }

    public class EmptyOutputFileNameException : ParserException
    {
        public EmptyOutputFileNameException() : base("Введите имя исходящего файла!")
        {
        }
    }

    public class InputFileDoesntExistException : ParserException
    {
        public InputFileDoesntExistException() : base("Не найден указанный входящий файл!")
        {
        }
    }

    public class InputFileIsEmptyException : ParserException
    {
        public InputFileIsEmptyException(string inputPath) : base($"Входящий файл \"{inputPath}\" пуст!")
        {
        }
    }

    public class InputFileIsBusyException : ParserException
    {
        public InputFileIsBusyException(string inputPath) : base(
            $"Входящий файл \"{inputPath}\" занят другим процессом!")
        {
        }
    }

    public class OutputFileIsBusyException : ParserException
    {
        public OutputFileIsBusyException(string outputPath) : base(
            $"Исходящий файл \"{outputPath}\" занят другим процессом.")
        {
        }
    }

    public class ExpressionTypeMismatchException : ParserException
    {
        public ExpressionTypeMismatchException(string columnName, object expression) : base(
            $"Указанное значение для парсинга (\"{expression}\") не соответстует типу данных в столбце \"{columnName}\"")
        {
        }
    }

    #endregion

    #region Column class exceptions

    public class MissingColumnTypeException : ParserException
    {
        public MissingColumnTypeException(string s) : base($"У столбца \"{s}\" не указан тип данных!")
        {
        }
    }

    public class MissingColumnNameException : ParserException
    {
        public MissingColumnNameException() : base("Указано пустое имя столбца!")
        {
        }
    }

    public class CsvHeaderIsEmptyException : ParserException
    {
        public CsvHeaderIsEmptyException() : base("Получена пустая строка вместо заголовка Csv файла!")
        {
        }
    }

    public class UnsupportedColumnTypeException : ParserException
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

    public class ExcessInfoInColumnException : ParserException
    {
        public ExcessInfoInColumnException(string csvHeader, char headerSplitter, string[] colArray) : base(
            $"В заголовке \"{csvHeader}\" при разборе искомого столбца получено более двух атрибутов " +
            $"(ожидается лишь имя без пробелов и тип через знак \"{headerSplitter}\", массив - \"{string.Join(' ', colArray)}\")")
        {
        }
    }

    public class NoTypeInColumnException : ParserException
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

    public class NoSuchColumnInCsvHeaderException : ParserException
    {
        public NoSuchColumnInCsvHeaderException(string columnName, string csvHeader) : base(
            $"Не удалось найти столбец \"{columnName}\" в заголовке csv файла (заголовок - \"{csvHeader}\").")
        {
        }
    }

    #endregion
}