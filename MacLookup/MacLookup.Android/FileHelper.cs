using MacLookup.Droid;
using MacLookup.Interfaces;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace MacLookup.Droid {
    public class FileHelper : IFileHelper {
        public string GetLocalFilePath(string filename) {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}