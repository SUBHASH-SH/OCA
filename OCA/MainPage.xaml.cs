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
using System.Xml.Linq;
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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

                    // Parse the JSON string into a JObject
                    JObject jsonObject = JObject.Parse(responseBody);

                    // Extract the string value from the JSON 
                    JToken expiryDates = jsonObject.SelectToken("records.expiryDates");
                    JToken data = jsonObject.SelectToken("records.data");
                    JToken timestamp = jsonObject.SelectToken("records.timestamp");
                    JToken underlyingvalue = jsonObject.SelectToken("records.underlyingValue");
                    JToken strikeprices = jsonObject.SelectToken("records.strikePrices");

                    JToken filtered = jsonObject.SelectToken("filtered.data");
                    JToken cE = jsonObject.SelectToken("filtered.CE");
                    JToken pE = jsonObject.SelectToken("filtered.PE");

                    string expiryDate = expiryDates.ToString();             // use it Directly.
                    string dataData = data.ToString();                      
                    string timeStamp = timestamp.ToString();                // use it Directly.
                    string underlyingValue = underlyingvalue.ToString();    // use it Directly.
                    string strikePrices = strikeprices.ToString();          // use it Directly.

                    string filteredData = filtered.ToString();          // use it Directly.
                    string cEData = cE.ToString();          // use it Directly.
                    string pEData = pE.ToString();          // use it Directly.
                        
                    List<Datum> DatumData = JsonConvert.DeserializeObject<List<Datum>>(dataData);
                    List<Filtered> FilteredData = JsonConvert.DeserializeObject<List<Filtered>>(filteredData);
                 

                }
            }
            catch(Exception ex) 
            { 
                Debug.WriteLine("Exception Thrown :" +ex); 
            }
        }
    }
}
