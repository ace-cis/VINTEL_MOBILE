using System;
using System.Collections.Generic;
using System.Net;
using VCargo_Courier.ViewModels;
using VCargo_Courier.Views;
using Xamarin.Forms;

namespace VCargo_Courier
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            Routing.RegisterRoute(nameof(DeliveredDetailPage), typeof(DeliveredDetailPage));
            Routing.RegisterRoute(nameof(ImageAndSignaturePage), typeof(ImageAndSignaturePage));
            Routing.RegisterRoute(nameof(ImageAndReasonPage), typeof(ImageAndReasonPage));
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

    }
}
