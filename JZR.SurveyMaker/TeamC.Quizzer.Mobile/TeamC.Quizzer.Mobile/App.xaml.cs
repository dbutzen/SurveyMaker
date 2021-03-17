using System;
using TeamC.Quizzer.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamC.Quizzer.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new HomePage { Title = "Quizzer"});
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
