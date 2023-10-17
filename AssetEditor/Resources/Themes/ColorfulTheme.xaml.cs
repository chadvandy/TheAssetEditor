// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace AssetEditor.Resources.Themes
{

    public partial class ColorfulTheme : ResourceDictionary
    {
        private void CloseWindow_Event(object sender, RoutedEventArgs e)
        {
            if (e.Source != null)
                try
                {
                    this.CloseWind(Window.GetWindow((FrameworkElement)e.Source));
                }
                catch
                {
                }
        }

        private void AutoMinimize_Event(object sender, RoutedEventArgs e)
        {
            if (e.Source != null)
                try
                {
                    this.MaximizeRestore(Window.GetWindow((FrameworkElement)e.Source));
                }
                catch
                {
                }
        }

        private void Minimize_Event(object sender, RoutedEventArgs e)
        {
            if (e.Source != null)
                try
                {
                    this.MinimizeWind(Window.GetWindow((FrameworkElement)e.Source));
                }
                catch
                {
                }
        }

        public void CloseWind(Window window) => window.Close();

        public void MaximizeRestore(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
                window.WindowState = WindowState.Normal;
            else if (window.WindowState == WindowState.Normal)
                window.WindowState = WindowState.Maximized;
        }

        public void MinimizeWind(Window window) => window.WindowState = WindowState.Minimized;
    }
}
