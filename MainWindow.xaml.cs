using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WenElevating.FileShared.Model;

namespace WenElevating.FileShared
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<FileVO> FileList
        {
            get; 
            set;
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            FileList = [];
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string? msg = string.Empty;
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    msg = ((Array)e.Data?.GetData(DataFormats.FileDrop))?.GetValue(0)?.ToString();
                }

                // 检查文件是否存在
                CheckFilePathExist(msg);

                // 获取文件图标并显示在列表中
                ShowFileOnList(msg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        private void ShowFileOnList(string? path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            try
            {
                FileVO? fileVO = FileList.FirstOrDefault((item) => item.Path == path);

                if (fileVO != null && MessageBox.Show("文件已存在，是否覆盖？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }

                // 获取文件图标，若无图标则设置默认图标
                Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(path) ?? SystemIcons.Application;
                FileInfo fileInfo = new(path);

                if (fileVO != null)
                {
                    fileVO.Path = path;
                    fileVO.Icon = icon;
                    fileVO.Size = fileVO.Size;
                    fileVO.Name = fileInfo.Name;
                    return;
                }
                
                // 检查是否存在，若不存在则构造VO对象并添加到集合中，若存在则询问是否覆盖
                fileVO = new FileVO()
                {
                    Path = path,
                    Icon = icon,
                    Size = fileInfo.Length,
                    Name = fileInfo.Name,
                };
                FileList.Add(fileVO);
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
            }
        }

        private bool CheckFilePathExist(string? path)
        {
            // 排除非文件
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {   
                throw new ArgumentNullException(nameof(path));
            }
            return true;
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("进入了..");
        }

        private void Window_DragLeave(object sender, DragEventArgs e)
        {

        }
    }
}