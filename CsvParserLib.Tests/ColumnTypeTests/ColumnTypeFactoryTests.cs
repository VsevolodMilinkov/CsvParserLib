using NUnit.Framework;

namespace CsvParserLib.Tests
{
    public class ColumnTypeFactoryTests
    {
        [TestCase("stringerung")]
        [TestCase("bool")]
        [TestCase("byte")]
        [TestCase("sbyte")]
        [TestCase("char")]
        [TestCase("decimal")]
        [TestCase("double")]
        [TestCase("int")] // потому что по тз указан как "Integer"
        [TestCase("uint")]
        [TestCase("long")]
        [TestCase("ulong")]
        [TestCase("short")]
        [TestCase("ushort")]
        [TestCase("object")]
        [TestCase("datetime")] // потому что есть "Date"
        public void GetColumnType_UpsupportedType_ExpectUnsupportedColumnTypeException(string typeString)
        {
            Assert.Catch<UnsupportedColumnTypeException>(() => ColumnTypeFactory.GetColumnType(typeString));
        }

        [TestCase("integer")]
        [TestCase("Integer")]
        public void GetColumnType_IntType_ExcpectSuccess(string typeString)
        {
            IColumnType testObj = ColumnTypeFactory.GetColumnType(typeString);            
            Assert.IsInstanceOf<ColumnTypeInt>(testObj);
        }        
        [TestCase("date")]
        [TestCase("Date")]
        public void GetColumnType_DateType_ExcpectSuccess(string typeString)
        {
            IColumnType testObj = ColumnTypeFactory.GetColumnType(typeString);            
            Assert.IsInstanceOf<ColumnTypeDate>(testObj);
        }      
        [TestCase("float")]
        [TestCase("Float")]
        public void GetColumnType_FloatType_ExcpectSuccess(string typeString)
        {
            IColumnType testObj = ColumnTypeFactory.GetColumnType(typeString);            
            Assert.IsInstanceOf<ColumnTypeFloat>(testObj);
        }        
        [TestCase("string")]
        [TestCase("String")]
        public void GetColumnType_StringType_ExcpectSuccess(string typeString)
        {
            IColumnType testObj = ColumnTypeFactory.GetColumnType(typeString);            
            Assert.IsInstanceOf<ColumnTypeString>(testObj);
        }
    }
}