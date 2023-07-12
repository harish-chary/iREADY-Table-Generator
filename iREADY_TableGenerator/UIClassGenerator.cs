using System.Collections.Generic;
using System.IO;

class UIClassGenerator
{
    public static void uiClassGenerator(string sourceFilePath, 
                                        string destinationFilePath, 
                                        string className, 
                                        string[] parameters, 
                                        string[] Bsonparameters,
                                        Dictionary<string, string> dataTypes)
    {
        string findText = "TableName";

        #region UI Class Generator
        bool flag = true;
        string[] lines = File.ReadAllLines(sourceFilePath);

        using (StreamWriter writer = new StreamWriter(destinationFilePath))
        {
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; flag && j < findText.Length; j++)
                {
                    lines[i] = lines[i].Replace(findText, className);
                    flag = false;
                }
                if (lines[i].Equals("    //insertHere"))
                {
                    for (int k = 0; k < Bsonparameters.Length; k++)
                    {
                        string tsDataType;
                        dataTypes.TryGetValue(parameters[k].Substring(0, parameters[k].IndexOf(" ")), out tsDataType);
                        writer.WriteLine("    " + Bsonparameters[k] + ": " + tsDataType + ";");
                    }
                }
                else
                {
                    writer.WriteLine(lines[i]);
                }
            }
        }
        #endregion

        System.Diagnostics.Process.Start("notepad.exe", destinationFilePath);
    }
}