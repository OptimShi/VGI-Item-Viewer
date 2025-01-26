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

            var test = 1;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
