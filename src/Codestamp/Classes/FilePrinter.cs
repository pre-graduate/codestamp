using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace CodeStamp.Classes
{
    public class FilePrinter
    {
        private const string LicenseStartToken = "/***";
        private const string LicenseEndToken = "***/";
        private const string LicenseNewLine = " * ";
        private const string EmailToken = "[EMAIL]";
        private const string NameToken = "[NAME]";
        private const string DateToken = "[DATE]";

        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;

        public List<string> ParseLicense(ref string[] body)
        {
            var parsedLicense = new List<string> { LicenseStartToken };
         
            foreach (var line in body)
            {
                var newLine = LicenseNewLine + line;

                newLine = newLine.Replace(EmailToken, Email);
                newLine = newLine.Replace(NameToken, Name);
                newLine = newLine.Replace(DateToken, Date);

                parsedLicense.Add(newLine);
            }

            parsedLicense.Add(LicenseNewLine);
            parsedLicense.Add(LicenseEndToken);
            return parsedLicense;
        }

        public bool RemoveLicense(string[] files)
        {
            try
            {
                foreach (var file in files)
                {
                    var stringLines = new List<string>();
                    var text = File.ReadAllLines(file);
                    var foundLicense = false;
                    var finished = false;

                    foreach (var line in text)
                    {
                        if (line.Contains(LicenseStartToken))
                        {
                            foundLicense = true;
                        }

                        if (finished || !foundLicense)
                        {
                            stringLines.Add(line);
                        }

                        if (line.Contains(LicenseEndToken))
                        {
                            finished = true;
                        }
                    }

                    File.WriteAllLines(file, stringLines.ToArray());
                }

                return true;
            }
            catch(Exception)
            {  
            }

            return false;
        }

        public int HasLicenses(string[] files)
        {
            var foundLicenses = 0;

            foreach (var file in files)
            {
                var foundLicense = false;
                var text = File.ReadAllLines(file);

                foreach (var line in text)
                {
                    if (line.Contains(LicenseStartToken))
                    {
                        foundLicense = true;
                    }

                    if (line.Contains(LicenseEndToken) && foundLicense)
                    {
                        foundLicense = false;
                        foundLicenses++;
                    }
                }
            }

            return foundLicenses;
        }

        public bool PrintLicense(string licensePath, string[] files)
        {
            try
            {
                var licenseBody = File.ReadAllLines(licensePath);
                var parsedLicenseBody = ParseLicense(ref licenseBody);

                foreach (var file in files)
                {
                    var currentText = File.ReadAllLines(file).ToList();
                    var mergedText = new List<string>();

                    parsedLicenseBody.ForEach(line => mergedText.Add(line));
                    currentText.ForEach(line => mergedText.Add(line));

                    File.WriteAllLines(file, mergedText.ToArray());
                }

                return true;
            }
            catch(Exception)
            { 
            }

            return false;
        }
    }
}
