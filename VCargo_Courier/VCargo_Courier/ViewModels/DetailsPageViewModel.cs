using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using VCargo_Courier.Models;
using Xamarin.Forms;

namespace VCargo_Courier.ViewModels
{
    [QueryProperty(nameof(Id),nameof(Id))]
 public   class DetailsPageViewModel:BaseViewModel
    {
        #region Declaration

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
        private int _bookingId;
        private string _client;
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

        public int BookingId
        {
            get => _bookingId;
            set => SetProperty(ref _bookingId, value);
        }

        public string Client
        {
            get => hwbdate;
            set => SetProperty(ref hwbdate, value);
        }

        public string HWBDate
        {
            get => hwbdate;
            set => SetProperty(ref hwbdate, value);
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

        #endregion

        public Command ConfirmCommand { get; }
        public Command CancelCommand { get; }

        public DetailsPageViewModel()
        {

            ConfirmCommand = new Command(OnConfirmAsync);
            CancelCommand = new Command(OnCancelAsync);
        }

        private async void OnConfirmAsync(object obj)
        {
            var client = new RestClient(Application.Current.Properties["URi"].ToString() + "/api/booking?$filter=bookingId eq " + "" + BookingId + "");
            //client.Authenticator = new JwtAuthenticator(Application.Current.Properties["token"].ToString());
            //client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());
            // request.AddHeader("Authorization", string.Format("Bearer {0}", Application.Current.Properties["token"].ToString()));
            //request.AddParameter("application/json", "", ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.StatusCode ==System.Net.HttpStatusCode.OK)
            {
                //var data = JsonConvert.DeserializeObject(response.Content)
                List<Booking> result = (List<Booking>)JsonConvert.DeserializeObject(response.Content, typeof(List<Booking>));


                Booking.BookingStatus booker = new Booking.BookingStatus()
                {
                    bookingId = BookingId,
                    refNo = MWAB,
                    destination = Destination,
                    bookStatus = "Pending Delivery",
                    createdBy = Application.Current.Properties["UserCode"].ToString()


                };

                foreach (var x in result)
                {
                    x.transactionType = -1;

                    x.bookingStatus.Clear();

                    x.bookingStatus.Add(booker);
                }

                // result.Add(booker);



                //var bookstatus = JsonConvert.SerializeObject(booker);
                // updateBook.bookingStatus.Add(bookstatus);

                string stringjson = JsonConvert.SerializeObject(result);
                string stringjsonX = stringjson.TrimStart('[').TrimEnd(']');


                var clientx = new RestClient(Application.Current.Properties["URi"].ToString() + "/api/booking/u");
                //client.Authenticator = new JwtAuthenticator(Application.Current.Properties["token"].ToString());
                //client.Timeout = -1;
                var requestX = new RestRequest(Method.PATCH);
                requestX.RequestFormat = RestSharp.DataFormat.Json;
                requestX.AddHeader("Content-Type", "application/json");
                requestX.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());
                requestX.AddParameter("application/json", JObject.Parse(stringjsonX), ParameterType.RequestBody);
                // request.AddHeader("Authorization", string.Format("Bearer {0}", Application.Current.Properties["token"].ToString()));
                //request.AddParameter("application/json", "", ParameterType.RequestBody);
                var responsex = clientx.Execute(requestX);


                if (responsex.StatusCode  == System.Net.HttpStatusCode.OK)
                {

                    await Application.Current.MainPage.DisplayAlert("Message", "UPDATED SUCCESSFULLY! " + HWBNo + "    " + DateTime.Now.ToString(), "ok");


                    Summary newItem = new Summary()
                    {
                        Id = Guid.NewGuid().ToString(),
                        BookingId = BookingId,
                        OrderDate = hwbdate,
                        OrdeConsignee = Consignee,
                        OrderStatus = "Pending Delivery",
                        OrderDestination = Destination,
                        Client =Client,
                        OrderRefNo = MWAB,
                        Shipper = Shipper,
                        hwbdate = HWBDate,
                        servicemode = servicemode
                    };
                    //MCA update model here
                    await DataStore.UpdateItemAsync(newItem);

                    await Shell.Current.GoToAsync("..");

                    return;

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", responsex.StatusDescription, "Ok");
                    return;
                }




            }


        }
        private async void OnCancelAsync(object obj)
            {

                if (await Application.Current.MainPage.DisplayAlert("Message", "Are you sure you want to cancel?", "Yes", "No"))
                {
                    await Shell.Current.GoToAsync("..");
                }
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
                Client = itemDetails.Client;
                Shipper = itemDetails.Shipper;
                Consignee = itemDetails.OrdeConsignee;
                Destination = itemDetails.OrderDestination;
                ServiceMode = itemDetails.servicemode;
                //description = "Sample " + itemDetails.BookingId;
                CW = itemDetails.cw;
                cbm = itemDetails.cbm;
                Carrier = itemDetails.carrier;
                MWAB = itemDetails.OrderRefNo;


            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            } finally
            {
                IsBusy = false;
            }


            IsBusy = false;
        }

    }
}
