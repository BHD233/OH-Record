using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;



[assembly: Dependency(typeof(OHRecord.Droid.FileHelper))]

namespace OHRecord.Droid
{
    class FileHelper : IFileHelper
    {
        public bool IsExit(string filename)
        {
            string filepath = GetFilePath(filename);
            return File.Exists(filename);
        }

        public void WriteText(string filename, string text)
        {
            string filepath = GetFilePath(filename);
            File.WriteAllText(filepath, text);
        }

        public string ReadText(string filename)
        {
            string filepath = GetFilePath(filename);
            return File.ReadAllText(filepath);
        }

        public IEnumerable<string> GetFiles()
        {
            return Directory.GetFiles(GetDocsPath());
        }

        public void Delete(String filename)
        {
            File.Delete(GetFilePath(filename));
        }

        //prtivate methods
        string GetFilePath(string filename)
        {
            return Path.Combine(GetDocsPath(), filename);
        }

        string GetDocsPath()
        {
            return "/storage/emulated/0/Documents";
        }
    }
}