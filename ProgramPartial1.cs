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
    }
}
