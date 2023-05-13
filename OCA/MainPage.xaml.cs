using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Protection.PlayReady;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OCA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        String url = "https://www.nseindia.com/api/option-chain-indices?symbol=NIFTY";
        // how to get response form ssl encreption
        
        public MainPage()
        {
            this.InitializeComponent();
            GetData();
           
        }

        public async void GetData() 
        {
            try {
                
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
                
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Read the content of the response as a string
                    var requestUri = response.RequestMessage.RequestUri;
                    HttpResponseMessage responseHeadders = await client.GetAsync(requestUri);

                    string responseBody = await responseHeadders.Content.ReadAsStringAsync();
                    Debug.WriteLine(responseBody);

                    // Do something with the content
                    Debug.WriteLine(requestUri);
                    

                    Debug.WriteLine(responseHeadders);
                }

                Debug.WriteLine(response.RequestMessage);
            }
            catch(Exception ex) 
            { 
                Debug.WriteLine("Exception Thrown :" +ex); 
            }
        }

    }
}
