using System.Windows;

namespace CodeStamp.Classes
{
    public static class Alerts
    {
        public static MessageBoxResult ShowSuccess(string title, string body)
        {
            return MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult ShowError(string title, string body)
        {
            return MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult ShowInfo(string title, string body)
        {
            return MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult ShowYesNo(string title, string body)
        {
            return MessageBox.Show(body, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        }
    }
}
