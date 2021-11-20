using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCargo_Courier.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VCargo_Courier.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage()
        {
            InitializeComponent();
            BindingContext = new DetailsPageViewModel();
        }
    }
}