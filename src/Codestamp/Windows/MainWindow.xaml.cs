using System.Windows;
using System.IO;
using System;
using System.Diagnostics;
using CodeStamp.Classes;
using static CodeStamp.Classes.Alerts;

namespace CodeStamp.Windows
{
    using Win32OpenFileDialog = Microsoft.Win32.OpenFileDialog;

    public partial class MainWindow : Window
    {
        private PreviewWindow LicensePreviewWindow { get; set; }
        private CreateWindow LicenseCreateWindow { get; set; }

        private FileLicenseList LicenseList { get; } = new FileLicenseList();
        private FilePrinter LicensePrinter { get; } = new FilePrinter();
        private FileList CodeFileList { get; } = new FileList();

        private const string WebsiteUpdateLink = "https://github.com/william-taylor/cs-experiments";
        private const string LicenseDirectory = "./Data/Licenses/";
        private const string AboutFileLink = "Data/About.txt";

        public MainWindow()
        {
            InitializeComponent();

            LicenseList.LoadLicenseList(LicenseDirectory);
            LicenseList.PushToComboBox(LicenseListComboBox);
        }

        public void RefreshLicenseList()
        {
            LicenseList.LoadLicenseList(LicenseDirectory);
            LicenseList.PushToComboBox(LicenseListComboBox);
        }

        private void ClearList(object sender, RoutedEventArgs e)
        {
            CodeFileList.ClearItems();
            FileList.Items.Clear();
        }

        private void RemoveFile(object sender, RoutedEventArgs e)
        {
            if(FileList.SelectedItem != null)
            {
                CodeFileList.RemoveItem(FileList.SelectedItem.ToString());
                FileList.Items.Remove(FileList.SelectedItem.ToString());
            }
            else
            {
                ShowInfo("No File Selected!", "Please select a file in the list to the left.");
            }
        }

        private void PreviewClick(object sender, RoutedEventArgs e)
        {
            if (LicenseListComboBox.SelectedIndex != -1)
            {
                LicensePreviewWindow = new PreviewWindow();
                LicensePreviewWindow.SetLicenseText(LicenseListComboBox.SelectedItem.ToString());
                LicensePreviewWindow.Show();
            }
            else
            {
                ShowInfo("No License Selected", "Please choose a license to preview it.");
            }
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            LicensePreviewWindow?.Close();
            LicenseCreateWindow?.Close();
        }

        private void InsertLicenseClick(object sender, RoutedEventArgs e)
        {
            if (LicenseListComboBox.SelectedIndex != -1)
            {
                if (FileList.Items.Count > 0)
                {
                    var boxResult = ShowYesNo("Are you sure?", "We will now insert the license into all the files listed, are you sure?");

                    if (boxResult != MessageBoxResult.Yes)
                    {
                        return;
                    }

                    var path = LicenseListComboBox.SelectedItem.ToString();

                    LicensePrinter.Email = EmailTextBlock.Text;
                    LicensePrinter.Name = NameTextBlock.Text;
                    LicensePrinter.Date = DateTextBlock.Text;
                    LicensePrinter.PrintLicense(LicenseList.GetFullFilename(path), CodeFileList.GetFiles());
                }
                else
                {
                    ShowInfo("No Files!", "Please open some files to insert the license into.");
                }
            }
            else
            {
                ShowInfo("No License", "Please choose a license push it to your source files.");
            }
            
        }

        private void FindButtonClick(object sender, RoutedEventArgs e)
        {
            var files = CodeFileList.GetFiles();

            if (files.Length == 0)
            {
                ShowInfo("No Files", "No files to check");
            }
            else
            {
                var filesWithLicensesCount = LicensePrinter.HasLicenses(files);

                if (files.Length == filesWithLicensesCount)
                {
                    ShowSuccess("Success", "We found a licence in all files");
                }
                else
                {
                    ShowSuccess("Success","We found a licence in " + filesWithLicensesCount + " out of the " + files.Length + " files");
                }
            }
           
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            var files = CodeFileList.GetFiles();

            if (files.Length == 0)
            {
                MessageBox.Show("No files to remote the license from.");
            }
            else
            {
                LicensePrinter.RemoveLicense(files);
            }
        }

        private void CreateLicenseOnClick(object sender, RoutedEventArgs e)
        {
            LicenseCreateWindow = new CreateWindow(this);
            LicenseCreateWindow.Show();
        }

        private void OpenItemOnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Win32OpenFileDialog
            {
                Multiselect = true,
                Filter = "Code Files (*.*)|*.*",
                Title = "Select Your Code Files"
            };

            if (fileDialog.ShowDialog().HasValue)
            {
                CodeFileList.AttachFiles(fileDialog.FileNames, FileList);
            }
        }

        private void AboutItemOnClick(object sender, RoutedEventArgs e)
        {
            var aboutTextArray = File.ReadAllLines(AboutFileLink);
            ShowInfo("About CodeStamp", string.Join("", aboutTextArray));
        }

        private void UpdateItemOnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(WebsiteUpdateLink);
        }

        private void ExitItemOnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
