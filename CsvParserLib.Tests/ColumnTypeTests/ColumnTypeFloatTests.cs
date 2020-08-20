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
        public void CanBeParsed_IncorrectString_ExpectFail(string value)
        {
            Assert.False(new ColumnTypeFloat().CanBeParsed(value));
        }

        [Test]
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

        [TestCase("a", "1")]
        [TestCase("a", "1,1")]
        [TestCase("a", "1.1")]
        public void IsEqual_StringAndInt_ExpectFalse(string value1, string value2)
        {
            Assert.False(new ColumnTypeFloat().IsEqual(value1, value2));
        }
    }
}