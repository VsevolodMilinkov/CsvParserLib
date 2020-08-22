using System;

namespace CsvParserLib
{
    public class ColumnTypeDate : IColumnType
    {
        public bool CanBeParsed(string expression)
        {
            return DateTime.TryParse(expression, out _);
        }

        public bool IsEqual(string first, string second)
        {
            return CanBeParsed(first) && CanBeParsed(second) && DateTime.Equals(Convert.ToDateTime(first), Convert.ToDateTime(second));    
        }
    }
}