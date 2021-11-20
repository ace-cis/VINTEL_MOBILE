using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VCargo_Courier.Models;
using Xamarin.Forms;

namespace VCargo_Courier.Services
{
    public class MockDataStore : IDataStore<Summary>
    {
        private  List<Summary> items;

        public MockDataStore()
        {

            var client = new RestClient(Application.Current.Properties["URi"].ToString() + "/api/booking?$filter=courier eq " + "'" + Application.Current.Properties["UserCode"].ToString() + "'");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());
            var response = client.Execute(request);

            if (response.IsSuccessful == true)
            {
                //var data = JsonConvert.DeserializeObject(response.Content)
                List<Booking> result = (List<Booking>)JsonConvert.DeserializeObject(response.Content, typeof(List<Booking>));


                items = new List<Summary>();
                foreach (Booking book in result)
                {
                    int i = 0;
                    int x = book.bookingStatus.Count;
                    var sortedItems = book.bookingStatus.OrderBy(c => c.lineNum);
                    //  var line = sortedItems.Select(g => g.Max(j => j.lineNum));
                    //var result = grouped.Where(g => ids.Contains(g.Id));


                    foreach (Booking.BookingStatus BS in sortedItems)
                    {
                        i++;


                        if (i == x)
                        {
                            items.Add(new Summary()
                            {
                                Id = Guid.NewGuid().ToString(),
                                BookingId = BS.bookingId,
                                hwbno = book.numAtCard,
                                OrderDate = book.postingDate.Substring(0, 10).ToString(),
                                OrdeConsignee = book.consignee,
                                OrderStatus = BS.bookStatus,
                                OrderDestination = BS.destination,
                                OrderRefNo = BS.refNo,
                                Shipper = book.shipper,
                                Client = book.client,
                                hwbdate = book.postingDate.Substring(0, 10).ToString(),
                                servicemode = book.serviceMode
                            });


                        }


                    }

                }





            }

        }

        public async Task<bool> AddItemAsync(Summary item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Summary item)
        {
            var oldItem = items.Where((Summary arg) => arg.BookingId == item.BookingId).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Summary arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Summary>> GetBookingAsync(bool forceRefresh = false, string _cardcode = "")
        {
            var client = new RestClient("https://18.139.49.140:8090/api/booking?$filter=cardCode eq " + "'" + _cardcode + "'");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            request.AddParameter("application/json", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful == true)
            {
                //var data = JsonConvert.DeserializeObject(response.Content)
                List<Booking> result = (List<Booking>)JsonConvert.DeserializeObject(response.Content, typeof(List<Booking>));


                items = new List<Summary>();
                foreach (Booking book in result)
                {
                    int i = 0;
                    int x = book.bookingStatus.Count;
                    foreach (Booking.BookingStatus BS in book.bookingStatus)
                    {
                        i++;

                        if (x == i)
                        {
                            items.Add(new Summary() { Id = Guid.NewGuid().ToString(), OrderRefNo = BS.bookingId.ToString(), OrdeConsignee = book.consignee, OrderStatus = BS.bookStatus ,OrderDestination = BS.destination });

                        }


                    }

                }





            }





            return await Task.FromResult(items);
        }

        public async Task<Summary> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.BookingId == id));
        }

        public async Task<IEnumerable<Summary>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}