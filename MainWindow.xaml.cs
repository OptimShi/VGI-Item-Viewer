using Microsoft.Win32;
using System.Collections.Generic;
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
using VGI_Item_Viewer.Controls;

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
            if (!File.Exists(dbName))
            {
                dbName = @"D:\games\Decal Plugins\VirindiPlugins\VirindiGlobalInventory\_Leafcull.db"; // Alt Pathing
            }
            if (File.Exists(dbName))
            {
                LoadVGIFile(dbName);
            }
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

        private void griMagicItems_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Magic_Defense":
                    {
                        // Format the column as a percentage to two decimal places
                        (e.Column as DataGridTextColumn).Binding.StringFormat = "P01";
                    }
                    break;
            }
        }

        private void gridPetItems_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Damage":
                    {
                        // Format the column as a percentage to two decimal places
                        (e.Column as DataGridTextColumn).Binding.StringFormat = "P02";
                    }
                    break;
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

        private void gridItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            // make sure we have a selected item...
            if (grid.SelectedItems.Count == 0)
            {
                return;
            }

            int objectId = 0;
            if (grid.SelectedItem is MagicGridItem)
            {
                objectId = ((MagicGridItem)grid.SelectedItem).GetObjectId();
            }
            else if (grid.SelectedItem is MissileGridItem)
            {
                objectId = ((MissileGridItem)grid.SelectedItem).GetObjectId();
            }
            else if (grid.SelectedItem is MeleeGridItem)
            {
                objectId = ((MeleeGridItem)grid.SelectedItem).GetObjectId();
            }
            else if (grid.SelectedItem is PetGridItem)
            {
                objectId = ((PetGridItem)grid.SelectedItem).GetObjectId();
            }

            if (objectId != 0)
            {
                var item = vgi.GetItem(objectId);
                if (item != null)
                {
                    var about = new DialogPreview(item);
                    about.Owner = this;
                    about.ShowDialog();
                }

            }
        }
    }
}