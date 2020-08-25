namespace CsvParserLib
{
    public class ColumnTypeString : IColumnType
    {
        public bool CanBeParsed(string expression)
        {
            //я не буду конвертить строку в строку, неет - только проверю, что строка не пустая;
            return !string.IsNullOrWhiteSpace(expression);
        }

        public bool IsEqual(string first, string second)
        {
            return string.Equals(first, second);
        }
    }
}