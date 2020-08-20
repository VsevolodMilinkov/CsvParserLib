using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvParserLib
{
    public class Column
    {
        #region Properties

        public string Name { get; set; }
        public IColumnType Type { get; set; }
        public int Index { get; set; }

        #endregion

        public Column()
        {
        }

        public Column(string name, IColumnType type, int index)
        {
            Name = name ?? throw new MisingColumnNameException();
            Type = type ?? throw new MissingColumnTypeException(name);
            Index = index;
        }

        public static Column ParseCsvHeader(string csvHeader, string columnName, char colSplitter,
            char headerSplitter)
        {
            if (csvHeader==null ||string.IsNullOrEmpty(csvHeader.Trim())) throw new CsvHeaderIsEmptyException();
            if (columnName==null || string.IsNullOrEmpty(columnName.Trim())) throw new CsvHeaderNoColumnNameException();
            // в заголовке CSV файла столбцы разделяются colSplitter,
            // а сам столбец - на название и тип данных - разделяется headerSplitter-ом;   
            byte nameIndex = 0; // название столбца д.б. первым, а тип данных - вторым
            byte typeIndex = 1;
            // получаем массив столбцов (строками вида "имя тип")
            var arr = csvHeader.Split(colSplitter);
            for (byte i = 0; i < arr.Length; i++)
            {
                var colArray = arr[i].Split(headerSplitter, StringSplitOptions.RemoveEmptyEntries);
                if (colArray.Length > 2)
                    throw new ExcessInfoInColumnException(csvHeader, headerSplitter, colArray);
                if (colArray.Length == 1)
                    throw new NoTypeInColumnException(csvHeader, columnName);
                if (colArray.Length == 0)
                    throw new Exception();//todo: сделать вызов кастомного исключения

                if (colArray[nameIndex].ToLower() == columnName.ToLower())
                {
                    IColumnType colType = ColumnTypeFactory.GetColumnType(colArray[typeIndex]);
                    return new Column(colArray[nameIndex], colType, i);
                }
            }
            throw new NoSuchColumnInCsvHeaderException(columnName, csvHeader);
        }
    }
}