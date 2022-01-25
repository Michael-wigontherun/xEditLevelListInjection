using System;
using System.Collections.Generic;
using System.Text;

namespace xEditLevelListInjection
{
    public class ItemForm
    {
        public string FormID { get; set; }
        public string Name { get; set; }
        public List<string> BipedOrType { get; set; }

        public ItemForm(string formID, string name, List<string> bipedOrType)
        {
            FormID = formID;
            Name = name;
            BipedOrType = bipedOrType;
        }

        public new string ToString()
        {
            string formatbt = BipedOrType[0];
            for (int i = 1; i < BipedOrType.Count; i++)
            {
                formatbt += ", " + BipedOrType[i];
            }
            return $"{FormID} {formatbt} {Name}";
        }
    }
}
