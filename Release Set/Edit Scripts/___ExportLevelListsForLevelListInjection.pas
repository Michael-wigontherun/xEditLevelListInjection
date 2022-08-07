unit ___ExportLevelListsForLevelListInjection;

interface
  implementation
  uses xEditAPI, Classes, SysUtils, StrUtils, Windows;

var slArmo : TStringList;
  
function Initialize: integer;
begin
  slArmo := TStringList.Create;
  slArmo.Add('FormID;Name;BipedOrType');
end;

function Process(e: IInterface): integer;
begin
  if Signature(e) <> 'LVLI' then Exit;
  slArmo.Add(Format('%s;%s;%s', [
    IntToHex(GetLoadOrderFormID(e), 8),
    GetElementEditValues(e, 'EDID - Editor ID'),
    'Level List'
  ]));
end;

procedure createOutputFolder(folderDirectory: TStringList);
var 
  i : integer;
begin
  for i := 0 to folderDirectory.Count -1 do
  begin 
    if not DirectoryExists(folderDirectory[i]) then 
    begin 
      if not CreateDir(folderDirectory[i]) then AddMessage('Could not create "' + folderDirectory[i] + '" Directory.')
      else AddMessage('Created "' + folderDirectory[i] + '" Directory.');
    end;
  end;
end;

function Finalize: integer;
var folderDirectory : TStringList;
begin
  folderDirectory := TStringList.Create;
  folderDirectory.add('xEditLevelListInjector');
  folderDirectory.add('xEditLevelListInjector\xEditOutput');
  createOutputFolder(folderDirectory);
  if not Assigned(slArmo) then
    Exit;
  if (slArmo.Count > 1) then begin
      slArmo.SaveToFile(ProgramPath+'xEditLevelListInjector\xEditOutput\LevelLists.csv');
  end;

  slArmo.Free;
  
 end;

end.