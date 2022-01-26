unit ___ExportWeaponsForLevelListInjection;

interface
  implementation
  uses xEditAPI, Classes, SysUtils, StrUtils, Windows;

var slArmo : TStringList;
  
function Initialize: integer;
begin
  slArmo := TStringList.Create;
  slArmo.Add('FormID;Name;BipedOrType');
end;

function WeaponType(e: IInterface) : string;
var 
  i : integer;
  keyword : string;
begin
  Result := 'Other / Keywords missing';
  if GetElementEditValues(e, 'KSIZ') = '' then Exit;
  for i := 0 to StrToInt(GetElementEditValues(e, 'KSIZ')) do
  begin
    keyword := GetEditValue(ElementByIndex(ElementByPath(e, 'KWDA'), i));
    if copy(keyword, 0, 8) = 'WeapType' then
    begin
      i := pos(' [', keyword) - 9;
      Result := copy(keyword, 9, i);
    end;
  end;
End;

function Process(e: IInterface): integer;
begin
  if Signature(e) <> 'WEAP' then Exit;
  slArmo.Add(Format('%s;%s;%s', [
    IntToHex(GetLoadOrderFormID(e), 8),
    GetElementEditValues(WinningOverride(e), 'FULL - Name'),
    WeaponType(e)
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
      slArmo.SaveToFile(ProgramPath+'xEditLevelListInjector\xEditOutput\Weapons.csv');
  end;

  slArmo.Free;
  
 end;

end.