using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WenElevating.FileShared.Common.Extension;
using WenElevating.FileShared.Common.Tips;
using WenElevating.FileShared.Model;

namespace WenElevating.FileShared
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<FileTreeModel> FileList
        {
            get; 
            set;
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            FileList = [
                new FileTreeModel(
                    new FileVO() {
                        Icon = SystemIcons.WinLogo,
                        Path = "\\",
                        Name = "\\",
                        Size = 0
            }),];
        }

        private async void Window_Drop(object sender, DragEventArgs e)
        {
            try
            {
                // 获取拖入的消息
                string dragMesssage = string.Empty;
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    dragMesssage = ((Array?)e.Data?.GetData(DataFormats.FileDrop))?.GetValue(0)?.ToString() ?? string.Empty;
                }

                // 检查文件是否存在
                if (dragMesssage != null && !CheckDragFilePathExists(dragMesssage))
                {
                    return;
                }

                // 获取文件图标并显示在列表中
                await UpdateFileTreeAsync(dragMesssage, FileList.First()).ConfigureAwait(false);

                e.Handled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        /// <summary>
        /// 文件拖拽上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TreeViewItem_Drop(object sender, DragEventArgs e)
        {
            try
            {
                // 上传文件
                await UploadFileAsync(sender, e);
                
                // 阻止向下传播
                HandledEvent(e);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task UploadFileAsync(object sender, DragEventArgs e)
        {
            // 获取拖拽消息
            string message = GetDragMessage(e);
            
            // 检查上传文件
            if (!CheckDragFilePathExists(message))
            {
                return;
            }

            // 获取拖拽进的树
            if (!TryGetTreeModel(sender, out FileTreeModel treeModel))
            {
                return;
            }

            // 更新到文件树
            await UpdateFileTreeAsync(message, treeModel).ConfigureAwait(false);
        }

        private async Task UpdateFileTreeAsync(string? filePath, FileTreeModel selectecdTree)
        {
            if (string.IsNullOrEmpty(filePath) || selectecdTree == null)
            {
                return;
            }

            try
            {
                // 检查文件树
                FileVO? fileModel = await TryGetFileVOAsync(filePath, FileList);
                if (fileModel is not null && !MessageBoxExtension.Query(CommonMessageTips.TipMessage, UploadFileMessageTips.FileOverwriteMessage))
                {
                    return;
                }

                // 更新或创建文件
                UpdateOrCreateFileModel(filePath, ref fileModel);
                
                // 更新到被选中树的子列表
                selectecdTree.AddChild(new FileTreeModel(fileModel));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private static FileVO UpdateOrCreateFileModel(string path, ref FileVO? model)
        {
            Icon icon = GetFileIcon(path);
            FileInfo fileInfo = new(path);

            if (model != null)
            {
                model.Update(fileInfo.Name, path, icon, fileInfo.Length);
            }
            else
            {
                model = new FileVO
                {
                    Path = path,
                    Icon = icon,
                    Size = fileInfo.Length,
                    Name = fileInfo.Name,
                };
            }

            return model;
        }

        private static async Task<FileVO?> TryGetFileVOAsync(string path, IEnumerable<FileTreeModel> fileTrees)
        {
            try
            {
                return await fileTrees.First().GetChildrenByKeyAsync(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        private static Icon GetFileIcon(string filePath)
        {
            return System.Drawing.Icon.ExtractAssociatedIcon(filePath) ?? SystemIcons.Application;
        }

        private static bool CheckDragFilePathExists(string? path)
        {
            // 排除非文件
            if (path == null || string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                MessageBox.Show("上传的内容不是文件或不存在！");
                return false;
            }
            return true;
        }

        private static bool TryGetTreeModel(object sender, out FileTreeModel model)
        {
            var item = sender as TreeViewItem;
            if (item?.DataContext is not FileTreeModel treeModel)
            {
                throw new Exception("DataContext isn't fileTreeModel");
            }
            model = treeModel;
            return true;
        }

        private static string GetDragMessage(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return ((Array?)e.Data?.GetData(DataFormats.FileDrop))?.GetValue(0)?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        private static void HandledEvent(RoutedEventArgs eventArgs)
        {
            eventArgs.Handled = true;
        }
    }
}