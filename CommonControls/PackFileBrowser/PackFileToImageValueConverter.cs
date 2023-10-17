﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using CommonControls.Resources;

namespace CommonControls.PackFileBrowser
{
    [ValueConversion(typeof(PackFileTreeNode), typeof(BitmapImage))]
    public class PackFileToImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is PackFileTreeNode node)
            {
                if (node.NodeType == NodeType.Root)
                    return ResourceController.CollectionIcon;
                else if (node.NodeType == NodeType.Directory)
                    return ResourceController.FolderIcon;
                if (node.NodeType == NodeType.File)
                    return ResourceController.FileIcon;
            }

            throw new Exception("Unknown type " + value.GetType().FullName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
