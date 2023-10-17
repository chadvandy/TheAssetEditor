﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using CommonControls.Common;
using Newtonsoft.Json;
using Serilog;

namespace CommonControls.Services
{
    public enum ThemeTypeEnum
    {
        Light, 
        Colorful,
        Dark,
        //DarkColourful
    }

    public class ApplicationSettings
    {
        public class GamePathPair
        {
            public GameTypeEnum Game { get; set; }
            public string Path { get; set; }
        }

        public ObservableCollection<string> RecentPackFilePaths = new ObservableCollection<string>();

        public List<GamePathPair> GameDirectories { get; set; } = new List<GamePathPair>();
        public GameTypeEnum CurrentGame { get; set; } = GameTypeEnum.Warhammer2;
        public bool UseTextEditorForUnknownFiles { get; set; } = true;
        public bool LoadCaPacksByDefault { get; set; } = true;
        public bool AutoResolveMissingTextures { get; set; } = true;
        public bool SkipLoadingWemFiles { get; set; } = true;
        public bool AutoGenerateAttachmentPointsFromMeshes { get; set; } = true;
        public bool IsFirstTimeStartingApplication { get; set; } = true;
        public bool IsDeveloperRun { get; set; } = false;
        public bool HideWh2TextureSelectors { get; set; } = false;
        public ThemeTypeEnum CurrentTheme { get; set; } = ThemeTypeEnum.Light;
    }

    public class ApplicationSettingsService
    {
        public delegate void SettingsChangedDelegate(ApplicationSettings settings);
        public event SettingsChangedDelegate SettingsChanged;

        ILogger _logger = Logging.Create<ApplicationSettingsService>();

        string SettingsFile
        {
            get
            {
                return Path.Combine(DirectoryHelper.ApplicationDirectory, "ApplicationSettings.json");
            }
        }

        public ApplicationSettings CurrentSettings { get; set; }


        public ApplicationSettingsService()
        {
            _logger.Here().Information("Creating ApplicationSettingsService");
            Load();
        }

        public string GetGamePathForCurrentGame()
        {
            var game = CurrentSettings.CurrentGame;
            if (game == GameTypeEnum.Unknown)
                return null;
            return GetGamePathForGame(game);
        }

        public void ValidateRecentPackFilePaths()
        {
            var recentPackfilePaths = CurrentSettings.RecentPackFilePaths;
            var invalidPacks = recentPackfilePaths.Where(path => !File.Exists(path)).ToList();

            foreach (var invalidPath in invalidPacks)
            {
                recentPackfilePaths.Remove(invalidPath);
            }
        }

        public ResourceDictionary ThemeDictionary
        {
            get { return Application.Current.Resources.MergedDictionaries[0]; }
            set { Application.Current.Resources.MergedDictionaries[0] = value; }
        }

        public void ChangeTheme(ThemeTypeEnum theme)
        {
            CurrentSettings.CurrentTheme = theme;

            string themeName = null;
            switch (theme)
            {
                case ThemeTypeEnum.Light:
                    themeName = "LightTheme";
                    break;
                case ThemeTypeEnum.Colorful:
                    themeName = "ColorfulTheme";
                    break;
                case ThemeTypeEnum.Dark:
                    themeName = "DarkTheme";
                    break;
            }

            try
            {
                if (!string.IsNullOrEmpty(themeName))
                {
                    ThemeDictionary = new ResourceDictionary() { Source = new Uri($"Resources/Themes/{themeName}.xaml", UriKind.RelativeOrAbsolute) };
                }
            }
            finally { }
        }

        public void AddRecentlyOpenedPackFile(string path)
        {
            var recentPackFilePaths = CurrentSettings.RecentPackFilePaths;

            if (recentPackFilePaths.Any() && recentPackFilePaths.Last() == path)
                return;

            if (recentPackFilePaths.Contains(path))
            {
                recentPackFilePaths.Remove(path);
            }

            recentPackFilePaths.Add(path);

            if (recentPackFilePaths.Count > 15)
            {
                recentPackFilePaths.RemoveAt(0);
            }
            Save();
        }

        public string GetGamePathForGame(GameTypeEnum game)
        {
            var gameDirInfo = CurrentSettings.GameDirectories.FirstOrDefault(x => x.Game == game);
            return gameDirInfo?.Path;
        }

        public void Save()
        {
            _logger.Here().Information($"Saving settings file {SettingsFile}");

            var jsonStr = JsonConvert.SerializeObject(CurrentSettings, Formatting.Indented);
            File.WriteAllText(SettingsFile, jsonStr);

            ChangeTheme(CurrentSettings.CurrentTheme);
            SettingsChanged?.Invoke(CurrentSettings);
        }

        void Load()
        {
            if (File.Exists(SettingsFile))
            {
                _logger.Here().Information($"Loading existing settings file {SettingsFile}");

                var content = File.ReadAllText(SettingsFile);
                _logger.Here().Information(content);
                CurrentSettings = JsonConvert.DeserializeObject<ApplicationSettings>(content);

                _logger.Here().Information($"Settings loaded.");
                ChangeTheme(CurrentSettings.CurrentTheme);
                ValidateRecentPackFilePaths();
            }
            else
            {
                _logger.Here().Warning("No settings found, saving default settings and using that");
                CurrentSettings = new ApplicationSettings();
                Save();
            }
        }
    }
}
