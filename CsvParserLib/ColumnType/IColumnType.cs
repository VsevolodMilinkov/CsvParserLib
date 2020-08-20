namespace CsvParserLib
{
    public interface IColumnType
    {
        public bool CanBeParsed(string expression);
        public bool IsEqual(string first, string second);
    }
}