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
            return CanBeParsed(first) && CanBeParsed(second) && float.Parse(first) == float.Parse(second);
        }
    }
}