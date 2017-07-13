using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class HelpInformation
{
    private static readonly string HELP_FILE_PATH = Application.dataPath + "/Editor/AssetProfiler/LocalResources/AssetProfilerHelp.txt";

    public static string GetHelpInformation()
    {
        if (!File.Exists(HELP_FILE_PATH))
            return "";

        StreamReader reader = new StreamReader(HELP_FILE_PATH);
        string result = reader.ReadToEnd();
        reader.Close();
        return result;
    }
}
