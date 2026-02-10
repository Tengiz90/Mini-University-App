using EduVerse.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduVerse.Data.Implementations
{
    public class FileManager : IFileManager
    {
        public string FilePath { get; set; }

        public void Add(string log)
        {
            using (StreamWriter writer = new StreamWriter(FilePath, append: true))
            {
                writer.WriteLine(log);
            }
        }

        public void ClearAll()
        {
            if (File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, string.Empty);
            }
        }
    }
}
