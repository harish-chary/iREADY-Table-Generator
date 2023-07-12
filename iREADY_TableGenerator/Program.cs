using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
class Program
{
    public static void Main(string[] args)
    {

        //Please paste your JSON folder location below
        string JSONFilePath = @"C:\Users\hchary\Freyr Solutions\Pullareddy Chinna Gurrala - iREADY V6.0.0\Json_Data_Files\1_Cosmetics\1_IRDB";

        #region User Inputs
        Console.SetWindowSize(85, 20);
        JSONFilePath += @"\";
        Console.Write("Enter Table Name: \n> ");
        string JSONFile, TblName = Console.ReadLine()+".json", dataService;
        catchLabel:
        JSONFilePath +=TblName;
        //JSONFilePath = @"C:\Users\hchary\OneDrive - Freyr Solutions\Desktop\Text files\" + TblName;

        try
        {
            JSONFile = System.IO.File.ReadAllText(JSONFilePath);
            Console.WriteLine("\nSelected file: "+JSONFilePath.Substring(JSONFilePath.LastIndexOf(@"\")+1));
        }
        catch
        {
            Console.Write("Cannot find file named \""+TblName+"\" at specified location: "+
                               JSONFilePath.Substring(0,JSONFilePath.LastIndexOf(@"\")) +
                               "\nRe-enter the table name or enter '0' to exit the application.\n>");
            JSONFilePath = JSONFilePath.Substring(0,JSONFilePath.IndexOf(TblName));
            TblName = Console.ReadLine()+".json";
            if (TblName == "0.json")
            {
                Process.GetCurrentProcess().Kill();
            }
            goto catchLabel;
        }

        string currdir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        currdir = currdir.Substring(0,currdir.IndexOf(@"\bin"));
        string CodeGeneratorProjectPath = currdir+@"\GeneratorDependencies";

        //enter your iREADY project path
        //string iREADYprojectPath = @"C:\Users\harish_chary\OneDrive - Freyr Solutions\Desktop\IngredientsService" + @"\Freyr.iREADY.IngredientsService\Freyr.iREADY.IngredientsService.Host";

        string className = TblName.Substring(0,TblName.Length-5);
        string[] replaceStrings = { className, char.ToLower(className[0]) + className.Substring(1) };
        Console.WriteLine("Table/Class name: "+className+"\n");

        DataServiceLabel:
        Console.Write("Which master data service does the table belong to? \n" +
                      "> Enter 1 for IngMasterDataService \n" +
                      "> Enter 2 for RulesMasterDataService\n> ");
        dataService = Console.ReadLine();
        if (dataService == "1") dataService = @"\IngMasterDataService";
        else if (dataService == "2") dataService = @"\RulesMasterDataService";
        else
        {
            Console.WriteLine("Invalid input! Try again.\n");
            goto DataServiceLabel;
        }

        #endregion

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        #region File Paths Initialization

        string Service_sourceFilePath = CodeGeneratorProjectPath + @"\ServiceFile.txt";
        string Service_destinationFilePath = CodeGeneratorProjectPath + @"\UpdatedServiceFile.txt";

        string Class_sourceFilePath = CodeGeneratorProjectPath + @"\ClassFile.txt";
        string Class_destinationFilePath = CodeGeneratorProjectPath + @"\UpdatedClassFile.txt";

        string Controller_sourceFilePath = CodeGeneratorProjectPath + @"\ControllerFile.txt";
        string Controller_destinationFilePath = CodeGeneratorProjectPath + @"\UpdatedControllerFile.txt";

        string HTML_sourceFilePath = CodeGeneratorProjectPath + @"\HTMLComponentGenerator.txt";
        string HTML_destinationFilePath = CodeGeneratorProjectPath + @"\UpdatedHTMLComponentGenerator.txt";

        string TS_sourceFilePath = CodeGeneratorProjectPath + @"\TSComponentGenerator.txt";
        string TS_destinationFilePath = CodeGeneratorProjectPath + @"\UpdatedTSComponentGenerator.txt";

        string UIClass_sourceFilePath = CodeGeneratorProjectPath + @"\UIClassFile.txt";
        string UIClass_destinationFilePath = CodeGeneratorProjectPath + @"\UpdatedUIClassFile.txt";

        #endregion

        #region Variables Initialization

        string[] Bsonparameters = Inputs.inputs(JSONFile);
        string[] parameters = new string[Bsonparameters.Length];

        for (int i = 0; i < Bsonparameters.Length; i++)
        {
            string newLine=Bsonparameters[i];
            int ind = newLine.IndexOf(" ") + 1;
            parameters[i] = Bsonparameters[i].Substring(0,ind)+char.ToUpper(Bsonparameters[i][ind])+Bsonparameters[i].Substring(ind+1);
            Bsonparameters[i] = Bsonparameters[i].Substring(ind);
        }

        Dictionary<string, string> dataTypes = new Dictionary<string, string>()
        {
            {"int", "number"},
            {"float", "number" },
            {"string", "string"},
            {"bool", "boolean"}
        };

        #endregion

        #region Invoking All Generator Functions

        Console.Clear();
        Console.WriteLine("Selected file: " + JSONFilePath.Substring(JSONFilePath.LastIndexOf(@"\") + 1));
        Console.WriteLine("Table/Class name: " + className);
        Console.WriteLine("Data service used: " + dataService.Substring(1));

        HTMLComponentGenerator.htmlComponentGenerator(HTML_sourceFilePath, HTML_destinationFilePath,className, replaceStrings, parameters, Bsonparameters, dataTypes);
        TSComponentGenerator.tsComponentGenerator(TS_sourceFilePath, TS_destinationFilePath, className, dataService);
        UIClassGenerator.uiClassGenerator(UIClass_sourceFilePath, UIClass_destinationFilePath, className, parameters, Bsonparameters, dataTypes);

        ControllerGenerator.controllerGenerator(Controller_sourceFilePath, Controller_destinationFilePath, className, replaceStrings);
        ServiceGenerator.serviceGenerator(Service_sourceFilePath, Service_destinationFilePath, replaceStrings, parameters);
        ClassGenerator.classGenerator(Class_sourceFilePath, Class_destinationFilePath, className, parameters, Bsonparameters);

        #endregion

        stopwatch.Stop();
        Console.WriteLine($"All tables successfully generated in {stopwatch.Elapsed.TotalSeconds:F2} seconds");

        Console.ReadKey();
    }
}