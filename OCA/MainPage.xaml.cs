using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;
using static OCA.OIViewModel1;
using Windows.Storage;
using System.Collections.Specialized;


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

        private List<OIViewModel1> OIViewModel1;
        

        public MainPage()
        {
            this.InitializeComponent();
            GetData();

            //viewModel = this.DataContext as Customer;
            OIViewModel1 = OIViewModelManager.GetCustomers();

            //GetData();
            //updateData();
            


        }

        public async void GetData() 
        {
            try {
                
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
                
                HttpResponseMessage response = client.GetAsync(url).Result;

               

                if (response.IsSuccessStatusCode)
                {
                    // Read the content of the response as a string
                    var requestUri = response.RequestMessage.RequestUri;
                    HttpResponseMessage responseHeadders = client.GetAsync(requestUri).Result;

                    string responseBody = responseHeadders.Content.ReadAsStringAsync().Result;
                    

                    // Parse the JSON string into a JObject
                    JObject jsonObject = JObject.Parse(responseBody);

                    // Extract the string value from the JSON 
                    JToken expiryDates = jsonObject.SelectToken("records.expiryDates");
                    JToken data = jsonObject.SelectToken("records.data");
                    JToken timestamp = jsonObject.SelectToken("records.timestamp");
                    JToken underlyingvalue = jsonObject.SelectToken("records.underlyingValue");
                    JToken strikeprices = jsonObject.SelectToken("records.strikePrices");

                    JToken filtered = jsonObject.SelectToken("filtered.data");
                    JToken cE = jsonObject.SelectToken("filtered.CE.totOI");
                    JToken pE = jsonObject.SelectToken("filtered.PE.totOI");

                    string expiryDate = expiryDates.ToString();             // use it Directly.
                    string dataData = data.ToString();                      
                    string timeStamp = timestamp.ToString();               // use it Directly.
                    double number = double.Parse(underlyingvalue.ToString());
                    int underlyingVal = (int)number;
                    string strikePrices = strikeprices.ToString();  // use it Directly.

                    IndexText.Text = underlyingVal.ToString();

                    string filteredData = filtered.ToString();
                    double cEData = double.Parse(cE.ToString());          // use it Directly.
                    double pEData = double.Parse(pE.ToString());          // use it Directly.

                    //Sentiment Analysis
                    double pcr = pEData / cEData;
                    String pcrValue = Math.Round(pcr, 2).ToString();

                    pcrText.Text = "PCR - " + pcrValue;

                    Model.expiryDate = expiryDate;



                    List<Datum> DatumData = JsonConvert.DeserializeObject<List<Datum>>(dataData);
                    List<Filtered> FilteredData = JsonConvert.DeserializeObject<List<Filtered>>(filteredData);


                    List<Filtered> getCE0 = new List<Filtered>();
                    List<Filtered> getCE1 = new List<Filtered>();
                    List<Filtered> getCE2 = new List<Filtered>();
                    List<Filtered> getCE3 = new List<Filtered>();
                    List<Filtered> getCE4 = new List<Filtered>();
                    List<Filtered> getCE5 = new List<Filtered>();
                    List<Filtered> getCE6 = new List<Filtered>();
                    List<Filtered> getCE7 = new List<Filtered>();
                    List<Filtered> getPE = new List<Filtered>();


                    

                    int count = 0;

                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("myconfig.json");
                    await FileIO.AppendTextAsync(file, "{ \"data\" : [");

                    //for first time
                    foreach (Filtered str in FilteredData) 
                    {
                        

                        if (str.strikePrice > underlyingVal - 400) //18,398
                        {

                            if (count < 17)
                            {

                                string json = JsonConvert.SerializeObject(str);
                                // write string to a file
                                await FileIO.AppendTextAsync(file, json);

                                if (count < 16) {
                                    await FileIO.AppendTextAsync(file, ",");
                                };

                                Debug.WriteLine(str.strikePrice);

                            }
                            
                            count++;
                        }
                        

                    }
                    await FileIO.AppendTextAsync(file, "]}");

                    var file2 = await ApplicationData.Current.LocalFolder.GetFileAsync("myconfig.json");
                    Debug.WriteLine(file2);
                    foreach(Filtered flr in getCE0) 
                    {
                        Debug.WriteLine(flr.CE.openInterest);
                        
                    }





                }
            }
            catch(Exception ex) 
            { 
                Debug.WriteLine("Exception Thrown :" +ex); 
            }
        }
    }

}
