﻿using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonControls.FileTypes.PackFiles.Models;
using CommonControls.Interfaces.AssetManagement;
using CommonControls.Services;

namespace CommonControls.Events.UiCommands
{
    public class ImportAssetCommand : IUiCommand
    {
        private readonly PackFileService _packFileService;
        private readonly IAssetManagementFactory _assetManagementFactory;

        public ImportAssetCommand(PackFileService packFileService, IAssetManagementFactory assetManagementFactory)
        {
            _packFileService = packFileService;
            _assetManagementFactory = assetManagementFactory;
        }

        public void Execute(PackFileContainer container, string parentPath)
        {
            if (container.IsCaPackFile)
            {
                MessageBox.Show("Unable to edit CA packfile");
                return;
            }

            var dialog = new OpenFileDialog
            {
                Filter = "FBX Files (*.fbx)|*.fbx|All files (*.*)|*.*\\",   // Clean this up so its correct based on the assetManagementFactory data
                Multiselect = false
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var filename = dialog.FileNames.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(filename))
                    return;

                // TODO: RE-ENABLE!!!
               //try
                {
                    var extension = Path.GetExtension(filename);
                    var importer = _assetManagementFactory.GetImporter(extension);  // TODO: What if no importer is found?
                    var packFile = importer.ImportAsset(filename);

                    if (packFile == null)
                        return;

                    _packFileService.AddFileToPack(container, parentPath, packFile);
                }
                //catch (Exception e)
                //{
                //    MessageBox.Show($"Failed to import model/scene file {filename}. Error : {e.Message}", "Error");
                //}
            }
        }
    }
}
