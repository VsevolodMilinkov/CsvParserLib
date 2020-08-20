using System;

namespace CsvParserLib
{
    public static class ColumnTypeFactory
    {
        public static IColumnType GetColumnType(string columnTypeString)
        {
            switch (columnTypeString.Trim().ToLower())
            {
                case "string": 
                    return new ColumnTypeString();
                case "integer": 
                    return new ColumnTypeInt();
                case "date": 
                    return new ColumnTypeDateTime();
                case "float": 
                    return new ColumnTypeFloat();
                default: 
                    throw new UnsupportedColumnTypeException(columnTypeString.Trim().ToLower());
            }
        }
    }
}