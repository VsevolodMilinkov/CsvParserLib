using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace CsvParserLib.Tests
{
    class TestFile : IDisposable
    {
        public string name = "TestFile.PleaseDeleteMe";

        public TestFile()
        {
            if (!File.Exists(name))
                File.Create(name).Close();
        }

        public void Dispose()
        {
            if (File.Exists(name))
                File.Delete(name);
        }
    }

    public class ParserTests
    {
        [SetUp]
        public void Setup()
        {
        }


        #region Constructor Tests

        [Test]
        public void Parser_InputPathFileDoExist_ExpectNoProblems()
        {
            using (var file = new TestFile())
            {
                try
                {
                    var parser = new Parser(file.name, "file2", Encoding.Default, "colName", "");
                    Assert.True(parser != null, "Экземпляр парсера успешно инициализирован");
                }
                catch (Exception e)
                {
                    Assert.Fail(
                        $"Неопределенное исключение при корректной (по идее) попытке инициализировать экземпляр парсера: {e.Message}, {e.StackTrace}");
                }
            }
        }

        [Test]
        public void Parser_InputFileDoesntExist_ExpectInputFileDoesntExistException()
        {
            string file = "TestFile.PleaseDeleteMe";
            try
            {
                if (File.Exists(file))
                    File.Delete(file);
                var parser = new Parser(file, "", Encoding.Default, "", "");
            }
            catch (InputFileDoesntExistException)
            {
                Assert.Pass("Успешно выброшено исключение о том, что входящий файл не существует.");
            }
            catch (Exception e)
            {
                Assert.Fail(
                    $"При попытке инициализации экземпляра парсера с именем несуществующего входящего файла возникло некорректное исключение: {e.Message}, {e.StackTrace}");
            }
        }

        [Test]
        public void Parser_InputPathIsEmpty_ExpectInputFileDoesntExistException()
        {
            try
            {
                var parser = new Parser("", "", Encoding.Default, "", "");
            }
            catch (InputFileDoesntExistException)
            {
                Assert.Pass("Успешно выброшено исключение о пустом имени входящего файла.");
            }
            catch (Exception e)
            {
                Assert.Fail(
                    $"При попытке инициализации экземпляра парсера с пустым именем входящего файла возникло некорректное исключение: {e.Message}, {e.StackTrace}");
            }
        }

        [Test]
        public void Parser_InputPathIsSpace_ExpectException()
        {
            try
            {
                var parser = new Parser(" ", "", Encoding.Default, "colName", "");
            }
            catch (InputFileDoesntExistException)
            {
                Assert.Pass("Успешно выброшено исключение о пустом имени входящего файла.");
            }
            catch (Exception e)
            {
                Assert.Fail(
                    $"При попытке инициализации экземпляра парсера с пустым именем входящего файла возникло некорректное исключение: {e.Message}, {e.StackTrace}");
            }
        }

        [Test]
        public void Parser_InputPathIsNull_ExpectInputFileDoesntExistException()
        {
            try
            {
                var parser = new Parser(null, "", Encoding.Default, "colName", "");
            }
            catch (InputFileDoesntExistException)
            {
                Assert.Pass("Успешно выброшено исключение о пустом имени входящего файла");
            }
            catch (Exception e)
            {
                Assert.Fail(
                    $"При попытке инициализации экземпляра парсера с пустым именем входящего файла возникло некорректное исключение: {e.Message}, {e.StackTrace}");
            }
        }

        [Test]
        public void Parser_OutputPathIsNull_ExpectEmptyOutputFileNameException()
        {
            using (var file = new TestFile())
            {
                try
                {
                    var parser = new Parser(file.name, null, Encoding.Default, "columnName", "");
                }
                catch (EmptyOutputFileNameException)
                {
                    Assert.Pass("Успешно выброшено исключение о том, что получено пустое имя исходящего файла");
                }
                catch (Exception e)
                {
                    Assert.Fail(
                        @$"Неопределённое исключение при попытке инициализации парсера с пустым именем исходящего файла: {e.Message}, {e.StackTrace}");
                }
            }
        }

        [Test]
        public void Parser_OutputPathIsEmpty_ExpectEmptyOutputFileNameException()
        {
            using (var file = new TestFile())
            {
                try
                {
                    var parser = new Parser(file.name, "", Encoding.Default, "columnName", "");
                }
                catch (EmptyOutputFileNameException)
                {
                    Assert.Pass("Успешно выброшено исключение о том, что получено пустое имя исходящего файла");
                }
                catch (Exception e)
                {
                    Assert.Fail(
                        @$"Неопределённое исключение при попытке инициализации парсера с пустым именем исходящего файла: {e.Message}, {e.StackTrace}");
                }
            }
        }

        [Test]
        public void Parser_OutputPathIsSpace_ExpectEmptyOutputFileNameException()
        {
            using (var file = new TestFile())
            {
                try
                {
                    var parser = new Parser(file.name, " ", Encoding.Default, "columnName", "");
                }
                catch (EmptyOutputFileNameException)
                {
                    Assert.Pass("Успешно формируется исключение о пустом имени столбца для парсинга.");
                }
                catch (Exception e)
                {
                    Assert.Fail(
                        @$"Неопределённое исключение при попытке инициализации парсера с пробелом вместо имени исходящего файла: {e.Message}, {e.StackTrace}");
                }
            }
        }

        [Test]
        public void Parser_ColumnNameIsNull_ExpectEmptyColumnNameException()
        {
            using (var file = new TestFile())
            {
                try
                {
                    var parser = new Parser(file.name, "file2", Encoding.Default, null, "");
                }
                catch (EmptyColumnNameException)
                {
                    Assert.Pass("Успешно формируется исключение о пустом имени столбца для парсинга.");
                }
                catch (Exception e)
                {
                    Assert.Fail(
                        @$"Неопределённое исключение при попытке инициализации парсера с null вместо имени столбца для парсинга: {e.Message}, {e.StackTrace}");
                }
            }
        }

        [Test]
        public void Parser_ColumnNameIsEmpty_ExpectEmptyColumnNameException()
        {
            using (var file = new TestFile())
            {
                try
                {
                    var parser = new Parser(file.name, "file2", Encoding.Default, "", "");
                }
                catch (EmptyColumnNameException)
                {
                    Assert.Pass("Успешно формируется исключение о пустом имени столбца.");
                }
                catch (Exception e)
                {
                    Assert.Fail(
                        $"Неопределённое исключение при попытке инициализации парсера с пустым именем столбца для парсинга: {e.Message}, {e.StackTrace}");
                }
            }
        }

        [Test]
        public void Parser_ColumnNameIsSpace_ExpectEmptyColumnNameException()
        {
            using (var file = new TestFile())
            {
                try
                {
                    var parser = new Parser(file.name, "file2", Encoding.Default, " ", "");
                }
                catch (EmptyColumnNameException)
                {
                    Assert.Pass();
                }
                catch (Exception e)
                {
                    Assert.Fail(
                        $"Неопределённое исключение при попытке инициализации парсера с пустым именем столбца для парсинга: {e.Message}, {e.StackTrace}");
                }
            }
        }

        #endregion

        #region Parse method Tests

        /*TODO:
         Проверка того, что входящий файл не пустой;
         Проверка того, что в файле первая строка корректно считывается и разделяется на n-ное кол-во столбцов;
         Проверка того, что первая строка в файле имеет в столбце и название, и тип данных: если нет типа - ругаца;
         Проверка того, что если в первой строке нет типа данных столбца - то ругаемся;
         Проверка того, что указан неподдерживаемый тип данных столбца;
         */
        /* TODO:
           а теперь серьезно:
           хочу знать, что первая строка разбивается на столбцы;
           хочу знать, что нет проблем с созданием нового файла;
           потом хочу знать, что эти же столбцы нормально записываются в новый выходной файл;
           
           потом уже идет обработка и фильтрация строк             
        */
        [Test]
        public void Parse_BusyInputFile_ExpectInputFileIsBusyException()
        {
            var name = "TestFile.PleaseDeleteMe";
            if (File.Exists(name))
                File.Delete(name);
            File.Create(name); //создаем пустой файл, но не закрываем соединение через .Close()
            try
            {
                var success = new Parser(name, "file2", Encoding.Default, "Column1", "").Parse();
                Assert.Fail();
            }
            catch (InputFileIsBusyException)
            {
                Assert.Pass();
            }
            catch (Exception e)
            {
                Assert.Fail($"Неопределенная ошибка при попытке парсинга входного файла, занятого другим процессом: {e.Message}: {e.StackTrace}");
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
            using (var file = new TestFile())
            {
                try
                {
                    var parser = new Parser(file.name, "file2", Encoding.Default, "Column1", "");
                }
                catch (InputFileIsEmptyException)
                {
                    Assert.Pass();
                }
                catch (Exception e)
                {
                    Assert.Fail(
                        $"Неопределённое исключение при попытке парсинга пустого входного файла: {e.Message}, {e.StackTrace}");
                }
            }
        }

        [Test]
        public void Parse_InputFileOnlyHeader_ExpectCorrectHeaderInOutputFile()
        {
            using (var file = new TestFile())
            {
                var inputFileHeader = "Column1 string; Column2 string";
                File.WriteAllText(file.name,inputFileHeader, Encoding.Default);
                try
                {
                    string outputFileName = "OutputTestFile.PleaseDeleteMe";
                    var success = new Parser(file.name, outputFileName, Encoding.Default, "Column1", "");
                    var outputFileHeader = File.ReadAllText(file.name, Encoding.Default);
                    Assert.True(inputFileHeader == outputFileHeader);
                }
                catch (Exception e)
                {
                    Assert.Fail(
                        $"Неопределённое исключение при попытке парсинга пустого входного файла: {e.Message}, {e.StackTrace}");
                }
            }
        }
        
        #endregion
    }
}