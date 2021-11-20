using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VCargo_Courier.Models;
using Xamarin.Forms;

namespace VCargo_Courier.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class ImageAndReasonViewModel : BaseViewModel
    {



        #region Declaration

        private ImageSource source;
        private int _bookingId;
        private int id;
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
        byte[] imageArray = null;
        private string _client;
        private Reasons pickItem;
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
        public Reasons PickItem
        {
            get => pickItem;
            set => SetProperty(ref pickItem, value);
        }
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
        public ObservableCollection<Reasons> ReasonList { get; }
        public ImageAndReasonViewModel()
        {
            ReasonList = new ObservableCollection<Reasons>();
            SubmitCommand = new Command(OnSubmit);
            AddImage = new Command(OnAddImage);
            
        }

        private async void OnAddImage(object obj)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "VCargo",
                Name = BookingId + "_Failed.jpg"
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

               
                if (await Application.Current.MainPage.DisplayAlert("Message", "Are you sure you want to submit?", "Yes", "No"))
                {


                    var client = new RestClient(Application.Current.Properties["URi"].ToString() + "/api/booking?$filter=bookingId eq " + "" + BookingId + "");
                    var request = new RestRequest(Method.GET);
                    request.RequestFormat = RestSharp.DataFormat.Json;
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());
                    var response = client.Execute(request);

                    if (response.IsSuccessful == true)
                    {
                        //var data = JsonConvert.DeserializeObject(response.Content)
                        List<Booking> result = (List<Booking>)JsonConvert.DeserializeObject(response.Content, typeof(List<Booking>));


                        Booking.BookingStatus booker = new Booking.BookingStatus()
                        {
                            bookingId = BookingId,
                            refNo = MWAB,
                            destination = Destination,
                            bookStatus = "Failed",
                            createdBy = Application.Current.Properties["UserCode"].ToString()


                        };

                        foreach (var x in result)
                        {
                            x.reason = PickItem.name;
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

                        if (responsex.IsSuccessful == true)
                        {

                            var ImageString = new ImageModel()
                            {
                                bookingId = BookingId,
                                imgType = "Attachement_Failed",
                                img = imageArray,
                                createdBy = Application.Current.Properties["UserCode"].ToString()
                            };

                            //Attachment
                            //  List<ImageModel> ImageString = new List<ImageModel>();

                            // ImageString.Add(new ImageModel()
                            // {
                            //     bookingId = int.Parse(HWBNo),
                            //     imgType = "Attachement",
                            //     img = imageArray,
                            //     createdBy = int.Parse(Application.Current.Properties["UserCode"].ToString())
                            // });
                            //Signature
                            // ImageString.Add(new ImageModel()
                            // {
                            //     bookingId = int.Parse(HWBNo),
                            //    imgType = "Signature",
                            //   img = imageArray,
                            //   createdBy = int.Parse(Application.Current.Properties["UserCode"].ToString())
                            // });


                            string imagejson = JsonConvert.SerializeObject(ImageString);

                            var clientY = new RestClient(Application.Current.Properties["URi"].ToString() + "/api/bookingimg");
                            var requestY = new RestRequest(Method.POST);
                            requestY.RequestFormat = RestSharp.DataFormat.Json;
                            requestY.AddHeader("Content-Type", "application/json");
                            requestY.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());
                            requestY.AddParameter("application/json", JObject.Parse(imagejson), ParameterType.RequestBody);
                            var responseY = clientY.Execute(requestY);

                            if (responseY.IsSuccessful == true)
                            {
                                await Application.Current.MainPage.DisplayAlert("Message", "Deliveries " + BookingId + " with order reference # " + MWAB + " is now in Failed status.", "Ok");

                                Summary newItem = new Summary()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    BookingId = BookingId,
                                    Client =Client,
                                    //HWBNo =hwbno,
                                    OrderDate = hwbdate,
                                    OrdeConsignee = Consignee,
                                    OrderStatus = "Failed",
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
                HWBDate = itemDetails.OrderDate;
                BookingId = itemDetails.BookingId;
                HWBNo = itemDetails.hwbno;
                Shipper = itemDetails.Shipper;
                Consignee = itemDetails.OrdeConsignee;
                Destination = itemDetails.OrderDestination;
                ServiceMode = itemDetails.servicemode;
                //description = "Sample " + itemDetails.BookingId;
                CW = itemDetails.cw;
                cbm = itemDetails.cbm;
                Carrier = itemDetails.carrier;
                MWAB = itemDetails.OrderRefNo;

                // Reasons

                var Reasons = await ReasonDataStore.GetReasonsAsync(true);

                foreach (var carrierX in Reasons)
                {
                    ReasonList.Add(new Reasons()
                    {
                        code = carrierX.code,
                        name = carrierX.name

                    });

                }

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
