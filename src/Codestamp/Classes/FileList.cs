using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace CodeStamp.Classes
{
    public class FileList
    {
        private Dictionary<string, string> Filenames { get; } = new Dictionary<string, string>();

        public string[] GetFiles()
        { 
            return Filenames.Values.ToArray();
        }

        public void ClearItems()
        {
            Filenames.Clear();
        }

        public void RemoveItem(string item)
        {
            Filenames.Remove(item);
        }

        public void AttachFiles(string[] files, ListBox list)
        {
            foreach (var filePath in files)
            {
                var filename = Path.GetFileName(filePath);

                if (string.IsNullOrEmpty(filename)) 
                    continue;

                Filenames.Add(filename, filePath);
                list.Items.Add(filename);
            }
        }
    }
}
