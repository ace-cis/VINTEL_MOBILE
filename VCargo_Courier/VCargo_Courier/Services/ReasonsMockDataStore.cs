using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCargo_Courier.Models;
using Xamarin.Forms;

namespace VCargo_Courier.Services
{
    public class ReasonsMockDataStore : IReasonDataStore<Reasons>
    {

        private List<Reasons> items;

        public ReasonsMockDataStore()
        {

            var client = new RestClient(Application.Current.Properties["URL"].ToString() + "/api/reasons");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());

            var response = client.Execute(request);

            if (response.IsSuccessful == true)
            {
                //var data = JsonConvert.DeserializeObject(response.Content)
                List<Reasons> result = (List<Reasons>)JsonConvert.DeserializeObject(response.Content, typeof(List<Reasons>));


                items = new List<Reasons>();
                foreach (Reasons book in result)
                {
                    items.Add(new Reasons
                    {

                        code = book.code,
                        name = book.name

                    });
                }





            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error", response.StatusDescription.ToString() + ", Please re-open this application", "Ok");
                return;
            }


        }


        public async Task<bool> AddReasonAsync(Reasons item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteReasonAsync(string id)
        {
            var oldItem = items.Where((Reasons arg) => arg.code == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Reasons> GetReasonAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.code == id));
        }



        public async Task<IEnumerable<Reasons>> GetReasonsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<bool> UpdateReasonAsync(Reasons item)
        {
            var oldItem = items.Where((Reasons arg) => arg.code == item.code).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }
    }
}
