using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WenElevating.FileShared.Model
{
    public partial class FileVO : ObservableObject
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _path;

        [ObservableProperty]
        private Icon? _icon;

        [ObservableProperty]
        private long _size;
    }
}
