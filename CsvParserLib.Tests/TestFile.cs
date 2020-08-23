using System;
using System.IO;

namespace CsvParserLib.Tests
{
    /// <summary>
    /// Класс для тестов, создающий входящий файл при иниализации и удаляющий при вызове деструктора
    /// </summary>
    internal class TestFile : IDisposable
    {
        public string name;

        public TestFile(string n = "TestFile.PleaseDeleteMe")
        {
            name = n;
            if (!File.Exists(name))
                File.Create(name).Close();
        }

        public void Dispose()
        {
            if (File.Exists(name))
                File.Delete(name);
        }
    }
}