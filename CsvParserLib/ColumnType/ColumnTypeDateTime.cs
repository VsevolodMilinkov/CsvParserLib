using System;

namespace CsvParserLib
{
    public class ColumnTypeDateTime : IColumnType
    {
        public bool CanBeParsed(string expression)
        {
            return DateTime.TryParse(expression, out _);
        }

        public bool IsEqual(string first, string second)
        {
            return CanBeParsed(first) && CanBeParsed(second) && DateTime.Equals(DateTime.Parse(first), DateTime.Parse(second));    
        }
    }
}