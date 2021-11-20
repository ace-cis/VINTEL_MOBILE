using System;
using System.Net;
using VCargo_Courier.Services;
using VCargo_Courier.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VCargo_Courier
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<UserMockDataStore>();
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<ReasonsMockDataStore>();
            MainPage = new LoginPage();
            Application.Current.Properties["URL"] = "https://161.49.173.194:8089";
            Application.Current.Properties["URi"] = "https://161.49.173.194:8089";
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
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
