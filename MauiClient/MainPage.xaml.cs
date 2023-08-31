using System.Net.Http.Json;

namespace MauiClient
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var uploadFile = await MediaPicker.PickPhotoAsync();

            // check for null
            if (uploadFile is null) return;

            // Read the file
            var httpContent = new MultipartFormDataContent();
            httpContent.Add(new StreamContent(await uploadFile.OpenReadAsync()), "file", uploadFile.FileName);

            // Create instance of HttpClient
            var httpClient = new HttpClient();
#if ANDROID
            var result = await httpClient.PostAsync("https://172.22.48.1:7172/api/UlpoadFile", httpContent);
#elif WINDOWS 
            var result = await httpClient.PostAsync("https://localhost:7172/api/UlpoadFile", httpContent);
            var response = await result.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Response From Server", response, "Ok");
        }
    }

}
