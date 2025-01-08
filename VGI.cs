using Microsoft.Data.Sqlite;
using System.Data;
using System.IO;
using System.Windows.Documents;
using System.Xml.Linq;

namespace VGI_Item_Viewer
{

    public class VGI
    {
        DataTable dt = new DataTable();
        SqliteConnection sqlite;

        public Dictionary<int, VGItem> MagicItems;
        public Dictionary<int, VGItem> Melee;
        public Dictionary<int, VGItem> Missile;
        public Dictionary<int, VGItem> Pets;

        public VGI()
        {
            SqliteCommand cmd;

            string dbName;// = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ace_world.db");
#if DEBUG
            dbName = "D:\\Games\\Decal\\VirindiPlugins\\VirindiGlobalInventory\\_Leafcull.db"; // For Testing Purposes
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
             * Misc                       = 0x00000080, (Pets)
             * 
             * Pets can be filtered by IconUnderlay = 06007420
             */
            // https://github.com/Mag-nus/Mag-Plugins/blob/43ee2ecf3e54f27b2362336d5e0c1336b22c1ca4/Shared/ObjectClass.cs#L61
            string query = "SELECT rowid, OwnerCharName, ObjectName, SerializedData, ObjectClass FROM ObjectData";// where ObjectClass = ";
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

                    VGItem item = new VGItem();
                    item.ItemName = itemName;
                    item.CharacterName = ownerName;
                    item.ItemType = (Enum.ItemType)objectClass;
                    if(itemName.Contains("Essence"))
                    {
                        var stop = true;
                    }
                    try
                    {
                        //item.LoadFromBlob(BLOB);
                    }
                    catch (Exception e)
                    {
                        // crap
                        var STOP = true;
                    }
                    
                }
            }
        }


    }
}
