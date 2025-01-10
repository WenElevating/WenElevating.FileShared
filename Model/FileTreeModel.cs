using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WenElevating.FileShared.Model
{
    public class FileTreeModel : BaseTreeModel<FileVO>
    {
        public FileTreeModel(FileVO? data) : base(data)
        {

        }

        public override Task<FileVO> GetChildrenByKeyAsync(string? path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path), "An empty path was send to get data");
            }

            try
            {

                if (Data?.Path == path)
                {
                    return Task.FromResult(Data);
                }

                for (int i = 0; i < Children.Count; i++) 
                {
                    Task<FileVO> res = Children[i].GetChildrenByKeyAsync(path);
                    if (res != null)
                    {
                        return res;
                    }
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
            }
            return Task.FromResult<FileVO>(null);
        }

        public override Task<int> RemoveChildOrSunByKeyAsync(FileVO fileVO) 
        {
            if (fileVO == null)
            {
                throw new ArgumentNullException(nameof(fileVO), "The input fileVO can not be null!");
            }

            try
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    if (Children[i].Data?.Path == fileVO.Path)
                    {
                        Children.Remove(Children[i]);
                        return Task.FromResult(1);
                    }
                    Task<int> res = Children[i].RemoveChildOrSunByKeyAsync(fileVO);
                    if (res.Result == 1)
                    {
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return Task.FromResult<int>(0);
        }
    }
}
