using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace CsvParserLib.Tests
{
    public class ParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Parser_InputPathFileDoExist_ExpectNoProblems()
        {
            string file = "filename";
            try
            {
                if (!File.Exists(file))
                    File.Create(file);
                var parser = new Parser(file, "file2", Encoding.Default, "colName", "");
                Assert.True(parser != null, "Экземпляр парсера успешно инициализирован");
            }
            catch (Exception e)
            {
                Assert.Fail(
                    $"Неопределенное исключение при корректной (по идее) попытке инициализировать экземпляр парсера: {e.Message}, {e.StackTrace}");
            }
            finally
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        [Test]
        public void Parser_InputFileDoesntExist_ExpectInputFileDoesntExistException()
        {
            string file = "filename";
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
            var file = "filename";
            try
            {
                if (!File.Exists(file))
                    File.Create(file);
                var parser = new Parser(file, null, Encoding.Default, "columnName", "");
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
            finally
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        [Test]
        public void Parser_OutputPathIsEmpty_ExpectEmptyOutputFileNameException()
        {
            var file = "filename";
            try
            {
                if (!File.Exists(file))
                    File.Create(file);
                var parser = new Parser(file, "", Encoding.Default, "columnName", "");
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
            finally
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        [Test]
        public void Parser_OutputPathIsSpace_ExpectEmptyOutputFileNameException()
        {
            var file = "filename";
            try
            {
                if (!File.Exists(file))
                    File.Create(file);
                var parser = new Parser(file, " ", Encoding.Default, "columnName", "");
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
            finally
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        [Test]
        public void Parser_ColumnNameIsNull_ExpectEmptyColumnNameException()
        {
            var file = "filename";
            try
            {
                if (!File.Exists(file))
                    File.Create(file);
                var parser = new Parser(file, "file2", Encoding.Default, null, "");
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
            finally
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        [Test]
        public void Parser_ColumnNameIsEmpty_ExpectEmptyColumnNameException()
        {
            var file = "filename";
            try
            {
                if (!File.Exists(file))
                    File.Create(file);
                var parser = new Parser(file, "file2", Encoding.Default, "", "");
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
            finally
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        [Test]
        public void Parser_ColumnNameIsSpace_ExpectEmptyColumnNameException()
        {
            var file = "filename";
            try
            {
                if (!File.Exists(file))
                    File.Create(file);
                var parser = new Parser(file, "file2", Encoding.Default, " ", "");
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
            finally
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }
    }
}