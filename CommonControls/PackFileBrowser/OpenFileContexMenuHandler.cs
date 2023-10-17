﻿using System.Collections.ObjectModel;
using CommonControls.Services;

namespace CommonControls.PackFileBrowser
{
    public class OpenFileContexMenuHandler : ContextMenuHandler
    {
        public OpenFileContexMenuHandler(PackFileService service) : base(service, null, null)
        { }

        public override void Create(PackFileTreeNode node)
        {
            _selectedNode = node;
            Items = new ObservableCollection<ContextMenuItem>();
            if (node == null)
                return;

            if (node.NodeType != NodeType.File)
            {
                Additem(ContextItems.Expand, Items);
                Additem(ContextItems.Collapse, Items);
                AddSeperator(Items);
                Additem(ContextItems.CreateFolder, Items);
            }
        }
    }
}
