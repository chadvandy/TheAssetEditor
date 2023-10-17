﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CommonControls.Common;

namespace CommonControls.PackFileBrowser
{
    /// <summary>
    /// Interaction logic for PackFileBrowserView.xaml
    /// </summary>
    public partial class PackFileBrowserView : UserControl
    {
        public PackFileBrowserView()
        {
            InitializeComponent();
        }

        public ContextMenu CustomContextMenu
        {
            get { return (ContextMenu)GetValue(CustomContextMenuProperty); }
            set { SetValue(CustomContextMenuProperty, value); }
        }

        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                TreeViewItem item = (TreeViewItem)sender;

                _lastMouseDown = e.GetPosition(tvParameters);

                _draggedItem = item.DataContext as PackFileTreeNode;
            }
        }
        Point _lastMouseDown;
        PackFileTreeNode _draggedItem;

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(tvParameters);

                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                    {
                        if (_draggedItem != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(tvParameters, tvParameters.SelectedValue, DragDropEffects.Move);
                        }
                    }
                }
                else
                {
                    _draggedItem = null;
                }
            }
            catch (Exception)
            {
            }
        }

        private void treeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (DataContext is IDropTarget<PackFileTreeNode> dropContainer)
                {
                    if (_draggedItem == null)
                        return;

                    var dropTargetItem = sender as TreeViewItem;
                    var dropTargetNode = dropTargetItem?.DataContext as PackFileTreeNode;
                    if (dropTargetNode == null)
                        return;

                    if (dropContainer.AllowDrop(_draggedItem, dropTargetNode))
                    {
                        dropContainer.Drop(_draggedItem, dropTargetNode);
                        e.Effects = DragDropEffects.None;
                        e.Handled = true;
                    }
                }
            }
            catch
            {
            }
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            tb_FilterItem.Focus();
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }

        public static readonly DependencyProperty CustomContextMenuProperty = DependencyProperty.Register("CustomContextMenu", typeof(ContextMenu), typeof(PackFileBrowserView), new UIPropertyMetadata(null));
    }


    public class SortedCollectionViewSource : IValueConverter
    {
        public string Property0 { get; set; }
        public string Property1 { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = CollectionViewSource.GetDefaultView(value);
            s.SortDescriptions.Add(new SortDescription(Property0, ListSortDirection.Ascending));
            s.SortDescriptions.Add(new SortDescription(Property1, ListSortDirection.Ascending));
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
