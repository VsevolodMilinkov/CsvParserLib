using System;

namespace CsvParserLib
{
    public class CsvColumn
    {
        #region Properties

        public string Name { get; set; }
        public Type Type { get; set; }
        public int Index { get; set; }

        #endregion

        public CsvColumn()
        {
        }

        public CsvColumn(string name, Type type, int index)
        {
            Name = name ?? throw new MisingColumnNameException();
            Type = type ?? throw new MissingColumnTypeException(name);
            Index = index;
        }

        public CsvColumn ParseCsvHeader(string csvHeader, string columnName, char colSplitter,
            char headerSplitter)
        {
            if (string.IsNullOrWhiteSpace(csvHeader)) throw new CsvHeaderIsEmptyException();
            if (string.IsNullOrWhiteSpace(columnName)) throw new CsvHeaderNoColumnNameException();
            // в заголовке CSV файла столбцы разделяются colSplitter,
            // а сам столбец - на название и тип данных - разделяется colSplitter-ом;   
            var nameIndex = 0; // название столбца д.б. первым, а тип данных - вторым
            var typeIndex = 1;
            CsvColumn column = new CsvColumn();
            // получаем массив столбцов (пока как "имя+тип")
            var arr = csvHeader.Split(colSplitter);
            for (int i = 0; i < arr.Length; i++)
            {
                var colArray = arr[i].Split(headerSplitter);
                if (colArray.Length > 2)
                    throw new Exception();
                if (colArray.Length == 1)
                    throw new Exception();
                if (colArray.Length == 0)
                    throw new Exception();

                if (colArray[nameIndex].ToLower() == columnName.ToLower())
                {
                    var colType = Type.GetType("System." + colArray[typeIndex]);
                    if (colType == null)
                        throw new IncorrectColumnTypeException(colArray[nameIndex], colArray[typeIndex]);
                    column = new CsvColumn(colArray[nameIndex], colType, i);
                    break;
                }
            }

            if (column == null)
                throw new Exception();
            return column;
        }
    }
}