using System;
using NUnit.Framework;

namespace CsvParserLib.Tests
{
    public class ColumnTypeStringTests
    {
        [Test]
        public void CanBeParsed_CorrectString_ExpectSuccess()
        {
            IColumnType testObject = ColumnTypeFactory.GetColumnType("string");
            Assert.True(testObject.CanBeParsed("test"));
        }

        [Test]
        public void CanBeParsed_EmptyString_ExpectFail()
        {
            IColumnType testObject = ColumnTypeFactory.GetColumnType("string");
            Assert.False(testObject.CanBeParsed(""));
        }

        [Test]
        public void CanBeParsed_NullString_ExpectFail()
        {
            IColumnType testObject = ColumnTypeFactory.GetColumnType("string");
            Assert.False(testObject.CanBeParsed(null));
        }

        [Test]
        public void CanBeParsed_NumberAsString_ExpectSuccess()
        {
            IColumnType testObject = ColumnTypeFactory.GetColumnType("string");
            Assert.True(testObject.CanBeParsed("1"));
        }

        
    }
}