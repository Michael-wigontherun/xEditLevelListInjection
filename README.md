# xEditLevelListInjection

Requires 
  - SSEEdit
    - https://www.nexusmods.com/skyrimspecialedition/mods/164
    - Might work with Fallout 4 xEdit
    
  - mteFunctions.pas
    - https://github.com/matortheeternal/TES5EditScripts 
    - download full package or just mteFunctions.pas
    
  - Add Items To Leveled List.pas 
    - https://gist.github.com/matortheeternal/91e77ef306242137184dcfc1b9631669
    - download in the top right of page
    
  - .Net 3.1 Core runtime or desktop runtime
    - https://dotnet.microsoft.com/en-us/download/dotnet/3.1

Install
  - Drag and drop release package into xEdit folder, SSEEdit works unsure about Fallout 4

Instructions

- 1. Install Requirments
- 2. Open xEdit go to mod you wish to export the armor for filtering. Apply Script "_ExportArmorForLevelListInjection.pas", let it finish.
- 3. Do not close xEdit or do not change load order before ready to re-run xEdit
- 4. Run "xEditLevelListInjection.exe" to filter the list outputed by the previous script. Following the Console application's menu's
  - Menu 1, options
    - 1 filters for keyword and returns the list with out forms not matching the keyword, you can run this multiple times to remove more and more forms form list
    - 2 filters for keyword exclusion and returns the list with out forms matching the keyword, you can run this multiple times to remove more and more forms from list
    - 3 outputs the list you have filtered to new file using the list you crafted from including and excluding filters
    - 4 re-imports the origonal outputed list from xEdit
	- 5 get different list. This will ask to input the absolute path to one of the different outputed files
    - 6 writes current list to console format writen, FormID, Biped or Type, Name of item
    - 7 closes the progam
  - Menu 2, from options 1 and 2 from Menu 1, they both use the same menu
    - 1 filters by Name of item, then asks you for the keyword
    - 2 filters from biped or item type, then asks you for the keyword
  - Menu 3, After outputing the list it will ask if you want to generate the script
    - 1 will generate the xEdit script to run in xEdit in step 5 of instructions, 
      generates in same folder as "xEditLevelListInjection.exe", copy this over to your xEdit Edit Scripts folder
    - 2 will not generate the xEdit script. You will need to manualy set the absolute file path to the outputed formList in the script "_ImportItemsToLevelList.pas"
      On line 12 set/change "slFormList.LoadFromFile(ProgramPath+'\xEditLevelListInjector\xEditLevelListInjectorOuput\LevelList.txt');" to
        "slFormList.LoadFromFile('{abslolute path to file with extention}');" with '' and without {}
  - Option 4 from Menu 1 does not have a menu
  - Option 5 only outputs the list so you can view this will be a long list depending on the size you started with and how many records you have already filtered out
  - Option 6 closes the functionaly the same as clicking the X button on the window
- 5. Select the level list you wish to import into inside of xEdit and run the new script or the script you edited in xEdit
  - If you manualy set it the script is called "_ImportItemsToLevelList.pas" without ""
  - If you had this output the file it will be called "_Import{string of words put together from kewords}ItemsToLevelList.pas" without {} or ""
  - Important Note: Do not run this ontop of a full plugin unless there is only one level list inside of it
  - Important Note: Running this on anything other then a level list will crash
  - Important Note: Running this on multiple level list will result in importing the list into the first one it xEdit grabs
- 6. Verify correct import and delete items from level list you missed when filtering

Arguments:
- first argument is always path to xEdit output file. Example: ".\xEditOutput\Armor.csv" xEditLevelListInjector is in xEditLevelListInjector folder inside xEdit folder.
- -outputScript will Ouput the xEditScript to import list with no conformation.
- -reimport will re-import origonal xEdit output after exporting the filtered list.

	Note: the first argument can be changed to get any of the 3 to start: 
	- LevelList.csv is for outputed level lists
	- Weapons.csv is for outputed weapons
	- Armor.csv is for outputed armor

Extra details:

- The reason I did not include mteFunctions.pas or "Add Items To Leveled List.pas", was I dont know if there open source, also mteFunctions.pas has tones of useful meathods

- "Add Items To Leveled List.pas" also imports items into a level list but it will import all items it runs over to a selected level list. 
  so use this for mods that have a single armor set
