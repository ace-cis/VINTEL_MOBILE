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
    public class UserMockDataStore : IUserDataStore<Users>
    {

        private List<Users> users;

        public UserMockDataStore()
        {

            var client = new RestClient(Application.Current.Properties["URi"].ToString() + "/api/employee");

            var request = new RestRequest(Method.GET);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + Application.Current.Properties["token"].ToString());

            var response = client.Execute(request);

            if (response.IsSuccessful == true)
            {
                //var data = JsonConvert.DeserializeObject(response.Content)
                List<Users> result = (List<Users>)JsonConvert.DeserializeObject(response.Content, typeof(List<Users>));


                users = new List<Users>();
                foreach (Users book in result)
                {
                    users.Add(new Users()
                    {
                        userCode = book.userCode,
                        userName = book.userName,
                        position = book.position,
                        employeeId = book.employeeId,
                        category = book.otherCat,
                        pssword = book.pssword
                    });
                }
            }



        }

        public Task<bool> AddUserAsync(Users item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Users> GetUserAsync(string UserName)
        {
            return await Task.FromResult(users.FirstOrDefault(s => s.userCode == UserName));
        }

        public async Task<IEnumerable<Users>> GetUserAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(users);
        }

        public Task<bool> UpdateUserAsync(Users item)
        {
            throw new NotImplementedException();
        }
    }
}
