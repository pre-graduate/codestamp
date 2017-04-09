using static CodeStamp.Classes.Alerts;
using System;
using System.Data;
using System.Windows;
using System.IO;

namespace CodeStamp.Windows
{
    using Win32SaveFileDialog = Microsoft.Win32.SaveFileDialog;

    public partial class CreateWindow : Window
    {
        private const string HelpMessageTitle = "How to write a license";
        private const string LicensesLocation = @"Data\Licenses\";
        private const string HelpLocation = @"Data\help.txt";
        private const string LicenseExtension = ".license";
        private MainWindow CoreWindow { get; }

        public CreateWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            CoreWindow = mainWindow;
        }

        private void ExitOnClick(object sender, RoutedEventArgs e)
        {
            CoreWindow.RefreshLicenseList();
            Close();
        }

        private void HelpOnClick(object sender, RoutedEventArgs e)
        {
           ShowInfo(HelpMessageTitle, string.Join(string.Empty, File.ReadAllLines(HelpLocation)));
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            var dialogStartPath = Path.Combine(Directory.GetCurrentDirectory(), LicensesLocation);
            var dialog = new Win32SaveFileDialog
            {
                InitialDirectory = dialogStartPath,
                DefaultExt = LicenseExtension
            };

            if (!dialog.ShowDialog().HasValue)
            {
                return;
            }

            var value = Environment.NewLine + LicenseTextBox.Text + Environment.NewLine + Environment.NewLine;
            var path = Path.ChangeExtension(dialog.FileName, LicenseExtension);

            File.WriteAllText(path, value);
            CoreWindow.RefreshLicenseList();
        }
    }
}
