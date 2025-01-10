using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WenElevating.FileShared.Common.Extension
{
    public static class MessageBoxExtension
    {
        public static bool Query(string title, string content)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"“{nameof(title)}”不能为 null 或空。", nameof(title));
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException($"“{nameof(content)}”不能为 null 或空。", nameof(content));
            }
            return MessageBox.Show(content, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
