using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCargo_Courier.Models;
using VCargo_Courier.Views;
using Xamarin.Forms;

namespace VCargo_Courier.ViewModels
{
public class MainPageViewModel:BaseViewModel
    {
        private bool _isRefreshing;
        private string _RefNo;
        private string _Type;
        private string itemId;


        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                //  LoadItemId(value);
            }
        }

        public string RefNo
        {
            get => _RefNo;
            set => SetProperty(ref _RefNo, value);
        }

        public string StatusType
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                // LoadItemId(value);
            }
        }
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        //Load
        public ObservableCollection<Deliver> DeliveredDocuments { get; }
        public ObservableCollection<Release> ReleasedDocuments { get; }
        public ObservableCollection<Success> SuccessDocuments { get; }
        public ObservableCollection<Failed> FailedDocuments { get; }
        public ObservableCollection<Summary> SummaryDocuments { get; }
        // Preview Load
        private Release _selected;
        private Deliver _selectedDeliver;
        public Command<Release> ReleasedTapped { get; }
        public Command<Deliver> DeliveredTapped { get; }
        //Command
        public Command LoadDocuments { get; }
        public Command RefreshCommand { get; set; }
        public Command ReleaseCommand { get; }
        public Command DeliveredCommand { get; }


        public MainPageViewModel()
        {
            // collection
            ReleasedDocuments = new ObservableCollection<Release>();
            DeliveredDocuments = new ObservableCollection<Deliver>();
            SuccessDocuments = new ObservableCollection<Success>();
            FailedDocuments = new ObservableCollection<Failed>();
            SummaryDocuments = new ObservableCollection<Summary>();
            //Load
            RefreshCommand = new Command(CmdRefresh);
            LoadDocuments = new Command(async () => await ExecuteLoadItemsCommand());
            // Preview
            ReleasedTapped = new Command<Release>(OnReleasedSelected);
            DeliveredTapped= new Command<Deliver>(OnDeliveredSelected);
        }

        #region Functions
        private async void CmdRefresh()
        {
            IsRefreshing = true;
            await Task.Delay(1000);
            IsRefreshing = false;
        }
        public void OnAppearing()
        {
            IsBusy = true;
            RefreshCommand.Execute(null);
            IsBusy = false;

        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {

                DeliveredDocuments.Clear();
                ReleasedDocuments.Clear();
                SuccessDocuments.Clear();
                FailedDocuments.Clear();
                SummaryDocuments.Clear();


                var items =  await DataStore.GetItemsAsync(true);
                var sortedItems = items.OrderByDescending(c => c.OrderDate);

                foreach (var itemx in sortedItems)
                {
                    SummaryDocuments.Add(itemx);

                    if (itemx.OrderStatus == "Pending Release")
                    {

                        ReleasedDocuments.Add(new Release()
                        {
                            Id = itemx.Id,
                            BookingId = itemx.BookingId,
                            OrderRefNo = itemx.OrderRefNo,
                            Client = itemx.Client,
                            OrdeConsignee = itemx.OrdeConsignee,
                            OrderDate = itemx.OrderDate,
                            OrderDestination = itemx.OrderDestination,
                            OrderStatus = itemx.OrderStatus,
                            Shipper = itemx.Shipper
                        });

                    }
                    else if (itemx.OrderStatus == "Pending Delivery")
                    {

                        DeliveredDocuments.Add(new Deliver()
                        {
                            Id = itemx.Id,
                            BookingId = itemx.BookingId,
                            OrderRefNo = itemx.OrderRefNo,
                            Client =itemx.Client,
                            OrdeConsignee = itemx.OrdeConsignee,
                            OrderDate = itemx.OrderDate,
                            OrderDestination = itemx.OrderDestination,
                            OrderStatus = itemx.OrderStatus,
                            Shipper = itemx.Shipper
                        });

                    }
                    else if (itemx.OrderStatus == "Success")
                    {
                        SuccessDocuments.Add(new Success()
                        {
                            Id = itemx.Id,
                            BookingId = itemx.BookingId,
                            OrderRefNo = itemx.OrderRefNo,
                            Client = itemx.Client,
                            OrdeConsignee = itemx.OrdeConsignee,
                            OrderDate = itemx.OrderDate,
                            OrderDestination = itemx.OrderDestination,
                            OrderStatus = itemx.OrderStatus,
                            Shipper = itemx.Shipper
                        });
                    }
                    else if (itemx.OrderStatus == "Failed")
                    {
                        FailedDocuments.Add(new Failed()
                        {
                            Id = itemx.Id,
                            BookingId = itemx.BookingId,
                            OrderRefNo = itemx.OrderRefNo,
                            Client = itemx.Client,
                            OrdeConsignee = itemx.OrdeConsignee,
                            OrderDate = itemx.OrderDate,
                            OrderDestination = itemx.OrderDestination,
                            OrderStatus = itemx.OrderStatus,
                            Shipper = itemx.Shipper
                        });
                    }


                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        //Release TAB Preview Details

        public Release ReleaseBooking
        {
            get => _selected;
            set
            {
                SetProperty(ref _selected, value);
                OnReleasedSelected(value);
            }
        }

        async void OnReleasedSelected(Release Book)
        {
            if (Book == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?{nameof(DetailsPageViewModel.Id)}={Book.BookingId}");

        }

        //Deliver TAB Preview Details
        public Deliver DeliveredBooking
        {
            get => _selectedDeliver;
            set
            {
                SetProperty(ref _selectedDeliver, value);
                OnDeliveredSelected(value);
            }
        }

        async void OnDeliveredSelected(Deliver Book)
        {
            if (Book == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(DeliveredDetailPage)}?{nameof(DeliveredDetailsPageViewModel.Id)}={Book.BookingId}");

        }

        #endregion



    }
}
