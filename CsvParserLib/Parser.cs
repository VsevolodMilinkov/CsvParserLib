using System;
using System.IO;
using System.Text;

namespace CsvParserLib
{
    public class Parser
    {
        #region Private fields

        private string _inputPath;
        private string _outputPath;
        private string _columnName;
        private string _expression;

        #endregion

        #region Protected properties

        internal string InputPath
        {
            get => _inputPath;
            set => _inputPath = File.Exists(value) ? value : throw new InputFileDoesntExistException();
        }

        internal string OutputPath
        {
            get => _outputPath;
            set => _outputPath = !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                ? value.Trim()
                : throw new EmptyOutputFileNameException();
        }

        internal Encoding Encoding { get; set; }

        internal string ColumnName
        {
            get => _columnName;
            set => _columnName = !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                ? value.Trim()
                : throw new EmptyColumnNameException();
        }

        internal string Expression
        {
            get => _expression;
            set => _expression = !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                ? value.Trim()
                : throw new EmptyExpressionException();
        }

        internal char ColSplitter { get; set; }

        internal char HeaderSplitter { get; set; }

        #endregion

        public Parser(string inputPath, string outputPath, Encoding encoding, string columnName, string expression,
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
            try
            {
                
                using (var sr = new StreamReader(InputPath))
                {
                    //получаем заголовок 
                    string firstLine = sr.ReadLine();
                    if (firstLine == null || string.IsNullOrEmpty(firstLine.Trim()))
                        throw new InputFileIsEmptyException(InputPath);
                    //получаем инфу об указанном в columnName столбце
                    Column column = Column.ParseCsvHeader(firstLine, ColumnName, ColSplitter, HeaderSplitter);
                    //проверяем, а соответствует ли значение для поиска из Expression типу данных из столбца в ColumnName
                    if (!column.Type.CanBeParsed(Expression))
                        throw new ExpressionTypeMismatchException(ColumnName, Expression);
                    //создаем исходящий csv-файл
                    try
                    {
                        if (!File.Exists(OutputPath))
                            File.Create(OutputPath).Close();
                        string currentValue;
                        string currentLine;
                        using (var sw = new StreamWriter(OutputPath, true, Encoding))
                        {
                            //записываем в исходящий файл заголовок входящего файла
                            sw.Write(firstLine);
                            //циклом по остальным строкам
                            while (!sr.EndOfStream)
                            {
                                
                                currentLine = sr.ReadLine();
                                currentValue = currentLine?.Split(ColSplitter)[column.Index];
                                if (column.Type.IsEqual(Expression, currentValue))
                                    sw.Write(currentLine);
                            }
                        }
                    }
                    catch (IOException)
                    {
                        throw new OutputFileIsBusyException(OutputPath);
                    }
                }

                return true;
            }
            catch (IOException)
            {
                throw new InputFileIsBusyException(InputPath);
            }
        }
    }
}