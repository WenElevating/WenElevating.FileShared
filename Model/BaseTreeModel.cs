using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WenElevating.FileShared.Model
{
    public abstract partial class BaseTreeModel<T> : ObservableObject where T : class 
    {
        // 数据
        [ObservableProperty]
        private T? _data;

        // 子节点列表
        public ObservableCollection<BaseTreeModel<T>> Children { get; set; } = [];

        public abstract Task<T> GetChildrenByKeyAsync(string key);

        public abstract Task<int> RemoveChildOrSunByKeyAsync(T node);

        protected BaseTreeModel(T? data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data), "Data cannot be null");
        }

        // 添加子节点
        public void AddChild(BaseTreeModel<T> child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child), "Child cannot be null");
            }
            Children.Add(child);
        }

        // 移除子节点
        public bool RemoveChild(BaseTreeModel<T> child)
        {
            return Children.Remove(child);
        }

        // 获取子节点数量
        public int GetChildCount()
        {
            return Children.Count;
        }

        // 判断是否有子节点
        public bool HasChildren()
        {
            return Children.Count > 0;
        }
    }
}
