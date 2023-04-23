﻿using CommonControls.FileTypes.PackFiles.Models;

namespace CommonControls.Editors.AudioEditor.BnkCompiler
{
    public class CompileResult
    {
        public PackFile OutputBnkFile { get; set; }
        public PackFile OutputDatFile { get; set; }
        public PackFile NameList { get; set; }
    }


    public class CompilerResultHandler
    {

        public string WWiserPath { get; set; }
        public string OutputPath { get; set; }
        /*
         * 
         *  public bool CompileAllProjects(out ErrorList outputList)
        {
            outputList = new ErrorList();

            if (_pfs.HasEditablePackFile() == false)
            {
                outputList.Error("Compiler", "No Editable pack is set");
                return false;
            }

            var allProjectFiles = _pfs.FindAllWithExtention(".xml").Where(x => x.Name.Contains("bnk.xml"));
            outputList.Ok("Compiler", $"{allProjectFiles.Count()} projects found to compile.");

            foreach (var file in allProjectFiles)
            {
                outputList.Ok("Compiler", $"Starting {_pfs.GetFullPath(file)}");
                var compileResult = CompileProject(file, out var instanceErrorList);
                if (compileResult == null)
                    outputList.AddAllErrors(instanceErrorList);

                if (compileResult != null)
                {
                    SaveHelper.SavePackFile(_pfs, "wwise\\audio", compileResult.OutputBnkFile, true);
                    SaveHelper.SavePackFile(_pfs, "wwise\\audio", compileResult.OutputDatFile, true);
                }

                outputList.Ok("Compiler", $"Finished {_pfs.GetFullPath(file)}. Overall result:{compileResult != null}");
            }
            return true;
        }
         */


        /*
         *             // Move somewhere else 
                if (audioProject.ProjectSettings.ExportResultToFile)
                {
                    var bnkPath = Path.Combine(audioProject.ProjectSettings.OutputFilePath, $"{audioProject.ProjectSettings.BnkName}.bnk");
                    File.WriteAllBytes(bnkPath, compileResult.OutputBnkFile.DataSource.ReadData());

                    var datPath = Path.Combine(audioProject.ProjectSettings.OutputFilePath, $"{audioProject.ProjectSettings.BnkName}.dat");
                    File.WriteAllBytes(datPath, compileResult.OutputDatFile.DataSource.ReadData());

                    if (audioProject.ProjectSettings.ConvertResultToXml)
                        BnkToXmlConverter.Convert(audioProject.ProjectSettings.WWiserPath, bnkPath, true);
                }
         */
    }

}
