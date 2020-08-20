using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace CsvParserLib.Tests
{
    public class ColumnTests
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ParseCsvHeader_EmptyInputString_ExpectCsvHeaderIsEmptyException(string value)
        {
            Assert.Catch<CsvHeaderIsEmptyException>(() =>
                    Column.ParseCsvHeader(value, "", ';', ' '),
                "Не удалось проверить вызов исключения CsvHeaderIsEmptyException при попытке подать пустую строку вместо csv заголовка");
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ParseCsvHeader_EmptyInputColumnName_ExpectCsvHeaderNoColumnNameException(string value)
        {
            Assert.Catch<CsvHeaderNoColumnNameException>(() =>
                    Column.ParseCsvHeader("Column1", value, ';', ' '),
                "Не удалось проверить вызов исключения CsvHeaderNoColumnNameException при попытке подать пустую строку вместо имени столбца для поиска");
        }

        [Test]
        public void ParseCsvHeader_NoTypeInCsvHeaderString_ExpectUnsupportedColumnTypeException()
        {
            Assert.Catch<UnsupportedColumnTypeException>(() =>
                    Column.ParseCsvHeader("Column1 Strang", "Column1", ';', ' '),
                "Не удалось проверить вызов исключения UnsupportedColumnTypeException при попытке подать csv заголовок с некорректным типом данных в указанном столбце");
        }

        [Test]
        public void ParseCsvHeader_SingleCorrectColumnTypeStringInHeader_ExpectSuccess()
        {
            string colName = "Column1";
            Column testObject = Column.ParseCsvHeader("Column1 String", colName, ';', ' ');

            Assert.AreEqual(testObject.Name, colName);
            Assert.IsInstanceOf<ColumnTypeString>(testObject.Type);
            Assert.AreEqual(testObject.Index, 0);
            Assert.True(testObject.Type.CanBeParsed("testString"));
        }

        [TestCase("Integer", "123")]
        [TestCase("Date", "31.12.2020")]
        [TestCase("Float", "123,456")]
        [TestCase("String", "String")]
        public void ParseCsvHeader_SingleCorrectColumnTypeInHeader_ExpectSuccess(string typeString, string value)
        {
            string colName = "Column1";
            Column testObject = Column.ParseCsvHeader($"{colName} {typeString}", colName, ';', ' ');

            Assert.AreEqual(testObject.Name, colName);
            Assert.That(testObject.Type, Is.Not.Null);
            Assert.AreEqual(testObject.Index, 0);
            Assert.True(testObject.Type.CanBeParsed(value));
        }


        [TestCase("Integer", "123")]
        [TestCase("Date", "31.12.2020")]
        [TestCase("Float", "123,456")]
        [TestCase("String", "String")]
        public void ParseCsvHeader_SingleLastCorrectColumnInHeader_ExpectSuccess(string typeString, string value)
        {
            string colName = "Column3";
            Column testObject =
                Column.ParseCsvHeader($"Column1 String; Column2 String; Column3 {typeString}", colName, ';', ' ');

            Assert.AreEqual(testObject.Name, colName);
            Assert.That(testObject.Type, Is.Not.Null);
            Assert.AreEqual(testObject.Index, 2);
            Assert.True(testObject.Type.CanBeParsed(value));
        }

        [Test]
        public void ParseCsvHeader_IncorrectColumnName_ExpectNoSuchColumnInCsvHeaderException()
        {
            using TestFile file = new TestFile();
            using TestFile outputFile = new TestFile("OutputTestFile.PleaseDeleteMe");

            string inputFileText = new StringBuilder()
                .AppendLine("Column1 string; Column2 string")
                .AppendLine("testString1; testString2")
                .ToString();
            File.WriteAllText(file.name, inputFileText, Encoding.Default);

            Assert.Catch<NoSuchColumnInCsvHeaderException>(
                () => new Parser(file.name, outputFile.name, Encoding.Default, "totallyNotColumn1",
                    "testString1").Parse(),
                "Не удалось проверить вызов исключения NoSuchColumnInCsvHeaderException при попытке парсинга csv файла с поданным именем несуществующего столбца"
            );
        }
    }
}