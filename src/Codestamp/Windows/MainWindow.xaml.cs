using System.Windows;
using System.IO;
using System;
using System.Diagnostics;
using CodeStamp.Classes;

namespace CodeStamp.Windows
{
    using Win32OpenFileDialog = Microsoft.Win32.OpenFileDialog;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    public partial class MainWindow : MetroWindow
    {
        private PreviewWindow LicensePreviewWindow { get; set; }
        private CreateWindow LicenseCreateWindow { get; set; }

        private FileLicenseList LicenseList { get; } = new FileLicenseList();
        private FilePrinter LicensePrinter { get; } = new FilePrinter();
        private FileList CodeFileList { get; } = new FileList();

        private const string WebsiteUpdateLink = "https://github.com/william-taylor/codestamp";
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

        private async void RemoveFile(object sender, RoutedEventArgs e)
        {
            if (FileList.SelectedItem != null)
            {
                CodeFileList.RemoveItem(FileList.SelectedItem.ToString());
                FileList.Items.Remove(FileList.SelectedItem.ToString());
            }
            else
            {
                await this.ShowMessageAsync("No File Selected!", "Please select a file in the list to the left.");
            }
        }

        private async void PreviewClick(object sender, RoutedEventArgs e)
        {
            if (LicenseListComboBox.SelectedIndex != -1)
            {
                LicensePreviewWindow = new PreviewWindow();
                LicensePreviewWindow.SetLicenseText(LicenseListComboBox.SelectedItem.ToString());
                LicensePreviewWindow.Show();
            }
            else
            {
                await this.ShowMessageAsync("No License Selected", "Please choose a license to preview it.");
            }
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            LicensePreviewWindow?.Close();
            LicenseCreateWindow?.Close();
        }

        private async void InsertLicenseClick(object sender, RoutedEventArgs e)
        {
            if (LicenseListComboBox.SelectedIndex != -1)
            {
                if (FileList.Items.Count > 0)
                {
                    var boxResult = await this.ShowMessageAsync("Are you sure?", "We will now insert the license into all the files listed, are you sure?", MessageDialogStyle.AffirmativeAndNegative);

                    if (boxResult != MessageDialogResult.Affirmative)
                    {
                        return;
                    }

                    var path = LicenseListComboBox.SelectedItem.ToString();

                    LicensePrinter.Email = EmailTextBlock.Text;
                    LicensePrinter.Name = NameTextBlock.Text;
                    LicensePrinter.Date = DateTextBlock.Text;

                    var success = LicensePrinter.PrintLicense(LicenseList.GetFullFilename(path), CodeFileList.GetFiles());

                    if (success)
                    {
                        await this.ShowMessageAsync("Finished!", "We have inserted the license into all source files!");
                    }
                    else
                    {
                        await this.ShowMessageAsync("Error!", "The program encountered an error and hasnt stamped the license.");
                    }
                }
                else
                {
                    await this.ShowMessageAsync("No Files!", "Please open some files to insert the license into.");
                }
            }
            else
            {
                await this.ShowMessageAsync("No License", "Please choose a license push it to your source files.");
            }
        }

        private async void FindButtonClick(object sender, RoutedEventArgs e)
        {
            var files = CodeFileList.GetFiles();

            if (files.Length == 0)
            {
                await this.ShowMessageAsync("No Files", "No files to check");
            }
            else
            {
                var filesWithLicensesCount = LicensePrinter.HasLicenses(files);

                if (files.Length == filesWithLicensesCount)
                {
                    await this.ShowMessageAsync("Success", "We found a licence in all files");
                }
                else
                {
                    var body = "We found a licence in " + filesWithLicensesCount + " out of the " + files.Length + " files";

                    await this.ShowMessageAsync("Success", body);
                }
            }

        }

        private async void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            var files = CodeFileList.GetFiles();

            if (files.Length == 0)
            {
                await this.ShowMessageAsync("Error", "No files to remove the license from.");
            }
            else
            {
                if (LicensePrinter.RemoveLicense(files))
                {
                    await this.ShowMessageAsync("Finished !", "We have successfully removed the license");
                }
                else
                {
                    await this.ShowMessageAsync("Error!", "The program encountered an error and hasnt removed the license.");
                }
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

        private async void AboutItemOnClick(object sender, RoutedEventArgs e)
        {
            var aboutTextArray = File.ReadAllLines(AboutFileLink);

            await this.ShowMessageAsync("About CodeStamp", string.Join("", aboutTextArray));
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
