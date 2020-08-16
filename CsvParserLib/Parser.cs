using System;
using System.IO;
using System.Text;

namespace CsvParserLib
{
    public class Parser
    {
        #region private fields

        private string _inputPath;
        private string _outputPath;
        private string _columnName;
        private object _expression;

        #endregion

        #region protected properties

        protected internal string InputPath
        {
            get => _inputPath;
            set => _inputPath = File.Exists(value) ? value : throw new InputFileDoesntExistException();
        }

        protected internal string OutputPath
        {
            get => _outputPath;
            set => _outputPath = !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                ? value.Trim()
                : throw new EmptyOutputFileNameException();
        }

        protected internal Encoding Encoding { get; set; }

        protected internal string ColumnName
        {
            get => _columnName;
            set => _columnName = !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                ? value.Trim()
                : throw new EmptyColumnNameException();
        }

        protected internal object Expression
        {
            get => _expression;
            set => _expression = value;
        }

        protected internal char ColSplitter { get; set; }

        protected internal char HeaderSplitter { get; set; }

        #endregion


        public Parser(string inputPath, string outputPath, Encoding encoding, string columnName, object expression,
            char colSplitter = ';', char headerSplitter = ' ')
        {
            InputPath = inputPath;
            OutputPath = outputPath;
            Encoding = encoding;
            ColumnName = columnName;
            Expression = expression;
            ColSplitter = colSplitter;
            HeaderSplitter = headerSplitter;
        }

        public bool Parse()
        {
            throw new NotImplementedException();
        }
    }

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
}