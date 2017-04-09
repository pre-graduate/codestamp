using System.Collections.Generic;
using System.Windows.Controls;
using System.IO;
using System.Linq;

namespace CodeStamp.Classes
{
    public class FileLicenseList
    {
        public const string LicenseExtension = ".license";

        private List<string> LicenseFilenames { get; set; } = new List<string>();

        public int LoadLicenseList(string path)
        {
            var licenseFiles = Directory.GetFiles(path).Where(f => f.Contains(LicenseExtension));
            LicenseFilenames = new List<string>(licenseFiles);
            return LicenseFilenames.Count;
        }

        public string GetFullFilename(string name)
        {
            return LicenseFilenames.Find((path) => path.Contains(name));
        }

        public void PushToComboBox(ComboBox comboBox)
        {
            comboBox.Items.Clear();

            foreach(var license in LicenseFilenames)
            {
                comboBox.Items.Add(Path.GetFileNameWithoutExtension(license));
            }
        }
    }
}
