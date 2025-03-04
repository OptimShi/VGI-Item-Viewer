using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGI_Item_Viewer.VGIItem;

namespace VGI_Item_Viewer.GridClasses
{
    public class PetGridItem
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public int Level { get; set; }
        public string Mastery { get; set; }
        public string Element { get; set; }
        public double Damage { get; set; }
        public int TotalRatings { get; set; }
        public int OffRatings { get; set; }
        public int DefRatings { get; set; }
        public int Dmg { get; set; }
        public int Crit { get; set; }
        public int CritDmg { get; set; }
        public int Res { get; set; }
        public int CritRes { get; set; }
        public int CritDmgRes { get; set; }

        private int ObjectId;

        public void CalcDamage()
        {
            float DPS = (100 + 25) / 2 * (1 + (float)Dmg / 100) * (1 - ((float)Crit + 10) / 100) + 100 * 2 * ((100 + (float)Dmg + CritDmg) / 100) * (((float)Crit + 10) / 100);
            Damage = DPS / 136.5;
        }
        public static PetGridItem ConvertFromItem(VGItem item)
        {
            var grid = new PetGridItem();
            grid.ObjectId = item.ObjectId;
            grid.Name = item.GetFullName();
            grid.Character = item.CharacterName;
            grid.Level = item.GetPetLevel();
            grid.Mastery = item.GetPetMastery();
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

            if (grid.Dmg > 0)
            {
                var stop = 0;
            }
            grid.CalcDamage();

            return grid;
        }

        public int GetObjectId()
        {
            return ObjectId;
        }

    }
}
