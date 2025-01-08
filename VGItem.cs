using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGI_Item_Viewer.Enum;

namespace VGI_Item_Viewer
{
    public class VGItem
    {

        public Dictionary<int, int> IntProps = new Dictionary<int, int>();
        public Dictionary<int, string> StrProps = new Dictionary<int, string>();
        public Dictionary<int, int> DidProps = new Dictionary<int, int>();
        public Dictionary<int, Double> FloatProps = new Dictionary<int, Double>();
        public Dictionary<int, int> IidProps = new Dictionary<int, int>();
        public List<int> Spells = new List<int>();
        public int WCID = 0;
        public ItemType ItemType;
        public string ItemName;
        public string CharacterName;

        public VGItem LoadFromBlob(byte[] BLOB) 
        {
            VGItem item = new VGItem();

            using (var reader = new BinaryReader(new MemoryStream(BLOB)))
            {
                var intVals = reader.ReadInt32();
                for (int i = 0; i < intVals; i++)
                {
                    var key = (reader.ReadInt32());
                    var value = reader.ReadInt32();
                    item.IntProps.Add(key, value);
                }

                var strVals = reader.ReadInt32();
                for (int i = 0; i < strVals; i++)
                {
                    var key = reader.ReadInt32();
                    var val = reader.ReadString();
                    item.StrProps.Add(key, val);
                }

                var boolVals = reader.ReadInt32();
                for (int i = 0; i < boolVals; i++)
                {
                    var key = reader.ReadInt32();
                    var val = reader.ReadByte();
                }

                var floatVals = reader.ReadInt32();
                for (int i = 0; i < floatVals; i++)
                {
                    var key = reader.ReadInt32();
                    var val = reader.ReadDouble();
                    item.FloatProps.Add(key, val);
                }

                var int64Vals = reader.ReadInt32();
                for (int i = 0; i < int64Vals; i++)
                {
                    var key = reader.ReadInt32();
                    var val = reader.ReadInt64();
                }

                var spellCount = reader.ReadInt32();
                for (int i = 0; i < spellCount; i++)
                {
                    var spell = reader.ReadInt32();
                    item.Spells.Add(spell);
                }

            }

            item.CleanUpKeys();

            return item;
        }

        public int GetPetLevel(uint icon_overlay)
        {
            switch (icon_overlay)
            {
                case 0x0600742D: return 15; // Mud Golem
                case 0x0600742E: return 30; // Sandstone Golem
                case 0x06007422: return 50;
                case 0x06007423: return 80;
                case 0x06007424: return 100;
                case 0x06007425: return 125;
                case 0x06007426: return 150;
                case 0x06007427: return 180;
                case 0x06007428: return 200;
            }

            return 0;
        }
        
        private void CleanUpKeys()
        {
            try
            {
                UpdateIntProps();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            List<int> keys = new List<int>(FloatProps.Keys);
            foreach (var k in keys)
            {
                var newKey = GetFloatKey(k);
                if (k != newKey)
                {
                    FloatProps.Add(newKey, FloatProps[k]);
                    FloatProps.Remove(k);
                }
            }
            if (FloatProps.ContainsKey(0x0A000009)){
                IntProps.Add((int)PropertyInt.ItemWorkmanship, (int)FloatProps[0x0A000009]);
                FloatProps.Remove(0x0A000009);
            }

            foreach (var e in FloatProps)
            {
                if (e.Key > 1000)
                {
                    var STOP = true;
                }
            }


            foreach (var e in IntProps)
            {
                if(e.Key > 1000)
                {
                    var STOP = true;
                }
            }


        }
        private void UpdateIntProps()
        {
            List<int> keys = new List<int>(IntProps.Keys);
            foreach (var k in keys)
            {
                switch (k)
                {
                    /* HEADER INFO */
                    case 0x0d000000:
                        {
                            WCID = IntProps[k];
                            IntProps.Remove(k);
                        }
                        break;
                    case 0xD00001B: //physicsDescriptionFlag
                    case 0xD000017: //aceObjectDescriptionFlags
                    case 0xD000018: //weenieHeaderFlags
                    case 0xD000019: //weenieHeaderFlags2
                    case 0xD000027: //physicsDescriptionFlag
                        {
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D00001A: // ITEM_TYPE_INT
                        {
                            IntProps.Add((int)PropertyInt.ItemType, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D00000D: // CLOTHING_PRIORITY_INT
                        {
                            IntProps.Add((int)PropertyInt.ClothingPriority, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D000004: // ITEMS_CAPACITY_INT
                        {
                            IntProps.Add((int)PropertyInt.ItemsCapacity, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D000005: // CONTAINERS_CAPACITY_INT
                        {
                            IntProps.Add((int)PropertyInt.ContainersCapacity, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D000024: // RADARBLIP_COLOR_INT
                        {
                            IntProps.Add((int)PropertyInt.RadarBlipColor, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D00000E: // LOCATIONS_INT
                        {
                            IntProps.Add((int)PropertyInt.ValidLocations, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D00000B: // CURRENT_WIELDED_LOCATION_INT
                        {
                            IntProps.Add((int)PropertyInt.CurrentWieldedLocation, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D000006: // STACK_SIZE_INT
                        {
                            IntProps.Add((int)PropertyInt.StackSize, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D000007: // MAX_STACK_SIZE_INT
                        {
                            IntProps.Add((int)PropertyInt.MaxStackSize, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d000009: // PLACEMENT_POSITION_INT 
                        {
                            IntProps.Add((int)PropertyInt.PlacementPosition, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d00000f: // COMBAT_USE_INT
                        {
                            IntProps.Add((int)PropertyInt.CombatUse, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d000010: // UI_EFFECTS_INT 
                        {
                            IntProps.Add((int)PropertyInt.UiEffects, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d000011: // AMMO_TYPE_INT 
                        {
                            IntProps.Add((int)PropertyInt.AmmoType, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d000012: // TARGET_TYPE_INT 
                        {
                            IntProps.Add((int)PropertyInt.TargetType, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d000014: // HOOK_TYPE_INT
                        {
                            IntProps.Add((int)PropertyInt.HookType, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d00001F: // WEAPON_TIME_INT
                        {
                            IntProps.Add((int)PropertyInt.WeaponTime, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d000020: // WEAPON_SKILL_INT
                        {
                            IntProps.Add((int)PropertyInt.WeaponSkill, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d000021: // DAMAGE_TYPE_INT
                        {
                            IntProps.Add((int)PropertyInt.DamageType, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d000022: // DAMAGE_INT
                        {
                            IntProps.Add((int)PropertyInt.Damage, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d000023: // ITEM_USABLE_INT
                        {
                            IntProps.Add((int)PropertyInt.ItemUseable, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    /*
                case 0x0A000009: // ITEM_WORKMANSHIP_INT
                    {
                        IntProps.Add((int)PropertyInt.ItemWorkmanship, IntProps[k]);
                        IntProps.Remove(k);
                    }
                    break;
                    */
                    /* DID Values */
                    case 0xD000016:
                        {
                            DidProps.Add((int)PropertyDataId.Setup, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D000001:
                        {
                            DidProps.Add((int)PropertyDataId.Icon, 0x06000000 + IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0D000008:
                        {
                            DidProps.Add((int)PropertyDataId.Spell, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0xd000029:
                        {
                            DidProps.Add((int)PropertyDataId.IconOverlay, 0x06000000 + IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0xd00002A:
                        {
                            DidProps.Add((int)PropertyDataId.IconUnderlay, 0x06000000 + IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    /* IID Props */
                    case 0x0d000002:
                        {
                            IidProps.Add((int)PropertyInstanceId.Owner, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                    case 0x0d00002b:
                        {
                            IidProps.Add((int)PropertyInstanceId.Container, IntProps[k]);
                            IntProps.Remove(k);
                        }
                        break;
                }
            }
        }
        /*
         * Converts Decal's Int keys values into correct server properties keys. Makes it easier to work with.
         */
        private int GetIntKey(int key)
        {
            return key;
        }

        private int GetDidKey(int key)
        {
            return key;
        }
        private int GetFloatKey(int key)
        {
            switch (key)
            {
                case 0x0A000000: return 13; //ARMOR_MOD_VS_SLASH_FLOAT 	
                case 0x0A000001: return 14; //ARMOR_MOD_VS_PIERCE_FLOAT 
                case 0x0A000002: return 15; //ARMOR_MOD_VS_BLUDGEON_FLOAT 
                case 0x0A000003: return 18; //ARMOR_MOD_VS_ACID_FLOAT 	
                case 0x0A000004: return 19; //ARMOR_MOD_VS_ELECTRIC_FLOAT 
                case 0x0A000005: return 17; //ARMOR_MOD_VS_FIRE_FLOAT 	
                case 0x0A000006: return 16; //ARMOR_MOD_VS_COLD_FLOAT 	
                case 0x0A000008: return 54; //USE_RADIUS_FLOAT 
                // case 167772167: "Heading";
                // case 167772169: "SalvageWorkmanship";
                case 0x0A00000A: return 39; //DEFAULT_SCALE_FLOAT 
                case 0x0A00000B: return 22;//DAMAGE_VARIANCE_FLOAT
                case 0x0A00000C: return 62; // WEAPON_OFFENSE_FLOAT 
                case 0x0A00000D: return 26; //MAXIMUM_VELOCITY_FLOAT 
                case 0x0A00000E: return 63; //DAMAGE_MOD_FLOAT 
            }

            return key;
        }
    }
}
