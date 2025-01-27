using Microsoft.Data.Sqlite;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;
using VGI_Item_Viewer.WeenieViewer.Appraisal;
using WeenieViewer.Db.weenie;
using WeenieViewer.Enums;

namespace VGI_Item_Viewer
{

    public class VGI
    {
        DataTable dt = new DataTable();
        SqliteConnection sqlite;

        public Dictionary<int, VGItem> MagicItems = new Dictionary<int, VGItem>();
        public Dictionary<int, VGItem> Melee = new Dictionary<int, VGItem>();
        public Dictionary<int, VGItem> Missile = new Dictionary<int, VGItem>();
        public Dictionary<int, VGItem> Pets = new Dictionary<int, VGItem>();

        public VGI(string dbName = "")
        {
            SqliteCommand cmd;

#if DEBUG
            if (dbName == "")
            {
                dbName = "D:\\Games\\Decal\\VirindiPlugins\\VirindiGlobalInventory\\_Leafcull.db"; // For Testing Purposes
            }
#endif

            sqlite = new SqliteConnection("Data Source="+dbName);
            sqlite.Open();
            cmd = sqlite.CreateCommand();

            // Let's count our rows
            string countQuery = "SELECT count(rowid) FROM ObjectData";
            cmd.CommandText = countQuery;
            int RowCount = 0;
            RowCount = Convert.ToInt32(cmd.ExecuteScalar());

            /*
             * ObjectClass
             * MeleeWeapon = 1,
             * MissileWeapon = 9,
             * WandStaffOrb = 31,
             * Misc = 8, // PETS ARE IN THIS CLASS
             * 
             * Pets can be filtered by IconUnderlay = 06007420
             */
            // https://github.com/Mag-nus/Mag-Plugins/blob/43ee2ecf3e54f27b2362336d5e0c1336b22c1ca4/Shared/ObjectClass.cs#L61
            string query = "SELECT rowid, OwnerCharName, ObjectName, SerializedData, ObjectClass, ObjectID FROM ObjectData where ObjectClass = 1 or ObjectClass = 9 or ObjectClass = 31 or ObjectClass = 8";
            /*
             * 
             * Table => ObjectData
                OwnerServer = SERVER
                OwnerCharName = CHARACTER_NAME
                ObjectID = INT
                ObjectName = ITEM NAME (no material)
                ObjectClass = INT
                SerializedData = [BLOB]
                IndexedBecause = 0
            */

            cmd.CommandText = query;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var rowId = reader.GetInt32(0);
                    var ownerName = reader.GetString(1);
                    var itemName = reader.GetString(2);
                    var BLOB = (byte[])reader[3];
                    var objectClass = reader.GetInt32(4);
                    var objectId = reader.GetInt32(5);

                    VGItem item = new VGItem();
                    item.ObjectId = objectId;
                    item.ItemName = itemName;
                    item.CharacterName = ownerName;
                    item.ObjectClass = (Enum.ObjectClass)objectClass;
                    try
                    {
                        item.LoadFromBlob(BLOB);

                        switch (objectClass)
                        {
                            case 1:
                                Melee.Add(rowId, item);
                                break;
                            case 9:
                                if (item.IntProps.ContainsKey(0x0d000011)) // AMMO_TYPE_INT
                                {
                                    Missile.Add(rowId, item);
                                }
                                break;
                            case 31:
                                MagicItems.Add(rowId, item);
                                break;
                            case 8:
                                if (item.GetPetLevel() > 0)
                                {
                                    Pets.Add(rowId, item);
                                }
                                break;
                        }

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("An unexpected error has occurred:\n\n" + e.Message);
                    }
                }
            }
        }

        public VGItem? GetItem(int objectId)
        {
            string query = $"SELECT rowid, OwnerCharName, ObjectName, SerializedData, ObjectClass, ObjectID FROM ObjectData where ObjectID = {objectId} limit 1";
            
            SqliteCommand cmd;
            cmd = sqlite.CreateCommand();
            cmd.CommandText = query;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var rowId = reader.GetInt32(0);
                    var ownerName = reader.GetString(1);
                    var itemName = reader.GetString(2);
                    var BLOB = (byte[])reader[3];
                    var objectClass = reader.GetInt32(4);

                    VGItem item = new VGItem();
                    item.ItemName = itemName;
                    item.CharacterName = ownerName;
                    item.ObjectClass = (Enum.ObjectClass)objectClass;
                    try
                    {
                        item.LoadFromBlob(BLOB);
                        item.CleanUpKeys();
                        return item;

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("An unexpected error has occurred:\n\n" + e.Message);
                    }

                }
            }

            return null;
        }
    }
}
