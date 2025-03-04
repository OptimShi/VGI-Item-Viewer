using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGI_Item_Viewer.VGIItem;

namespace VGI_Item_Viewer.GridClasses
{
    public class ArmorGridItem
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public int Armor { get; set; }
        public int LoreReq { get; set; }
        public int Workmanship { get; set; }
        public int Tinks { get; set; }
        public int WieldReq { get; set; }

        private int ObjectId;

        public static ArmorGridItem ConvertFromItem(VGItem item)
        {
            var grid = new ArmorGridItem();
            grid.ObjectId = item.ObjectId;
            grid.Name = item.GetFullName();
            grid.Character = item.CharacterName;

            if (item.IntProps.ContainsKey(28)) // ARMOR_LEVEL_INT 
                grid.Armor = item.IntProps[28];

            if (item.IntProps.ContainsKey(109)) // ITEM_DIFFICULTY_INT 
                grid.LoreReq = item.IntProps[109];

            if (item.IntProps.ContainsKey(171)) // NUM_TIMES_TINKERED_INT 
                grid.Tinks = item.IntProps[171];

            if (item.IntProps.ContainsKey(105)) // ITEM_WORKMANSHIP_INT 
                grid.Workmanship = item.IntProps[105];

            if (item.IntProps.ContainsKey(160)) // WIELD_DIFFICULTY_INT 
                grid.WieldReq = item.IntProps[160];

            return grid;
        }
        public int GetObjectId()
        {
            return ObjectId;
        }

    }
}
