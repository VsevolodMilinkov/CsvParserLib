using System;

namespace CsvParserLib
{
    public class ColumnTypeInt : IColumnType
    {
        public bool CanBeParsed(string expression)
        {
            return expression != null && int.TryParse(expression, out _);
        }

        public bool IsEqual(string first, string second)
        {
            //todo: проверить, что входящие строки не пустые
            return Convert.ToInt64(first) == Convert.ToInt64(second);
        }
    }
}