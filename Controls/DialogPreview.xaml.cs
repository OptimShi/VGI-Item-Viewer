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

            var examine = new ItemExamine(weenie);
            txtItemDescription.Text = item.GetFullName() + "\r\n\r\n";
            txtItemDescription.Text += examine.Text;

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
