using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGI_Item_Viewer.VGIItem;

namespace VGI_Item_Viewer.GridClasses
{
    public class MissileGridItem
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public string Damage_Type { get; set; }
        public int LoreReq { get; set; }
        public int Workmanship { get; set; }
        public int Tinks { get; set; }
        public float Melee_Defense { get; set; }
        public float Damage { get; set; }
        public float Magic_Defense { get; set; }
        public int WieldReq { get; set; }
        public string Cantrips { get; set; }

        private int ObjectId;

        public static MissileGridItem ConvertFromItem(VGItem item)
        {
            var grid = new MissileGridItem();
            grid.ObjectId = item.ObjectId;
            grid.Name = item.GetFullName();
            grid.Character = item.CharacterName;
            grid.Damage_Type = item.GetDamageType();
            grid.Melee_Defense = item.GetMeleeDefense();
            grid.Damage = item.GetDamage();

            if (item.IntProps.ContainsKey(109)) // ITEM_DIFFICULTY_INT 
                grid.LoreReq = item.IntProps[109];

            if (item.IntProps.ContainsKey(171)) // NUM_TIMES_TINKERED_INT 
                grid.Tinks = item.IntProps[171];

            if (item.IntProps.ContainsKey(105)) // ITEM_WORKMANSHIP_INT 
                grid.Workmanship = item.IntProps[105];

            if (item.IntProps.ContainsKey(160)) // WIELD_DIFFICULTY_INT 
                grid.WieldReq = item.IntProps[160];

            if (item.FloatProps.ContainsKey(150)) // WEAPON_MAGIC_DEFENSE_FLOAT  
            {
                grid.Magic_Defense = (int)((item.FloatProps[150] - 1) * 100);
            }

            grid.Cantrips = item.GetCantrips();


            return grid;
        }

        public int GetObjectId()
        {
            return ObjectId;
        }
    }

}
