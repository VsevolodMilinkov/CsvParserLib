using System;
using NUnit.Framework;

namespace CsvParserLib.Tests
{
    public class CsvColumnTests
    {
        [Test]
        public void ParseCsvHeader_NullInputString_ExpectCsvHeaderIsEmptyException()
        {
            try
            {
                var testObject = new CsvColumn().ParseCsvHeader(null, "", ';', ' ');
                Assert.Fail();
            }
            catch (CsvHeaderIsEmptyException)
            {
                Assert.Pass();
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void ParseCsvHeader_EmptyInputString_ExpectCsvHeaderIsEmptyException()
        {
            try
            {
                var testObject = new CsvColumn().ParseCsvHeader("", "", ';', ' ');
                Assert.Fail();
            }
            catch (CsvHeaderIsEmptyException)
            {
                Assert.Pass();
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void ParseCsvHeader_NullInputColumnName_ExpectArgumentException()
        {
            try
            {
                var testObject = new CsvColumn().ParseCsvHeader("Column1", null, ';', ' ');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void ParseCsvHeader_EmptyInputColumnName_ExpectArgumentException()
        {
            try
            {
                var testObject = new CsvColumn().ParseCsvHeader("Column1", "", ';', ' ');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void ParseCsvHeader_NoTypeInCsvHeaderString_ExpectException()
        {
            try
            {
                var testObject = new CsvColumn().ParseCsvHeader("Column1 Strang", "Column1", ';', ' ');
                Assert.Fail();
            }
            catch (IncorrectColumnTypeException)
            {
                Assert.Pass();
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void ParseCsvHeader_SingleCorrectColumnInHeader_ExpectSuccess()
        {
            string colName = "Column1";
            var testObject = new CsvColumn().ParseCsvHeader("Column1 String", colName, ';', ' ');

            Assert.True(testObject.Name == colName && testObject.Type == typeof(string) && testObject.Index == 0);
        }     
        [Test]
        public void ParseCsvHeader_TwoCorrectColumnInHeader_ExpectSuccess()
        {
            string colName = "Column1";
            var testObject = new CsvColumn().ParseCsvHeader("Column1 String; Column2 String", colName, ';', ' ');

            Assert.True(testObject.Name == colName && testObject.Type == typeof(string) && testObject.Index == 0);
        }
    }
}