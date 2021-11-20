using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCargo_Courier.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VCargo_Courier.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageAndSignaturePage : ContentPage
    {
        public ImageAndSignaturePage()
        {
            InitializeComponent();
            BindingContext = new ImageAndSignatureViewModel(signature);
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("Exit?", "Are you sure you want to exit from this page?", "Yes", "No"))
                {
                    base.OnBackButtonPressed();
                    await Shell.Current.GoToAsync($"{nameof(DeliveredDetailPage)}?{nameof(DeliveredDetailsPageViewModel.Id)}={ Application.Current.Properties["BookingId"].ToString()}");
                   

                }
            });

            return true;


        }

 
    }
}