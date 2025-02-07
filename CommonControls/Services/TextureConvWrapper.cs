﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Reflection;
using CommonControls.Common;
using CommonControls.FileTypes.RigidModel.Types;
using Serilog;

namespace CommonControls.Services
{
    public class TextureConvWrapper
    {
        ILogger _logger = Logging.Create<TextureConvWrapper>();

        public TextureConvWrapper()
        {
            EnsureTexconvExists();
        }

        void EnsureTexconvExists()
        {
            var texconvPath = $"{DirectoryHelper.Applications}\\texconv.exe";

            if (!File.Exists(texconvPath))
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("View3D.Content.Other.texconv.exe");
                using var fStream = new FileStream(texconvPath, FileMode.OpenOrCreate);
                stream!.CopyTo(fStream);
                _logger.Here().Information("Creating instance of texConv.exe");
            }
        }

        public void SavePNGTextureAsDDS(string pngFilePath, TextureType texureType = TextureType.Diffuse)
        {
            var texconvArguments = texureType switch
            {
                TextureType.Mask => "-f BC3_UNORM",
                TextureType.Normal => "-f BC3_UNORM",
                TextureType.Gloss => "-f BC1_UNORM",
                _ => "-f BC7_UNORM_SRGB",
            };

            var cmd = $"{texconvArguments} -y -o \"{Path.GetDirectoryName(pngFilePath)}\" \"{pngFilePath}\"";
            RunTextConv(cmd);
        }

        public void SaveDDSTextureAsPNG(string texturePath)
        {
            RunTextConv($"-ft png -y -o \"{Path.GetDirectoryName(texturePath)}\" \"{texturePath}\"");
        }

        void RunTextConv(string cmd)
        {
            var texconvPath = $"{DirectoryHelper.Applications}\\texconv.exe";

            using var pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = texconvPath;
            pProcess.StartInfo.Arguments = cmd;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            var result = pProcess.StandardOutput.ReadToEnd();
            _logger.Here().Information(result);
        }
    }


}

