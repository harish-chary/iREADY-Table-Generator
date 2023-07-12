using System.IO;

class ClassGenerator
{
    public static void classGenerator(string sourceFilePath, 
                                      string destinationFilePath, 
                                      string className, 
                                      string[] parameters, 
                                      string[] Bsonparameters)
    {
        string findText = "TableName";
        string[] lines = File.ReadAllLines(sourceFilePath);
        int lineNumber = 12, BsonlineNumber = 18 + parameters.Length;

        #region API Class File Generator

        using (StreamWriter writer = new StreamWriter(destinationFilePath))
        {
            for (int i = 0; i < lines.Length+parameters.Length; i++)
            {
                if (i == lineNumber - 1)
                {
                    for (int j = 0; j < parameters.Length; j++)
                    {
                        string newLine = "        " + parameters[j] + " {get; set;}";
                        writer.WriteLine(newLine);
                    }
                }

                if (i == BsonlineNumber - 1)
                {
                    for (int j = 0; j < parameters.Length; j++)
                    {
                        string newLine = "        [BsonElement(\"" + Bsonparameters[j] + "\")]\n        public " + parameters[j] + " {get; set;}";
                        writer.WriteLine(newLine);
                    }
                    break;
                }

                if (i<18)
                {
                    string line = lines[i];
                    if (line.Contains(findText))
                    {
                        line = line.Replace(findText, className);
                    }
                    writer.WriteLine(line);
                }

            }
            writer.WriteLine("    }\n}");
        }

        #endregion

        System.Diagnostics.Process.Start("notepad.exe", destinationFilePath);
    }
}
