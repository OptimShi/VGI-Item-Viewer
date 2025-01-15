using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VGI_Item_Viewer
{
    public class MagicGridItem
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public int LoreReq { get; set; }
        public int Workmanship { get; set; }
        public int Tinks { get; set; }
        public int Melee_Defense { get; set; }
        public int Damage { get; set; }
        public int Magic_Defense { get; set; }
        public int WieldReq { get; set; }
        public string Cantrips { get; set; }

        public static MagicGridItem ConvertFromItem(VGItem item)
        {
            var grid = new MagicGridItem();
            grid.Name = item.GetFullName();
            grid.Character = item.CharacterName;
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

            if (grid.Name == "Yellow Topaz Piercing Baton")
            {
                var stop = 1;
            }

            return grid;
        }
    }

    public class MissileGridItem
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public string Damage_Type { get; set; }
        public int LoreReq { get; set; }
        public int Workmanship { get; set; }
        public int Tinks { get; set; }
        public int Melee_Defense { get; set; }
        public int Damage { get; set; }
        public int Magic_Defense { get; set; }
        public int WieldReq { get; set; }
        public string Cantrips { get; set; }

        public static MissileGridItem ConvertFromItem(VGItem item)
        {
            var grid = new MissileGridItem();
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
    }

    public class MeleeGridItem
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public string Damage_Type { get; set; }
        public int LoreReq { get; set; }
        public int Workmanship { get; set; }
        public int Tinks { get; set; }
        public int Melee_Defense { get; set; }
        public int Damage { get; set; }
        public int Magic_Defense { get; set; }
        public int WieldReq { get; set; }
        public string Cantrips { get; set; }

        public static MeleeGridItem ConvertFromItem(VGItem item)
        {
            var grid = new MeleeGridItem();
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
    }

    public class PetGridItem
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public int Level { get; set; }
        public string Element { get; set; }
        public int TotalRatings { get; set; }
        public int OffRatings { get; set; }
        public int DefRatings { get; set; }
        public double Damage { get; set; }
        public int Dmg { get; set; }
        public int Crit { get; set; }
        public int CritDmg { get; set; }
        public int Res { get; set; }
        public int CritRes { get; set; }
        public int CritDmgRes { get; set; }

        public void CalcDamage()
        {
            float DPS= ((100 + 25) / 2 * (1 + ((float)Dmg / 100)) * (1 - (((float)Crit + 10) / 100))) + (100 * 2 * ((100 + (float)Dmg + (float)CritDmg) / 100) * (((float)Crit + 10) / 100));
            Damage = (DPS / 136.5) * 100;
        }
        public static PetGridItem ConvertFromItem(VGItem item)
        {
            var grid = new PetGridItem();
            grid.Name = item.GetFullName();
            grid.Character = item.CharacterName;
            grid.Level = item.GetPetLevel();
            grid.Element = item.GetPetDamageType();
            if (item.IntProps.ContainsKey(370))
                grid.Dmg = item.IntProps[370];
            if (item.IntProps.ContainsKey(371))
                grid.Res = item.IntProps[371];

            if (item.IntProps.ContainsKey(372))
                grid.Crit = item.IntProps[372];
            if (item.IntProps.ContainsKey(374))
                grid.CritDmg = item.IntProps[374];
            if (item.IntProps.ContainsKey(373))
                grid.CritRes = item.IntProps[373];
            if (item.IntProps.ContainsKey(375))
                grid.CritDmgRes = item.IntProps[375];

            grid.OffRatings = grid.Dmg + grid.Crit + grid.CritDmg;
            grid.DefRatings = grid.Res + grid.CritRes + grid.CritDmgRes;
            grid.TotalRatings = grid.OffRatings + grid.DefRatings;

            if(grid.Dmg> 0)
            {
                var stop = 0;
            }
            grid.CalcDamage();

            return grid;
        }
    }

}
