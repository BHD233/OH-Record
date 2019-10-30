using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.IO;

namespace OHRecord
{
    public interface IFileHelper
    {
        bool IsExit(string filename);

        void WriteText(string filename, string text);

        string ReadText(string filename);

        IEnumerable<string> GetFiles();

        void Delete(String filename);
    }

    class FileHelper : IFileHelper
    {
        IFileHelper fileHelper = DependencyService.Get<IFileHelper>();

        public bool IsExit(string filename)
        {
            return fileHelper.IsExit(filename);
        }

        public void WriteText(string filename, string text)
        {
            filename += ".BHD";
            fileHelper.WriteText(filename, text);
        }

        public string ReadText(string filename)
        {
            return fileHelper.ReadText(filename);
        }

        public IEnumerable<string> GetFiles()
        {
            IEnumerable<string> filepaths = fileHelper.GetFiles();
            List<string> filenames = new List<string>();

            foreach (string filepath in filepaths)
            {
                string filename = Path.GetFileName(filepath);
                filenames.Add(filename);
            }

            return filenames;
        }

        public void Delete(string filename)
        {
            fileHelper.Delete(filename);
        }

    }
}
