using System;
using System.Collections.Generic;
using System.Text;

namespace xEditLevelListInjection
{
    static partial class Program
    {
        static void outputListToConsole(List<ItemForm> itemList)
        {
            foreach (ItemForm itemForm in itemList)
            {
                Console.WriteLine(itemForm.ToString());
            }
        }

        static string BuildxEditImportScript(string absoluteListFilePath)
        {
            StringBuilder ImportScript = new StringBuilder();
            ImportScript.Append($"unit ____Import{FileOutputName}ItemsToLevelList;");
            ImportScript.AppendLine("");
            ImportScript.AppendLine("interface");
            ImportScript.AppendLine("  implementation");
            ImportScript.AppendLine("    uses xEditAPI, 'Add Items To Leveled List', mteFunctions, Classes, SysUtils, StrUtils, Windows;");
            ImportScript.AppendLine("var  ");
            ImportScript.AppendLine("    slFormList: TStringList;");
            ImportScript.AppendLine("");
            ImportScript.AppendLine("function Initialize: integer;");
            ImportScript.AppendLine("begin ");
            ImportScript.AppendLine("  slFormList := TStringList.create;");
            ImportScript.AppendLine(String.Format("  slFormList.LoadFromFile('{0}');", absoluteListFilePath).ToString());
            ImportScript.AppendLine("end;");
            ImportScript.AppendLine("");
            ImportScript.AppendLine("function Process(e: IInterface): integer;");
            ImportScript.AppendLine("begin ");
            ImportScript.AppendLine("  CreateTransferFormList(e);");
            ImportScript.AppendLine("end;");
            ImportScript.AppendLine("");
            ImportScript.AppendLine("function Finalize: integer;");
            ImportScript.AppendLine("begin ");
            ImportScript.AppendLine("end;");
            ImportScript.AppendLine("end.");

            return ImportScript.ToString();
        }
    }
}
