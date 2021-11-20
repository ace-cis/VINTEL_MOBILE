using Android.Content.Res;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using VCargo_Courier.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ResolutionGroupName(name: "PlainEntryGroup")]
[assembly: ExportEffect(typeof(AndroidPlainEntryEffect), "PlainEntryEffect")]

namespace VCargo_Courier.Droid
{


    public class AndroidPlainEntryEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                if (Control != null)
                {
                    Android.Graphics.Color entryLineColor = Android.Graphics.Color.WhiteSmoke;
                    Control.BackgroundTintList = ColorStateList.ValueOf(entryLineColor);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error... Unable to set property on attached control", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
        }
    }


   
}