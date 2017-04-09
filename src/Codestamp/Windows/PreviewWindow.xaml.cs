using System.Windows;
using System.IO;
using System;

namespace CodeStamp.Windows
{
    public partial class PreviewWindow : Window
    {
        private const string LicenseDirectory = "Data/Licenses/";
        private const string LicenseExtension = ".license";

        public PreviewWindow()
        {
            InitializeComponent();
        }

        public void SetLicenseText(string name)
        {
            try
            {
                var path = Path.Combine(LicenseDirectory, Path.ChangeExtension(name, LicenseExtension));
                PreviewText.Content = string.Join(Environment.NewLine, File.ReadAllLines(path));
            }
            catch(Exception)
            {
                PreviewText.Content = "Error couldnt find license " + name;
            }
        }
    }
}
