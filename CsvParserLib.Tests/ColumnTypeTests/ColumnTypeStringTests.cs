using NUnit.Framework;

namespace CsvParserLib.Tests
{
    public class ColumnTypeStringTests
    {
        [TestCase("123,15")]
        [TestCase("12315")]
        [TestCase("Hoba")]
        [TestCase("+/*-")]
        [TestCase("\"")]
        public void CanBeParsed_CorrectString_ExpectSuccess(string value)
        {
            Assert.True(new ColumnTypeString().CanBeParsed(value));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CanBeParsed_IncorrectString_ExpectFail(string value)
        {
            Assert.False(new ColumnTypeString().CanBeParsed(value));
        }

        [Test]
        [TestCase("1", "1")]
        [TestCase("1,1", "1,1")]
        [TestCase("hoba", "hoba")]
        [TestCase("\"quote\"", "\"quote\"")]
        public void IsEqual_CorrectStrings_ExpectTrue(string value1, string value2)
        {
            Assert.True(new ColumnTypeString().IsEqual(value1, value2));
        }

        [TestCase("1,1", "1.1")]
        [TestCase("hoba", "Hoba")]
        [TestCase("hoba", "Hoba")]
        [TestCase("quote", "\"quote\"")]
        [TestCase("null", null)]
        [TestCase("null", "")]
        public void IsEqual_DifferentStrings_ExpectFalse(string value1, string value2)
        {
            Assert.False(new ColumnTypeString().IsEqual(value1, value2));
        }

        
    }
}