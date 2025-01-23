using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace VGI_Item_Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VGI vgi;
        public MainWindow()
        {
            InitializeComponent();
            lblFilename.Text = "";

#if DEBUG
            string dbName = "D:\\Games\\Decal\\VirindiPlugins\\VirindiGlobalInventory\\_Leafcull.db"; // For Testing Purposes
            LoadVGIFile(dbName);
#endif
        }

        private void LoadVGIFile(string filename) {
            if (File.Exists(filename))
            {
                string baseFileName = System.IO.Path.GetFileName(filename);

                try
                {
                    ClearGrids();

                    vgi = new VGI(filename);
                    lblFilename.Text = baseFileName;

                    LoadGrids();
                }
                catch (Exception e)
                {
                    MessageBox.Show("There was an error opening this file:\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void ClearGrids()
        {
            gridMagicItems.ItemsSource = new List<object>();
            gridMeleeItems.ItemsSource = new List<object>();
            gridMissileItems.ItemsSource = new List<object>();
            gridPetItems.ItemsSource = new List<object>();
        }

        private void LoadGrids()
        {
            LoadMagicGrid();
            LoadMissileGrid();
            LoadMeleeGrid();
            LoadPetsGrid();
        }

        private void LoadMagicGrid()
        {
            List<MagicGridItem> magicGridItems = new List<MagicGridItem>();
            foreach (var item in vgi.MagicItems)
            {
                var gridItem = MagicGridItem.ConvertFromItem(item.Value);
                magicGridItems.Add(gridItem);
            }
            gridMagicItems.ItemsSource = magicGridItems;
        }

        private void LoadMissileGrid()
        {
            List<MissileGridItem> gridItems = new List<MissileGridItem>();
            foreach (var item in vgi.Missile)
            {
                var gridItem = MissileGridItem.ConvertFromItem(item.Value);
                gridItems.Add(gridItem);
            }
            gridMissileItems.ItemsSource = gridItems;
        }

        private void LoadMeleeGrid()
        {
            List<MeleeGridItem> gridItems = new List<MeleeGridItem>();
            foreach (var item in vgi.Melee)
            {
                var gridItem = MeleeGridItem.ConvertFromItem(item.Value);
                gridItems.Add(gridItem);
            }
            gridMeleeItems.ItemsSource = gridItems;
        }

        private void LoadPetsGrid()
        {
            List<PetGridItem> gridItems = new List<PetGridItem>();
            foreach (var item in vgi.Pets)
            {
                var gridItem = PetGridItem.ConvertFromItem(item.Value);
                gridItems.Add(gridItem);
            }
            gridPetItems.ItemsSource = gridItems;
            
            //gridPetItems.Columns["Damage"].DefaultCellStyle.Format("P");
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void gridPetItems_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if(e.PropertyName == "Damage")
            {
                // Format the column as a percentage to two decimal places
                (e.Column as DataGridTextColumn).Binding.StringFormat = "P02";
            }
        }

        private void miEditOptions_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not Yet Implemented");
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Open a file...
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SQLite files (*.db)|*.db|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                // Handle the file open logic here
                string filePath = openFileDialog.FileName;
                LoadVGIFile(filePath);
            }
        }
    }
}