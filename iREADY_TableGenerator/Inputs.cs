using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

class Inputs
{
    public static string[] inputs(string JSONFilePath)
    {

        dynamic data = JsonConvert.DeserializeObject(JSONFilePath);

        // Get first record
        dynamic firstRecord = data[0];

        // Generate C# class from first record
        string className = "MyClass";
        string []classDefinition = GenerateClassDefinition(className, firstRecord).ToArray();

        // Print C# class definition
        return classDefinition;
    }

    static List<string> GenerateClassDefinition(string className, dynamic obj)
    {
        List<string> classList = new List<string>();

        foreach (var prop in obj)
        {
                
            string propName = prop.Name;
            string propType = GetCSharpType(prop.Value);
            if (propName == "_id" || 
                propName == "isActive" || 
                propName == "isDelete" ||
                propName == "createdBy" || 
                propName == "createdDate" || 
                propName == "modifiedBy" || 
                propName == "modifiedDate") continue;

            classList.Add(propType +" "+ propName);
        }

        return classList;
    }

    static string GetCSharpType(dynamic value)
    {
        switch (value.Type)
        {
            case JTokenType.Integer:
                return "int";
            case JTokenType.Float:
                return "float";
            case JTokenType.Boolean:
                return "bool";
            case JTokenType.String:
                return "string";
            case JTokenType.Date:
                return "DateTime";
            case JTokenType.Array:
                // Assume array of objects, and get type from first object
                var first = value.First;
                if (first == null) return "object[]";
                return $"{GetCSharpType(first)}[]";
            case JTokenType.Object:
             //Assume nested object, and generate class for it
                string className = "NestedClass";
                string classDef = GenerateClassDefinition(className, value);
                return className;
            default:
                return "object";
        }
    }
}
