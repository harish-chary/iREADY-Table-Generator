using System.Collections.Generic;
using System.IO;

 class HTMLComponentGenerator
{
    public static void htmlComponentGenerator(string sourceFilePath, 
                                              string destinationFilePath, 
                                              string className,
                                              string[] replaceStrings,
                                              string[] parameters, 
                                              string[] Bsonparameters,
                                              Dictionary<string, string> dataTypes)
    {
        string variableName = char.ToLower(className[0]) + className.Substring(1);

        #region HTML File Generator

        string[] lines = File.ReadAllLines(sourceFilePath);

        bool flag1 = true, flag2 = true, flag3 = true;
        using (StreamWriter writer = new StreamWriter(destinationFilePath))
        {
            for (int i = 0; i < lines.Length; i++)
            {

                if (lines[i].Contains("ClassName"))
                    lines[i] = lines[i].Replace("ClassName", className);
                if (lines[i].Contains("VariableName"))
                    lines[i] = lines[i].Replace("VariableName", variableName);

                if(flag1 && lines[i].Contains("                <!-- First Insert -->"))
                {
                    for(int j=0; j < parameters.Length; j++)
                    {
                        string temp;
                        dataTypes.TryGetValue(parameters[j].Substring(0,parameters[j].IndexOf(" ")), out temp);
                        writer.WriteLine("                <div class=\"col-md-4\">\n" +
                            "                    <div class=\"form-group field\">\n" +
                            "                        <label class=\"block\">" + parameters[j].Substring(parameters[j].IndexOf(" ")+1)+"</label>\n"+
                            "                        <input type=\""+temp+"\" [(ngModel)]=\"obj"+className+"."+Bsonparameters[j]+"\"pInputText/>\n"+
                            "                    </div>\n"+
                            "                </div>\n");
                    }   
                    flag1 = false;
                }

                if (flag2 && lines[i].Contains("                    <!-- Second Insert -->"))
                {
                    for (int j = 0; j < parameters.Length; j++)
                    {
                        writer.WriteLine("                    <th>"+ parameters[j].Substring(parameters[j].IndexOf(" ") + 1)+ "</th>");
                    }
                    flag2 = false;
                }

                if (flag3 && lines[i].Contains("                    <!-- Third Insert -->"))
                {
                    for (int j = 0; j < parameters.Length; j++)
                    {
                        writer.WriteLine("                    <td>{{"+variableName+"."+Bsonparameters[j]+"}}</td>");
                    }
                    flag3 = false;
                }

                writer.WriteLine(lines[i]);
            }
        }

        #endregion

        System.Diagnostics.Process.Start("notepad.exe", destinationFilePath);
    }
}
