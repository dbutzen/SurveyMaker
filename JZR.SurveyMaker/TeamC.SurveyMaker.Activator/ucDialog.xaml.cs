using MaterialDesignThemes.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeamC.SurveyMaker.Activator
{
    /// <summary>
    /// Interaction logic for ucDialog.xaml
    /// </summary>
    public partial class ucDialog : UserControl
    {
        public MessageBoxResult MessageBoxResult;
        public ucDialog()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.Yes;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.No;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
