using System;

namespace CsvParserLib
{
    #region Parser Class exceptions

    // Todo: а исключения здесь наследуются от верных классов?
    public class EmptyColumnNameException : ArgumentException
    {
        public override string Message { get; } = "Получено пустое имя столбца для парсинга!";
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
    
    public class IncorrectExpressionDataTypeException : Exception
    {
        public override string Message { get; }
        public IncorrectExpressionDataTypeException(string columnName, object expression)
        {
            Message = $"Указанное значение для парсинга (\"{expression}\") не соответстует типу данных в столбце \"{columnName}\"";
        }
    }

    #endregion

    #region CsvColumn class exceptions

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

    public class IncorrectColumnTypeException : ArgumentException
    {
        public override string Message { get; }

        public IncorrectColumnTypeException(string columnName, string typeInHeader)
        {
            Message = $"Невозможно определить тип данных столбца \"{columnName}\"! (Указанный в файле тип - \"{typeInHeader}\")";
        }
    }

    public class CsvHeaderNoColumnNameException : ArgumentNullException
    {
        public override string Message { get; } =
            "Получена пустая строка вместо имени столбца для поиска - укажите имя столбца.";
    }

    #endregion
}