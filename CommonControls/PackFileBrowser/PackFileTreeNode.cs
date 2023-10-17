// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommonControls.Common;
using CommonControls.FileTypes.PackFiles.Models;

namespace CommonControls.PackFileBrowser
{
    public enum NodeType
    {
        Root,
        Directory,
        File
    }

    public class PackFileTreeNode : NotifyPropertyChangedImpl
    {
        public PackFileContainer FileOwner { get; set; }
        public PackFile Item { get; set; }

        bool _isExpanded = false;
        public bool IsNodeExpanded
        {
            get => _isExpanded;
            set
            {
                SetAndNotify(ref _isExpanded, value);
            }
        }

        public NodeType NodeType { get; set; }
        public PackFileTreeNode Parent { get; set; }
        public ObservableCollection<PackFileTreeNode> Children { get; set; } = new ObservableCollection<PackFileTreeNode>();

        bool _unsavedChanged;
        public bool UnsavedChanged { get => _unsavedChanged; set => SetAndNotify(ref _unsavedChanged, value); }

        bool _isMainEditabelPack;
        public bool IsMainEditabelPack { get => _isMainEditabelPack; set => SetAndNotify(ref _isMainEditabelPack, value); }

        bool _Visibility = true;
        public bool IsVisible { get => _Visibility; set => SetAndNotify(ref _Visibility, value); }

        string _name = "";
        public string Name { get => _name; set => SetAndNotify(ref _name, value); }
        public PackFileTreeNode(string name, NodeType type, PackFileContainer ower, PackFileTreeNode parent, PackFile packFile = null)
        {
            Name = name;
            NodeType = type;
            Item = packFile;
            FileOwner = ower;
            Parent = parent;

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception($"Packfile name or folder is empty '{GetFullPath()}', this is not allowed! Please report as a bug if it happens outside of packfile loading! If it happens while loading clean up the packfile in RPFM");
        }

        public string GetFullPath()
        {
            if (NodeType == NodeType.Root)
                return "";

            var currentParent = Parent;
            var path = Name;
            while (currentParent != null)
            {
                if (currentParent.NodeType == NodeType.Root)
                    break;

                path = currentParent.Name + "\\" + path;
                currentParent = currentParent.Parent;
            }

            return path;
        }

        public override string ToString()
        {
            return Name;
        }


        public List<PackFileTreeNode> GetAllChildFileNodes()
        {
            var output = new List<PackFileTreeNode>();

            var nodes = new Stack<PackFileTreeNode>(new[] { this });
            while (nodes.Any())
            {
                PackFileTreeNode node = nodes.Pop();
                if (node.NodeType == NodeType.File)
                    output.Add(node);

                foreach (var n in node.Children)
                    nodes.Push(n);
            }


            return output;
        }

        public void RemoveSelf()
        {
            foreach (var child in Children)
                child.RemoveSelf();

            Children.Clear();
            Parent = null;
        }

        public void ForeachNode(Action<PackFileTreeNode> func)
        {
            func.Invoke(this);
            foreach (var child in Children)
                child.ForeachNode(func);
        }

        public void ExpandIfVisible(bool includeChildren = true)
        {
            if (IsVisible)
            {
                IsNodeExpanded = true;
                if (includeChildren)
                {
                    foreach (var child in Children)
                        child.ExpandIfVisible(includeChildren);
                }
            }
        }
    }
}
