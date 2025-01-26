﻿using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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
        public int ObjectId;

        public void LoadFromBlob(byte[] BLOB) 
        {
            using (var reader = new BinaryReader(new MemoryStream(BLOB)))
            {
                var intVals = reader.ReadInt32();
                for (int i = 0; i < intVals; i++)
                {
                    var key = (reader.ReadInt32());
                    var value = reader.ReadInt32();
                    IntProps.Add(key, value);
                }

                var strVals = reader.ReadInt32();
                for (int i = 0; i < strVals; i++)
                {
                    var key = reader.ReadInt32();
                    var val = reader.ReadString();
                    StrProps.Add(key, val);
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
                    FloatProps.Add(key, val);
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
                    Spells.Add(spell);
                }

            }

            // item.CleanUpKeys();

//            return item;
        }

        public string GetFullName()
        {
            string name = ItemName;
            if (IntProps.ContainsKey(131)) {
                string matName = GetMaterialName(IntProps[131]);
                if(matName != "")
                {
                    name = matName + " " + name;
                }
            }

            return name;
        }

        public int GetPetLevel()
        {
            if (IntProps.ContainsKey(0xd000029))
            {
                int icon_overlay = 0x06000000 + IntProps[0xd000029];
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
            }

            return 0;
        }

        public string GetPetDamageType()
        {
            if (IntProps.ContainsKey(0x0d000010)) // UI_EFFECTS_INT
            {
                switch (IntProps[0x0d000010])
                {
                    case 1: return "Bludgeon"; break;
                    case 32: return "Fire"; break;
                    case 64: return "Lightning"; break;
                    case 128: return "Frost"; break;
                    case 256: return "Acid"; break;
                }
            }

            return "Unknown";

        }

        public int GetMeleeDefense()
        {
            int defense = 0;
            if (FloatProps.ContainsKey(29)) // WEAPON_DEFENSE_FLOAT 
            {
                defense = (int)((FloatProps[29] - 1) * 100);
            }

            int cantripMod = 0;
            foreach (var s in Spells)
            {
                switch (s)
                {
                    case 2600: if (cantripMod < 3) cantripMod = 3; break; // Minor Defender
                    case 3985: if (cantripMod < 4) cantripMod = 4; break; // Mukkir Sense
                    case 2588: if (cantripMod < 5) cantripMod = 5; break; // Major Defender
                    case 4663: if (cantripMod < 7) cantripMod = 7; break; // Epic Defender
                    case 2488: if (cantripMod < 8) cantripMod = 8; break; // Weapon Familiarity
                    case 6091: if (cantripMod < 9) cantripMod = 9; break; // Legendary Defender
                }
            }

            return defense + cantripMod;
        }

        public int GetDamage()
        {
            int attack = 0;
            if (FloatProps.ContainsKey(62)) // WEAPON_OFFENSE_FLOAT 
                attack = (int)((FloatProps[62] - 1) * 100);
            if (FloatProps.ContainsKey(152)) // ELEMENTAL_DAMAGE_MOD_FLOAT  
                attack = (int)((FloatProps[152] - 1) * 100);
            if (IntProps.ContainsKey(0x0d000022)) // DAMAGE_INT  
                attack = (int)IntProps[0x0d000022];

            int cantripMod = 0;
            foreach (var s in Spells)
            {
                switch (s)
                {
                    // Magic
                    case 3251: if (cantripMod < 1) cantripMod = 1; break; // Minor Spirit Thirst
                    case 6035: if (cantripMod < 1) cantripMod = 1; break; // Spirit of Izexi
                    case 3252: if (cantripMod < 2) cantripMod = 2; break; // Spirit Thirst
                    case 3250: if (cantripMod < 3) cantripMod = 3; break; // Major Spirit Thirst
                    case 4670: if (cantripMod < 5) cantripMod = 5; break; // Epic Spirit Thirst
                    case 6098: if (cantripMod < 7) cantripMod = 7; break; // Legendary Spirit Thirst                }

                    // Melee/Missile
                    case 2453: if (cantripMod < 2) cantripMod = 2; break; // Lesser Thorns
                    case 2486: if (cantripMod < 2) cantripMod = 2; break; // Blood Thirst
                    case 2487: if (cantripMod < 2) cantripMod = 2; break; // Spirit Strike
                    case 2598: if (cantripMod < 2) cantripMod = 2; break; // Minor Blood Thirst
                    case 3828: if (cantripMod < 3) cantripMod = 3; break; // Rage of Grael
                    case 2454: if (cantripMod < 4) cantripMod = 4; break; // Thorns
                    case 2586: if (cantripMod < 4) cantripMod = 4; break; // Major Blood Thirst
                    case 2629: if (cantripMod < 5) cantripMod = 5; break; // Huntress' Boon
                    case 4661: if (cantripMod < 7) cantripMod = 7; break; // Epic Blood Thirst
                    case 6089: if (cantripMod < 10) cantripMod = 10; break; // Legendary Blood Thirst
                    case 2452: if (cantripMod < 6) cantripMod = 6; break; // Greater Thorns
                }
            }

            return attack + cantripMod;
        }

        public string GetCantrips()
        {
            List<string> cantrips = new List<string>();
            foreach (var s in Spells)
            {
                switch (s)
                {
                    case 3251: cantrips.Add("Minor Spirit Thirst"); break;
                    case 6035: cantrips.Add("Spirit of Izexi"); break;
                    case 2453: cantrips.Add("Lesser Thorns"); break;
                    case 2486: cantrips.Add("Blood Thirst"); break;
                    case 2487: cantrips.Add("Spirit Strike"); break;
                    case 2598: cantrips.Add("Minor Blood Thirst"); break;
                    case 3252: cantrips.Add("Spirit Thirst"); break;
                    case 3250: cantrips.Add("Major Spirit Thirst"); break;
                    case 3828: cantrips.Add("Rage of Grael"); break;
                    case 2454: cantrips.Add("Thorns"); break;
                    case 2586: cantrips.Add("Major Blood Thirst"); break;
                    case 4670: cantrips.Add("Epic Spirit Thirst"); break;
                    case 2629: cantrips.Add("Huntress' Boon"); break;
                    case 4661: cantrips.Add("Epic Blood Thirst"); break;
                    case 6098: cantrips.Add("Legendary Spirit Thirst"); break;
                    case 6089: cantrips.Add("Legendary Blood Thirst"); break;
                    case 2452: cantrips.Add("Greater Thorns"); break;
                    case 2600: cantrips.Add("Minor Defender"); break;
                    case 3985: cantrips.Add("Mukkir Sense"); break;
                    case 2588: cantrips.Add("Major Defender"); break;
                    case 4663: cantrips.Add("Epic Defender"); break;
                    case 2488: cantrips.Add("Weapon Familiarity"); break;
                    case 6091: cantrips.Add("Legendary Defender"); break;
                }
            }

            return String.Join(",", cantrips);

        }

        public string GetDamageType()
        {
            if (IntProps.ContainsKey(45)) // DAMAGE_TYPE_INT  
            {
                return DamageTypeExtensions.GetDamageTypes((DamageType)IntProps[45]);
            }

            return "";
        }

        /*
         * Re-arranges the keys and properties by adjusting the Decal values into the proper server specific values
         */
        public void CleanUpKeys()
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
            if (FloatProps.ContainsKey(0x0A000009) && !IntProps.ContainsKey((int)PropertyInt.ItemWorkmanship)){
                IntProps.Add((int)PropertyInt.ItemWorkmanship, (int)FloatProps[0x0A000009]);
                FloatProps.Remove(0x0A000009);
            }

            
            // Debug functions to check for Props that fall out of bounds of server properties
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

        private static string GetMaterialName(int MaterialID)
        {
            switch (MaterialID)
            {
                case 1: return "Ceramic";
                case 2: return "Porcelain";
                case 3: return "Cloth";
                case 4: return "Linen";
                case 5: return "Satin";
                case 6: return "Silk";
                case 7: return "Velvet";
                case 8: return "Wool";
                case 9: return "Gem";
                case 10: return "Agate";
                case 11: return "Amber";
                case 12: return "Amethyst";
                case 13: return "Aquamarine";
                case 14: return "Azurite";
                case 15: return "Black Garnet";
                case 16: return "Black Opal";
                case 17: return "Bloodstone";
                case 18: return "Carnelian";
                case 19: return "Citrine";
                case 20: return "Diamond";
                case 21: return "Emerald";
                case 22: return "Fire Opal";
                case 23: return "Green Garnet";
                case 24: return "Green Jade";
                case 25: return "Hematite";
                case 26: return "Imperial Topaz";
                case 27: return "Jet";
                case 28: return "Lapis Lazuli";
                case 29: return "Lavender Jade";
                case 30: return "Malachite";
                case 31: return "Moonstone";
                case 32: return "Onyx";
                case 33: return "Opal";
                case 34: return "Peridot";
                case 35: return "Red Garnet";
                case 36: return "Red Jade";
                case 37: return "Rose Quartz";
                case 38: return "Ruby";
                case 39: return "Sapphire";
                case 40: return "Smoky Quartz";
                case 41: return "Sunstone";
                case 42: return "Tiger Eye";
                case 43: return "Tourmaline";
                case 44: return "Turquoise";
                case 45: return "White Jade";
                case 46: return "White Quartz";
                case 47: return "White Sapphire";
                case 48: return "Yellow  Garnet";
                case 49: return "Yellow Topaz";
                case 50: return "Zircon";
                case 51: return "Ivory";
                case 52: return "Leather";
                case 53: return "Armoredillo Hide";
                case 54: return "Gromnie Hide";
                case 55: return "Reed Shark Hide";
                case 56: return "Metal";
                case 57: return "Brass";
                case 58: return "Bronze";
                case 59: return "Copper";
                case 60: return "Gold";
                case 61: return "Iron";
                case 62: return "Pyreal";
                case 63: return "Silver";
                case 64: return "Steel";
                case 65: return "Stone";
                case 66: return "Alabaster";
                case 67: return "Granite";
                case 68: return "Marble";
                case 69: return "Obsidian";
                case 70: return "Sandstone";
                case 71: return "Serpentine";
                case 72: return "Wood";
                case 73: return "Ebony";
                case 74: return "Mahogany";
                case 75: return "Oak";
                case 76: return "Pine";
                case 77: return "Teak";
            }

            return "";
        }
    }
}
