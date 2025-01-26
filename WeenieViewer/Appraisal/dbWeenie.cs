using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Linq;
using WeenieViewer.Db.weenie;
using WeenieViewer.Enums;

namespace VGI_Item_Viewer.WeenieViewer.Appraisal
{
    public class dbWeenie
    {
        public Dictionary<PropertyBool, bool> Bools = new Dictionary<PropertyBool, bool>();
        public Dictionary<PropertyFloat, float> Floats = new Dictionary<PropertyFloat, float>();
        public Dictionary<PropertyDID, int> DIDs = new Dictionary<PropertyDID, int>();
        public Dictionary<PropertyIID, int> IIDs = new Dictionary<PropertyIID, int>();
        public Dictionary<PropertyInt, int> Ints = new Dictionary<PropertyInt, int>();
        public Dictionary<PropertyInt64, long> Int64s = new Dictionary<PropertyInt64, long>();
        public Dictionary<PropertyString, string> Strings = new Dictionary<PropertyString, string>();
        public List<SpellBook> SpellBook = new List<SpellBook>(); // SpellID, Probability
        public int WeenieType;
        public string WeenieClass;
        public string AppraisalText;

        public bool IsCreature()
        {
            if (Bools.ContainsKey(PropertyBool.NPC_LOOKS_LIKE_OBJECT_BOOL) && Bools[PropertyBool.NPC_LOOKS_LIKE_OBJECT_BOOL] == true)
                return false;

            if (Ints.ContainsKey(PropertyInt.ITEM_TYPE_INT) && Ints[PropertyInt.ITEM_TYPE_INT] == 16) // ItemType.Creature
                return true;


            return false;
        }

    }
}
