using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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
            set => _outputPath = string.IsNullOrWhiteSpace(value)
                ? throw new EmptyOutputFileNameException()
                : value.Trim();
        }

        internal Encoding Encoding { get; set; }

        internal string ColumnName
        {
            get => _columnName;
            set => _columnName = string.IsNullOrWhiteSpace(value)
                ? throw new EmptyColumnNameException()
                : value.Trim();
        }

        internal string Expression
        {
            get => _expression;
            set => _expression = string.IsNullOrWhiteSpace(value)
                ? throw new EmptyExpressionException()
                : value.Trim();
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
                    // получаем заголовок 
                    string firstLine = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(firstLine))
                        throw new InputFileIsEmptyException(InputPath);
                    // получаем инфу об искомом столбце columnName
                    Column column = Column.ParseCsvHeader(firstLine, ColumnName, ColSplitter, HeaderSplitter);
                    // проверяем, а соответствует ли значение для поиска из Expression типу данных из столбца в ColumnName
                    if (!column.Type.CanBeParsed(Expression))
                        throw new ExpressionTypeMismatchException(ColumnName, Expression);
                    // создаем/перезаписываем исходящий csv-файл
                    try
                    {
                        string currentValue;
                        string currentLine;
                        using var sw = new StreamWriter(OutputPath, false, Encoding);
                        // записываем в исходящий файл заголовок входящего файла
                        sw.Write(firstLine);
                        // цикл по остальным строкам
                        while (!sr.EndOfStream)
                        {
                            currentLine = sr.ReadLine();
                            currentValue = ParseLine(currentLine, ColSplitter, column.Index);
                            // если искомое выражение идентично значению ячейки, то добавляем перенос строки и записываем текущую строку
                            if (column.Type.IsEqual(Expression, currentValue))
                                sw.Write(sw.NewLine + currentLine);
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

        public static string ParseLine(string currentLine, char colSplitter, int columnIndex)
        {
            // регулярное выражение разделяет строку csv-файла на массив строк, разделяя строку
            // на элементы массива либо по знаку colSplitter, либо по двойным кавычкам,
            // обозначающих начало и конец ячейки в строке
            Regex regex = new Regex($"(\".*\"|[^{colSplitter}])+", RegexOptions.Compiled);
            return regex.Matches(currentLine)[columnIndex].Value.Trim();
        }
    }
}