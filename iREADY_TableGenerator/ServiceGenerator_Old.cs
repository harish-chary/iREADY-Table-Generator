using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ServiceGenerator_Old
{
    public static void serviceGenerator(string sourceFilePath, string destinationFilePath, string replaceText)
    {
        //Console.WriteLine("Enter the path of the text file to search: ");
        //string sourceFilePath = Console.ReadLine();
        //string sourceFilePath = @"C:\Users\hchary\OneDrive - Freyr Solutions\Desktop\Text files\ServiceFile.txt";

        //Console.WriteLine("Enter the path where you want to store the updated file: ");
        //string destinationFilePath = Console.ReadLine();
        //string destinationFilePath = @"C:\Users\hchary\OneDrive - Freyr Solutions\Desktop\Text files\UpdatedServiceFile.txt";

        //Console.WriteLine("Enter the string(s) to find (separate with commas if multiple): ");
        string[] findStrings = { "ClassName", "VariableName" };

        //Console.Write("Enter the class/table name: ");
        //string replaceText = Console.ReadLine();
        string[] replaceStrings = { replaceText, char.ToLower(replaceText[0]) + replaceText.Substring(1) };

        try
        {
            string[] lines = File.ReadAllLines(sourceFilePath);

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < findStrings.Length; j++)
                {
                    lines[i] = lines[i].Replace(findStrings[j], replaceStrings[j]);
                }
            }

            File.WriteAllLines(destinationFilePath, lines);
            Console.WriteLine("Service File Generated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        //System.Diagnostics.Process.Start("notepad.exe", @"C:\Users\hchary\OneDrive - Freyr Solutions\Desktop\Text files\UpdatedServiceFile.txt");
    }
}