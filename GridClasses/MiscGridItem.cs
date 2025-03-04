using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGI_Item_Viewer.Enum;
using VGI_Item_Viewer.VGIItem;

namespace VGI_Item_Viewer.GridClasses
{
    public class MiscGridItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Character { get; set; }

        public string ObjectClass { get; set; }

        private int ObjectId;

        public static MiscGridItem ConvertFromItem(VGItem item)
        {
            var grid = new MiscGridItem();
            grid.ObjectId = item.ObjectId;
            grid.Name = item.GetFullName();
            grid.Character = item.CharacterName;
            if (item.IntProps.ContainsKey((int)IntValueKey.StackCount)) // STACK_SIZE_INT 
                grid.Quantity = item.IntProps[(int)IntValueKey.StackCount];
            else
                grid.Quantity = 1;

            grid.ObjectClass = item.ObjectClass.ToString(); ;

            return grid;
        }

        public int GetObjectId()
        {
            return ObjectId;
        }
    }
}
