using NUnit.Framework;

namespace CsvParserLib.Tests
{
    public class ColumnTypeFloatTests
    {
        [TestCase("123,15")]
        [TestCase("12315")]
        public void CanBeParsed_CorrectNumber_ExpectSuccess(string value)
        {
            Assert.True(new ColumnTypeFloat().CanBeParsed(value));
        }

        [TestCase("123.15")]
        [TestCase("trust me I'm float")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CanBeParsed_IncorrectString_ExpectFail(string value)
        {
            Assert.False(new ColumnTypeFloat().CanBeParsed(value));
        }

        [TestCase("1", "1")]
        [TestCase("1,1", "1,1")]
        public void IsEqual_CorrectNumbers_ExpectTrue(string value1, string value2)
        {
            Assert.True(new ColumnTypeFloat().IsEqual(value1, value2));
        }

        [TestCase("1,1", "1")]
        [TestCase("1", "1,1")]
        public void IsEqual_IntAndFloat_ExpectFalse(string value1, string value2)
        {
            Assert.False(new ColumnTypeFloat().IsEqual(value1, value2));
        }

        [TestCase("1,1", "")]
        [TestCase("1,1", "a")]
        [TestCase("1,1", "0 1")]
        [TestCase("1,1", " ")]
        [TestCase("1,1", null)]
        public void IsEqual_IncorrectValues_ExpectFalse(string value1, string value2)
        {
            Assert.False(new ColumnTypeFloat().IsEqual(value1, value2));
        }
    }
}