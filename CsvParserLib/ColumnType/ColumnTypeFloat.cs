using System;

namespace CsvParserLib
{
    public class ColumnTypeFloat : IColumnType
    {
        public bool CanBeParsed(string expression)
        {
            return float.TryParse(expression, out _);
        }

        public bool IsEqual(string first, string second)
        {
            return CanBeParsed(first) && CanBeParsed(second) && Convert.ToSingle(first) == Convert.ToSingle(second);
        }
    }
}