﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommonControls.Behaviors
{
    public static class TreeViewExtension
    {
        public static readonly DependencyProperty SelectItemOnRightClickProperty = DependencyProperty.RegisterAttached(
           "SelectItemOnRightClick",
           typeof(bool),
           typeof(TreeViewExtension),
           new UIPropertyMetadata(false, OnSelectItemOnRightClickChanged));

        public static bool GetSelectItemOnRightClick(DependencyObject d)
        {
            return (bool)d.GetValue(SelectItemOnRightClickProperty);
        }

        public static void SetSelectItemOnRightClick(DependencyObject d, bool value)
        {
            d.SetValue(SelectItemOnRightClickProperty, value);
        }

        private static void OnSelectItemOnRightClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool selectItemOnRightClick = (bool)e.NewValue;

            TreeView treeView = d as TreeView;
            if (treeView != null)
            {
                if (selectItemOnRightClick)
                    treeView.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
                else
                    treeView.PreviewMouseRightButtonDown -= OnPreviewMouseRightButtonDown;
            }
        }

        private static void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        public static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
    }
}
