using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WenElevating.FileShared.Model;

namespace WenElevating.FileShared.Services
{
    public interface ITreeService
    {
        public Task UpdateTreeModelWithPathAsync(string? path, FileTreeModel fileTree);
    }
}
