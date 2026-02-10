using System;
using System.Collections.Generic;
using System.Text;

namespace EduVerse.Data.Interfaces
{
    public interface IFileManager
    {
        string FilePath { get; set; }
        void Add(string log);
        void ClearAll();
    }
}
