unit _ImportItemsToLevelList;

interface
  implementation
    uses xEditAPI, 'Add Items To Leveled List', mteFunctions, Classes, SysUtils, StrUtils, Windows;
var  
    slFormList: TStringList;

function Initialize: integer;
begin 
  slFormList := TStringList.create;
  slFormList.LoadFromFile('\xEditLevelListInjector\xEditLevelListInjectorOuput\LevelList.txt');
end;

function Process(e: IInterface): integer;
begin 
  CreateTransferFormList(e);
end;

function Finalize: integer;
begin 
end;
end.