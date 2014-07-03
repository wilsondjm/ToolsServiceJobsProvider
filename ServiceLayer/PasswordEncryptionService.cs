﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class PasswordEncryptionService
    {
        public string encryptString(string source, string basePath, string relativePath)
        {
            basePath = basePath + relativePath + "\\P4PluginPasswordEncryption\\encrypt.bat"; //relative path :"\\.." or "", etc.
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = basePath;
            pProcess.StartInfo.Arguments = source;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            
            try
            {
                pProcess.Start();

            }catch(Exception e)
            {
                string error = e.StackTrace;
            }

            StreamReader outputReader = pProcess.StandardOutput;
            String result = outputReader.ReadToEnd();
            
            return result.Remove(result.Length - 2, 2).Substring(11);
        }
    }

    
}
