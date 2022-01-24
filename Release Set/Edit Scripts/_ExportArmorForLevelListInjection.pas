unit _ExportArmorForLevelListInjection;

interface
  implementation
  uses xEditAPI, Classes, SysUtils, StrUtils, Windows;

var slArmo : TStringList;
  
function Initialize: integer;
begin
  slArmo := TStringList.Create;
  slArmo.Add('FormID;Name;BipedOrType');
end;

function BipedTypes(e: IInterface) : string;
var 
  i : integer;
  BipedList : string;
begin
  if ElementCount(ElementByPath(e, 'BOD2 - Biped Body Template\First Person Flags')) = 1 then begin
    Result := BaseName(ElementByIndex(ElementByPath(e, 'BOD2 - Biped Body Template\First Person Flags'), 0));
    Exit;
  end;

  if ElementCount(ElementByPath(e, 'BOD2 - Biped Body Template\First Person Flags')) > 1 then begin
    BipedList := BaseName(ElementByIndex(ElementByPath(e, 'BOD2 - Biped Body Template\First Person Flags'), 0));
    for i := 1 to ElementCount(ElementByPath(e, 'BOD2 - Biped Body Template\First Person Flags')) - 1 do
    begin
      BipedList := BipedList + ',' + BaseName(ElementByIndex(ElementByPath(e, 'BOD2 - Biped Body Template\First Person Flags'), i));
    end;
    Result := BipedList;
    Exit;
  end;

  Result := 'Invis';
End;

function Process(e: IInterface): integer;
begin
  if Signature(e) <> 'ARMO' then Exit;
  slArmo.Add(Format('%s;%s;%s;%s', [
    BaseName(GetFile(e)),
    IntToHex(GetLoadOrderFormID(e), 8),
    GetElementEditValues(WinningOverride(e), 'FULL - Name'),
    BipedTypes(e)
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
      slArmo.SaveToFile(ProgramPath+'xEditLevelListInjector\xEditOutput\Armor.csv');
  end;

  slArmo.Free;
  
 end;

end.