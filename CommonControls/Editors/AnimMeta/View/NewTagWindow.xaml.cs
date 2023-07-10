﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommonControls.Common;

namespace CommonControls.Editors.AnimMeta.View
{
    /// <summary>
    /// Interaction logic for NewTagWindow.xaml
    /// </summary>
    public partial class NewTagWindow : Window
    {
        public NewTagWindow()
        {
            InitializeComponent();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            var model = DataContext as NewTagWindowViewModel;
            if (model.SelectedItem == null)
            {
                MessageBox.Show("Nothing selected");
                return;
            }

            DialogResult = true;
            Close();

        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as NewTagWindowViewModel;
            if (model.SelectedItem != null)
                OnOkClick(null, null);
        }
    }

    class NewTagWindowViewModel : NotifyPropertyChangedImpl
    {
        public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>();
        string _selectedItem;
        public string SelectedItem { get => _selectedItem; set { SetAndNotify(ref _selectedItem, value); } }
    }
}
