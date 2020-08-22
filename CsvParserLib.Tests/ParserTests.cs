using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CsvParserLib.Tests
{
    public class ParserTests
    {
        #region Constructor Tests

        [Test]
        public void Parser_InitiateObject_ExpectSuccess()
        {
            using var file = new TestFile();
            var parser = new Parser(file.name, "OutputFile", Encoding.Default, "colName", "testString1");
            Assert.That(parser, Is.Not.Null, "Экземпляр парсера не удалось инициализировать");
        }

        [Test]
        public void Parser_InputFileDoesntExist_ExpectInputFileDoesntExistException()
        {
            string file = "TestFile.PleaseDeleteMe";
            if (File.Exists(file))
                File.Delete(file);
            Assert.Catch<InputFileDoesntExistException>(
                () => new Parser(file, "", Encoding.Default, "", ""),
                "При инициализации парсера с именем несуществующего входящего файла не возникло корректного исключения"
            );
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Parser_InputPathIsEmpty_ExpectInputFileDoesntExistException(string value)
        {
            Assert.Catch<InputFileDoesntExistException>(
                () => new Parser(value, "", Encoding.Default, "", ""),
                "При инициализации парсера с пустой строкой вместо имени входящего файла не возникло корректного исключения"
            );
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Parser_OutputPathIsEmpty_ExpectEmptyOutputFileNameException(string value)
        {
            using var file = new TestFile();
            Assert.Catch<EmptyOutputFileNameException>(
                () => new Parser(file.name, value, Encoding.Default, "columnName", ""),
                "При инициализации парсера с пустой строкой вместо имени исходящего файла не вышло корректного собщения об ошибке"
            );
        }


        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Parser_ColumnNameIsEmpty_ExpectEmptyColumnNameException(string value)
        {
            using var file = new TestFile();
            Assert.Catch<EmptyColumnNameException>(
                () => new Parser(file.name, "file2", Encoding.Default, value, ""),
                "Не удалось проверить вызов исключения EmptyColumnNameException при попытке инициализации парсера с пустым именем столбца для парсинга"
            );
        }

        #endregion

        #region Parse method Tests

        [Test]
        public void Parse_BusyInputFile_ExpectInputFileIsBusyException()
        {
            var name = "TestFile.PleaseDeleteMe";

            try
            {
                if (File.Exists(name))
                    File.Delete(name);
                File.Create(name); //создаем пустой файл, но не закрываем соединение через .Close()
                Assert.Catch<InputFileIsBusyException>(() =>
                        new Parser(name, "file2", Encoding.Default, "Column1", "1").Parse(),
                    "Не удалось проверить вызов исключения InputFileIsBusyException при попытке парсинга входного файла, занятого другим процессом"
                );
            }
            catch (Exception e)
            {
                Assert.Fail(
                    $"Неопределенная ошибка при попытке парсинга входного файла, занятого другим процессом: {e.Message}: {e.StackTrace}");
            }
            finally
            {
                if (File.Exists(name))
                    File.Delete(name);
            }
        }

        [Test]
        public void Parse_EmptyInputFile_ExpectInputFileIsEmptyException()
        {
            using var file = new TestFile();
            Assert.Catch<InputFileIsEmptyException>(() =>
                    new Parser(file.name, "file2", Encoding.Default, "Column1", "1").Parse(),
                "Неопределённое исключение при попытке парсинга пустого входного файла"
            );
        }

        [Test]
        public void Parse_InputFileOnlyHeader_ExpectCorrectHeaderInOutputFile()
        {
            using var file = new TestFile();
            using var outputFile = new TestFile("OutputTestFile.PleaseDeleteMe");

            var inputFileHeader = "Column1 string; Column2 string";
            string inputFileText = new StringBuilder()
                .AppendLine(inputFileHeader)
                .ToString();
            File.WriteAllText(file.name, inputFileText, Encoding.Default);
            bool success = new Parser(file.name, outputFile.name, Encoding.Default, "Column1", "1").Parse();
            string outputFileHeader = File.ReadAllText(outputFile.name, Encoding.Default);

            Assert.True(success);
            Assert.True(File.Exists(outputFile.name));
            Assert.AreEqual(inputFileHeader, outputFileHeader);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Parse_EmptyExpression_ExpectEmptyExpressionException(string value)
        {
            using var file = new TestFile();
            Assert.Catch<EmptyExpressionException>(
                () => new Parser(file.name, "file2", Encoding.Default, "Column1", value).Parse(),
                "Неопределённое исключение при попытке парсинга без указанного значения для поиска"
            );
        }

        [TestCase( //в столбце есть значение - вернется 2 строки вместе с заголовком
            new[] {"Column1 string; Column2 string", "testString1; testString2"},
            "Column1",
            "testString1",
            2
        )]
        [TestCase( //тоже самое, но ищется строка с пробелом - должно вернуться 2 строки вместе с заголовком
            new[] {"Column1 string; Column2 string", "testString 1; testString2"},
            "Column1",
            "testString 1",
            2
        )]
        [TestCase( //в столбце Column1 нет строк со значением "testString2" - в файле будет 1 строка заголовка 
            new[] {"Column1 string; Column2 string", "testString1; testString2"},
            "Column1",
            "testString2",
            1
        )]
        [TestCase( //в столбце Column1 искомая дата - в файле будет 3 записи
            new[]
            {
                "Column1 Date; Column2 string", 
                "12.03.2020; testString1", 
                "10.05.2020; testString2",
                "12.03.2020; testString3"
            },
            "Column1",
            "12.03.2020",
            3
        )]
        [TestCase( //в столбце Projects искомая строка с экранированием - в файле будет 4 записи
            new[]
            {
                "Name String; Birthdate Date; Somenumber Integer; Somenumber2 Float; Projects String",
                "Иванов Иван Иванович; 18.06.1983; 31; 6,45; \"Работал над проектами: \"\"АБС\"\";\"\"КВД\"\"\"",
                "Петров Петр Иванович; 18.01.1973; 33; 6,05; \"Работал над проектами: \"\"АБС\"\";\"\"КВД\"\"\"",
                "Пупкин Василий Карпович; 15.06.1985; 14; 5,45; \"Работал над проектами: \"\"АБС\"\";\"\"КВД\"\"\"",
                "Пельмень Кондратий Пэтрович; 10.01.1985; 14; 5,50; \"Работал над проектами: \"\"АБС\"\""
            },
            "Projects",
            "\"Работал над проектами: \"\"АБС\"\";\"\"КВД\"\"\"",
            4
        )]
        public void Parse_InputFileDoExist_ExpectOutputFileWithCorrectRowCount
        (
            string[] inputLines,
            string columnName,
            string expression,
            int expectedRowCount
        )
        {
            using var inputFile = new TestFile();
            StringBuilder sb = new StringBuilder();
            foreach (var line in inputLines)
                sb.AppendLine(line);
            string inputFileText = sb.ToString();
            File.WriteAllText(inputFile.name, inputFileText, Encoding.Default);
            using var outputFile = new TestFile("OutputTestFile.PleaseDeleteMe");
            //пропарсилось
            Assert.True(new Parser(inputFile.name, outputFile.name, Encoding.Default, columnName, expression).Parse());
            //исходящий файл есть
            Assert.True(File.Exists(outputFile.name));
            //кол-во строк в нем соответствует ожиданиям
            Assert.AreEqual(expectedRowCount, File.ReadLines(outputFile.name).Count());
        }

        #endregion

        [TestCase("testString1; testString2", 0, "testString1")]
        [TestCase(
            "Иванов Иван Иванович; 18.06.1983; 31; 6,45; \"Работал над проектами: \"\"АБС\"\";\"\"КВД\"\"\"",
            4,
            "\"Работал над проектами: \"\"АБС\"\";\"\"КВД\"\"\""
        )]
        public void ParseLine_TryCorrectString_ExpectSuccess(string line, int index, string expectedResult)
        {
            Assert.AreEqual(expectedResult, Parser.ParseLine(line, ';', index));
        }
    }
}