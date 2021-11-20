using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using RestSharp;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VCargo_Courier.Models;
using Xamarin.Forms;

namespace VCargo_Courier.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class ImageAndSignatureViewModel:BaseViewModel
    {



        #region Declaration
        SignaturePadView signaturePad;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                PreviewAsync(value);

            }
        }

        private ImageSource signaturePadSource;

        public ImageSource SignaturePadSource
        {
            get { return signaturePadSource; }
            set { signaturePadSource = value; }
        }
        private SignaturePadView _sign;
        public SignaturePadView Sign
        {
            get { return _sign; }
            set { SetProperty(ref _sign, value); }
        }
        private ImageSource source;
        private int id;
        private int _bookingId;
        private string hwbdate;
        private string hwbno;
        private string shipper;
        private string consignee;
        private string destination;
        private string servicemode;
        private string description;
        private string cw;
        private decimal cbm;
        private string carrier;
        private string mwab;
        private string _client;
        byte[] imageArray = null;
        byte[] imageSignatureArray = null;
        public int BookingId
        {
            get => _bookingId;
            set => SetProperty(ref _bookingId, value);
        }

        public string Client
        {
            get => _client;
            set => SetProperty(ref _client, value);
        }
        public string HWBDate
        {
            get => hwbdate;
            set => SetProperty(ref hwbdate, value);
        }
        public string receivername;

        public string Receivername
        {
            get => receivername;
            set => SetProperty(ref receivername, value);
        }
        public string HWBNo
        {
            get => hwbno;
            set => SetProperty(ref hwbno, value);
        }
        public string Shipper
        {
            get => shipper;
            set => SetProperty(ref shipper, value);
        }
        public string Consignee
        {
            get => consignee;
            set => SetProperty(ref consignee, value);
        }
        public string Destination
        {
            get => destination;
            set => SetProperty(ref destination, value);
        }
        public string ServiceMode
        {
            get => servicemode;
            set => SetProperty(ref servicemode, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public string CW
        {
            get => cw;
            set => SetProperty(ref cw, value);
        }
        public decimal CBM
        {
            get => cbm;
            set => SetProperty(ref cbm, value);
        }
        public string Carrier
        {
            get => carrier;
            set => SetProperty(ref carrier, value);
        }
        public string MWAB
        {
            get => mwab;
            set => SetProperty(ref mwab, value);
        }

        public ImageSource Source
        {
            get => source;
            set => SetProperty(ref source, value);
        }

        #endregion 

        public Command SubmitCommand { get; }
        public Command AddImage { get; }
        public ImageAndSignatureViewModel(SignaturePadView sfSignaturePad)
        {
            SubmitCommand = new Command(OnSubmit);
            AddImage = new Command(OnAddImage);
            signaturePad = sfSignaturePad;
        }

        private async void OnAddImage(object obj)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
             await  Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "VCargo",
                Name = BookingId +".jpg"
            });

            if (file == null)
                return;

            await Application.Current.MainPage.DisplayAlert("Message", file.Path, "OK");

            using (MemoryStream memory = new MemoryStream())
            {

                Stream stream = file.GetStream();
                stream.CopyTo(memory);
                imageArray = memory.ToArray();
            }

            Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });


        }
        private void OnSubmit(object obj)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                //MCA get Signaturepad image byte
                var image = await signaturePad.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);
                var mStream = (MemoryStream)image;
                imageSignatureArray = mStream.ToArray();


                if (await Application.Current.MainPage.DisplayAlert("Message", "Are you sure you want to submit?", "Yes", "No"))
                {


                    var client = new RestClient(Application.Current.Properties["URi"].ToString() + "/api/booking?$filter=bookingId eq " + "" + BookingId + "");
                    var request = new RestRequest(Method.GET);
                    request.RequestFormat = RestSharp.DataFormat.Json;
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());
                    var response = client.Execute(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //var data = JsonConvert.DeserializeObject(response.Content)
                        List<Booking> result = (List<Booking>)JsonConvert.DeserializeObject(response.Content, typeof(List<Booking>));


                        Booking.BookingStatus booker = new Booking.BookingStatus()
                        {
                            bookingId = BookingId,
                            refNo = MWAB,
                            destination = Destination,
                            bookStatus = "Success",
                            createdBy = Application.Current.Properties["UserCode"].ToString()


                        };

                        foreach (var x in result)
                        {
                            x.transactionType = -1;
                            x.bookingStatus.Clear();

                            x.bookingStatus.Add(booker);
                        }

                        string stringjson = JsonConvert.SerializeObject(result);
                        string stringjsonX = stringjson.TrimStart('[').TrimEnd(']');


                        var clientx = new RestClient(Application.Current.Properties["URi"].ToString() + "/api/booking/u");
                        var requestX = new RestRequest(Method.PATCH);
                        requestX.RequestFormat = RestSharp.DataFormat.Json;
                        requestX.AddHeader("Content-Type", "application/json");
                        requestX.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());
                        requestX.AddParameter("application/json", JObject.Parse(stringjsonX), ParameterType.RequestBody);
                        var responsex = clientx.Execute(requestX);

                        if (responsex.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            int x = 0;


                            var ImageString = new ImageModel()
                            {
                                bookingId = BookingId,
                                imgType = "Attachement",
                                img = imageArray,
                                createdBy = Application.Current.Properties["UserCode"].ToString()
                            };

                            string imagejsonX = JsonConvert.SerializeObject(ImageString);
                            //string imageX = imagejson.TrimStart('[').TrimEnd(']');

                            //var x = JObject.Parse(imageX);

                            var clientY = new RestClient(Application.Current.Properties["URi"].ToString() + "/api/bookingimg");
                            var requestY = new RestRequest(Method.POST);
                            requestY.RequestFormat = RestSharp.DataFormat.Json;
                            requestY.AddHeader("Content-Type", "application/json");
                            requestY.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());
                            requestY.AddParameter("application/json", JObject.Parse(imagejsonX), ParameterType.RequestBody);
                            var responseY = clientY.Execute(requestY);

                            if (responseY.IsSuccessful == true)
                            {
                                x = x + 1;
                            }




                            var ImageSignatureString = new ImageModel()
                            {
                                bookingId = BookingId,
                                imgType = "Signature",
                                img = imageSignatureArray,
                                createdBy = Application.Current.Properties["UserCode"].ToString()
                            };

                            string imagejson = JsonConvert.SerializeObject(ImageSignatureString);
                            //string imageX = imagejson.TrimStart('[').TrimEnd(']');

                            //var x = JObject.Parse(imageX);

                            var clientZ = new RestClient(Application.Current.Properties["URi"].ToString() + "/api/bookingimg");
                            var requestZ = new RestRequest(Method.POST);
                            requestZ.RequestFormat = RestSharp.DataFormat.Json;
                            requestZ.AddHeader("Content-Type", "application/json");
                            requestZ.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());
                            requestZ.AddParameter("application/json", JObject.Parse(imagejson), ParameterType.RequestBody);
                            var responseZ = clientZ.Execute(requestZ);

                            if (responseZ.IsSuccessful == true)
                            {

                                x = x + 1;

                            }


                            if (x == 2)
                            {
                                await Application.Current.MainPage.DisplayAlert("Message", "Deliveries " + BookingId + " with order reference # " + MWAB + " is now in Success status.", "Ok");

                                Summary newItem = new Summary()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    BookingId = BookingId,
                                    hwbno = HWBNo,
                                    Client =Client,
                                    OrderDate = hwbdate,
                                    OrdeConsignee = Consignee,
                                    OrderStatus = "Success",
                                    OrderDestination = Destination,
                                    OrderRefNo = MWAB,
                                    Shipper = Shipper,
                                    hwbdate = HWBDate,
                                    servicemode = servicemode
                                };
                                //MCA update model here
                                await DataStore.UpdateItemAsync(newItem);

                                Application.Current.MainPage = new AppShell();
                            }

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", responsex.StatusDescription, "Ok");
                            return;
                        }


                    }


                }
            });
        }
        public async Task PreviewAsync(int id)
        {
            IsBusy = true;

            try
            {

                var itemDetails = await DataStore.GetItemAsync(id);
                BookingId = itemDetails.BookingId;
                HWBDate = itemDetails.OrderDate;
                HWBNo = itemDetails.hwbno;
                Shipper = itemDetails.Shipper;
                Client = itemDetails.Client;
                Consignee = itemDetails.OrdeConsignee;
                Destination = itemDetails.OrderDestination;
                ServiceMode = itemDetails.servicemode;
                //description = "Sample " + itemDetails.BookingId;
                CW = itemDetails.cw;
                cbm = itemDetails.cbm;
                Carrier = itemDetails.carrier;
                MWAB = itemDetails.OrderRefNo;

                Source = "AddPhoto.jpg";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }


            IsBusy = false;
        }

    }
}
