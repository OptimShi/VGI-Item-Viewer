using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGI_Item_Viewer.VGIItem;

namespace VGI_Item_Viewer.GridClasses
{
    public class MeleeGridItem
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public string Damage_Type { get; set; }
        public int LoreReq { get; set; }
        public int Workmanship { get; set; }
        public int Tinks { get; set; }
        public float Melee_Defense { get; set; }
        public float Damage { get; set; }
        public float Attack { get; set; }
        public int WieldReq { get; set; }
        public int Cleave { get; set; }
        public string Cantrips { get; set; }

        private int ObjectId;

        public static MeleeGridItem ConvertFromItem(VGItem item)
        {
            var grid = new MeleeGridItem();
            grid.ObjectId = item.ObjectId;
            grid.Name = item.GetFullName();
            grid.Character = item.CharacterName;
            grid.Damage_Type = item.GetDamageType();
            grid.Melee_Defense = item.GetMeleeDefense();
            grid.Damage = (float)item.CalcVirindiMeleeDamage();

            grid.Attack = (float)item.GetBuffedDoubleValueKey(Enum.DoubleValueKey.AttackBonus) - 1;

            if (item.IntProps.ContainsKey(109)) // ITEM_DIFFICULTY_INT 
                grid.LoreReq = item.IntProps[109];

            if (item.IntProps.ContainsKey(171)) // NUM_TIMES_TINKERED_INT 
                grid.Tinks = item.IntProps[171];

            if (item.IntProps.ContainsKey(105)) // ITEM_WORKMANSHIP_INT 
                grid.Workmanship = item.IntProps[105];

            if (item.IntProps.ContainsKey(160)) // WIELD_DIFFICULTY_INT 
                grid.WieldReq = item.IntProps[160];


            grid.Cantrips = item.GetCantrips();


            return grid;
        }

        public int GetObjectId()
        {
            return ObjectId;
        }
    }
}
