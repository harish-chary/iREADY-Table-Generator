using System;
using System.IO;

class TSComponentGenerator
{
    public static void tsComponentGenerator(string sourceFilePath, 
                                            string destinationFilePath, 
                                            string className, 
                                            string dataService)
    {
        string variableName = char.ToLower(className[0])+className.Substring(1);
        string fileName="",fileCode=className.Substring(0,1);
        for(int i = 0; i < variableName.Length; i++)
        {
            if (char.IsUpper(variableName[i]))
            {
                fileCode += variableName[i];
                fileName += "-";
            }
            fileName += char.ToLower(variableName[i]);
        }

        dataService = dataService == @"\IngMasterDataService" ? @"IngMasterData" : @"RulesMasterData";

        #region TS File Generator

        string[] lines = File.ReadAllLines(sourceFilePath);

        using (StreamWriter writer = new StreamWriter(destinationFilePath))
        {
            for (int i = 0; i < lines.Length; i++)
            {
                    if(lines[i].Contains("ClassName"))
                        lines[i] = lines[i].Replace("ClassName", className);
                    if (lines[i].Contains("VariableName"))
                        lines[i] = lines[i].Replace("VariableName", variableName);
                    if (lines[i].Contains("FileName"))
                        lines[i] = lines[i].Replace("FileName", fileName);
                    if (lines[i].Contains("MasterData"))
                        lines[i] = lines[i].Replace("MasterData", dataService);

                    writer.WriteLine(lines[i]);
            }
        }

        #endregion

        #region Code Snippets to be added in UI

        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"\n\nNavigate to your in-active.components.ts file and add the below snippet at the end.", Console.ForegroundColor = ConsoleColor.Black, Console.BackgroundColor = ConsoleColor.White);
        Console.ResetColor();
        Console.WriteLine();
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n          else if(this.moduleType==\"{className}\")\n" +
            $"         {{\n" +
            $"          this.ireadyApi.postData(\"{dataService}/Delete{className}ById?id=\" + this.objModel.id + \"&modifiedBy=\" + this.objModel.modifiedBy + \"&isactive=\" + this.objModel.isDelete,{{ }}).toPromise().then((resp: any) => {{\n" +
            $"          this.activeModal.close(this.moduleType);\n" +
            $"            }})\n" +
            $"          }}\n");
        Console.ResetColor();

        Console.Write($"\nNavigate to your master-data files and add the below snippets to finish adding UI.", Console.ForegroundColor = ConsoleColor.Black, Console.BackgroundColor = ConsoleColor.White);
        Console.ResetColor();
        Console.WriteLine("\n\nHTML\n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"                <ng-template [ngIf]=\"is" +className+"\">\n"+
"                    <div>\n"+
"                        <app-" +fileName+"></app-" +fileName+">\n" +
"                    </div>\n"+
"                </ng-template>");
        Console.ResetColor();

        Console.WriteLine("\nTS\n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("is"+className+ ": boolean=false;\n,{ name:'"+fileName+"', code: '"+fileCode+ "'}\nthis.is"+className+"=false;");
        Console.ResetColor();

        Console.WriteLine("\n\n");

        #endregion

        System.Diagnostics.Process.Start("notepad.exe", destinationFilePath);
    }
}