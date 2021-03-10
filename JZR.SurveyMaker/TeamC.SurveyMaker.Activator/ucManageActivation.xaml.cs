using JZR.SurveyMaker.BL.Models;
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
    /// Interaction logic for ucManageActivation.xaml
    /// </summary>
    public partial class ucManageActivation : UserControl
    {
        Activation activation;
        public ucManageActivation(Activation activation = null)
        {
            InitializeComponent();
            this.activation = activation;
            InitialLoad();

        }

        private void InitialLoad()
        {
            if (activation != null)
            {
                txtTitle.Text = "Edit Activation";
                btnSave.Visibility = Visibility.Visible;
                btnAdd.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtTitle.Text = "New Activation";
                btnSave.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Visible;
                activation = new Activation();
            }
            Load();
        }

        private void Load()
        {
            txtActivationCode.Text = activation.ActivationCode;
            dpStartDate.SelectedDate = activation.StartDate != DateTime.MinValue ? activation.StartDate : DateTime.Today;
            dpEndDate.SelectedDate = activation.EndDate != DateTime.MinValue ? activation.EndDate : DateTime.Today;
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dpStartDate.SelectedDate = dpStartDate.SelectedDate > dpEndDate.SelectedDate ? dpEndDate.SelectedDate : dpStartDate.SelectedDate;
            dpEndDate.SelectedDate = dpEndDate.SelectedDate < dpStartDate.SelectedDate ? dpStartDate.SelectedDate : dpEndDate.SelectedDate;
        }

        private void txtActivationCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var length = txtActivationCode.Text.Length;
            if (length > 6)
            {
                txtActivationCode.Text = txtActivationCode.Text.Remove(length - 1);
            }

        }
    }
}
