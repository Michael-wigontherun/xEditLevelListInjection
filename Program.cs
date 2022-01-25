﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace xEditLevelListInjection
{
    static partial class Program
    {
        static StringBuilder FileOutputName = new StringBuilder();
        static bool ReimportFile = false;
        static bool OutputScriptNoConformation = false;
        static string OrigonalListPath = "";
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                args = new string[] { @".\xEditOutput\Armor.csv" };
            }
            else
            {
                if (args.Length >= 2)
                {
                    for(int i = 1; i < args.Length; i++)
                    {
                        switch (args[i])
                        {
                            case "-reimport":
                                ReimportFile = true;
                                Console.WriteLine("-reimport argument detected. Will re-import origonal xEdit ouput after exporting the filtered list.");
                                break;
                            case "-outputScript":
                                OutputScriptNoConformation = true;
                                Console.WriteLine("-outputScript argument detected. Will Ouput the xEditScript to import list with no conformation.");
                                break;
                            default:
                                Console.WriteLine($"Invalid console argument: {args[i]}");
                                break;
                        }
                    }
                }
            }
            List<ItemForm> itemList = new List<ItemForm>();
            itemList = tryGetOutputList(args[0]);

            bool close = false;
            try
            {
                do
                {
                    Console.WriteLine("\n\n\n\n\n\n\n");
                    Console.WriteLine($"Current filter line: {FileOutputName}");
                    Console.WriteLine("Input number of operation. Then press enter");
                    Console.WriteLine("\n");
                    Console.WriteLine("1 to filter for keyword.");
                    Console.WriteLine("2 to exclude keyword.");
                    Console.WriteLine("3 to output list for import");
                    Console.WriteLine("4 to re-get origonal list. This clears your current list.");
                    Console.WriteLine("5 to list in console");
                    Console.WriteLine("6 to close.");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            itemList = Filter(itemList, true);
                            break;
                        case "2":
                            itemList = Filter(itemList, false);
                            break;
                        case "3":
                            itemList = OutputList(itemList);
                            break;
                        case "4":
                            itemList = tryGetOutputList(OrigonalListPath);
                            break;
                        case "5":
                            outputListToConsole(itemList);
                            break;
                        case "6":
                            close = true;
                            break;
                        default:
                            Console.WriteLine("Didn't understand request.");
                            break;
                    }
                } while (close == false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }

        static List<ItemForm> OutputList(List<ItemForm> itemList)
        {
            FileOutputName = FileOutputName.Replace(" ", "");
            List<string> FormIDList = new List<string>();
            string filePath = $".\\xEditLevelListInjectorOuput\\{FileOutputName}.txt";
            foreach (ItemForm itemForm in itemList)
            {
                FormIDList.Add(itemForm.FormID);
            }
            Directory.CreateDirectory(".\\xEditLevelListInjectorOuput");
            File.WriteAllLines(filePath, FormIDList);
            //@".\xEditOutput\Armor.csv"
            string fileFullPath = Path.GetFullPath(filePath);
            Console.WriteLine($"Output form list to \"{fileFullPath}\"");
            if(OutputScriptNoConformation == false)
            { 
                Console.WriteLine($"Do you want to export a new xEdit script \"_Import{FileOutputName}ItemsToLevelList.pas\"");
                Console.WriteLine("1 for yes.");
                Console.WriteLine("2 for no");
                if (Console.ReadLine().Equals("1"))
                {
                    File.WriteAllText($"_Import{FileOutputName}ItemsToLevelList.pas", BuildxEditImportScript(fileFullPath));
                    Console.WriteLine($"Please move \"_Import{FileOutputName}ItemsToLevelList.pas\" to inside your Edit Scripts folder with the file path to the absolute file path to the outputed list located on line 12 or contained inside of:");
                    Console.WriteLine("slFormList.LoadFromFile('');");
                }
                else
                {
                    Console.WriteLine("You will need to manually change line 12 of \"_ImportItemsToLevelList.pas\" to");
                    Console.WriteLine("slFormList.LoadFromFile('{Absolute File path with extention}');");
                    Console.WriteLine("Without {} surrounding it");
                }
            }
            else
            {
                File.WriteAllText($"_Import{FileOutputName}ItemsToLevelList.pas", BuildxEditImportScript(fileFullPath));
                Console.WriteLine($"Please move \"_Import{FileOutputName}ItemsToLevelList.pas\" to inside your Edit Scripts folder with the file path to the absolute file path to the outputed list located on line 12 or contained inside of:");
                Console.WriteLine("slFormList.LoadFromFile('');");
            }
            if(ReimportFile == true)
            {
                return tryGetOutputList(OrigonalListPath);
            }
            return itemList;
        }

        static string BuildxEditImportScript(string absoluteListFilePath)
        {
            StringBuilder ImportScript = new StringBuilder();
            ImportScript.Append($"unit _Import{FileOutputName}ItemsToLevelList;");
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

        static List<ItemForm> Filter(List<ItemForm> itemList, bool include)
        {
            Console.WriteLine("Input number of operation. Then press enter.");
            Console.WriteLine("1 to filter for Name.");
            Console.WriteLine("2 to filter for biped or item type.");
            string operationType = Console.ReadLine();

            Console.WriteLine("Input filter for inclusion. Then press enter.");
            string filter = Console.ReadLine();

            FileOutputName.Append(filter);
            List<ItemForm> newList = new List<ItemForm>();
            if (operationType.Equals("2"))
            {
                Console.WriteLine("Filtering by Beped or item type.");
                foreach (ItemForm itemForm in itemList)
                {
                    foreach (string bt in itemForm.BipedOrType)
                    {
                        if (bt.ToLower().Contains(filter.ToLower()) == include)
                        {
                            newList.Add(itemForm);
                        }
                    }
                    
                }
            }
            else
            {
                Console.WriteLine("Filtering by Name.");
                foreach (ItemForm itemForm in itemList)
                {
                    if (itemForm.Name.ToLower().Contains(filter.ToLower()) == include)
                    {
                        newList.Add(itemForm);
                    }
                }
            }

            if (newList.Count < 1 || newList.Count == itemList.Count)
            {
                Console.WriteLine("No forms added or removed. Returning to previous step.");
                return itemList;
            }

            return newList;
        }

        static List<ItemForm> tryGetOutputList(string filePath)
        {
            List<ItemForm> itemList = new List<ItemForm>();
            try
            {
                itemList = GetOutputList(filePath);
                OrigonalListPath = filePath;
                FileOutputName = new StringBuilder();
                return itemList;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Could not find xEdit output file.");
                Console.WriteLine("Input absolute path of xEdit output file with header row: \"FormID;Name;BipedOrType\"");
                filePath = Console.ReadLine();
                return tryGetOutputList(filePath);
            }
        }

        static List<ItemForm> GetOutputList(string filePath) 
        {
            List<ItemForm> itemList = new List<ItemForm>();
            using (var reader = new StreamReader(filePath))
            {
                string[] stringArr;
                string s = reader.ReadLine();
                Console.WriteLine(s);
                if (!s.Equals("FormID;Name;BipedOrType"))
                {
                    throw new FileNotFoundException();
                }
                while (!reader.EndOfStream)
                {
                    stringArr = reader.ReadLine().Split(';');
                    itemList.Add(new ItemForm(stringArr[0], stringArr[1], stringArr[2].Split(',').ToList()));
                }
            }
            return itemList;
        }

    }

    

    
}
