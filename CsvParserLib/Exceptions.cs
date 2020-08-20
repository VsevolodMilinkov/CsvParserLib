using System;

namespace CsvParserLib
{
    #region Parser Class exceptions

    // Todo: а исключения здесь наследуются от верных классов?
    public class EmptyColumnNameException : ArgumentException
    {
        public override string Message { get; } = "Получено пустое имя столбца для парсинга!";
    }

    public class EmptyExpressionException : Exception
    {
        public override string Message { get; } = "Получена пустая строка вместо выражения для поиска!";
    }

    public class EmptyOutputFileNameException : ArgumentException
    {
        public override string Message { get; } = "Введите имя исходящего файла!";
    }

    public class InputFileDoesntExistException : ArgumentException
    {
        public override string Message { get; } = "Введите имя входящего файла!";
    }

    public class InputFileIsEmptyException : ArgumentException
    {
        public override string Message { get; }

        public InputFileIsEmptyException(string inputPath)
        {
            Message = $"Входящий файл \"{inputPath}\" пуст!";
        }
    }

    public class InputFileIsBusyException : ArgumentException
    {
        public override string Message { get; }

        public InputFileIsBusyException(string inputPath)
        {
            Message = $"Входящий файл \"{inputPath}\" занят другим процессом!";
        }
    }

    // todo: затестить и понять - надо ли вообще? это же исходящий файл, его либо не должно быть, либо ругаться, что такой файл уже есть, не?
    public class OutputFileIsBusyException : Exception
    {
        public override string Message { get; }

        public OutputFileIsBusyException(string outputPath)
        {
            Message = $"Исходящий файл \"{outputPath}\" занят другим процессом.";
        }
    }

    public class ExpressionTypeMismatchException : Exception
    {
        public override string Message { get; }

        public ExpressionTypeMismatchException(string columnName, object expression)
        {
            Message =
                $"Указанное значение для парсинга (\"{expression}\") не соответстует типу данных в столбце \"{columnName}\"";
        }
    }

    #endregion

    #region Column class exceptions

    internal class MissingColumnTypeException : Exception
    {
        public override string Message { get; }

        public MissingColumnTypeException(string s)
        {
            Message = $"У столбца \"{s}\" не указан тип данных!";
        }
    }

    internal class MisingColumnNameException : ArgumentException
    {
        public override string Message { get; } = "Указано пустое имя столбца!";
    }

    public class CsvHeaderIsEmptyException : ArgumentException
    {
        public override string Message { get; } = "Получена пустая строка вместо заголовка Csv файла!";
    }

    public class UnsupportedColumnTypeException : Exception
    {
        public override string Message { get; }

        public UnsupportedColumnTypeException(string columnTypeString)
        {
            Message = $"В файле указан столбец неподдерживаемого типа \"{columnTypeString}\".";
        }
    }

    public class CsvHeaderNoColumnNameException : ArgumentNullException
    {
        public override string Message { get; } =
            "Получена пустая строка вместо имени столбца для поиска - укажите имя столбца.";
    }

    public class ExcessInfoInColumnException : Exception
    {
        public override string Message { get; }

        public ExcessInfoInColumnException(string csvHeader, char headerSplitter, string[] colArray)
        {
            Message = $"В заголовке \"{csvHeader}\" при разборе столбца получено более двух атрибутов " +
                      $"(ожидается лишь имя и тип через знак \"{headerSplitter}\", массив - \"{string.Join(' ', colArray)}\")";
        }
    }

    public class NoTypeInColumnException : Exception
    {
        public override string Message { get; }

        public NoTypeInColumnException(string csvHeader, string columnName)
        {
            Message = $"В заголовке файла столбец \"{columnName}\" не имеет типа! (заголовок - \"{csvHeader}\")";
        }
    }

    public class NoSuchColumnInCsvHeaderException : Exception
    {
        public override string Message { get; }

        public NoSuchColumnInCsvHeaderException(string columnName, string csvHeader)
        {
            Message = $"Не удалось найти столбец \"{columnName}\" в заголовке csv файла (заголовок - \"{csvHeader}\").";
        }
    }

    #endregion
}