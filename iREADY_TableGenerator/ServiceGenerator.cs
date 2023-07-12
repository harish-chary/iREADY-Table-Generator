using System;
using System.IO;

class ServiceGenerator
{
    public static void serviceGenerator(string sourceFilePath, 
                                        string destinationFilePath,
                                        string[] replaceStrings,
                                        string[] parameters)
    {
        string[] findStrings = { "ClassName", "VariableName" };
        //string[] replaceStrings = { replaceText, char.ToLower(replaceText[0]) + replaceText.Substring(1) };

        #region Service File Generator

        try
        {
            string[] lines = File.ReadAllLines(sourceFilePath);

            using (StreamWriter writer = new StreamWriter(destinationFilePath))
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    for (int j = 0; j < findStrings.Length; j++)
                    {
                        lines[i] = lines[i].Replace(findStrings[j], replaceStrings[j]);
                    }
                    if (lines[i].Equals("                //InitialInsert"))
                    {
                        for (int k = 0; k < parameters.Length; k++)
                        {
                            string temp = parameters[k].Substring(parameters[k].IndexOf(" ") + 1);
                            writer.WriteLine("                _" + replaceStrings[1]+"."+temp+" = "+ replaceStrings[1]+"."+temp+";");
                        }
                    }
                    if (lines[i].Equals("                    //SecondInsert"))
                    {
                        for (int k = 0; k < parameters.Length; k++)
                        {
                            string temp = parameters[k].Substring(parameters[k].IndexOf(" ") + 1);
                            writer.WriteLine("                    .Set(p => p." + temp + "," + replaceStrings[1]+ "." +temp+")");
                        }
                    }
                    else
                    {
                        writer.WriteLine(lines[i]);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }

        #endregion

        System.Diagnostics.Process.Start("notepad.exe", destinationFilePath);
    }
}