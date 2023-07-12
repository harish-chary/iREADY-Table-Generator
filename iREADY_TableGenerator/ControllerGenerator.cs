using System;
using System.IO;

class ControllerGenerator
    {
        public static void controllerGenerator(string sourceFilePath, 
                                               string destinationFilePath, 
                                               string replaceText,
                                               string[] replaceStrings)
        {
        string[] findStrings = { "ClassName", "VariableName" };
        //string[] replaceStrings = { replaceText, char.ToLower(replaceText[0]) + replaceText.Substring(1) };

        #region Controller File Generator

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
                    writer.WriteLine(lines[i]);
                }
            }
            Console.WriteLine("\nNavigate to your startup.cs file and add the below snippet.", Console.ForegroundColor = ConsoleColor.Black, Console.BackgroundColor=ConsoleColor.White);
            Console.WriteLine("services.AddScoped<I" + replaceStrings[0] + "Service, " + replaceStrings[0] + "Service>();\n", Console.ForegroundColor = ConsoleColor.Green, Console.BackgroundColor = ConsoleColor.Black);
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Navigate to your controller file and add the below 3 snippets to complete adding API.", Console.ForegroundColor = ConsoleColor.Black, Console.BackgroundColor = ConsoleColor.White);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("private readonly I"+replaceStrings[0]+"Service _"+replaceStrings[1]+"Service = null;");
            Console.WriteLine("I"+ replaceStrings[0] + "Service "+ replaceStrings[1] + "Service");
            Console.WriteLine("_"+replaceStrings[1]+"Service = "+replaceStrings[1]+"Service;");
            Console.ResetColor();
            Console.WriteLine("\n\n");

        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        #endregion

        System.Diagnostics.Process.Start("notepad.exe", destinationFilePath);
    }
}
