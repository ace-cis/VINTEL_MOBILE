using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VCargo_Courier.Models;
using VCargo_Courier.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VCargo_Courier.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        private login Login;

        //public Command LoginCommand { get; }

        public string _UserName;
        public string UserName
        {
            get => _UserName;
            set
            {
                _UserName = value;

            }
        }

        public string _Password;
        public string Password
        {
            get => _Password;
            set
            {
                _Password = value;

            }
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);

            // LoginCommand = new Command(async () => await OnLoginClickedAsync());
        }


        private async void OnLoginClicked(object obj)
        {


            //if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            //{
            //    await Application.Current.MainPage.DisplayAlert("VCargo", "No internet!", "Ok");
            //    return;
            //}



            if (_UserName == "" || _UserName == null)
            {
                await Application.Current.MainPage.DisplayAlert("VCargo", "Login Failed!", "Ok");
                return;
            }

            if (_Password == "" || _Password == null)
            {
                await Application.Current.MainPage.DisplayAlert("VCargo", "Login Failed!", "Ok");
                return;
            }



            Login = new login()
            {
                userCode = _UserName,
                pssword = _Password
            };
            String URL = Application.Current.Properties["URL"].ToString() + "/api/login";
            var client = new RestClient(URL);

            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            //MCA Covert object model to string
            string stringjson = JsonConvert.SerializeObject(Login);
            request.AddParameter("application/json", JObject.Parse(stringjson), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Bearer result = (Bearer)JsonConvert.DeserializeObject(response.Content, typeof(Bearer));


                Application.Current.Properties["token"] = result.token;
              

                // var user = await UserDataStore.GetUserAsync(true);
                var userValidation = await UserDataStore.GetUserAsync(_UserName);

                string UserX = userValidation.userCode;
                string PasswordX = userValidation.pssword;
                string CustomerType = userValidation.position;

                if (UserX.ToString() == _UserName && PasswordX.ToString() == _Password)
                {
                    //Application.Current.MainPage = new AppShell();


                    if (CustomerType == "Courier")
                    {


                        Application.Current.Properties["UserCode"] = _UserName;
                        // await Application.Current.MainPage.DisplayAlert("Message", " Login successfully.", "Ok");


                        Application.Current.MainPage = new AppShell();



                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "This application is for Courier only.", "Ok");
                        return;
                    }




                }
                else
                {

                    await Application.Current.MainPage.DisplayAlert("VCargo", " Login failed!", "Ok");
                }

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("VCargo", " Invalid Username/Password!", "Ok");
                return;
            }




        }


    }
}
