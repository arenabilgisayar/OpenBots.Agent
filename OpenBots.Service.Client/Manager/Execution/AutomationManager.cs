﻿using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json.Linq;
using OpenBots.Agent.Core.Model;
using OpenBots.Service.API.Model;
using OpenBots.Service.Client.Manager.API;
using System;
using System.IO;
using System.Linq;

namespace OpenBots.Service.Client.Manager.Execution
{
    public static class AutomationManager
    {
        public static string DownloadAndExtractAutomation(Automation automation, out string configFilePath)
        {
            configFilePath = "";

            // Check if (Root) Automations Directory Exists (under User's AppData Folder), If Not create it
            var automationsDirectory = Path.Combine(new EnvironmentSettings().GetEnvironmentVariable(), "Automations",
                automation.AutomationEngine);

            if (!Directory.Exists(automationsDirectory))
                Directory.CreateDirectory(automationsDirectory);

            // Automation Directory
            var processDirectoryPath = Path.Combine(automationsDirectory, automation.Id.ToString());

            // Create Automation Directory named as Automation Id If it doesn't exist
            if (!Directory.Exists(processDirectoryPath))
                Directory.CreateDirectory(processDirectoryPath);
            var processNugetFilePath = Path.Combine(processDirectoryPath, automation.Name.ToString() + ".nuget");
            var processZipFilePath = Path.Combine(processDirectoryPath, automation.Name.ToString() + ".zip");
            
            // Check if Automation (.nuget) file exists if Not Download it
            if (!File.Exists(processNugetFilePath))
            {
                // Download Automation by Id
                var apiResponse = AutomationsAPIManager.ExportAutomation(AuthAPIManager.Instance, automation.Id.ToString());

                // Write Downloaded(.nuget) file in the Automation Directory
                File.WriteAllBytes(processNugetFilePath, apiResponse.Data.ToArray());
            }

            // Create .zip file if it doesn't exist
            if (!File.Exists(processZipFilePath))
                File.Copy(processNugetFilePath, processZipFilePath);

            var extractToDirectoryPath = Path.ChangeExtension(processZipFilePath, null);

            // Extract Files/Folders from (.zip) file
            DecompressFile(processZipFilePath, extractToDirectoryPath);

            // Delete .zip File
            File.Delete(processZipFilePath);

            string mainFileName;
            switch (automation.AutomationEngine.ToString())
            {
                case "OpenBots":
                    configFilePath = Directory.GetFiles(extractToDirectoryPath, "project.config", SearchOption.AllDirectories).First();
                    mainFileName = JObject.Parse(File.ReadAllText(configFilePath))["Main"].ToString();
                    break;

                case "Python":
                    mainFileName = "__main__.py";
                    break;

                default:
                    throw new NotImplementedException($"Specified execution engine \"{automation.AutomationEngine}\" is not implemented on the OpenBots Agent.");
            }

            // Return "Main" Script File Path of the Automation
            return Directory.GetFiles(extractToDirectoryPath, mainFileName, SearchOption.AllDirectories).First();
        }

        private static void DecompressFile(string processZipFilePath, string targetDirectory)
        {
            // Create Target Directory If it doesn't exist
            if(!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            // Extract Files/Folders from downloaded (.zip) file
            FileStream fs = File.OpenRead(processZipFilePath);
            ZipFile file = new ZipFile(fs);

            foreach (ZipEntry zipEntry in file)
            {
                if (!zipEntry.IsFile)
                {
                    // Ignore directories
                    continue;
                }

                string entryFileName = zipEntry.Name;

                // 4K is optimum
                byte[] buffer = new byte[4096];
                Stream zipStream = file.GetInputStream(zipEntry);

                // Manipulate the output filename here as desired.
                string fullZipToPath = Path.Combine(targetDirectory, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);

                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = File.Create(fullZipToPath))
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
            }

            if (file != null)
            {
                file.IsStreamOwner = true;
                file.Close();
            }
        }
    }
}