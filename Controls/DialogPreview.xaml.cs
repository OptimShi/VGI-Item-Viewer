using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeenieViewer.Appraisal;

namespace VGI_Item_Viewer.Controls
{
    /// <summary>
    /// Interaction logic for DialogPreview.xaml
    /// </summary>
    public partial class DialogPreview : Window
    {
        public DialogPreview(VGItem item)
        {
        
            InitializeComponent();

            var weenie = item.ConvertToWeenie();
            if (weenie != null)
            {
                var examine = new ItemExamine(weenie);
                string fullText = $"{item.GetFullName()}\r\nCharacter: {item.CharacterName }\r\n\r\n";
                    
                txtItemDescription.Text = fullText + examine.Text;
            }
            else
            {
                txtItemDescription.Text = "Unabled to convert item to weenie.";
            }

            // Bind ESC to close window
            PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
